using Physics.Bodies;
using Physics.Constraints;
using SFML.Graphics;

namespace Samples.Drawables
{
    public static class Extensions
    {
        public static Drawable ToDrawable(this Body body)
        {
            return body is RigidBody
                ? (Drawable) new DrawableRigidBody(body as RigidBody)
                : new DrawableSoftBody(body as SoftBody);
        }

        public static Drawable ToDrawable(this IConstraint constraint)
        {
            return new DrawableConstraint(constraint);
        }
    }
}