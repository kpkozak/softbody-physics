using Physics.Bodies;
using SFML.Graphics;
using SFML.Window;

namespace Samples
{
    internal class ParasoftDrawableItem : Drawable
    {
        private readonly RigidBody[] _bodies;

        public ParasoftDrawableItem(RigidBody[] bodies)
        {
            _bodies = bodies;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            var shape = new ConvexShape((uint) _bodies.Length);
            for(uint i=0; i< _bodies.Length; i++)
            {
                var point = _bodies[i].Position;
                shape.SetPoint(i, new Vector2f((float) point.X, (float) point.Y));
            }
            shape.FillColor = new Color(0, 0, 0, 100);
            shape.OutlineColor = Color.Black;
            shape.OutlineThickness = 1;
            target.Draw(shape,states);
        }
    }
}