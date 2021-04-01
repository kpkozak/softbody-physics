using System.Diagnostics;
using Geometry.Vector;

namespace Physics.Collision.Detection
{
    [DebuggerDisplay("{Point} Length:{Point.Length}")]
    public class CollisionPoint
    {
        public CollisionPoint(Vector2 point, Vector2 interpenetration, Vector2 normal)
        {
            Point = point;
            Interpenetration = interpenetration;
            Normal = normal;
        }

        public Vector2 Point { get; }
        public Vector2 Interpenetration { get; }
        public Vector2 Normal { get; }
    }
}