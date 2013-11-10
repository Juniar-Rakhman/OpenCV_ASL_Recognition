using System;
using System.Collections.Generic;
using System.Text;

using CxCore;
using Cv;
using OtherLibs;

namespace OpenCVLib2
{
    class AbsDiff
    {
        private MainForm form;
        private IplImage imgLast;
        private IplImage imgDiff;
        private bool sudah_ambil = false;

        int diam = 0, gerak = 0, wave = 0;
        
        public AbsDiff(MainForm form)
        {
            this.form = form;
        }

        public void countWhitePix(IplImage image)
        {
            int p, white = 0 ;

            byte pix;
                      
            byte[] data = image.ImageDataUChar;

            for (int x = 0; x < image.widthStep; x++)
            {
                for (int y = 0; y < image.height; y++)
                {
                    p = y * image.widthStep + x;

                    pix = data[p];

                    if (pix == 255)
                        white++;
                }
            }

            if (white < 50 && white < 5)
                diam++;
            else
                diam = 0;

            if (white > 100) 
            {
                gerak++;
                if (white > 500)
                    wave++;                
            }

            if (diam > 10)
            {
                gerak = 0;
                wave = 0;
                diam = 0;
                form.match = true;
            }

            if (wave > 10)
            {
                form.reset = true;               
                wave = 0;
                gerak = 0;
                diam = 0;
            }
            
            cxcore.CvReleaseImage(ref image);
        }

        public void Absolute(IplImage imgNow)
        {
            imgDiff = cxcore.CvCreateImage(cxcore.CvGetSize(ref imgNow), imgNow.depth, imgNow.nChannels);

            if (!sudah_ambil)
            {
                imgLast = cxcore.CvCreateImage(cxcore.CvGetSize(ref imgNow), imgNow.depth, imgNow.nChannels);
                imgLast = cxcore.CvCloneImage(ref imgNow);
                sudah_ambil = true;
            }
            else
                sudah_ambil = false;

            cxcore.CvAbsDiff(ref imgNow, ref imgLast, ref imgDiff);

            cv.CvSmooth(ref imgDiff, ref imgDiff);
            cv.CvSmooth(ref imgDiff, ref imgDiff);

            if(form.showAbs)
                highgui.CvShowImage("Motion", ref imgDiff);
            
            countWhitePix(imgDiff);

            if (!sudah_ambil)
                cxcore.CvReleaseImage(ref imgLast);

            cxcore.CvReleaseImage(ref imgNow);
            cxcore.CvReleaseImage(ref imgDiff);
        }
    }
}
