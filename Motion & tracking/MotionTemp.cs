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
    class MotionTemp
    {
        private MainForm form;

        const double MHI_DURATION = 1;
        const double MAX_TIME_DELTA = 0.5;
        const double MIN_TIME_DELTA = 0.05;

        const int N = 4;
        int last = 0;

        IplImage[] buf = new IplImage[10];
        IplImage mhi;
        IplImage orient;
        IplImage mask;
        IplImage segmask;
        CvMemStorage storage;

        public MotionTemp(MainForm form)
        {
            this.form = form;
        }

        public void update_mhi(IplImage imgMain, ref IplImage imgDst, int diff_threshold)
        {
            double timestamp = (double)DateTime.Now.Second;
            CvSize size = new CxCore.CvSize(imgMain.width, imgMain.height);
            int i, idx1 = last, idx2;
            IplImage silh;
            CvSeq seq;
            CvRect comp_rect;
            double count;
            double angle;
            CvPoint center;
            double magnitude;
            CvScalar color;

            //allocate images at the beginning or reallocate them if the frame size is changed
            if (mhi.ptr == null || mhi.width != size.width || mhi.height != size.height)
            {
                for (i = 0; i < N; i++)
                {
                    buf[i] = cxcore.CvCreateImage(size, (int)cxtypes.IPL_DEPTH_8U, 1);
                    cxcore.CvZero(ref buf[i]);
                }
                cxcore.CvReleaseImage(ref mhi);
                cxcore.CvReleaseImage(ref orient);
                cxcore.CvReleaseImage(ref segmask);
                cxcore.CvReleaseImage(ref mask);

                mhi = cxcore.CvCreateImage(size, (int)cxtypes.IPL_DEPTH_32F, 1);
                cxcore.CvZero(ref mhi);
                orient = cxcore.CvCreateImage(size, (int)cxtypes.IPL_DEPTH_32F, 1);
                segmask = cxcore.CvCreateImage(size, (int)cxtypes.IPL_DEPTH_32F, 1);
                mask = cxcore.CvCreateImage(size, (int)cxtypes.IPL_DEPTH_32F, 1);
            }

            cv.CvCvtColor(ref imgMain, ref buf[last], cvtypes.CV_BGR2GRAY);

            idx2 = (last + 1) % N;
            last = idx2;

            silh = buf[idx2];
            cxcore.CvAbsDiff(ref buf[idx1], ref buf[idx2], ref silh);

            cv.CvThreshold(ref silh, ref silh, diff_threshold, 1, cv.CV_THRESH_BINARY);
            cv.CvUpdateMotionHistory(ref silh, ref mhi, timestamp, MHI_DURATION);

            cxcore.CvConvertScale(ref mhi, ref mask, 255 / MHI_DURATION, (MHI_DURATION - timestamp) * 255 / MHI_DURATION);
            cxcore.CvZero(ref imgDst);
            cxcore.CvMerge(ref mask, ref imgDst);
            cv.CvCalcMotionGradient(ref mhi, ref mask, ref orient, MAX_TIME_DELTA, MIN_TIME_DELTA, 3);
            if (storage.ptr == null)
                storage = cxcore.CvCreateMemStorage();
            else
                cxcore.CvClearMemStorage(ref storage);
            seq = cv.CvSegmentMotion(ref mhi, ref segmask, ref storage, timestamp, MAX_TIME_DELTA);
            for (i = -1; i < seq.total; i++)
            {
                if (i < 0)
                {
                    comp_rect = new CvRect(0, 0, size.width, size.height);
                    color = cxcore.CV_RGB(255, 255, 255);
                    magnitude = 100;
                }
                else
                {
                    IntPtr ptr = cxcore.CvGetSeqElem(ref seq, i);
                    CvConnectedComp c = (CvConnectedComp)cvconvert.PtrToType(ptr, typeof(CvConnectedComp));
                    comp_rect = c.rect;
                    if (comp_rect.width + comp_rect.height < 100)
                        continue;
                    color = cxcore.CV_RGB(255, 0, 0);
                    magnitude = 30;
                }

                //select component ROI
                cxcore.CvSetImageROI(ref silh, comp_rect);
                cxcore.CvSetImageROI(ref mhi, comp_rect);
                cxcore.CvSetImageROI(ref orient, comp_rect);
                cxcore.CvSetImageROI(ref mask, comp_rect);

                //calculate orientation
                angle = cv.CvCalcGlobalOrientation(ref orient, ref mask, ref mhi, timestamp, MHI_DURATION);
                angle = 360 - angle;

                count = cxcore.CvNorm(ref silh); //<<<<<<<<<<<<<<< recheck

                cxcore.CvResetImageROI(ref mhi);
                cxcore.CvResetImageROI(ref orient);
                cxcore.CvResetImageROI(ref mask);
                cxcore.CvResetImageROI(ref silh);

                //check for the case of little motion
                if (count < comp_rect.width * comp_rect.height * 0.05)
                    continue;

                //draw a clock with arrow indicating the direction
                center = new CvPoint((comp_rect.x + comp_rect.width / 2), (comp_rect.y + comp_rect.height / 2));

                cxcore.CvCircle(ref imgDst, center, cxcore.CvRound(magnitude * 1.2), color, 3, cxcore.CV_AA, 0);
                cxcore.CvLine(ref imgDst, center,
                    new CvPoint(cxcore.CvRound(center.x + magnitude * Math.Cos(angle * Math.PI / 180)),
                    cxcore.CvRound(center.y - magnitude * Math.Sin(angle * Math.PI / 180))),
                    color, 3, cxcore.CV_AA, 0);
            }
        }
    }
}
