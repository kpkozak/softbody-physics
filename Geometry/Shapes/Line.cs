using System;
using Geometry.Vector;

namespace Geometry.Shapes
{
    public class Line
    {
        // TODO Unit tests dla linii
        // TODO postaæ ogólna a nie kierunkowa. Hax na pionowa
        // TODO Equality members
        public double A { get; }
        public double B { get; }

        /// <summary>
        /// Returns direction versor
        /// </summary>
        public Vector2 Direction
        {
            get
            {
                if (Double.IsNaN(A))
                    return new Vector2(0, 1);
                return new Vector2(1, A).Normalize(); 
            }
        }

        public double X0
        {
            get
            {
                if (A == 0 || Double.IsNaN(A))
                    throw new Exception("There is no single X0 for a line");

                return -B/A;
            }
        }

        public bool IsVertical
        {
            get { return Double.IsNaN(A); }
        }

        public bool IsHorizontal
        {
            get { return A == 0 || Double.IsInfinity(A); }
        }

        private Line(double a, double b)
        {
            A = a;
            B = b;
        }

        public override string ToString()
        {
            return string.Format("Line: Y = {0} x + {1}", A, B);
        }

        public static class Builder
        {
            public static Line CreateFromPoints(Vector2 point1, Vector2 point2)
            {
                var directionVector = point2 - point1;
                var a = directionVector.Y/directionVector.X;
                if (Double.IsInfinity(a))
                {
                    if (point1.X != point2.X)
                        throw new Exception(string.Format("something went wrong. {0} {1}", point1, point2));

                    return new Line(Double.NaN, point1.X);
                }
                // B = Y-AX
                var b = point1.Y - a*point1.X;
                return new Line(a, b);
            }

            public static Line CreateFromParameters(double a, double b)
            {
                return new Line(a, b);
            }

            public static Line CreateFromSegment(Segment segment)
            {
                return CreateFromPoints(segment.A, segment.B);
            }

            public static Line CreateFromPointWithGivenA(Vector2 point, double a)
            {
                // vertical line
                if (Double.IsInfinity(a) || Double.IsNaN(a))
                    return new Line(Double.NaN, point.X);

                // TODO can be optimized
                var direction = new Vector2(1, a);
                return CreateFromPoints(point, point + direction);
            }
        }


        public double DistanceTo(Vector2 point)
        {
            if (Double.IsNaN(A))
                return Math.Abs(point.X - B);
            //todo tests
            // https://pl.wikipedia.org/wiki/Odleg%C5%82o%C5%9B%C4%87_punktu_od_prostej
            // A = a
            // B = -1
            // C = b
            return Math.Abs(A * point.X - point.Y + B) / Math.Sqrt(A * A + 1);
        }

        public double Y(double x)
        {
            return A * x + B;
        }
    }
}