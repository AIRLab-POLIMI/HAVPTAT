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
        VideoCapture capture;        
        Timer My_Timer = new Timer();        
        int status = 0;
        OpenFileDialog ofd;
                

        public FormFrameCapture()
        {
            InitializeComponent();
            this.bntNextFrame.Enabled = false;
            this.bntPrevFrame.Enabled = false;
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

                this.bntNextFrame.Enabled = true;
            }
        }                

        private void bntPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void Play()
        {
            if (capture == null)
            {
                return;
            }            
            My_Timer.Interval = 1000 / Fps;
            My_Timer.Tick += new EventHandler(My_Timer_Tick);
            My_Timer.Start();
            this.bntPrevFrame.Enabled = true;
            status = 1;            
        }   

        private void My_Timer_Tick(object sender, EventArgs e)
        {
            if (currentFrameNum < TotalFrame)
            {                                
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);                

                pictureBox1.Image = capture.QueryFrame().ToBitmap();
                currentFrameNum += Convert.ToInt16(numericUpDown1.Value);                
                label1.Text = currentFrameNum.ToString() + '/' + TotalFrame.ToString();               
            }

            else
            {
                My_Timer.Stop();             
                status = 0;                
            }
        }

        private void bntNextFrame_Click(object sender, EventArgs e)
        {
            NextFrame();              
        }

        private void NextFrame()
        {
            if (currentFrameNum < TotalFrame)
            {                
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
                pictureBox1.Image = capture.QueryFrame().ToBitmap();
                currentFrameNum += 1;

                label1.Text = currentFrameNum.ToString() + '/' + TotalFrame.ToString();
            }

            else
            {             
                return;
            }

            this.bntPrevFrame.Enabled = true;
            status = 0;
        }

        private void bntPrevFrame_Click(object sender, EventArgs e)
        {
            PreviousFrame();
        }

        private void PreviousFrame()
        {
            if (currentFrameNum > 1 && currentFrameNum <= TotalFrame)
            {                
                currentFrameNum -= 1;
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
                pictureBox1.Image = capture.QueryFrame().ToBitmap();
                

                label1.Text = currentFrameNum.ToString() + '/' + TotalFrame.ToString();
            }            
            status = 0;
        }        

        private void bntPause_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void Pause()
        {            
            My_Timer.Stop();
            status = 0;
        }        

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Right))
            {
                NextFrame();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Left))
            {
                PreviousFrame();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Space))
            {                                
                if (status == 0)
                    Play();
                else
                    Pause();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        
    }
}
