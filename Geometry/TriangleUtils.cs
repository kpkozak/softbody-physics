using Geometry.Shapes;
using Geometry.Vector;

namespace Geometry
{
    public static class TriangleUtils
    {
        public static Vector2 CountCenterMass(Vector2 A, Vector2 B, Vector2 C)
        {
            var a = new Segment(B, C);
            var b = new Segment(A, C);

            var middleA = new Segment(A, a.MidPoint());
            var middleB = new Segment(B, b.MidPoint());

            var massCenter = middleA.Line.GetIntersectionPoint(middleB.Line);
            return massCenter;
        }
    }
}