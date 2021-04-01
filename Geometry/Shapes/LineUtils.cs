using System;
using Geometry.Vector;

namespace Geometry.Shapes
{
    public static class LineUtils
    {
        public static Vector2 GetIntersectionPoint(this Segment segment, Line line)
        {
            return line.GetIntersectionPoint(segment);
        }

        public static Vector2 GetIntersectionPoint(this Line line, Segment other)
        {
            var intersectionPoint = GetIntersectionPoint(line, other.Line);
            var plotA = line.Direction.Dot(other.A);
            var plotB = line.Direction.Dot(other.B);
            var plotIntersection = line.Direction.Dot(intersectionPoint);

            return IsBetween(plotIntersection, plotA, plotB) ? intersectionPoint : new Vector2(double.NaN, double.NaN);
        }

        private static bool IsBetween(double candidate, double plotA, double plotB)
        {
            if (plotA > plotB)
                (plotA, plotB) = (plotB, plotA);

            return candidate >= plotA && candidate <= plotB;
        }

        public static Vector2 GetIntersectionPoint(this Line line, Line other)
        {
            if (line.IsVertical)
            {
                return other.IsVertical ? new Vector2(double.NaN, double.NaN) : new Vector2(line.B, other.Y(line.B));
            }

            if (other.IsVertical)
            {
                return new Vector2(other.B, line.Y(other.B));
            }

            var x = (line.B - other.B)/(other.A - line.A);
            var y = (other.A*line.B - other.B*line.A)/(other.A - line.A);
            return new Vector2(x, y);
        }

        public static Line GetParallelThroughPoint(this Line line, Vector2 point)
        {
            var a = line.A;
            var b = point.Y - (line.A*point.X);
            return Line.Builder.CreateFromParameters(a, b);
        }

        /// <summary> returns line perpendicular to given line that passes through point (cane be used e.g in setting triangle height) </summary>
        public static Line GetPerpendicularThroughPoint(this Line line, Vector2 point)
        {
            var a = -1/line.A;
            var b = point.Y - (a*point.X);
            return Line.Builder.CreateFromParameters(a, b);
        }

        /// <summary> Direction vector (normalized) of line rotated by degrees </summary>
        public static Vector2 Rotate(this Line line, double degrees)
        {
            var rad = DegreesToRad(degrees);
            var a = Math.Tan(Math.Atan(line.A) + rad);
            return new Vector2(1,a).Normalize();
        }

        private static double DegreesToRad(double degrees)
        {
            return Math.PI*degrees/180.0d;
        }
    }
}