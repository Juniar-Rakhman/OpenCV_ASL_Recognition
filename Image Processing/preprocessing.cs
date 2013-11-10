using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CxCore;
using Cv;
using OtherLibs;

namespace OpenCVLib2
{
    class preprocessing
    {
        private CvMat data;

        private MainForm form;
        
        #region preprocessing functions

        public void cariX(IplImage imgSrc, ref int min, ref int max)
        {
            bool minTemu = false;

            data = new CvMat();

            CvScalar maxVal = cxtypes.cvRealScalar(imgSrc.width * 255);
            CvScalar val = cxtypes.cvRealScalar(0);
            
            //utk setiap kolom sum, jika sum < width*255 maka kita temukan min
            //kemudian lanjutkan hingga akhir utk menemukan max, jika sum < width*255 maka ditemukan max baru 
            for (int i = 0; i < imgSrc.width; i++)
            {
                cxcore.CvGetCol(ref imgSrc, ref data, i); //col
                val = cxcore.CvSum(ref data);
                if (val.val1 < maxVal.val1)
                {
                    max = i;
                    if (!minTemu)
                    {
                        min = i;
                        minTemu = true;
                    }
                }
            }
        }

        public void cariY(IplImage imgSrc, ref int min, ref int max)
        {
            bool minFound = false;

            data = new CvMat();

            CvScalar maxVal = cxtypes.cvRealScalar(imgSrc.width * 255);
            CvScalar val = cxtypes.cvRealScalar(0);

            //utk setiap baris sum, jika sum < width*255 maka kita temukan min
            //kemudian lanjutkan hingga akhir utk menemukan max, jika sum < width*255 maka ditemukan max baru 
            for (int i = 0; i < imgSrc.height; i++)
            {
                cxcore.CvGetRow(ref imgSrc, ref data, i); //row
                val = cxcore.CvSum(ref data);
                if (val.val1 < maxVal.val1)
                {
                    max = i;
                    if (!minFound)
                    {
                        min = i;
                        minFound = true;
                    }
                }
            }
        }

        public CvRect cariBB(IplImage imgSrc)
        {
            CvRect aux;
            int xmin, xmax, ymin, ymax, height, width;
            xmin = xmax = ymin = ymax = height = width = 0;

            cariX(imgSrc, ref xmin, ref xmax);
            cariY(imgSrc, ref ymin, ref ymax);

            width = xmax - xmin;
            height = ymax - ymin;

            double lebar = width * 1.5;

            height = height >= (width * 1.5) ? (int)lebar : height;

            //form.WriteLine("height = " + height.ToString(), true, true);
            //form.WriteLine("width = " + width.ToString(), true, true);

            aux = new CvRect(xmin, ymin, width, height);

            return aux;
        }

        public preprocessing(MainForm form)
        {
            this.form = form;
        }

        public IplImage preprocess(IplImage imgSrc, int new_width, int new_height)
        {
            IplImage result;
            IplImage scaledResult;

            // A = aspect ratio maintained
            CvMat data = new CvMat();
            CvMat dataA = new CvMat();
            CvRect bb = new CvRect();
            CvRect bbA = new CvRect();

            bb = cariBB(imgSrc);
            //Cari data bounding box
            cxcore.CvGetSubRect(ref imgSrc, ref data, new CvRect(bb.x, bb.y, bb.width, bb.height));
            
            //Buat image dengan data width dan height (aspect ratio = 1)
            int size = (bb.width > bb.height) ? bb.width : bb.height;
            result = cxcore.CvCreateImage(new CvSize(size, size), 8, 1);
            cxcore.CvSet(ref result, new CvScalar(255, 255, 255));

            int x = (int)Math.Floor((size - bb.width) / 2.0f);
            int y = (int)Math.Floor((size - bb.height) / 2.0f);
            cxcore.CvGetSubRect(ref result, ref dataA, new CvRect(x, y, bb.width, bb.height));
            cxcore.CvCopy(ref data, ref dataA);

            scaledResult = cxcore.CvCreateImage(new CvSize(new_width, new_height), 8, 1);
            cv.CvResize(ref result, ref scaledResult, cv.CV_INTER_NN);

            return scaledResult;
        }

        #endregion 
    }
}
