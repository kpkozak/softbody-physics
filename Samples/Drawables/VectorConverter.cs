using Geometry.Vector;
using SFML.Window;

namespace Samples {
    public static class VectorConverter
    {
        public static Vector2f AsSFML(this Vector2 vector)
        {
            return new Vector2f((float) vector.X, (float) vector.Y);
        }
    }
}