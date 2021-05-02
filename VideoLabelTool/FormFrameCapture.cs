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
using Newtonsoft.Json.Linq;

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

        public class Prediction
        {
            public List<double> keypoints { get; set; }
            public List<double> bbox { get; set; }
            public double score { get; set; }
            public int category_id { get; set; }
            public int id_ { get; set; }
            public string action { get; set; }
        }

        public class FrameObj
        {
            public int frame { get; set; }
            public List<Prediction> predictions { get; set; }
        }
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
        
        private List<FrameObj> addActionFieldToJson(string jsonFile)
        {
            List<dynamic> listFramesFormatted = new List<dynamic>();

            var jsonSerializer = new JsonSerializer();
            var jsonReaderExport = new JsonTextReader(new StringReader(jsonFile))
            {
                SupportMultipleContent = true // This is important for multiple content JSON file reading!
            };
            while (jsonReaderExport.Read())
            {
                listFramesFormatted.Add(jsonSerializer.Deserialize<JObject>(jsonReaderExport));
            }            

            string jsonData = JsonConvert.SerializeObject(listFramesFormatted);
            List<FrameObj> framesAct = JsonConvert.DeserializeObject<List<FrameObj>>(jsonData);

            return framesAct;
        }

        private void bntLoadLabels_Click(object sender, EventArgs e)
        {            
            ofd = new OpenFileDialog();
            int currentFrameNum = 1;
            lineByFrame = new List<List<string>>();
            lineByFrame.Add(new List<string>());
            listRec = new List<List<Rectangle>>();            
            listAction = new List<List<String>>();            
            listPersonColor = new List<PersonColor>();            
            int x;
            int y;
            int weight;
            int height;
            bool semilabled = false;

            if (ofd.ShowDialog() == DialogResult.OK)            
            {                
                string json = File.ReadAllText(ofd.FileName);                                                                                       
                
                if (json[0] == '[' && json[json.Length - 1] == ']')
                {
                    json = json.Substring(1, json.Length - 2);
                    semilabled = true;
                }                
                listFrames = addActionFieldToJson(json);
                
                for (int i = 0; i < listFrames.Count; i++)
                {
                    listRec.Add(new List<Rectangle>());
                    listAction.Add(new List<string>());

                    for (int j = 0; j < listFrames[i].predictions.Count; j++)
                    {                        
                        x = (int)listFrames[i].predictions[j].bbox[0];
                        y = (int)listFrames[i].predictions[j].bbox[1];                        
                        weight = (int)listFrames[i].predictions[j].bbox[2];
                        height = (int)listFrames[i].predictions[j].bbox[3];

                        listRec[listFrames[i].frame - 1].Add(new Rectangle(x, y, weight, height));
                        
                        // Add new pen/color for plotting bounding box to new appeared person. Each person has only a color for all the frames
                        if (!listPersonColor.Any(a => a.personID == listFrames[i].predictions[j].id_))
                        {
                            penTemp = new Pen(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                            penTemp.Width = 3.0F;
                            listPersonColor.Add(new PersonColor { personID = listFrames[i].predictions[j].id_, pen = penTemp });
                        }
                        if (semilabled == true)                        
                            listAction[currentFrameNum - 1].Add(listFrames[i].predictions[j].action);                        
                        else
                            listAction[currentFrameNum - 1].Add(null);
                    }
                    currentFrameNum++;
                }
            }
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
                if (!selectedPersonID.Any(idx => idx == listFrames[currentFrameNum].predictions[spi].id_))
                {                    
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
                    for (int i = 0; i < listFrames.Count; i++)
                    {
                        for (int j = 0; j < listFrames[i].predictions.Count; j++)
                        {                            
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
                        for (int j = 0; j < listFrames[i].predictions.Count; j++)
                        {                     
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
                        for (int j = 0; j < listFrames[i].predictions.Count; j++)
                        {                            
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
            string[] splittedopenedFilePath = openedFilePath.Split('\\');
            string openedFileName = splittedopenedFilePath[splittedopenedFilePath.Length - 1].Split('.')[0];

            SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";            
            sfd.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
            sfd.FileName = "action_" + openedFileName;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName, false))
                {                  
                    for (int i = 0; i < listFrames.Count; i++)
                    {
                        for (int j = 0; j < listFrames[i].predictions.Count; j++)
                        {                      
                            if (listAction[i][j] != null)
                            {                               
                                listFrames[i].predictions[j].action = listAction[i][j];
                            }                                                                
                        }
                    }
                }
            }

            using (StreamWriter sw = File.CreateText(sfd.FileName))
            {
                sw.Write(JsonConvert.SerializeObject(listFrames, Formatting.Indented));
            }
        }

        private void bntWalking_Click(object sender, EventArgs e)
        {
            actionAssociate("walking");
        }        

        private void bntDrinking_Click(object sender, EventArgs e)
        {
            actionAssociate("drinking");
        }        

        private void bntStanding_Click(object sender, EventArgs e)
        {
            actionAssociate("standing");
        }

        private void buttonSitting_Click(object sender, EventArgs e)
        {
            actionAssociate("sitting");
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


