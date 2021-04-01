using System;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;

namespace Geometry
{
    public static class InertiaCalculator
    {
        public static Polygon AdjustToMassCenter(this Polygon polygon)
        {
            var massCenter = polygon.CountCenterMass();

            return new Polygon(polygon.Points.Select(x => x - massCenter).ToArray());
        }

        public static double CountInertia(this Shape shape, double mass, Vector2 massCenter = null)
        {
            switch (shape)
            {
                case Polygon polygon: return polygon.CountInertia(mass);
                case Circle circle: return circle.CountInertia(mass);
                case Point _: return double.PositiveInfinity;
                default: throw new NotSupportedException();
            }
        }

        public static double CountInertia(this Circle circle, double mass)
        {
            var geometricInertia = Math.PI * Math.Pow(circle.Radius * 2, 4) / 32;
            var circleField = Math.PI * circle.Radius * circle.Radius;
            var density = mass / circleField;
            return geometricInertia * density;
        }

        public static double CountInertia(this Polygon polygon, double mass)
        {
            var area = AreaCalculator.CountArea(polygon);
            var density = mass / area;

            return CountGeometricInertia(polygon)*density;
        }

        public static double CountGeometricInertia(this Polygon polygon)
        {
            var centerMass = CountCenterMass(polygon);

            return PolygonIntegral.IntegralOnPolygon(polygon, centerMass, 
                (dPhi, h) => 1d / 4 * Math.Pow(h, 4) * dPhi * (1 + Math.Pow(dPhi / 2, 2)));
        }

        public static Vector2 CountCenterMass(this Polygon polygon)
        {
            var points = polygon.Points;
            var triangles = polygon.Points.GetTriangulationIndices();

            var centerMass = Vector2.Zero;
            double totalTrianglesArea = 0;

            foreach (var triangle in triangles)
            {
                var A = points[triangle.Item1];
                var B = points[triangle.Item2];
                var C = points[triangle.Item3];

                var a = new Segment(B,C);
                var b = new Segment(A,C);
                var c = new Segment(A, B);

                var area = AreaCalculator.CountTriangleArea(a.Length, b.Length, c.Length);
                var triangleCenterMass = TriangleUtils.CountCenterMass(A, B, C);

                centerMass += triangleCenterMass * area;
                totalTrianglesArea += area;
            }
            return 1d / totalTrianglesArea * centerMass;
        }
    }
}