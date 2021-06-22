using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AssignmentShapes
{
    class Tmatrix
    {
        static PointF coor;
        static PointF pointestInside;
        static float angle;
        PointF result;

        //Getting the param and setting it up
        public Tmatrix(float angleParam, PointF coorParam, PointF pointest)
        {
            angle = angleParam;
            coor = coorParam;
            pointestInside = pointest;

            //Using this way so that can be return back the points
            result = matrixRotate();
        }

        //Formula running with the parameter that already set in constructor
        public static PointF matrixRotate()
        {

            //This is radiant
            float cosa = (float)Math.Cos(angle * Math.PI / 180.0);
            float sina = (float)Math.Sin(angle * Math.PI / 180.0);

            float X = (coor.X - pointestInside.X) * cosa - (coor.Y - pointestInside.Y) * sina + pointestInside.X; 
            float Y = (coor.X - pointestInside.X) * sina + (coor.Y - pointestInside.Y) * cosa + pointestInside.Y;

            return new PointF(X, Y);
        }

        public PointF getterPoint()
        {
            return result;
        }
    }
}
