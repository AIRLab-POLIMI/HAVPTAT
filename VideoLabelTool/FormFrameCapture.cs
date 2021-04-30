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
using System.Globalization;
using Newtonsoft.Json;

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
        string openedFilePath;
        string[] lines;        
        int widthPictureBox;
        int heightPictureBox;
        public List<int> selectedPersonID = new List<int>();
        public int selectedPersonIDUnique;
        public List<int> selectedPersonIndex = new List<int>();
        int selectedPersonIndexUnique;
        Mat m;
        Pen penTemp;

        private static Random rnd = new Random();
        List<FrameObj> listFrames;
        List<List<Rectangle>> listRec;
        List<List<string>> lineByFrame;
        List<List<string>> listAction;        
        List<int> listPersonIDAssociated = new List<int>();

        public class PersonColor
        {
            public int personID { get; set; }
            public Pen pen { get; set; }
        }
        List<PersonColor> listPersonColor;

        Font myFont = new Font("Arial", 14);
        const string message = "You have already labeled this person";
        const string caption = "Warning";        

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
                //if (listRec.Count == lineByFrame.Count)
                //{
                    string word;
                    if (listRec != null && currentFrameNum < TotalFrame)
                    {
                        foreach (Rectangle ret in listRec[currentFrameNum])
                        {
                        var a = (from n in listPersonColor                                     
                                 where n.personID == listFrames[currentFrameNum].predictions[listRec[currentFrameNum].IndexOf(ret)].id_
                                 select n).FirstOrDefault();

                            e.Graphics.DrawRectangle(a.pen, ret);                            
                            word = listFrames[currentFrameNum].predictions[listRec[currentFrameNum].IndexOf(ret)].id_.ToString();                            
                            word += listAction[currentFrameNum][listRec[currentFrameNum].IndexOf(ret)];
                            
                            // Version: string color is Red
                            e.Graphics.DrawString(word, myFont, Brushes.Red, new Point(ret.X, ret.Y));
                            
                            // Version: string color is the same with bounding box
                            //e.Graphics.DrawString(word, myFont, new SolidBrush(a.pen.Color), new Point(ret.X, ret.Y));
                        }
                    }
                //}
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
                openedFilePath = ofd.FileName;
                capture = new VideoCapture(openedFilePath);
                m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.ToBitmap();

                TotalFrame = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = (int) capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                My_Timer.Interval = 1000 / Fps;
                My_Timer.Tick += new EventHandler(My_Timer_Tick);
                counterFrame.Text = (currentFrameNum).ToString() + '/' + (TotalFrame - 1).ToString();

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
            if (currentFrameNum < TotalFrame)
            {
                // SetCaptureProperty could slow down, but avoid crash
                counterFrame.Text = (currentFrameNum).ToString() + '/' + (TotalFrame - 1).ToString();
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
                m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.ToBitmap();
                currentFrameNum += 1;
                
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
                currentFrameNum += 1;
                setFrame(currentFrameNum);

                this.Invalidate();      
            }

            else
            {             
                return;
            }

            this.bntPrevFrame.Enabled = true;
            status = 0;

            if (currentFrameNum > this.nudStart.Value)
                this.nudEnd.Value = currentFrameNum;
            else
            {
                this.nudStart.Value = currentFrameNum;
                this.nudEnd.Value = currentFrameNum + 1;         
            }
        }

        private void bntPrevFrame_Click(object sender, EventArgs e)
        {
            PreviousFrame();
        }

        private void PreviousFrame()
        {
            if (currentFrameNum > 0 && currentFrameNum <= TotalFrame && capture != null)
            {                
                currentFrameNum -= 1;
                setFrame(currentFrameNum);
            }            
            status = 0;

            if (currentFrameNum > this.nudStart.Value)
                this.nudEnd.Value = currentFrameNum;
            else
            {
                this.nudStart.Value = currentFrameNum;
                this.nudEnd.Value = currentFrameNum + 1;
            }
        }

        private void buttonFirstFrame_Click(object sender, EventArgs e)
        {
            setFrame(0);
            currentFrameNum = 0;
        }

        private void buttonLastFrame_Click(object sender, EventArgs e)
        {
            setFrame((int)TotalFrame - 1);
            currentFrameNum = (int)TotalFrame - 1;
        }

        private void setFrame(int currentFrameNum)
        {
            // SetCaptureProperty could slow down, but avoid crash
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrameNum);
            try
            {
                //To avoid CRASH: Remove capture.QueryFrame()
                //pictureBox1.Image = capture.QueryFrame().ToBitmap();

                // Replaced by capture.Read(m)
                m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.ToBitmap();
            }
            catch (NullReferenceException e)
            {
                throw new NullReferenceException(e.Message);
            }

            counterFrame.Text = (currentFrameNum).ToString() + '/' + (TotalFrame - 1).ToString();
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
        
        public class Prediction
        {
            public List<double> keypoints { get; set; }
            public List<double> bbox { get; set; }
            public double score { get; set; }
            public int category_id { get; set; }
            public int id_ { get; set; }
        }

        public class FrameObj
        {
            public int frame { get; set; }
            //public List<int> predictions { get; set; }
            public List<Prediction> predictions { get; set; }
        }

        private void bntLoadJsonLabels_Click()
        {
            /*Single-Content JSON file*/
            //string json = File.ReadAllText(@"C:\\Users\\quan\\Downloads\\prova1.json");
            //FrameObj frames = JsonConvert.DeserializeObject<FrameObj>(json);
            //Debug.WriteLine(frames.predictions[0].score);

            /*Multiple-Content JSON file*/
            listFrames = new List<FrameObj>();
            string json = File.ReadAllText(@"C:\\Users\\quan\\Downloads\\OpenPifPaf_PoseTrack.json");
            var jsonReader = new JsonTextReader(new StringReader(json))
            {
                SupportMultipleContent = true // This is important!
            };

            var jsonSerializer = new JsonSerializer();
            while (jsonReader.Read())
            {
                listFrames.Add(jsonSerializer.Deserialize<FrameObj>(jsonReader));
            }            
        }

        private void bntLoadLabels_Click(object sender, EventArgs e)
        {
            //bntLoadJsonLabels_Click();

            ofd = new OpenFileDialog();
            int currentFrameNum = 1;
            lineByFrame = new List<List<string>>();
            lineByFrame.Add(new List<string>());

            listRec = new List<List<Rectangle>>();            

            listAction = new List<List<String>>();
            //listAction.Add(new List<string>());

            listPersonColor = new List<PersonColor>();

            String[] words;
            int x;
            int y;
            int weight;
            int height;

            //if (ofd.ShowDialog() == DialogResult.OK)
            if (true)
            {
                string json = File.ReadAllText(@"C:\\Users\\quan\\Downloads\\OpenPifPaf_PoseTrack.json");
                listFrames = new List<FrameObj>();
                
                var jsonReader = new JsonTextReader(new StringReader(json))
                {
                    SupportMultipleContent = true // This is important!
                };

                var jsonSerializer = new JsonSerializer();
                while (jsonReader.Read())
                {
                    listFrames.Add(jsonSerializer.Deserialize<FrameObj>(jsonReader));
                }
                
                foreach (FrameObj fobj in listFrames)
                {
                    listRec.Add(new List<Rectangle>());
                    listAction.Add(new List<string>());

                    foreach (Prediction person in fobj.predictions)
                    {                        
                        x = (int) person.bbox[0];
                        y = (int) person.bbox[1];                        
                        weight = (int) person.bbox[2];
                        height = (int) person.bbox[3];

                        listRec[fobj.frame - 1].Add(new Rectangle(x, y, weight, height));
                        
                        // Add new pen/color for plotting bounding box to new appeared person. Each person has only a color for all the frames
                        if (!listPersonColor.Any(a => a.personID == person.id_))
                        {
                            penTemp = new Pen(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                            penTemp.Width = 3.0F;
                            listPersonColor.Add(new PersonColor { personID = person.id_, pen = penTemp });
                        }

                        listAction[currentFrameNum - 1].Add(null);

                        //if (words.Length == 11)
                        //{
                        //    // Already have some person in some frames labeled 
                        //    listAction[currentFrameNum - 1].Add(words[10]);
                        //}
                        //else
                        //{
                        //    listAction[currentFrameNum - 1].Add(null);
                        //}
                    }
                    currentFrameNum++;
                }
            }

            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    lines = System.IO.File.ReadAllLines(@ofd.FileName);

            //    foreach (string line in lines)
            //    {
            //        words = line.Split(',');

            //        // Original version
            //        //x = (int)(Convert.ToDouble(words[2]) * 2 / 3);
            //        //y = (int)(Convert.ToDouble(words[3]) * 2 / 3);
            //        //weight = (int)(Convert.ToDouble(words[4]) * 2 / 3);
            //        //height = (int)(Convert.ToDouble(words[5]) * 2 / 3);

            //        //New version
            //        // Different OS has different personalized Setting for number format, this parameter to use uniform number format
            //        x = (int)double.Parse(words[2], CultureInfo.InvariantCulture) * 2 / 3;
            //        y = (int)double.Parse(words[3], CultureInfo.InvariantCulture) * 2 / 3;
            //        weight = (int)double.Parse(words[4], CultureInfo.InvariantCulture) * 2 / 3;
            //        height = (int)double.Parse(words[5], CultureInfo.InvariantCulture) * 2 / 3;

            //        if (Int32.Parse(words[0]) != currentFrameNum)
            //        {
            //            currentFrameNum++;
            //            lineByFrame.Add(new List<string>());
            //            listRec.Add(new List<Rectangle>());
            //            listAction.Add(new List<string>());
            //        }
            //        lineByFrame[currentFrameNum - 1].Add(line);
            //        listRec[currentFrameNum - 1].Add(new Rectangle(x, y, weight, height));

            //        // Add new pen/color for plotting bounding box to new appeared person. Each person has only a color for all the frames
            //        if (!listPersonColor.Any(a => a.personID == Int32.Parse(words[1])))
            //        {
            //            penTemp = new Pen(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            //            penTemp.Width = 3.0F;
            //            listPersonColor.Add(new PersonColor { personID = Int32.Parse(words[1]), pen = penTemp });
            //        }

            //        if (words.Length == 11)
            //        {
            //            // Already have some person in some frames labeled 
            //            listAction[currentFrameNum - 1].Add(words[10]);
            //        }
            //        else
            //        {
            //            listAction[currentFrameNum - 1].Add(null);
            //        }
            //    }
            //}
        }                

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            foreach (Rectangle r in listRec[currentFrameNum])
                if (r.Contains(e.Location) && !selectedPersonIndex.Any(idx => idx == listRec[currentFrameNum].IndexOf(r)))
                {
                    // enter only if the index does not exist in selectedPersonIndex to ensure no duplicated value is inserted
                    selectedPersonIndex.Add(listRec[currentFrameNum].IndexOf(r));
                    
                }
            foreach (int spi in selectedPersonIndex)
            {
                // Through "selectedPersonIndex" list to get "selectedPersonID" list
                //if (!selectedPersonID.Any(idx => idx == Int32.Parse(lineByFrame[currentFrameNum][spi].Split(',')[1])))
                if (!selectedPersonID.Any(idx => idx == listFrames[currentFrameNum].predictions[spi].id_))
                {
                    //selectedPersonID.Add(Int32.Parse(lineByFrame[currentFrameNum][spi].Split(',')[1]));
                    selectedPersonID.Add(listFrames[currentFrameNum].predictions[spi].id_);
                    Console.WriteLine("You have hit Rectangle Person ID.: " + selectedPersonID[selectedPersonID.Count - 1]);
                }
            }            
        }

        private void actionAssociate(string actionLabel)
        {
            if (selectedPersonID.Count > 1)
            {
                FormSelection formPopup = new FormSelection(this);
                formPopup.ShowDialog(this);
                //selectedPersonIndexUnique = lineByFrame[currentFrameNum].FindIndex(a => Int32.Parse(a.Split(',')[1]) == selectedPersonIDUnique);
                selectedPersonIndexUnique = listFrames[currentFrameNum].predictions.FindIndex(a => a.id_ == selectedPersonIDUnique);
            }
            else if (selectedPersonID.Count == 0)
            {
                MessageBox.Show("You should select a person to be labeled.", "Warning");
                return;
            }            
            else
            {
                selectedPersonIDUnique = selectedPersonID[0];
                selectedPersonIndexUnique = selectedPersonIndex[0];
            }

            if (!listPersonIDAssociated.Contains(selectedPersonIDUnique))
            {
                if (this.cbInter.Checked == false)
                {
                    for (int i = 0; i < lineByFrame.Count; i++)
                    {
                        for (int j = 0; j < lineByFrame[i].Count; j++)
                        {
                            //if (Int32.Parse(lineByFrame[i][j].Split(',')[1]) == selectedPersonIDUnique)
                            if (listFrames[i].predictions[j].id_ == selectedPersonIDUnique)
                            {
                                listAction[i][j] = actionLabel;
                            }
                        }
                    }
                    listPersonIDAssociated.Add(selectedPersonIDUnique);
                }

                else
                {
                    for (int i = (int) nudStart.Value; i <= (int)nudEnd.Value; i++)
                    {
                        //for (int j = 0; j < lineByFrame[i].Count; j++)
                        for (int j = 0; j < listFrames[i].predictions.Count; j++)
                        {
                            //if (Int32.Parse(lineByFrame[i][j].Split(',')[1]) == selectedPersonIDUnique)
                            if (listFrames[i].predictions[j].id_ == selectedPersonIDUnique)
                            {
                                listAction[i][j] = actionLabel;
                            }
                        }
                    }                    
                }                
            }
            else
            {
                if (this.cbInter.Checked == false)
                {
                    listAction[currentFrameNum][selectedPersonIndexUnique] = actionLabel;
                }
                else
                {
                    for (int i = (int)nudStart.Value; i <= (int)nudEnd.Value; i++)
                    {
                        //for (int j = 0; j < lineByFrame[i].Count; j++)
                        for (int j = 0; j < listFrames[i].predictions.Count; j++)
                        {
                            //if (Int32.Parse(lineByFrame[i][j].Split(',')[1]) == selectedPersonIDUnique)
                            if (listFrames[i].predictions[j].id_ == selectedPersonIDUnique)
                            {
                                listAction[i][j] = actionLabel;
                            }
                        }
                    }
                }
            }

            selectedPersonID.Clear();
            selectedPersonIndex.Clear();
        }        

        private void bntExport_Click(object sender, EventArgs e)
        {
            string lineToWrite = null;
            string[] splittedLine;
            string[] splittedopenedFilePath = openedFilePath.Split('\\');
            string openedFileName = splittedopenedFilePath[splittedopenedFilePath.Length - 1].Split('.')[0];

            SaveFileDialog sfd = new SaveFileDialog();            
            sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";            
            sfd.FileName = "AL_" + openedFileName;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName, false))
                {
                    for (int i = 0; i < lineByFrame.Count; i++)
                    {
                        for (int j = 0; j < lineByFrame[i].Count; j++)
                        {
                            splittedLine = lineByFrame[i][j].Split(',');
                            if (listAction[i][j] != null && splittedLine.Length != 11)
                            {
                                lineToWrite = lineByFrame[i][j] + "," + listAction[i][j];
                            }
                            else if (listAction[i][j] != null && splittedLine.Length == 11 && splittedLine[10] != listAction[i][j])
                            {                                
                                splittedLine[10] = listAction[i][j];
                                lineToWrite = String.Join(",", splittedLine);
                            }
                            else
                            {
                                lineToWrite = lineByFrame[i][j];
                            }

                            writer.WriteLine(lineToWrite);
                        }
                    }
                }
            }
        }

        private void bntWalking_Click(object sender, EventArgs e)
        {
            actionAssociate("Walking");
        }        

        private void bntDrinking_Click(object sender, EventArgs e)
        {
            actionAssociate("Drinking");
        }        

        private void bntStanding_Click(object sender, EventArgs e)
        {
            actionAssociate("Standing");
        }

        private void buttonSitting_Click(object sender, EventArgs e)
        {
            actionAssociate("Sitting");
        }

        private void cbInter_CheckedChanged(object sender, EventArgs e)
        {
            if (cbInter.Checked == true)
            {
                this.nudStart.Enabled = true;
                this.nudEnd.Enabled = true;
                this.nudStart.Show();
                this.nudEnd.Show();
                this.labelTo.Show();
                this.nudStart.Value = currentFrameNum;
                this.nudEnd.Value = currentFrameNum + 1;
            }
            else
            {
                this.nudStart.Enabled = false;
                this.nudEnd.Enabled = false;
                this.labelTo.Hide();
                this.nudStart.Hide();
                this.nudEnd.Hide();                
            }
        }

        
    }
}


