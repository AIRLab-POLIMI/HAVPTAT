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
using System.IO;

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
        string[] lines;
        List<List<string>> lineByFrame;
        int widthPictureBox;
        int heightPictureBox;

        Pen pen = new Pen(Color.Red);
        List<List<Rectangle>> listRec;
        Graphics g;

        
        public FormFrameCapture()
        {
            InitializeComponent();
            this.bntNextFrame.Enabled = false;
            this.bntPrevFrame.Enabled = false;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            pictureBox1.Paint += new PaintEventHandler(this.plotROI);
            this.WindowState = FormWindowState.Maximized;            
            
            widthPictureBox = pictureBox1.Width;
            heightPictureBox = pictureBox1.Height;

            pictureBox1.Width = 1280;
            pictureBox1.Height = 720;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void plotROI(object sender, PaintEventArgs e)
        {   
            if (listRec != null)
            {
                foreach (Rectangle ret in listRec[currentFrameNum])
                {
                    e.Graphics.DrawRectangle(pen, ret);
                }        
            }    
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();            
            if (ofd.ShowDialog() == DialogResult.OK)
            {                
                capture = new VideoCapture(ofd.FileName);                
                Mat m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.ToBitmap();

                TotalFrame = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = (int) capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                My_Timer.Interval = 1000 / Fps;
                My_Timer.Tick += new EventHandler(My_Timer_Tick);

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
            
            My_Timer.Start();
            this.bntPrevFrame.Enabled = true;
            status = 1;            
        }        

        private void My_Timer_Tick(object sender, EventArgs e)
        {
            if (currentFrameNum < TotalFrame - 1)
            {
                // SetCaptureProperty could slow down, but avoid crash
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
                pictureBox1.Image = capture.QueryFrame().ToBitmap();

                //currentFrameNum += Convert.ToInt16(numericUpDown1.Value);                
                currentFrameNum += 1;
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
            if (currentFrameNum < TotalFrame - 1 && capture != null)
            {
                // SetCaptureProperty could slow down, but avoid crash
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
                try
                {         
                    pictureBox1.Image = capture.QueryFrame().ToBitmap();
                }
                catch(NullReferenceException e)
                {
                    throw new NullReferenceException(e.Message);
                }
                currentFrameNum += 1;
                label1.Text = currentFrameNum.ToString() + '/' + TotalFrame.ToString();

                this.Invalidate();      
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
            if (currentFrameNum > 1 && currentFrameNum <= TotalFrame && capture != null)
            {                
                currentFrameNum -= 1;
                // SetCaptureProperty could slow down, but avoid crash
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
                try
                {
                    pictureBox1.Image = capture.QueryFrame().ToBitmap();
                }
                catch (NullReferenceException e)
                {
                    throw new NullReferenceException(e.Message);
                }

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

        private void bntLoadLabels_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            int currentFrameNum = 1, personID = 0;
            lineByFrame = new List<List<string>>();
            lineByFrame.Add(new List<string>());
            listRec = new List<List<Rectangle>>();
            listRec.Add(new List<Rectangle>());            
            String[] words;
            int x;
            int y;
            int weight;
            int height;

            if (ofd.ShowDialog() == DialogResult.OK)
            {                
                lines = System.IO.File.ReadAllLines(@ofd.FileName);
                
                foreach (string line in lines)
                {
                    words = line.Split(',');

                    x = (int)(Convert.ToDouble(words[2]) * 2 / 3);
                    y = (int)(Convert.ToDouble(words[3]) * 2 / 3);
                    weight = (int)(Convert.ToDouble(words[4]) * 2 / 3);
                    height = (int)(Convert.ToDouble(words[5]) * 2 / 3);

                    if (Int32.Parse(words[0]) != currentFrameNum)
                    {
                        currentFrameNum++;
                        lineByFrame.Add(new List<string>());
                        listRec.Add(new List<Rectangle>());
                    }                    
                    lineByFrame[currentFrameNum - 1].Add(line);
                    listRec[currentFrameNum - 1].Add(new Rectangle(x, y, weight, height));
                }                           
            }
        }

        int selectedBBIndex;
        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            foreach (Rectangle r in listRec[currentFrameNum])
                if (r.Contains(e.Location))
                {
                    // do things here
                    Console.WriteLine("You have hit Rectangle Person ID.: " + listRec[currentFrameNum].IndexOf(r) + 1);
                    selectedBBIndex = listRec[currentFrameNum].IndexOf(r) + 1;
                }
        }

        private void bntWalking_Click(object sender, EventArgs e)
        {
            string actionLabel = "Walking";            
            string[] sourceFile = lines;
            string destinationFile = @"D:\project\VideoLabelToolSol\VideoLabelTool\output\labled.txt";            
            
            string lineToWrite = null;

            using (StreamWriter writer = new StreamWriter(destinationFile, true))
            {
                for (int i = 0; i < lineByFrame.Count; i++)
                {
                    for (int j = 0; j < lineByFrame[i].Count; j++)
                    {
                        if (Int32.Parse(lineByFrame[i][j].Split(',')[1]) == selectedBBIndex)
                        {
                            lineToWrite = lineByFrame[i][j];
                            writer.WriteLine(lineToWrite + "," + actionLabel);
                        }
                    }
                }
            }

            if (lineToWrite == null)
                throw new InvalidDataException("Line does not exist in " + sourceFile);

        }
    }
}


