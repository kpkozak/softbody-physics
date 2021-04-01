using System;
using Geometry.Vector;

namespace Geometry.Shapes
{
    public static class CircleUtils
    {
        public static Vector2[] GetCommonPoints(this Circle circle, Line line)
        {
            var delta = -((circle.Center.X*circle.Center.X)*(line.A*line.A)) +
                        (2.0d*circle.Center.X*circle.Center.Y*line.A) - (2.0d*circle.Center.X*line.A*line.B) -
                        (circle.Center.Y*circle.Center.Y) + (2.0d*circle.Center.Y*line.B) +
                        ((line.A*line.A)*(circle.Radius*circle.Radius)) -
                        line.B*line.B + circle.Radius*circle.Radius;
            var a = line.A*line.A + 1.0d;
            var b = circle.Center.X + circle.Center.Y*line.A - line.A*line.B;

            // TODO rozwa¿yæ gdy punkty le¿¹ na prostej pionowej
            if (delta < 0)
            {
                return new Vector2[0];
            }
            var x1 = (-Math.Sqrt(delta) + b)/a;
            var x2 = (Math.Sqrt(delta) + b)/a;
            return new[]
            {
                new Vector2(x1,(x1*line.A+line.B)),
                new Vector2(x2, x2*line.A+line.B)
            };
        }

        public static Circle Translate(this Circle circle, Vector2 vector)
        {
            return new Circle(circle.Center + vector, circle.Radius);
        }
    }
}