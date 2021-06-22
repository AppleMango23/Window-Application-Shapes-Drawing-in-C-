using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentShapes
{
    class Circle : Shape
    {
        //This class contains the specific details for a square defined in terms of opposite corners
        Point keyPt, oppPt;      // these points identify opposite corners of the square

        public Circle(Point keyPt, Point oppPt)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }


        public void draw(Graphics g, Pen blackPen)
        {

            Rectangle rect = new Rectangle(keyPt.X, keyPt.Y, oppPt.X - keyPt.X, oppPt.Y - keyPt.Y);
            g.DrawEllipse(blackPen, rect);
        }

        
    }
}
