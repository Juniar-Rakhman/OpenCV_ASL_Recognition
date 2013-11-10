using System;
using System.Collections.Generic;
using System.Text;

using CxCore;
using Cv;
using OtherLibs;

namespace OpenCVLib2
{
    class SkinDetect
    {
        private MainForm form;

        public SkinDetect(MainForm form)
        {
            this.form = form;
        }

        struct num
        {
            public byte H;
            public byte S;
            public byte V;
        }

        byte find_max_color(byte red, byte green, byte blue)
        {
            byte max = 0;

            max = Math.Max(max, red);
            max = Math.Max(max, green);
            max = Math.Max(max, blue);

            return max;
        }

        byte find_min_color(byte red, byte green, byte blue)
        {
            byte min = 255;

            min = Math.Min(min, red);
            min = Math.Min(min, green);
            min = Math.Min(min, blue);

            return min;
        }

        public IplImage skin_hsv(IplImage image)
        {
            int xi, x, y, p;
            IplImage img_hsv;
            img_hsv = cxcore.CvCreateImage(cxcore.CvGetSize(ref image), 8, 3);
            cv.CvCvtColor(ref image, ref img_hsv, cvtypes.CV_BGR2HSV);

            num[,] bmpdata;
            bmpdata = new num[image.height, image.width];

            byte[] dataIn = img_hsv.ImageDataUChar;

            for (y = 0; y < image.height; y++)
            {
                for (xi = 0, x = 0; xi < image.widthStep; xi += 3, x++)
                {
                    //column position
                    p = y * image.widthStep + xi;

                    //ambil pixel data
                    bmpdata[y, x].H = dataIn[p];
                    bmpdata[y, x].S = dataIn[p + 1];
                    bmpdata[y, x].V = dataIn[p + 2];
                }
            }

            for (y = 0; y < image.height; y++)
            {
                for (x = 0; x < image.width; x++)
                {
                    if (bmpdata[y, x].H <= 19 && bmpdata[y, x].S >= 48) //jika kondisi cocok maka jgn d hitamkan
                        bmpdata[y, x].H += 0;
                    else
                        bmpdata[y, x].H = bmpdata[y, x].S = bmpdata[y, x].V = 0;
                }
            }

            for (y = 0; y < image.height; y++)
            {
                for (xi = 0, x = 0; xi < image.widthStep; xi += 3, x++)
                {
                    //column position
                    p = y * image.widthStep + xi;

                    //ambil pixel data
                    dataIn[p] = bmpdata[y, x].H;
                    dataIn[p + 1] = bmpdata[y, x].S;
                    dataIn[p + 2] = bmpdata[y, x].V;
                }
            }

            img_hsv.ImageDataUChar = dataIn;

            IplImage res = cxcore.CvCreateImage(cxcore.CvGetSize(ref image), 8, 3);
            cv.CvCvtColor(ref img_hsv, ref res, cvtypes.CV_HSV2BGR);

            cxcore.CvReleaseImage(ref img_hsv);
            return res;
        }

        public IplImage skin_rgb(IplImage image)
        {
            int x, y, p;
            byte red, green, blue; //dalam byte

            //ambil raw data
            byte[] data = image.ImageDataUChar;

            //gunakan widthStep (widthStep = 3 * width)
            for (x = 0; x < image.widthStep; x += 3)
            {
                for (y = 0; y < image.height; y++)
                {
                    //column position yg benar
                    p = y * image.widthStep + x;

                    //ambil pixel data
                    blue = data[p];
                    green = data[p + 1];
                    red = data[p + 2];

                    if (red > 95 && green > 40 && blue > 20 && find_max_color(red, green, blue) - find_min_color(red, green, blue) > 15 && Math.Abs(red - green) > 15 && red > green && red > blue && green > blue)
                    {
                        data[p] = blue;
                        data[p + 1] = green;
                        data[p + 2] = red;
                    }
                    else
                    {
                        data[p] = 0;
                        data[p + 1] = 0;
                        data[p + 2] = 0;
                    }
                }
            }
                        
            IplImage res = cxcore.CvCreateImage(cxcore.CvGetSize(ref image), 8, 3);
            res.ImageDataUChar = data;
            return res;
        }
    }
}
