using System;
using Geometry.Shapes;
using Geometry.Vector;

namespace Geometry
{
    public static class AreaCalculator
    {
        /// <summary>
        /// Heron's formula. Parameters are lengths of sides.
        /// </summary>
        /// <returns>Area of triangle</returns>
        public static double CountTriangleArea(double a, double b, double c)
        {
            var s = (a + b + c) / 2;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        // TODO tests

        public static double CountArea(this Polygon polygon)
        {
            return Math.Abs(AreaPoly2(polygon.Points.Length, polygon.Points)/2);
        }

        /* Returns twice the area of polygon P. */
        internal static double AreaPoly2(int n, Vector2[] p)
        {
            int i;
            double sum = 0;
            for (i = 1; i < n - 1; i++)
                sum += Area2(p[0], p[i], p[i + 1]);
            return sum;
        }

        /*
        Returns twice the signed area of the triangle determined by a,b,c
        positive if a,b,c are oriented ccw, and negative if cw.
        */
        internal static double Area2(Vector2 a, Vector2 b, Vector2 c)
        {
            return (b.X - a.X)*(c.Y - a.Y) -
                   (c.X - a.X)*(b.Y - a.Y);
        }
    }
}
