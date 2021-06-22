using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace AssignmentShapes
{
    public partial class Form1 : Form
    {
        //Getting the mainMenu for upper tab
        private bool selectSquareStatus = false;
        private bool selectTriangleStatus = false;
        private bool selectCircleStatus = false;
        private int clicknumber = 0;
        private Point one;
        private Point two;
        private Point three;

        //The linked list
        public List<GraphLine> Lines = new List<GraphLine>();

        GraphLine SelectedLine = null;
        MoveInfo Moving = null;
        double xDiff, yDiff, xMid, yMid;   
        bool drawSquare = false;
        bool drawCircle = false;
        bool drawTriangle = false;
        bool selectingFeature = false;
        bool DeleteStatus = false;
        int turnLeft = 0;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            WindowState = FormWindowState.Maximized;
            BackColor = Color.White;

            MainMenu mainMenu = new MainMenu();
            MenuItem createItem = new MenuItem();
            MenuItem selectItem = new MenuItem();
            MenuItem moveItem = new MenuItem();
            MenuItem rotateItem1 = new MenuItem();
            MenuItem deleteShape = new MenuItem();
            MenuItem exitItem = new MenuItem();
            MenuItem squareItem = new MenuItem();
            MenuItem triangleItem = new MenuItem();
            MenuItem circleItem = new MenuItem(); //--------------------------Added circle item

            createItem.Text = "&Create";
            squareItem.Text = "&Square";
            triangleItem.Text = "&Triangle";
            exitItem.Text = "&Exit";
            circleItem.Text = "&Circle"; //-----------------------------------Added circle name
            selectItem.Text = "&Select";
            moveItem.Text = "&Move";
            rotateItem1.Text = "&Rotate 20° to right";
            deleteShape.Text = "&Delete last shape";

            mainMenu.MenuItems.Add(createItem);
            mainMenu.MenuItems.Add(selectItem);
            mainMenu.MenuItems.Add(moveItem);
            mainMenu.MenuItems.Add(rotateItem1);
            mainMenu.MenuItems.Add(deleteShape);
            mainMenu.MenuItems.Add(exitItem);
            createItem.MenuItems.Add(squareItem);
            createItem.MenuItems.Add(triangleItem);
            createItem.MenuItems.Add(circleItem); 

            exitItem.Click += new EventHandler(this.exitItem);
            squareItem.Click += new EventHandler(selectSquare);
            triangleItem.Click += new EventHandler(selectTriangle);
            circleItem.Click += new EventHandler(selectCircle); 
            selectItem.Click += new EventHandler(triggerSelect);
            moveItem.Click += new EventHandler(triggerMove);
            rotateItem1.Click += new EventHandler(triggerRotateLeft);
            deleteShape.Click += new EventHandler(triggerDeleteLast);

            DoubleBuffered = true ;
            Paint += new PaintEventHandler(LineMover_Paint);
            MouseMove += new MouseEventHandler(LineMover_MouseMove);
            MouseDown += new MouseEventHandler(LineMover_MouseDown);
            MouseUp += new MouseEventHandler(LineMover_MouseUp);

            //This is the linked list that set aldy
            Menu = mainMenu;
            MouseClick += mouseClick;
        }

        void LineMover_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //Showing out the Final drawing lines
            foreach (var line in Lines.ToList())
            {
                var color = line == SelectedLine ? Color.Red : Color.Black;
                if (DeleteStatus == true)
                {
                    Lines.Remove(SelectedLine);
                    DeleteStatus = false;
                }
                if(drawSquare == true && line.ItemShapes == "Square")
                {
                    // calculate ranges and mid points
                    xDiff = line.StartPoint.X - line.EndPoint.X;
                    yDiff = line.StartPoint.Y - line.EndPoint.Y;
                    xMid = (line.StartPoint.X + line.EndPoint.X) / 2;
                    yMid = (line.StartPoint.Y + line.EndPoint.Y) / 2;

                    // draw square
                    PointF firstP = new PointF();
                    PointF secondP = new PointF();
                    PointF thirdP = new PointF();
                    PointF fourthP = new PointF();

                    firstP.X = line.StartPoint.X;
                    firstP.Y = line.StartPoint.Y;
                    secondP.X = (int)(xMid - yDiff / 2);
                    secondP.Y = (int)(yMid + xDiff / 2);
                    thirdP.X = line.EndPoint.X;
                    thirdP.Y = line.EndPoint.Y;
                    fourthP.X = (int)(xMid + yDiff / 2);
                    fourthP.Y = (int)(yMid - xDiff / 2);

                    Point center = new Point();
                    center.X = (int)(firstP.X + thirdP.X) / 2;
                    center.Y = (int)(firstP.Y + thirdP.Y) / 2;

                    e.Graphics.DrawLine(new Pen(color, 3), firstP.X, firstP.Y, secondP.X, secondP.Y); //Top line 
                    e.Graphics.DrawLine(new Pen(color, 3), thirdP.X, thirdP.Y, fourthP.X, fourthP.Y); //Bottom Last line 
                    e.Graphics.DrawLine(new Pen(color, 3), fourthP.X, fourthP.Y, firstP.X, firstP.Y); //Left line 
                    e.Graphics.DrawLine(new Pen(color, 3), secondP.X, secondP.Y, thirdP.X, thirdP.Y); //Right Line
                }
                else
                {
                    if(drawCircle == true && line.ItemShapes == "Circle")
                    {
                        Rectangle rect = new Rectangle((int)line.StartPoint.X, (int)line.StartPoint.Y, (int)line.EndPoint.X - (int)line.StartPoint.X, (int)line.EndPoint.Y - (int)line.StartPoint.Y);
                        e.Graphics.DrawEllipse(new Pen(color, 3), rect);
                    }
                    else
                    {
                        if(drawTriangle == true && line.ItemShapes == "Triangle")
                        {
                            PointF point1 = new PointF((int)line.StartPoint.X, (int)line.StartPoint.Y);
                            PointF point2 = new PointF((int)line.MidPoint.X, (int)line.MidPoint.Y);
                            PointF point3 = new PointF((int)line.EndPoint.X, (int)line.EndPoint.Y);
                            
                            PointF[] curvePoints =
                            {
                                point1,
                                point2,
                                point3,
                            };
                            e.Graphics.DrawPolygon(new Pen(color, 3), curvePoints);
                        }
                    }
                    
                }
            }
        }

        void LineMover_MouseUp(object sender, MouseEventArgs e)
        {
            if (Moving != null)
            {
                Capture = false;
                Moving = null;
            }
            RefreshLineSelection(e.Location);
        }

        void LineMover_MouseDown(object sender, MouseEventArgs e)
        {
            RefreshLineSelection(e.Location);

            if (SelectedLine != null && Moving == null)
            {
                Point center = new Point();
                center.X = (int)(SelectedLine.StartPoint.X + SelectedLine.EndPoint.X) / 2;
                center.Y = (int)(SelectedLine.StartPoint.Y + SelectedLine.EndPoint.Y) / 2;
                int angle = SelectedLine.ItemShapes == "Square" ? 20 : 90;

                Tmatrix testMatrix = new Tmatrix(angle, SelectedLine.StartPoint, center);
                Tmatrix testMatrix1 = new Tmatrix(angle, SelectedLine.EndPoint, center);                
                Tmatrix testMatrix3 = new Tmatrix(angle, SelectedLine.MidPoint, center);
                Capture = true;

                if(turnLeft != 0)
                {
                    Moving = new MoveInfo
                    {
                        Line = SelectedLine,
                        StartLinePoint = testMatrix.getterPoint(),
                        MidLinePoint = testMatrix3.getterPoint(),
                        EndLinePoint = testMatrix1.getterPoint(),
                        StartMoveMousePoint = e.Location
                    };
                    turnLeft = 0;
                }
                else
                {
                    Moving = new MoveInfo
                    {
                        Line = SelectedLine,
                        StartLinePoint = SelectedLine.StartPoint,
                        MidLinePoint = SelectedLine.MidPoint,
                        EndLinePoint = SelectedLine.EndPoint,
                        StartMoveMousePoint = e.Location
                    };   
                }
            }
            RefreshLineSelection(e.Location);
        }

        void LineMover_MouseMove(object sender, MouseEventArgs e)
        {
            if (Moving != null)
            {
                Moving.Line.StartPoint = new PointF(Moving.StartLinePoint.X + e.X - Moving.StartMoveMousePoint.X, Moving.StartLinePoint.Y + e.Y - Moving.StartMoveMousePoint.Y);
                Moving.Line.MidPoint = new PointF(Moving.MidLinePoint.X + e.X - Moving.StartMoveMousePoint.X, Moving.MidLinePoint.Y + e.Y - Moving.StartMoveMousePoint.Y);
                Moving.Line.EndPoint = new PointF(Moving.EndLinePoint.X + e.X - Moving.StartMoveMousePoint.X, Moving.EndLinePoint.Y + e.Y - Moving.StartMoveMousePoint.Y);
            }

            if(selectingFeature == false)
            {
                RefreshLineSelection(e.Location);
            }
        }

        private void RefreshLineSelection(Point point)
        {
            var selectedLine = FindLineByPoint(Lines, point);
            if (selectedLine != SelectedLine)
            {
                SelectedLine = selectedLine;
                Invalidate();
            }
            if (Moving != null)
                Invalidate();

            Cursor =
                Moving != null ? Cursors.Hand :
                SelectedLine != null ? Cursors.SizeAll :
                  Cursors.Default;
        }

        static GraphLine FindLineByPoint(List<GraphLine> lines, Point p)
        {
            var size = 10;
            var buffer = new Bitmap(size * 2, size * 2);

            foreach (var line in lines)
            {
                double xDiff = line.StartPoint.X - line.EndPoint.X;
                double yDiff = line.StartPoint.Y - line.EndPoint.Y;
                double xMid = (line.StartPoint.X + line.EndPoint.X) / 2;
                double yMid = (line.StartPoint.Y + line.EndPoint.Y) / 2;

                using (var g = Graphics.FromImage(buffer))
                {
                    g.Clear(Color.Black);
                    g.DrawLine(new Pen(Color.Green, 20), line.StartPoint.X - p.X + size, line.StartPoint.Y - p.Y + size, line.EndPoint.X - p.X + size, line.EndPoint.Y - p.Y + size);
                }

                if (buffer.GetPixel(size, size).ToArgb() != Color.Black.ToArgb())
                return line;
            }
            return null;
        }

        private void selectSquare(object sender, EventArgs e)
        {
            selectSquareStatus = true;
            MessageBox.Show("Click OK and then click once each at two locations to create a square");
        }

        private void selectTriangle(object sender, EventArgs e)  
        {
            selectTriangleStatus = true;
        }

        private void selectCircle(object sender, EventArgs e)  
        {
            selectCircleStatus = true;
            MessageBox.Show("Click OK and then click once each at two locations to create a circle");
        }

        private void exitItem(object sender, EventArgs e)
        {

            MessageBox.Show("Programe Exited..           ");
            this.Close();
        }

        private void triggerSelect(object sender, EventArgs e)
        {
            selectingFeature = true;
        }

        private void triggerMove(object sender, EventArgs e)
        {
            selectingFeature = false;
        }

        private void triggerRotateLeft(object sender, EventArgs e)
        {
            turnLeft = 20;
        }

        private void triggerDeleteLast(object sender, EventArgs e)
        {
            DeleteStatus = true;
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            //This is the select method
            RefreshLineSelection(e.Location);

            if (e.Button == MouseButtons.Left)
            {
                if (selectSquareStatus == true)
                {
                    if (clicknumber == 0)
                    {
                        //timer.Start();
                        one = new Point(e.X, e.Y);
                        clicknumber = 1;
                        drawSquare = true;
                    }
                    else
                    {
                        two = new Point(e.X, e.Y);
                        clicknumber = 0;
                        selectSquareStatus = false;

                        Graphics g = CreateGraphics();
                        Pen blackpen = new Pen(Color.Black);
                        Square aShape = new Square(one, two);
                        aShape.draw(g, blackpen);

                        //Linked list adding
                        Lines.Add(new GraphLine(one.X, one.Y, two.X, two.Y, 0, 0, "Square"));
                    }
                }
                else
                {
                    if (selectCircleStatus == true)
                    {
                        if (clicknumber == 0)
                        {
                            one = new Point(e.X, e.Y);
                            clicknumber = 1;
                            drawCircle = true;
                        }
                        else
                        {
                            two = new Point(e.X, e.Y);
                            clicknumber = 0;
                            selectCircleStatus = false;

                            Graphics g = CreateGraphics();
                            Pen blackpen = new Pen(Color.Black); 
                            Circle aShape = new Circle(one, two);
                            aShape.draw(g, blackpen);

                            Lines.Add(new GraphLine(one.X, one.Y, two.X, two.Y, 0, 0, "Circle"));
                        }
                    }

                    else
                    {
                        if (selectTriangleStatus == true)     
                        {
                            if (clicknumber == 0)
                            {
                                drawTriangle = true;
                                one = new Point(e.X, e.Y);
                                clicknumber = 1;
                            }
                            else if (clicknumber == 1)
                            {
                                two = new Point(e.X, e.Y);
                                clicknumber = 2;
                            }
                            else
                            {
                                three = new Point(e.X, e.Y);
                                clicknumber = 0;
                                selectTriangleStatus = false;

                                Graphics g = CreateGraphics();
                                Pen blackpen = new Pen(Color.Black);
                                Triangle aShape = new Triangle(one, two, three);
                                aShape.draw(g, blackpen);

                                //Add linked list
                                Lines.Add(new GraphLine(one.X, one.Y, two.X, two.Y, three.X, three.Y, "Triangle"));
                            }

                        }
                    }
                }
            }
            
        }

        //The rubberBanding Part
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = CreateGraphics();
            Pen blackpen = new Pen(Color.Black);
            Point drag = new Point(e.X, e.Y);

            if (selectSquareStatus == true && clicknumber == 1)
            {
                Square aShape = new Square(one, drag);
                aShape.draw(g, blackpen);
                Refresh();
                aShape.draw(g, blackpen);
            }
            else
            {
                if (selectCircleStatus == true && clicknumber == 1)
                {
                    Circle aShape = new Circle(one, drag);
                    aShape.draw(g, blackpen);
                    Refresh();
                    aShape.draw(g, blackpen);
                }
                else
                {
                    //Or change the whole thing to the manual shit line
                    //Currently not working
                    if (selectTriangleStatus == true && clicknumber == 1)
                    {
                        g.DrawLine(blackpen, one, drag);
                        Refresh();
                        g.DrawLine(blackpen, one, drag);
                    }
                    else
                    {
                        if (selectTriangleStatus == true && clicknumber == 2)
                        {
                            //Drawing line with rubber banding
                            g.DrawLine(blackpen, one, drag);
                            g.DrawLine(blackpen, one, two);
                            g.DrawLine(blackpen, two, drag);
                            Refresh();
                            g.DrawLine(blackpen, one, drag);
                            g.DrawLine(blackpen, one, two);
                            g.DrawLine(blackpen, two, drag);
                        }
                    }
                }
            }

        }
    }

    public class MoveInfo
    {
        public GraphLine Line;
        public PointF StartLinePoint;
        public PointF MidLinePoint;
        public PointF EndLinePoint;
        public Point StartMoveMousePoint;
        public String ItemShapes;
    }
    public class GraphLine
    {
        public GraphLine(float x1, float y1, float x2, float y2, float x3, float y3, String shapes)
        {
            StartPoint = new PointF(x1, y1);
            EndPoint = new PointF(x2, y2);
            MidPoint = new PointF(x3, y3); 
            ItemShapes = shapes;
        }
        public PointF StartPoint;
        public PointF MidPoint;
        public PointF EndPoint;
        public String ItemShapes;
    }

    abstract class Shape
    {
        public Shape()   // constructor
        {
        }

        public static void Main()
        {
            Application.Run(new Form1());
        }
    }
}


