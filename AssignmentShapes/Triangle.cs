using System.Drawing;

namespace AssignmentShapes
{
    class Triangle : Shape
    {
        //This class contains the specific details for a square defined in terms of opposite corners
        Point keyPt, oppPt, last;      // these points identify opposite corners of the square

        public Triangle(Point keyPt, Point oppPt, Point last)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
            this.last = last;
        }

        public void draw(Graphics g, Pen blackPen)
        {
            // Create points that define polygon.
            PointF point1 = new PointF(keyPt.X, keyPt.Y);
            PointF point2 = new PointF(oppPt.X, oppPt.Y);
            PointF point3 = new PointF(last.X, last.Y);

            PointF[] curvePoints =
            {
                point1,
                point2,
                point3,
            };

            // Draw polygon curve to screen.
            g.DrawPolygon(blackPen, curvePoints);
        }


    }
}
