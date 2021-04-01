using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Vector;

namespace Physics.Collision.Detection
{
    class DuplicateCollisionPointsMerger
    {
        public IEnumerable<CollisionPoint> Merge(IEnumerable<CollisionPoint> allCollisionPoints)
        {
            var points = allCollisionPoints.ToList();
            points.Sort(new CustomComparer());

            var interpenetration = Vector2.Zero;
            var collisionNormal = Vector2.Zero;
            var pointsCount = 0;
            var point = points[0].Point;

            foreach (var collisionPoint in points)
            {
                if (!collisionPoint.Point.Equals(point))
                {
                    // reset i raise
                    if(collisionNormal.Equals(Vector2.Zero) == false){
                        yield return new CollisionPoint(point, interpenetration * (1.0d / pointsCount),
                        collisionNormal.Normalize());
                    }
                    interpenetration = Vector2.Zero;
                    collisionNormal = Vector2.Zero;
                    pointsCount = 0;
                    point = collisionPoint.Point;
                }

                interpenetration += collisionPoint.Interpenetration;
                collisionNormal += collisionPoint.Normal;
                pointsCount++;
            }
            if (collisionNormal.Equals(Vector2.Zero) == false)
            {
                yield return new CollisionPoint(point, interpenetration * (1.0d / pointsCount),
                    collisionNormal.Normalize());
            }
        }

        internal class CustomComparer : IComparer<CollisionPoint>
        {
            public int Compare(CollisionPoint x, CollisionPoint y)
            {
                return Math.Sign(y.Point.Length - x.Point.Length);
            }
        }
    }
}