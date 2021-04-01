using System;
using System.Linq;
using Geometry.Shapes;
using Physics.Bodies;
using SFML.Graphics;
using SFML.Window;
using Shape = SFML.Graphics.Shape;

namespace Samples.Drawables
{
    internal class DrawableRigidBody : Drawable
    {
        private readonly RigidBody _body;

        public DrawableRigidBody(RigidBody body)
        {
            _body = body;
        }

        private Shape GetShape(RigidBody body)
        {
            if (body.Shape is Circle)
            {
                var circle = body.Shape as Circle;
                return new CircleShape((float)circle.Radius, (uint)circle.Radius / 3 + 10);
            }
            if (body.Shape is Polygon)
            {
                var polygon2D = body.Shape as Polygon;
                var shape = new ConvexShape((uint)polygon2D.Points.Count());
                for (uint i = 0; i < polygon2D.Points.Count(); i++)
                {
                    var point = polygon2D.Points[i];
                    shape.SetPoint(i, new Vector2f((float)point.X, (float)point.Y));
                }
                return shape;
            }
            throw new NotSupportedException();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            var shape = GetShape(_body);
            if (_body.Shape is Circle)
            {
                var circle = (Circle)_body.Shape;
                shape.Origin = new Vector2f((float)circle.Radius, (float)circle.Radius);
            }
            shape.FillColor = new Color(0, 0, 0, 100);
            shape.OutlineColor = Color.Black;
            shape.OutlineThickness = 1;

            shape.Position = new Vector2f((float)(_body.Position.X), (float)(_body.Position.Y));
            shape.Rotation = (float) (_body.Rotation/Math.PI*180);
            shape.Draw(target, states);
        }
    }
}