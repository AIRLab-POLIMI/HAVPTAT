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
using System.Drawing;


namespace VideoLabelTool
{
    public partial class FormFrameCapture : Form
    {
        double TotalFrame;
        double Fps;
        int FrameNo;
        bool IsReadingFrame;
        VideoCapture capture;        

        Timer My_Timer = new Timer();
        int FPS = 15;

        public FormFrameCapture()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                capture = new VideoCapture(ofd.FileName);
                //new Capture(ofd.FileName);
                Mat m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.ToBitmap();

                TotalFrame = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

                My_Timer.Interval = 1000 / FPS;
                My_Timer.Tick += new EventHandler(My_Timer_Tick);
                My_Timer.Start();                

            }
        }

        private void My_Timer_Tick(object sender, EventArgs e)
        {            
            pictureBox1.Image = capture.QueryFrame().ToBitmap();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bntPlay_Click(object sender, EventArgs e)
        {
            if (capture == null)
            {
                return;
            }
            IsReadingFrame = true;
            ReadAllFrames();
        }

        private async void ReadAllFrames()
        {
            Mat m = new Mat();
            while (IsReadingFrame == true && FrameNo < TotalFrame)
            {
                FrameNo += Convert.ToInt16(numericUpDown1.Value);
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, FrameNo);
                capture.Read(m);
                pictureBox1.Image = m.ToBitmap();
                //await Task.Delay(500 / Convert.ToInt16(Fps));
                await Task.Delay(1);
                label1.Text = FrameNo.ToString() + '/' + TotalFrame.ToString(); 
            }
        }

        private void bntPause_Click(object sender, EventArgs e)
        {
            IsReadingFrame = false;
        }
  
    }
}
