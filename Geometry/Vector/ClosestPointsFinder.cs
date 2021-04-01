using System;
using System.Collections.Generic;
using System.Linq;

namespace Geometry.Vector {
    public static class ClosestPointsFinder
    {
        /// <exception cref="ArgumentException">Not enough points</exception>
        public static (Vector2 Point, double Distance)[] Find2Closest(this IEnumerable<Vector2>points, Vector2 point)
        {
            if(points.Count()<2)
                throw new ArgumentException("Not enough points");

            var closest1 = (Point: Vector2.Zero, Distance: Double.PositiveInfinity);
            var closest2 = (Point: Vector2.Zero, Distance: Double.PositiveInfinity);
            foreach (var closestCandidate in points)
            {
                var distance = (closestCandidate - point).Length;
                if (distance < closest2.Distance) closest2 = (Point:closestCandidate, Distance: distance);
                if (closest1.Distance > closest2.Distance) (closest1, closest2) = (closest2, closest1);
            }
            return new[] {closest1, closest2};
        }
    }
}