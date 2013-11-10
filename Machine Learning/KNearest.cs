using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using CxCore;
using Cv;
using OtherLibs;
using MachineLearning;

namespace OpenCVLib2
{
    class KNearest
    {
        private MainForm form;
        private preprocessing p;
           
        #region global vars
        string file_path = "..\\..\\Training\\";

        int train_samples = 50;
        int classes = 19;

        public IplImage src_image;
        public IplImage prs_image;
        public IplImage img;
        public IplImage img32;

        private CvMat trainData;
        private CvMat trainClasses;

        int size = 150;

        int K;
        CvKNearest knn;
        #endregion

        public KNearest(MainForm form)
        {
            this.form = form;

            trainData = cxcore.CvCreateMat(train_samples * classes, size * size, cxtypes.CV_32FC1);
            trainClasses = cxcore.CvCreateMat(train_samples * classes, 1, cxtypes.CV_32FC1);

            K = int.Parse(form.txtK.Text);

            p = new preprocessing(form);
        }

        //Gunakan method getData utk membuat training data dan classes
        //training folder terdapat subfolder sesuai dengan class masing2
        //setiap file mempunya nama cnn.pbm dimana c = class {0..99} dan nn = urutan image {00..99}
        public void getData()
        {
            CvMat row = new CvMat();
            CvMat data = new CvMat();
            string file;
            int i = 7, j = 0;
            for (i = 0; i < classes; i++)
            {
                for (j = 0; j < train_samples; j++)
                {
                    if (j < 10)
                        file = file_path + i.ToString() + "\\" + i.ToString() + "0" + j.ToString() + ".pbm";
                    else
                        file = file_path + i.ToString() + "\\" + i.ToString() + j.ToString() + ".pbm";
                    
                    //form.WriteLine("Training..." + file,true,true);

                    src_image = highgui.CvLoadImage(file, highgui.CV_LOAD_IMAGE_GRAYSCALE);
                    
                    if (src_image.ptr == null)
                    {
                        form.WriteLine("Error: Cant load image: " + file + "\n", true, true);
                    }

                    //process file
                    prs_image = p.preprocess(src_image, size, size);
                    
                    //set class label
                    cxcore.CvGetRow(ref trainClasses, ref row, i * train_samples + j);
                    cxcore.CvSet(ref row, cxtypes.cvRealScalar(i));

                    CvMat row_header = new CvMat();
                    CvMat row1 = new CvMat();

                    //set data
                    cxcore.CvGetRow(ref trainData, ref row, i * train_samples + j);
                    img = cxcore.CvCreateImage(new CvSize(size, size), (int)cxtypes.IPL_DEPTH_32F, 1);
                    //convert 8bits image to 32 bits
                    cxcore.CvConvertScale(ref prs_image, ref img, 0.0039215, 0);
                    cxcore.CvGetSubRect(ref img, ref data, new CvRect(0, 0, size, size));                                                            
                    //convert data matrix size x size to vector
                    row1 = cxcore.CvReshape(ref data, ref row_header, 0, 1);                                
                    cxcore.CvCopy(ref row1, ref row); 
                    
                    cxcore.CvReleaseImage(ref src_image);
                    cxcore.CvReleaseImage(ref prs_image);
                    cxcore.CvReleaseImage(ref img);
                }
            }
        }

        //invoke knn class
        public void train()
        {
            knn = new CvKNearest(trainData, trainClasses, false, K);
        }
        
        //method utk mengklasifikasi img berdasarkan class yg terdekat
        public float classify(ref IplImage img, bool showResult)
        {
            CvMat data = new CvMat();
            CvMat results = new CvMat();                                                                    //<<<< check
            CvMat dist = new CvMat();                                                                       //<<<< check
            CvMat nearest = cxcore.CvCreateMat(1, K, cxtypes.CV_32FC1);                                     //<<<< check        

            float result;
            //process file
            prs_image = p.preprocess(img, size, size);

            //set data
            img32 = cxcore.CvCreateImage(new CvSize(size, size), (int)cxtypes.IPL_DEPTH_32F, 1);
            cxcore.CvConvertScale(ref prs_image, ref img32, 0.0039215, 0);
            cxcore.CvGetSubRect(ref img32, ref data, new CvRect(0, 0, size, size));             //possible memory leak??
            
            CvMat row_header = new CvMat();
            CvMat row1 = new CvMat();                                                                       //<<< check
            
            //convert data matrix size x size to vector
            row1 = cxcore.CvReshape(ref data, ref row_header, 0, 1);                                        //<<< check

            result = knn.find_nearest(row1, K, results, IntPtr.Zero, nearest, dist);

            int accuracy = 0;
            for (int i = 0; i < K; i++)
            {
                if (nearest.Data_Fl[i] == result)
                    accuracy++;
            }
            float pre = 100 * ((float)accuracy / (float)K);
            if (showResult == true)
            {
                form.WriteLine("|\tClass\t\t|\tPrecision\t\t|\tAccuracy/K\t|\n", false, false);
                form.WriteLine("|\t" + result.ToString() + "\t\t|\t" + pre.ToString("N2") + "% \t\t|\t" + accuracy.ToString() + "/" + K.ToString() + "\t\t|" + "\n", false, false);
                form.WriteLine(" -------------------------------------------------------------------------------------------------------------------------------------------------\n", false, false);
            }

            cxcore.CvReleaseImage(ref img);
            
            return result;
        }                
        
        //method utk mengetest sekumpulan image (gunakan utk mengetes validasi data training)
        public void test()
        {
            string file = null;
            int i, j;
            int error = 0;
            int testCount = 0;
            for (i = 5; i < classes; i++)
            {
                for (j = 90; j < 100; j++)
                {
                    file = file_path + i.ToString() + "\\" + i.ToString() + j.ToString() + ".pbm";
                    src_image = highgui.CvLoadImage(file, highgui.CV_LOAD_IMAGE_GRAYSCALE);
                    if (src_image.ptr == null)
                    {
                        form.textBox.AppendText("Error: Cant load image: " + file + "\n");
                    }
                    //process file
                    //prs_image = p.preprocess(src_image, size, size);
                    float r = classify(ref src_image, true);
                    if ((int)r != i)
                        error++;

                    testCount++;
                }
            }
            float totalerror = 100 * (float)error / (float)testCount;
            form.textBox.AppendText("System Error : " + totalerror.ToString() + "\n");
        }
        
    } 
}
