using System;
using System.Collections.Generic;
using Geometry.Vector;
using Physics.Constraints;
using SFML.Graphics;
using SFML.Window;

namespace Samples.Drawables {
    internal class DrawableConstraint:Drawable
    {
        private readonly IConstraint _constraint;

        public DrawableConstraint(IConstraint constraint)
        {
            _constraint = constraint;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            switch (_constraint)
            {
                case SpringConstraint spring:
                    Draw(spring, target,states);
                    break;
                case DistanceConstraint distance:
                    Draw(distance,target,states);
                    break;
                case OrientationConstraint orientation:
                    Draw(orientation, target,states);
                    break;
                default: throw new NotSupportedException();
            }
        }

        private void Draw(OrientationConstraint orientation, RenderTarget target, RenderStates states)
        {
            var lineStart = orientation.BodyA.Position;
            var lineEnd = orientation.BodyB.Position;
            var color = new Color(100, 30, 100, 128);

            var vertices = new[]
            {
                new Vertex(lineStart.AsSFML(), color),
                new Vertex(lineEnd.AsSFML(), color)
            };
            var circle1 = new CircleShape(5)
            {
                Origin = new Vector2f(5, 5),
                FillColor = color,
                Position = lineStart.AsSFML()
            };
            var circle2 = new CircleShape(5)
            {
                Origin = new Vector2f(5, 5),
                FillColor = color,
                Position = lineEnd.AsSFML()
            };

            target.Draw(vertices, PrimitiveType.LinesStrip, states);
            target.Draw(circle1);
            target.Draw(circle2);
        }

        private void Draw(DistanceConstraint distance, RenderTarget target, RenderStates states)
        {
            var lineStart = distance.BodyA.GetTransformMatrix() * distance.AAnchorPoint;
            var lineEnd = distance.BodyB.GetTransformMatrix() * distance.BAnchorPoint;

            var color = distance.IsRigid ? Color.Red : Color.Green;

            var vertices = new[]
            {
                new Vertex(lineStart.AsSFML(), color),
                new Vertex(lineEnd.AsSFML(), color)
            };

            target.Draw(vertices, PrimitiveType.LinesStrip, states);
        }

        private void Draw(SpringConstraint spring, RenderTarget target, RenderStates states)
        {
            var lineStart = spring.BodyA.GetTransformMatrix() * spring.AAnchorPoint;
            var lineEnd = spring.BodyB.GetTransformMatrix() * spring.BAnchorPoint;

            var length = (lineEnd - lineStart).Length;
            var direction = (lineEnd - lineStart).Normalize();
            var diff = direction.GetNormalVector()*2;

            var extension = Math.Min(Math.Abs(length - spring.Length) / spring.Length*2, 1);
            var colorDiff = (byte) (255 * extension);
            var color = new Color(colorDiff, 0, (byte) (255 - colorDiff));

            var vertices = new List<Vertex>();
            vertices.Add(new Vertex(lineStart.AsSFML(), color));
            for (double d = 0; d < length; d += 6)
            {
                vertices.Add(new Vertex((lineStart + direction * d + diff).AsSFML(),color));
                vertices.Add(new Vertex((lineStart + direction * (d+3) - diff).AsSFML(), color));
            }
            vertices.Add(new Vertex(lineEnd.AsSFML(), color));

            target.Draw(vertices.ToArray(), PrimitiveType.LinesStrip, states);
        }
    }
}