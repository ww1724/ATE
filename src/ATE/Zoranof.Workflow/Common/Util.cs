using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Zoranof.Workflow.Common
{
    public static class Util
    {
        public static double GetDistanceOfP2P(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X)*(p1.X - p2.X) + (p1.Y - p2.Y)*(p1.Y - p2.Y));
        }

        public static double DistanceFromPointToLine(Point point, Point start, Point end)
        {
            double a = end.Y - start.Y;
            double b = start.X - end.X;
            double c = end.X * start.Y - start.X * end.Y;
            double distance = Math.Abs(a * point.X + b * point.Y + c) / Math.Sqrt(a * a + b * b);
            return distance;
        }

        public static bool IsPointNearToLine(this Point point, Point start, Point end, double threshold)
        {
            double distance = DistanceFromPointToLine(point, start, end);
            if (distance < threshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static double Distance(this Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((b.X - a.X), 2) + Math.Pow((b.Y - a.Y), 2)); ;
        }


    }
}
