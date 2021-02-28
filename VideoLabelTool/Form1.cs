using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;


namespace VideoLabelTool
{
    public partial class FormFrameCapture : Form
    {
        double TotalFrame;
        int Fps;
        int currentFrameNum;
        bool IsReadingFrame;
        VideoCapture capture;        
        Timer My_Timer = new Timer();
        int count = 0;        
        OpenFileDialog ofd;
                

        public FormFrameCapture()
        {
            InitializeComponent();
            this.button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                capture = new VideoCapture(ofd.FileName);                
                Mat m = new Mat();
                capture.Read(m);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = m.ToBitmap();

                TotalFrame = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = (int) capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                this.button2.Enabled = true;
            }
        }                

        private void bntPlay_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                return;
            }
            IsReadingFrame = true;
            My_Timer.Interval = 1000 / Fps;
                       
            My_Timer.Tick += new EventHandler(My_Timer_Tick);
            
            My_Timer.Start();

            // Playback video with delay version
            //ReadAllFrames();
        }

        private void My_Timer_Tick(object sender, EventArgs e)
        {
            if (currentFrameNum < TotalFrame-1)
            {
                if (currentFrameNum != 0)
                    capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum + 1);
                else
                    capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
                pictureBox1.Image = capture.QueryFrame().ToBitmap();
                currentFrameNum += Convert.ToInt16(numericUpDown1.Value);                
                label1.Text = currentFrameNum.ToString() + '/' + TotalFrame.ToString();               
            }

            else
            {
                My_Timer.Stop();
                capture.Dispose();
            }
        }

        private void bntNextFrame_Click(object sender, EventArgs e)
        {
            if (currentFrameNum < TotalFrame && currentFrameNum != TotalFrame - 1)
            {                
                currentFrameNum = (int) capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);                
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
                pictureBox1.Image = capture.QueryFrame().ToBitmap();
                
                label1.Text = currentFrameNum.ToString() + '/' + TotalFrame.ToString();                
            }

            else
            {
                this.button2.Enabled = false;
            }
           
        }

        //private async void ReadAllFrames()
        //{
        //    Mat m = new Mat();
        //    while (IsReadingFrame == true && currentFrameNum < TotalFrame)
        //    {
        //        currentFrameNum += Convert.ToInt16(numericUpDown1.Value);
        //        capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
        //        capture.Read(m);
        //        pictureBox1.Image = m.ToBitmap();
        //        await Task.Delay(1000 / Convert.ToInt16(Fps));
        //        //await Task.Delay(1);
        //        label1.Text = currentFrameNum.ToString() + '/' + TotalFrame.ToString(); 
        //    }
        //}

        private void bntPause_Click(object sender, EventArgs e)
        {
            IsReadingFrame = false;
            My_Timer.Stop();
        }        
        
    }
}
