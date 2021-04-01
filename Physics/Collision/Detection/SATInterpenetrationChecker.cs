using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;

namespace Physics.Collision.Detection
{
    internal class SATInterpenetrationChecker
    {
        private readonly OneDimensionIntersectionChecker _intersectionChecker;
        public SATInterpenetrationChecker(OneDimensionIntersectionChecker intersectionChecker)
        {
            _intersectionChecker = intersectionChecker;
        }

        public bool AreColliding(Vector2[] shape1, Vector2[] shape2, out Vector2 interpenetration,
            out Vector2 collisionNormal)
        {
            collisionNormal = null;
            interpenetration = null;
            var normalWithMinimalInterpenetration = Vector2.Zero;
            var minimalInterpenetration = double.PositiveInfinity;

            foreach (var normal in shape1.GetEdgesNormals()
                .Concat(shape2.GetEdgesNormals()))
            {
                var min2 = 1e15;
                var max2 = -1e15;

                var min1 = 1e15;
                var max1 = -1e15;
                var currentNormal = normal;
                foreach (var projection in shape1.Select(point => point.Dot(currentNormal)))
                {
                    min1 = Math.Min(min1, projection);
                    max1 = Math.Max(max1, projection);
                }
                foreach (var projection in shape2.Select(point => point.Dot(currentNormal)))
                {
                    min2 = Math.Min(min2, projection);
                    max2 = Math.Max(max2, projection);
                }

                if (!_intersectionChecker.AreIntersecting(min1, max1, min2, max2))
                    return false;
                var currentInterpenetration = _intersectionChecker.CountInterpenetration(min1, max1, min2, max2);
                
                if (currentInterpenetration < minimalInterpenetration)
                {
                    minimalInterpenetration = currentInterpenetration;
                    normalWithMinimalInterpenetration = normal;
                }
            }

            interpenetration = normalWithMinimalInterpenetration.Normalize() * minimalInterpenetration;
            collisionNormal =
                SecondIsOnRight(
                    shape1.Select(point => point.Dot(normalWithMinimalInterpenetration)),
                    shape2.Select(point => point.Dot(normalWithMinimalInterpenetration)))
                    ? normalWithMinimalInterpenetration
                    : -normalWithMinimalInterpenetration;
            return true;
        }

        private bool SecondIsOnRight(IEnumerable<double> points1, IEnumerable<double> points2)
        {
            var first = points1.Average();
            var second = points2.Average();

            return second > first;
        }
    }
}