using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoLabelTool
{
    public class Keypoint
    {
        public double x;
        public double y;
        public double visibility;

        public Keypoint(double X, double Y, double Visibility)
        {
            this.x = X;
            this.y = Y;
            this.visibility = Visibility;

            //this.X = X;
            //this.Y = Y;
            //this.Visibility = Visibility;
        }

        //public double X { get; set; }
        //public double Y { get; set; }
        //public double Visibility { get; set; }
    }

    public class Keypoints : IEnumerable
    {
        public Keypoint[] pose;
        public Keypoints(double x1, double y1, double v1, 
                         double x2, double y2, double v2,
                         double x3, double y3, double v3,
                         double x4, double y4, double v4,
                         double x5, double y5, double v5,
                         double x6, double y6, double v6,
                         double x7, double y7, double v7,
                         double x8, double y8, double v8,
                         double x9, double y9, double v9,
                         double x10, double y10, double v10,
                         double x11, double y11, double v11,
                         double x12, double y12, double v12,
                         double x13, double y13, double v13,
                         double x14, double y14, double v14,
                         double x15, double y15, double v15,
                         double x16, double y16, double v16,
                         double x17, double y17, double v17
                        )
        {
            pose = new Keypoint[17]
            {
                new Keypoint(x1, y1, v1),
                new Keypoint(x2, y2, v2),
                new Keypoint(x3, y3, v3),
                new Keypoint(x4, y4, v4),
                new Keypoint(x5, y5, v5),
                new Keypoint(x6, y6, v6),
                new Keypoint(x7, y7, v7),
                new Keypoint(x8, y8, v8),
                new Keypoint(x9, y9, v9),
                new Keypoint(x10, y10, v10),
                new Keypoint(x11, y11, v11),
                new Keypoint(x12, y12, v12),
                new Keypoint(x13, y13, v13),
                new Keypoint(x14, y14, v14),
                new Keypoint(x15, y15, v15),
                new Keypoint(x16, y16, v16),
                new Keypoint(x17, y17, v17)
            };
        }

        public IEnumerator GetEnumerator()
        {
            return new MyEnumerator(pose);
        }

        private class MyEnumerator : IEnumerator
        {
            public Keypoint[] pose;
            int position = 1;

            public MyEnumerator(Keypoint[] list)
            {
                pose = list;
            }
            public IEnumerator GetEnumerator()
            {
                return (IEnumerator)this;
            }

            public bool MoveNext()
            {
                position++;
                return (position < pose.Length);
            }

            public void Reset()
            {
                position = 0;
            }

            public object Current
            {
                get
                {
                    try
                    {
                        return pose[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
            }
        }
    }
}
