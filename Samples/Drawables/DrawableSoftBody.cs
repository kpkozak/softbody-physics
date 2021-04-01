using System;
using System.Collections.Generic;
using System.Linq;
using Physics.Bodies;
using SFML.Graphics;
using SFML.Window;

namespace Samples.Drawables
{
    internal class DrawableSoftBody : Drawable
    {
        private readonly SoftBody _body;
        private readonly List<DrawableConstraint> _constraints;

        public DrawableSoftBody(SoftBody body)
        {
            _body = body;
            _constraints = _body._joints.Select(x => new DrawableConstraint(x)).ToList();
        }

        private Shape GetShape(SoftBody body)
        {
            if (body.Shape is FlexConcavePolygon)
            {
                var polygon2D = (body.Shape as FlexConcavePolygon).Points.ToArray();
                var shape = new ConvexShape((uint)polygon2D.Count());
                for (uint i = 0; i < polygon2D.Count(); i++)
                {
                    var point = polygon2D[i];
                    shape.SetPoint(i, new Vector2f((float)point.X, (float)point.Y));
                }
                return shape;
            }
            throw new NotSupportedException();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            var shape = GetShape(_body);
            shape.FillColor = new Color(0, 0, 0, 100);
            shape.OutlineColor = Color.Black;
            shape.OutlineThickness = 1;

            shape.Rotation = (float)(_body.Rotation / Math.PI * 180);
            shape.Draw(target, states);
            foreach (var constraint in _constraints)
            {
                constraint.Draw(target, states);
            }
        }
    }
}