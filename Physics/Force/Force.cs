using Geometry;
using Geometry.Vector;

namespace Physics.Force
{
    public class Force
    {
        public static readonly Force Zero = new Force(Vector2.Zero);
        // TODO probably delete Force (use Vector3 instead) or use always
        public Force(Vector2 force)
        {
            Value = force;
        }

        public Vector2 Value { get; private set; }
    }
}