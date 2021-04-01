namespace Geometry.Vector
{
    public static class Vector3Utils
    {
        public static double CountRadius2(this Vector3 point, Vector3 point2)
        {
            return System.Math.Pow(point.X - point2.X, 2)
                   + System.Math.Pow(point.Y - point2.Y, 2)
                   + System.Math.Pow(point.Z - point2.Z, 2);
        }

        public static double CountRadius(this Vector3 point, Vector3 point2)
        {
            return System.Math.Sqrt(point.CountRadius2(point2));
        }

        public static Vector3 CountDistance(this Vector3 point, Vector3 point2)
        {
            return new Vector3(point2.X-point.X,point2.Y-point.Y, point2.Z-point.Z);
        }

        public static Vector3 Invert(this Vector3 vector)
        {
            return -vector;
        }

        public static Vector3 Multiply(this Vector3 vector, double a)
        {
            return vector*a;
        }

        public static Vector3 Normalize(this Vector3 vector)
        {
            return new Vector3(vector.X / vector.Length, vector.Y / vector.Length, vector.Z / vector.Length);
        }

        public static Vector3 Cross(this Vector3 vector, Vector3 vector2)
        {
            return new Vector3(
                vector.Y * vector2.Z - vector.Z * vector2.Y,
                vector.Z * vector2.X - vector.X * vector2.Z,
                vector.X * vector2.Y - vector.Y * vector2.X);
        }

        public static double Dot(this Vector3 vector, Vector3 other)
        {
            return vector.X * other.X + vector.Y * other.Y + vector.Z * other.Z;
        }
    }
}
