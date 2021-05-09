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

using System.Diagnostics;
using Xabe.FFmpeg;

namespace VideoLabelTool
{
    public partial class FormFrameCapture : Form
    {
        float TotalFrame;
        int Fps;
        int currentFrameNum;
        int width;
        int height;
        bool resizeImage = false;
        VideoCapture capture;        
        Timer My_Timer = new Timer();  
        int status = 0;
        OpenFileDialog ofd;
        string openedFilePath;       
        int widthPictureBox;
        int heightPictureBox;
        int? rotated = null;
        Bitmap bp;
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
        List<List<Keypoints>> listKeypoints;
        List<int> listPersonIDAssociated = new List<int>();
        List<PersonColor> listPersonColor;

        Font myFont = new Font("Arial", 12, FontStyle.Bold);
        const string message = "You have already labeled this person";
        const string caption = "Warning";

        public class PersonColor
        {
            public int personID { get; set; }
            public Pen pen { get; set; }
        }        
        
        public class Prediction
        {
            public List<float> keypoints { get; set; }
            public List<float> bbox { get; set; }
            public float score { get; set; }
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

        /*  COCO Person Keypoints mapping
         *  "categories": [
            {
                "supercategory": "person",
                "id": 1,
                "name": "person",
                "keypoints": [
                    "nose","left_eye","right_eye","left_ear","right_ear",
                    "left_shoulder","right_shoulder","left_elbow","right_elbow",
                    "left_wrist","right_wrist","left_hip","right_hip",
                    "left_knee","right_knee","left_ankle","right_ankle"
                ],
                "skeleton": [
                    [16,14],[14,12],[17,15],[15,13],[12,13],[6,12],[7,13],[6,7],
                    [6,8],[7,9],[8,10],[9,11],[2,3],[1,2],[1,3],[2,4],[3,5],[4,6],[5,7]
                ]
                 "skeleton_index": [
                    [15,13],[13,11],[16,14],[14,12],[11,12],[5,11],[6,12],[5,6],
                    [5,7],[6,8],[7,9],[8,10],[1,2],[0,1],[0,2],[1,3],[2,4],[3,5],[4,76]
                ]
        
            }
        ]*/


        private void drawPose(PaintEventArgs e, Pen pen, List<FrameObj> listFrames, int frameNum, int personNum, int pointA, int pointB)
        {            
            if (listKeypoints[listFrames[frameNum].frame - 1][personNum].pose[pointA].visibility != 0 && listKeypoints[listFrames[frameNum].frame - 1][personNum].pose[pointB].visibility != 0)
                e.Graphics.DrawLine(pen, new PointF(listKeypoints[listFrames[frameNum].frame - 1][personNum].pose[pointA].x, listKeypoints[listFrames[frameNum].frame - 1][personNum].pose[pointA].y), new PointF(listKeypoints[listFrames[frameNum].frame - 1][personNum].pose[pointB].x, listKeypoints[listFrames[frameNum].frame - 1][personNum].pose[pointB].y));
        }

        private void plotROI(object sender, PaintEventArgs e)
        {
            if (listRec != null)
            {
                string word;
                int currentPersonID;
                if (listRec != null && currentFrameNum < TotalFrame)
                {
                    foreach (Rectangle ret in listRec[currentFrameNum])
                    {
                        var a = (from n in listPersonColor                                     
                                where n.personID == listFrames[currentFrameNum].predictions[listRec[currentFrameNum].IndexOf(ret)].id_
                                select n).FirstOrDefault();

                        e.Graphics.DrawRectangle(a.pen, ret);
                        currentPersonID = listFrames[currentFrameNum].predictions[listRec[currentFrameNum].IndexOf(ret)].id_;
                        word = currentPersonID.ToString();                            
                        word += listAction[currentFrameNum][listRec[currentFrameNum].IndexOf(ret)];

                        // Version: string color is Red
                        e.Graphics.DrawString(word, myFont, Brushes.Red, new Point(ret.X, ret.Y));           

                        // Version: string color is the same with bounding box
                        //e.Graphics.DrawString(word, myFont, new SolidBrush(a.pen.Color), new Point(ret.X, ret.Y));

                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 15, 13);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 16, 14);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 14, 12);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 13, 11);                        
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 11, 12);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 5, 11);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 6, 12);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 5, 6);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 5, 7);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 6, 8);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 7, 9);                        
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 8, 10);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 1, 2);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 0, 1);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 0, 2);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 1, 3);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 2, 4);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 3, 5);
                        drawPose(e, a.pen, listFrames, currentFrameNum, listRec[currentFrameNum].IndexOf(ret), 4, 6);
                    }
                }
                if (listRec != null && currentFrameNum == TotalFrame)
                {
                    foreach (Rectangle ret in listRec[currentFrameNum - 1])
                    {
                        var a = (from n in listPersonColor
                                 where n.personID == listFrames[currentFrameNum - 1].predictions[listRec[currentFrameNum - 1].IndexOf(ret)].id_
                                 select n).FirstOrDefault();

                        e.Graphics.DrawRectangle(a.pen, ret);
                        currentPersonID = listFrames[currentFrameNum - 1].predictions[listRec[currentFrameNum - 1].IndexOf(ret)].id_;
                        word = currentPersonID.ToString();
                        word += listAction[currentFrameNum - 1][listRec[currentFrameNum - 1].IndexOf(ret)];

                        // Version: string color is Red
                        e.Graphics.DrawString(word, myFont, Brushes.Red, new Point(ret.X, ret.Y));

                        // Version: string color is the same with bounding box
                        //e.Graphics.DrawString(word, myFont, new SolidBrush(a.pen.Color), new Point(ret.X, ret.Y));

                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 15, 13);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 16, 14);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 14, 12);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 13, 11);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 11, 12);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 5, 11);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 6, 12);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 5, 6);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 5, 7);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 6, 8);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 7, 9);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 8, 10);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 1, 2);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 0, 1);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 0, 2);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 1, 3);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 2, 4);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 3, 5);
                        drawPose(e, a.pen, listFrames, currentFrameNum - 1, listRec[currentFrameNum - 1].IndexOf(ret), 4, 6);
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

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();
            ofd.Filter = "MP4 files|*.mp4|AVI files|*.avi|All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                openedFilePath = ofd.FileName;

                var FFmpegpath = "C:/ffmpeg/bin";
                FFmpeg.SetExecutablesPath(FFmpegpath, ffmpegExeutableName: "FFmpeg");
                IMediaInfo mInfo = await FFmpeg.GetMediaInfo(openedFilePath);
                rotated = mInfo.VideoStreams.FirstOrDefault().Rotation;                

                capture = new VideoCapture(openedFilePath);                
                m = new Mat();
                capture.Read(m);
                Bitmap bp = m.ToBitmap();
                if (rotated != null && rotated == 180)                    
                    bp.RotateFlip(RotateFlipType.Rotate180FlipX);
                pictureBox1.Image = bp;                

                TotalFrame = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                Fps = (int) capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                width = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
                height = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
                if (width != 1280 && height != 720)
                    resizeImage = true;
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
            if (capture == null || currentFrameNum == TotalFrame - 1)
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
                bp = m.ToBitmap();
                if (rotated != null && rotated == 180)
                    bp.RotateFlip(RotateFlipType.Rotate180FlipX);
                pictureBox1.Image = bp;

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
                bp = m.ToBitmap();                
                if (rotated != null && rotated == 180)
                    bp.RotateFlip(RotateFlipType.Rotate180FlipX);
                pictureBox1.Image = bp;
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

        private float getKeyPoint(List<FrameObj> listFrames, int i, int j, int index)
        {
            if (resizeImage == true)
                return float.Parse(listFrames[i].predictions[j].keypoints[index].ToString(), CultureInfo.InvariantCulture) * 2 / 3;
            else
                return listFrames[i].predictions[j].keypoints[index];
        }

        private void bntLoadLabels_Click(object sender, EventArgs e)
        {            
            ofd = new OpenFileDialog();
            ofd.Filter = "JSON files|*.json|TXT files|*.txt|All files|*";

            int currentFrameNum = 1;
            lineByFrame = new List<List<string>>();
            lineByFrame.Add(new List<string>());
            listRec = new List<List<Rectangle>>();            
            listAction = new List<List<String>>();            
            listPersonColor = new List<PersonColor>();
            listKeypoints = new List<List<Keypoints>>();
            int x;
            int y;
            int weight;
            int height;
            bool semilabeled = false;

            if (ofd.ShowDialog() == DialogResult.OK)            
            {                
                string json = File.ReadAllText(ofd.FileName);                                                                                       
                
                if (json[0] == '[' && json[json.Length - 1] == ']')
                {
                    json = json.Substring(1, json.Length - 2);
                    semilabeled = true;
                }                
                listFrames = addActionFieldToJson(json);
                
                for (int i = 0; i < listFrames.Count; i++)
                {
                    listRec.Add(new List<Rectangle>());
                    listAction.Add(new List<string>());
                    listKeypoints.Add(new List<Keypoints>());

                    for (int j = 0; j < listFrames[i].predictions.Count; j++)
                    {
                        if (resizeImage == false)
                        {
                            x = (int)listFrames[i].predictions[j].bbox[0];
                            y = (int)listFrames[i].predictions[j].bbox[1];
                            weight = (int)listFrames[i].predictions[j].bbox[2];
                            height = (int)listFrames[i].predictions[j].bbox[3];
                        }
                            
                        else
                        {
                            //New version
                            // Different OS has different personalized Setting for number format, this parameter to use uniform number format
                            x = (int)double.Parse(listFrames[i].predictions[j].bbox[0].ToString(), CultureInfo.InvariantCulture) * 2 / 3;
                            y = (int)double.Parse(listFrames[i].predictions[j].bbox[1].ToString(), CultureInfo.InvariantCulture) * 2 / 3;
                            weight = (int)double.Parse(listFrames[i].predictions[j].bbox[2].ToString(), CultureInfo.InvariantCulture) * 2 / 3;
                            height = (int)double.Parse(listFrames[i].predictions[j].bbox[3].ToString(), CultureInfo.InvariantCulture) * 2 / 3;
                        }


                        listRec[listFrames[i].frame - 1].Add(new Rectangle(x, y, weight, height));

                        listKeypoints[listFrames[i].frame - 1].Add(new Keypoints(getKeyPoint(listFrames, i, j, 0), getKeyPoint(listFrames, i, j, 1), getKeyPoint(listFrames, i, j, 2),
                                                              getKeyPoint(listFrames, i, j, 3), getKeyPoint(listFrames, i, j, 4), getKeyPoint(listFrames, i, j, 5),
                                                              getKeyPoint(listFrames, i, j, 6), getKeyPoint(listFrames, i, j, 7), getKeyPoint(listFrames, i, j, 8),
                                                              getKeyPoint(listFrames, i, j, 9), getKeyPoint(listFrames, i, j, 10), getKeyPoint(listFrames, i, j, 11),
                                                              getKeyPoint(listFrames, i, j, 12), getKeyPoint(listFrames, i, j, 13), getKeyPoint(listFrames, i, j, 14),
                                                              getKeyPoint(listFrames, i, j, 15), getKeyPoint(listFrames, i, j, 16), getKeyPoint(listFrames, i, j, 17),
                                                              getKeyPoint(listFrames, i, j, 18), getKeyPoint(listFrames, i, j, 19), getKeyPoint(listFrames, i, j, 20),
                                                              getKeyPoint(listFrames, i, j, 21), getKeyPoint(listFrames, i, j, 22), getKeyPoint(listFrames, i, j, 23),
                                                              getKeyPoint(listFrames, i, j, 24), getKeyPoint(listFrames, i, j, 25), getKeyPoint(listFrames, i, j, 26),
                                                              getKeyPoint(listFrames, i, j, 27), getKeyPoint(listFrames, i, j, 28), getKeyPoint(listFrames, i, j, 29),
                                                              getKeyPoint(listFrames, i, j, 30), getKeyPoint(listFrames, i, j, 31), getKeyPoint(listFrames, i, j, 32),
                                                              getKeyPoint(listFrames, i, j, 33), getKeyPoint(listFrames, i, j, 34), getKeyPoint(listFrames, i, j, 35),
                                                              getKeyPoint(listFrames, i, j, 36), getKeyPoint(listFrames, i, j, 37), getKeyPoint(listFrames, i, j, 38),
                                                              getKeyPoint(listFrames, i, j, 39), getKeyPoint(listFrames, i, j, 40), getKeyPoint(listFrames, i, j, 41),
                                                              getKeyPoint(listFrames, i, j, 42), getKeyPoint(listFrames, i, j, 43), getKeyPoint(listFrames, i, j, 44),
                                                              getKeyPoint(listFrames, i, j, 45), getKeyPoint(listFrames, i, j, 46), getKeyPoint(listFrames, i, j, 47),
                                                              getKeyPoint(listFrames, i, j, 48), getKeyPoint(listFrames, i, j, 49), getKeyPoint(listFrames, i, j, 50)
                                                              ));

                        // Add new pen/color for plotting bounding box to new appeared person. Each person has only a color for all the frames
                        if (!listPersonColor.Any(a => a.personID == listFrames[i].predictions[j].id_))
                        {
                            penTemp = new Pen(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                            penTemp.Width = 3.0F;
                            listPersonColor.Add(new PersonColor { personID = listFrames[i].predictions[j].id_, pen = penTemp });
                        }
                        if (semilabeled == true)                        
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
            if (currentFrameNum < TotalFrame)
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

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to RESET?",
                                     "Warning",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {                
                Application.Restart();
                //Environment.Exit(0);
            }                        
        }

        private void walkingWhileCallingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileCalling");
        }

        private void walkingWhileDrinkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileDrinking");
        }

        private void walkingWhileEatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileEating");
        }

        private void walkingWhileHoldingBabyInArmsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileHoldingBabyInArms");
        }

        private void walkingWhileHoldingCartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileHoldingCart");
        }

        private void walkingWhileHoldingStrollerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileHoldingStroller");
        }

        private void walkingWhileSmokingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileSmoking");
        }

        private void walkingWhileTalkingWithPhoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileTalkingWithPhone");
        }

        private void standingTogetherWhileLookingAtShopsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingTogetherWhileLookingAtShops");
        }

        private void standingTogetherWhileWatchingPhoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("StandingTogetherWhileWatchingPhone");
        }

        private void standingWhileCallingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileCalling");
        }

        private void standingWhileDrinkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileDrinking");
        }

        private void standingWhileEatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("StandingWhileEating");
        }

        private void standingWhileHoldingBabyInArmsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileHoldingBabyInArms");
        }

        private void standingWhileLookingAtShopsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileLookingAtShops");
        }

        private void standingWhileHoldingStrollerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileHoldingStroller");
        }
        private void standingWhileSmokingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileSmoking");
        }

        private void standingWhileTalkingTogetherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileTalkingTogether");
        }

        private void standingWhileTalkingWithPhoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileTalkingWithPhone");
        }

        private void standingWhileWatchingPhoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileWatchingPhone");
        }

        private void standingWhileWatchingPhoneTogetherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileWatchingPhoneTogether");
        }

        private void sittingWhileCallingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileCalling");
        }

        private void sittingWhileDrinkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileDrinking");
        }

        private void sittingWhileEatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileEating");
        }

        private void sittingWhileHoldingBabyInArmsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileHoldingBabyInArms");
        }

        private void sittingWhileTalkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileTalking");
        }

        private void sittingWhileTalkingWithPhoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileTalkingWithPhone");
        }

        private void sittingWhileWatchingPhoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileWatchingPhone");
        }

        private void sittingWhileWatchingPhoneTogetherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileWatchingPhoneTogether");
        }

        private void cleaningFloorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("cleaningFloor");
        }

        private void crouchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("crouching");
        }

        private void fallingDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("fallingDown");
        }

        private void fightingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("fighting");
        }

        private void jumpingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("jumping");
        }

        private void kickingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("kicking");
        }

        private void ridingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("riding");
        }

        private void runningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("running");
        }

        private void scooterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("scooter");
        }

        private void throwingTrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("throwingTrash");
        }

        private void throwingSomethingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            actionAssociate("throwingSomething");
        }
        private void buttonSittingWhileWatchingPhone_Click(object sender, EventArgs e)
        {
            actionAssociate("sittingWhileWatchingPhone");
        }

        private void buttonStandingWhileWatchingPhone_Click(object sender, EventArgs e)
        {
            actionAssociate("standingWhileWatchingPhone");
        }

        private void buttonWalkingWhileWatchingPhone_Click(object sender, EventArgs e)
        {
            actionAssociate("walkingWhileWatchingPhone");
        }

        private void buttonSittingTogether_Click(object sender, EventArgs e)
        {
            actionAssociate("SittingTogether");
        }

        private void buttonStandingTogether_Click(object sender, EventArgs e)
        {
            actionAssociate("StandingTogether");
        }

        private void buttonWalkingTogether_Click(object sender, EventArgs e)
        {
            actionAssociate("WalkingTogether");
        }       
    }
}


