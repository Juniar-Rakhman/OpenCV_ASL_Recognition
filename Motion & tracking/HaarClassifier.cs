using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using CvTools;
using Cv;
using CxCore;

namespace OpenCVLib2
{
    class HaarClassifier
    {
        private MainForm form;

        CvHaarClassifierCascade cascadeO;
        CvHaarClassifierCascade cascadeC;
        SkinDetect skinDet;
        CvRect hc_rect, ho_rect;

        private IplImage imgSkin;

        public HaarClassifier(MainForm form)
        {
            this.form = form;
            skinDet = new SkinDetect(this.form);
        }

        public IplImage cariHaar(IplImage image)
        {   
            cxcore.CvFlip(ref image, 1);
            
            imgSkin = new IplImage();
            imgSkin = cxcore.CvCreateImage(cxcore.CvGetSize(ref image), 8, 3);
            imgSkin = skinDet.skin_hsv(image);
                        
            IplImage gray = cxcore.CvCreateImage(new CvSize(imgSkin.width, imgSkin.height), (int)cxtypes.IPL_DEPTH_8U, 1);
            cv.CvCvtColor(ref imgSkin, ref gray, cvtypes.CV_BGR2GRAY);
            
            IplImage small_image = imgSkin;
            CvMemStorage storage = cxcore.CvCreateMemStorage(0);
            CvSeq handOpen, handClose;
            int i, scale = 1;
            bool do_pyramids = true;

            #region percepat proses
            if (do_pyramids)
            {
                small_image = cxcore.CvCreateImage(new CvSize(imgSkin.width / 2, imgSkin.height / 2), (int)cxtypes.IPL_DEPTH_8U, 3);
                cv.CvPyrDown(ref imgSkin, ref small_image, (int)CvFilter.CV_GAUSSIAN_5x5);
                scale = 2;
            }
            #endregion                     

            #region open hand
            IntPtr ptrO = cxcore.CvLoad("..\\..\\Training\\handOpen.xml");
            cascadeO = (CvHaarClassifierCascade)cvconvert.PtrToType(ptrO, typeof(CvHaarClassifierCascade));
            cascadeO.ptr = ptrO;
            handOpen = cv.CvHaarDetectObjects(ref small_image, ref cascadeO, ref storage, 1.2, 2, cv.CV_HAAR_DO_CANNY_PRUNING, new CvSize(0, 0));
            if (handOpen.total != 0)
            {
                for (i = 0; i < handOpen.total; i++)
                {
                    ho_rect = (CvRect)cvconvert.PtrToType(cxcore.CvGetSeqElem(ref handOpen, i), typeof(CvRect));
                    cxcore.CvRectangle(ref image, new CvPoint(ho_rect.x * scale - 10, ho_rect.y * scale - 10),
                        new CvPoint((ho_rect.x + ho_rect.width) * scale + 10, (ho_rect.y + ho_rect.height) * scale + 10),
                        cxcore.CV_RGB(255, 0, 0), 1, 8, 0);
                }
                form.closex = 0;
                form.closey = 0;
                form.openx = image.width - ((ho_rect.x * scale) + ((ho_rect.width * scale) / 2));
                form.openy = ho_rect.y * scale + ((ho_rect.height * scale) / 2);
                
                form.roiX = 640 - (ho_rect.x * scale - 10) - (ho_rect.width * scale + 10);
                form.roiY = ho_rect.y * scale - 10;
                form.roiW = ho_rect.width * scale + 10;
                form.roiH = ho_rect.height * scale + 10;
            }            
            #endregion

            #region close hand
            if (handOpen.total == 0)
            {
                IntPtr ptrC = cxcore.CvLoad("..\\..\\Training\\handClose.xml");
                cascadeC = (CvHaarClassifierCascade)cvconvert.PtrToType(ptrC, typeof(CvHaarClassifierCascade));
                cascadeC.ptr = ptrC;
                handClose = cv.CvHaarDetectObjects(ref small_image, ref cascadeC, ref storage, 1.2, 2, cv.CV_HAAR_DO_CANNY_PRUNING, new CvSize(0, 0));
                if (handClose.total != 0)
                {
                    for (i = 0; i < handClose.total; i++)
                    {
                        hc_rect = (CvRect)cvconvert.PtrToType(cxcore.CvGetSeqElem(ref handClose, i), typeof(CvRect));
                        cxcore.CvRectangle(ref image, new CvPoint(hc_rect.x * scale, hc_rect.y * scale),
                                            new CvPoint((hc_rect.x + hc_rect.width) * scale, (hc_rect.y + hc_rect.height) * scale),
                                            cxcore.CV_RGB(0, 0, 255), 1, 8, 0);
                    }
                    form.closex = image.width - ((hc_rect.x * scale) + ((hc_rect.width * scale) / 2));
                    form.closey = hc_rect.y * scale + ((hc_rect.height * scale) / 2);
                }
            }
            #endregion

            cxcore.CvReleaseMemStorage(ref storage); 
            cv.CvReleaseHaarClassifierCascade(ref cascadeO);
            if(handOpen.total == 0)
                cv.CvReleaseHaarClassifierCascade(ref cascadeC);
            cxcore.CvReleaseImage(ref gray);
            cxcore.CvReleaseImage(ref small_image);
            cxcore.CvReleaseImage(ref imgSkin);
            cxcore.CvFlip(ref image, 1);
            return image;
        }
    }
}
