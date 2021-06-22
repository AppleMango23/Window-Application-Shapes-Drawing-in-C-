using System.Drawing;


namespace AssignmentShapes
{
    class Square : Shape
    {
        //This class contains the specific details for a square defined in terms of opposite corners
        Point keyPt, oppPt;      // these points identify opposite corners of the square
        double xDiff, yDiff, xMid, yMid;   // range and mid points of x & y  

        public Square(Point keyPt, Point oppPt)   // constructor
        {
            this.keyPt = keyPt;
            this.oppPt = oppPt;
        }

        // You will need a different draw method for each kind of shape. Note the square is drawn
        // from first principles. All other shapes should similarly be drawn from first principles. 
        // Ideally no C# standard library class or method should be used to create, draw or transform a shape
        // and instead should utilse user-developed code.
        public void draw(Graphics g, Pen blackPen)
        {
            // This method draws the square by calculating the positions of the other 2 corners

            // calculate ranges and mid points
            xDiff = oppPt.X - keyPt.X;
            yDiff = oppPt.Y - keyPt.Y;
            xMid = (oppPt.X + keyPt.X) / 2;
            yMid = (oppPt.Y + keyPt.Y) / 2;

            // draw square
            g.DrawLine(blackPen, keyPt.X, keyPt.Y, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid + yDiff / 2), (int)(yMid - xDiff / 2), oppPt.X, oppPt.Y);
            g.DrawLine(blackPen, oppPt.X, oppPt.Y, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2));
            g.DrawLine(blackPen, (int)(xMid - yDiff / 2), (int)(yMid + xDiff / 2), keyPt.X, keyPt.Y);

        }

        //At this moment i dont know what thing he want to use for the detection
        public double getCoor()
        {
            //This one will return back the coordinates so that it can do something.
            return (xDiff);
        }


    }
}
