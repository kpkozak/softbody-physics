using Geometry.Vector;

namespace Geometry.Shapes
{
    public class Circle:Shape
    {
        public Vector2 Center { get; private set; }
        public double Radius { get; private set; }

        public Circle(Vector2 center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public Circle(double radius)
        {
            Center = Vector2.Zero;
            Radius = radius;
        }

        public static Circle Create(double radius)
        {
            return new Circle(radius);
        }
    }
}