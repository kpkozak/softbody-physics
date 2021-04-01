using System.Collections.Generic;
using Geometry.Vector;

namespace GeometryTests
{
    public class TestVectorComparer:IEqualityComparer<Vector2>
    {
        public static TestVectorComparer Default = new TestVectorComparer(0.05);

        private readonly double _tolerance;

        private TestVectorComparer(double tolerance)
        {
            _tolerance = tolerance;
        }

        public bool Equals(Vector2 x, Vector2 y)
        {
            return (x - y).Length < _tolerance;
        }

        public int GetHashCode(Vector2 obj)
        {
            return obj.GetHashCode();
        }
    }
}