using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using CxCore;
using Cv;
using OtherLibs;

namespace OpenCVLib2
{   
    public partial class MainForm : Form
    {

        #region user defined vars

        private DlgParams dlgParam = null;
        private DlgParams dlgCanny = null;
        private KNearest KNN = null;

        private CvCapture videoCapture;
        private SkinDetect skinDet = null;
        private HaarClassifier hc = null;
        private AbsDiff abs = null;

        private int fps = 0, hasil;

        public bool showROI = false,
            showThres = true,
            showEdge = false,
            showSkinHSV = true,
            showSkinRGB = false,
            showAbs = true,
            reset = false,
            initialized = false,
            capture = false,
            match = false,
            show_letter = false;
        
        #region deklarasi warna/scalar
        private CvScalar merah = cxcore.CV_RGB(250, 0, 0);
        private CvScalar hijau = cxcore.CV_RGB(0, 250, 0);
        private CvScalar biru = cxcore.CV_RGB(0, 0, 250);
        #endregion

        #region IplImages
        private IplImage frame;
        private IplImage imgMain;
        private IplImage imgSkin;
        private IplImage imgBin;
        private IplImage imgGray;
        private IplImage imgCrop;
        private IplImage imgMot;
        #endregion

        public int openx, openy, closex, closey;
        public int roiX, roiY, roiH, roiW;

        #region deklarasi directory & files
        private string namaFile = string.Empty;
        private string finalName = string.Empty;
        private string tempName = string.Empty;
        string dir = "..\\..\\Training\\";
        string ext = ".pbm";
        string[] Signs = { "A", "B", "C", "D", "F", "G", "H", "I", "K", "L", "O", "P", "Q", "R", "U", "V", "W", "X", "Y", " ", "?"};
        #endregion

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        public void WriteLine(string s, bool crlf, bool date)
        {
            if ((s.Length + textBox.TextLength) > textBox.MaxLength)
                textBox.Clear();
            if (!crlf && !date)
                textBox.AppendText(s);
            if (!crlf && date)
                textBox.AppendText(DateTime.Now.ToString() + ">> " + s);
            if (crlf && !date)
                textBox.AppendText(s + "\r\n");
            if (crlf && date)
                textBox.AppendText(DateTime.Now.ToString() + ">> " + s + "\r\n");
        }                       
        
        #region keyboard & button click events
        private void btnVideo_Click(object sender, EventArgs e)
        {
            double vidWidth, vidHeight;

            if (btnVideo.Text.Equals("Start Video"))
            {
                train_data();

                videoCapture = highgui.CvCreateCameraCapture(0);

                //check bila valid
                if (videoCapture.ptr == IntPtr.Zero)
                {
                    MessageBox.Show("Pengambilan gambar gagal");
                    return;
                }

                btnVideo.Text = "Stop Video";
                
                highgui.CvSetCaptureProperty(ref videoCapture, highgui.CV_CAP_PROP_FRAME_WIDTH, 640);
                highgui.CvSetCaptureProperty(ref videoCapture, highgui.CV_CAP_PROP_FRAME_HEIGHT, 320);

                highgui.CvQueryFrame(ref videoCapture);                                

                vidWidth = highgui.cvGetCaptureProperty(videoCapture, highgui.CV_CAP_PROP_FRAME_WIDTH);
                vidHeight = highgui.cvGetCaptureProperty(videoCapture, highgui.CV_CAP_PROP_FRAME_HEIGHT);
                
                picBoxMain.Width = (int)vidWidth;
                picBoxMain.Height = (int)vidHeight;                

                WriteLine("Pengambilan gambar dari webcam dengan resolusi: " + vidWidth.ToString() + " x " + vidHeight.ToString(), true, false);
                
                timerGrab.Interval = 42;
                timerFPS.Interval = 1100;
                timerGrab.Enabled = true;
                timerFPS.Enabled = true;

                hc = new HaarClassifier(this);
                abs = new AbsDiff(this);
            }
            else
            {
                btnVideo.Text = "Start Video";
                timerFPS.Enabled = false;
                timerGrab.Enabled = false;

                if (videoCapture.ptr != IntPtr.Zero)
                {
                    highgui.CvReleaseCapture(ref videoCapture);
                    videoCapture.ptr = IntPtr.Zero;
                }
            }
        }
        private void train_data()
        {
            KNN = new KNearest(this);

            WriteLine("Training...", true, true);

            KNN.getData();
            KNN.train();

            WriteLine("Proses training telah selesai", true, true);
        }
          
        private void trainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            train_data();
        }

        private void thresholdToolStripMenuItem_Click()
        {
            showThres = true;
            thresholdToolStripMenuItem.Checked = true;
            edgeToolStripMenuItem.Checked = false;
            showEdge = false;
        }
        
        private void edgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showEdge = true;
            edgeToolStripMenuItem.Checked = true;
            thresholdToolStripMenuItem.Checked = false;
            showThres = false;
        }

        private void hSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showSkinHSV = true;
            hSVToolStripMenuItem.Checked = true;
            rGBToolStripMenuItem.Checked = false;
            showSkinRGB = false;
        }

        private void rGBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showSkinRGB = true;
            rGBToolStripMenuItem.Checked = true;
            hSVToolStripMenuItem.Checked = false;
            showSkinHSV = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            namaFile = e.KeyCode.ToString();
            capture = true;
        }
        #endregion

        #region other methods
        private void skin_dlg()
        {
            if (skinDet == null)
                skinDet = new SkinDetect(this);

            if (dlgParam == null)
            {
                dlgParam = new DlgParams();
                dlgParam.BackColor = Color.FromArgb(244, 239, 240);
                dlgParam.Icon = this.Icon;
                dlgParam.Text = "Noise Removal";
                dlgParam.AddTrackbar("Dilate", 0, 0, 10, 1, 1);
                dlgParam.AddTrackbar("Erode", 1, 0, 10, 1, 1);
                dlgParam.AddTrackbar("Smooth", 2, 0, 10, 1, 1);
            }
        }

        private void edge_dlg()
        {
            if (dlgCanny == null)
            {
                dlgCanny = new DlgParams();
                dlgCanny.BackColor = Color.FromArgb(244, 239, 240);
                dlgCanny.Icon = this.Icon;
                dlgCanny.Text = "Canny";
                dlgCanny.AddTrackbar("Thres 1", 0, 0, 255, 1, 0);
                dlgCanny.AddTrackbar("Thres 2", 1, 0, 255, 1, 0);
                dlgCanny.Show();
                dlgCanny.Location = new Point(675, 500);
            }
        }

        public void euclidean()
        {
            int jarak = (int)Math.Sqrt(Math.Pow((openx - closex), 2) + Math.Pow((openy - closey), 2));
            if (jarak < 50)
                showROI = true;                
            else
                openy = openx = closex = closey = 0;
        }

        public void initialize()
        {            
            highgui.CvNamedWindow("Crop");
            highgui.CvMoveWindow("Crop", 675, 10);
                 
            highgui.CvNamedWindow("Motion");
            highgui.CvMoveWindow("Motion", 950, 10);

            skin_dlg();
            dlgParam.Show();
            dlgParam.Location = new Point(675, 300);

            initialized = true;
        }

        public void resetting()
        {
            highgui.CvDestroyAllWindows();
            
            openy = openx = closex = closey = 0;

            if(dlgParam != null)
                dlgParam.Hide();
            if(dlgCanny != null)
                dlgCanny.Hide();

            if (thresholdToolStripMenuItem.Checked)
                showThres = true;
            if (edgeToolStripMenuItem.Checked)
                showEdge = true;
            if (hSVToolStripMenuItem.Checked)
                showSkinHSV = true;
            if (rGBToolStripMenuItem.Checked)
                showSkinRGB = true;

            textBox.Clear();                        

            reset = false;
        }

        public bool adaBlackPix(IplImage image)
        {
            int p, black = 0;

            byte pix;

            byte[] data = image.ImageDataUChar;

            for (int x = 0; x < image.widthStep; x++)
            {
                for (int y = 0; y < image.height; y++)
                {
                    p = y * image.widthStep + x;

                    pix = data[p];

                    if (pix == 0)
                        black++;
                }
            }

            if (black < 1000)
                return false;
            else
                return true;
        }

        #endregion

        #region timer events
        private void timerGrab_Tick(object sender, EventArgs e)
        {
            frame = highgui.CvQueryFrame(ref videoCapture);

            if (frame.ptr == IntPtr.Zero)
            {
                timerGrab.Stop();
                MessageBox.Show("Invalid Frame");
                return;
            }

            imgMain = cxcore.CvCreateImage(cxcore.CvGetSize(ref frame), 8, 3);
            
            if (reset)
            {
                showROI = false;
                initialized = false;
                resetting();
            }
            
            cxcore.CvCopy(ref frame, ref imgMain);
            cxcore.CvFlip(ref imgMain, 0);

            #region ROI
            if (showROI && initialized)
            {
                cxcore.CvRectangle(ref imgMain, new CvPoint(roiX, roiY), new CvPoint(roiX + roiW, roiY + roiH), cxcore.CV_RGB(255,0,125), 1, 8, 0);
                imgCrop = cxcore.CvCreateImage(new CvSize(roiW, roiH), 8, 3);

                #region skinHSV/RGB
                if (showSkinHSV || showSkinRGB)
                {
                    imgSkin = new IplImage();
                    imgSkin = cxcore.CvCreateImage(cxcore.CvGetSize(ref frame), 8, 3);
                    if (showSkinHSV)
                        imgSkin = skinDet.skin_hsv(imgMain);
                    else if (showSkinRGB)
                        imgSkin = skinDet.skin_rgb(imgMain);
                    cxcore.CvSetImageROI(ref imgSkin, new CvRect(roiX, roiY, roiW, roiH));
                    cxcore.CvCopy(ref imgSkin, ref imgCrop);
                    cxcore.CvReleaseImage(ref imgSkin);

                    //noise removal
                    cv.CvDilate(ref imgCrop, ref imgCrop, dlgParam.GetP(0).i);
                    cv.CvErode(ref imgCrop, ref imgCrop, dlgParam.GetP(1).i);
                    for (int i = 0; i < dlgParam.GetP(2).i; i++)
                        cv.CvSmooth(ref imgCrop, ref imgCrop);                    
                }
                #endregion

                #region show threshold
                if (showThres || showEdge)
                {
                    imgGray = cxcore.CvCreateImage(cxcore.CvGetSize(ref imgCrop), 8, 1);
                    imgBin = cxcore.CvCreateImage(cxcore.CvGetSize(ref imgCrop), 8, 1);
                    imgMot = cxcore.CvCreateImage(cxcore.CvGetSize(ref imgCrop), 8, 1);

                    cv.CvCvtColor(ref imgCrop, ref imgGray, cvtypes.CV_BGR2GRAY);

                    cv.CvThreshold(ref imgGray, ref imgMot, 0, 255, cv.CV_THRESH_BINARY_INV);
                    abs.Absolute(imgMot);

                    if (showThres)
                        cv.CvThreshold(ref imgGray, ref imgBin, 0, 255, cv.CV_THRESH_BINARY_INV);
                    else if (showEdge)
                    {
                        edge_dlg();
                        cv.CvCanny(ref imgGray, ref imgBin, dlgCanny.GetP(0).i, dlgCanny.GetP(1).i);
                    }

                    highgui.CvShowImage("Crop", ref imgBin);

                    #region matching
                    if (match)
                    {
                        if (adaBlackPix(imgBin))
                            hasil = (int)KNN.classify(ref imgBin, false);
                        else
                            hasil = 19;
                        WriteLine(Signs[hasil], false, false);
                      
                        match = false;
                        show_letter = true;
                    }
                    #endregion

                    cxcore.CvReleaseImage(ref imgGray);
                    cxcore.CvReleaseImage(ref imgCrop);
                    cxcore.CvReleaseImage(ref imgBin);
                    cxcore.CvReleaseImage(ref imgMot);
                }
                else
                {
                    highgui.CvShowImage("Crop", ref imgCrop);
                    cxcore.CvReleaseImage(ref imgCrop);
                }
                #endregion
            }
            else if(!initialized && !showROI)
                imgMain = hc.cariHaar(imgMain);
            else if (!initialized) //initialize windows
                initialize();
            #endregion

            if (show_letter)
            {
                CvFont font = new CvFont();
                cxcore.CvInitFont(ref font, cxcore.CV_FONT_HERSHEY_SIMPLEX, 5, 5, 0, 10, cxcore.CV_AA);
                cxcore.CvPutText(ref imgMain, Signs[hasil], new CvPoint(50, 200), ref font, new CvScalar(0,255,0));
            }

            picBoxMain.Image = highgui.ToBitmap(imgMain, false);            
            
            cxcore.CvReleaseImage(ref imgMain);            
                        
            fps++;

            if ((openx != 0 && openy != 0 && closex != 0 && closey != 0) && !showROI)
                euclidean();
        }

        private void timerFPS_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = "Frame Rate: " + fps.ToString() + " Fps";
            fps = 0;
        }
        #endregion
    }
}
