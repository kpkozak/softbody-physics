using Geometry.Vector;

namespace Geometry.Shapes
{
    public class Segment
    {
        private Line _line;

        public Segment(Vector2 a, Vector2 b)
        {
            A = a;
            B = b;
        }

        public Vector2 B { get; private set; }
        public Vector2 A { get; private set; }
        public double Length { get { return (B - A).Length; } }
        public Line Line
        {
            get { return _line ?? (_line = Line.Builder.CreateFromPoints(A, B)); }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", A, B);
        }

        // todo probably as extension method
        public Vector2 MidPoint()
        {
            return 0.5 * (A + B);
        }
    }
}