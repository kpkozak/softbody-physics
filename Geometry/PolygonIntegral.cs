using System;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;

namespace Geometry
{
    public static class PolygonIntegral
    {
        public delegate double IntegralFunction(double dPhi, double h);

        public static double IntegralOnPolygon(Polygon polygon, Vector2 centralPoint, IntegralFunction integralFunc)
        {
            var edges = polygon.GetEdges().Select(x => x.Segment).ToArray();
            var integral = 0d;
            for (double phi = 0; phi < Math.PI; phi += Config.InertiaAngleStep)
            {
                var line = Line.Builder.CreateFromPointWithGivenA(centralPoint, Math.Tan(phi));

                var intersectionPoints = edges.Select(edge => line.GetIntersectionPoint(edge))
                    .Where(point => point.IsValid).ToArray();

                var D = intersectionPoints[0];
                var E = intersectionPoints[1];

                var h1 = (centralPoint - D).Length;
                var h2 = (centralPoint - E).Length;

                var int1 = integralFunc(Config.InertiaAngleStep, h1);
                var int2 = integralFunc(Config.InertiaAngleStep, h2);

                integral += int1 + int2;
            }

            return integral;
        }

        public static double IntegralOnTriangle(Vector2 A, Vector2 B, Vector2 C, IntegralFunction integralFunc)
        {
            var a = new Segment(B, C);
            var b = new Segment(A, C);
            var c = new Segment(A, B);

            var massCenter = TriangleUtils.CountCenterMass(A, B, C);

            var integral = 0d;
            for (double phi = 0; phi < Math.PI; phi += Config.InertiaAngleStep)
            {
                var line = Line.Builder.CreateFromPointWithGivenA(massCenter, Math.Tan(phi));

                var intersectionA = line.GetIntersectionPoint(a.Line);
                var intersectionB = line.GetIntersectionPoint(b.Line);
                var intersectionC = line.GetIntersectionPoint(c.Line);

                var lineDirection = line.Direction;
                var plotA = lineDirection.Dot(intersectionA);
                var plotB = lineDirection.Dot(intersectionB);
                var plotC = lineDirection.Dot(intersectionC);
                var plotCenter = lineDirection.Dot(massCenter);

                var D = FindClosestOnLeft(plotCenter, (intersectionA, plotA), (intersectionB, plotB),
                    (intersectionC, plotC));
                var E = FindClosestOnRight(plotCenter, (intersectionA, plotA), (intersectionB, plotB),
                    (intersectionC, plotC));

                var h1 = (massCenter - D).Length;
                var h2 = (massCenter - E).Length;

                var int1 = integralFunc(Config.InertiaAngleStep, h1);
                var int2 = integralFunc(Config.InertiaAngleStep, h2);

                integral += int1 + int2;
            }
            return integral;
        }

        private static Vector2 FindClosestOnRight(double plotCenter, params (Vector2 Point, double Plot)[] plots)
        {
            var closest = (Point: Vector2.Zero, Plot: double.PositiveInfinity);
            foreach (var plot in plots)
            {
                var distance = plot.Plot - plotCenter;
                var closestDistance = closest.Plot - plotCenter;
                if (distance > 0 && distance < closestDistance)
                    closest = plot;
            }
            return closest.Point;
        }

        private static Vector2 FindClosestOnLeft(double plotCenter, params (Vector2 Point, double Plot)[] plots)
        {
            var closest = (Point: Vector2.Zero, Plot: double.NegativeInfinity);
            foreach (var plot in plots)
            {
                var distance = plot.Plot - plotCenter;
                var closestDistance = closest.Plot - plotCenter;
                if (distance < 0 && distance > closestDistance)
                    closest = plot;
            }
            return closest.Point;
        }
    }
}