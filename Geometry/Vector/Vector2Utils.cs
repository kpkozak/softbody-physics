namespace Geometry.Vector
{
    public static class Vector2Utils
    {
        public static double CountRadius2(this Vector2 point, Vector2 point2)
        {
            return System.Math.Pow(point.X - point2.X, 2)
                   + System.Math.Pow(point.Y - point2.Y, 2);
        }

        public static double CountRadius(this Vector2 point, Vector2 point2)
        {
            return System.Math.Sqrt(point.CountRadius2(point2));
        }

        public static Vector2 CountDistance(this Vector2 point, Vector2 point2)
        {
            return new Vector2(point2.X - point.X, point2.Y - point.Y);
        }

        public static Vector2 Invert(this Vector2 vector)
        {
            return -vector;
        }

        public static Vector2 Multiply(this Vector2 vector, double a)
        {
            return vector * a;
        }

        public static Vector2 Normalize(this Vector2 vector)
        {
            return new Vector2(vector.X / vector.Length, vector.Y / vector.Length);
        }

        public static Vector3 Cross(this Vector2 vector, Vector2 vector2)
        {
            return new Vector3(
                0,
                0,
                vector.X * vector2.Y - vector.Y * vector2.X);
        }

        public static double Dot(this Vector2 vector, Vector2 other)
        {
            return vector.X * other.X + vector.Y * other.Y;
        }
    }
}