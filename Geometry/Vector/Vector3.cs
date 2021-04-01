using System;

namespace Geometry.Vector
{
    public class Vector3 : IEquatable<Vector3>
    {
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 ZeroCoords = new Vector3(0, 0, 1);
        public static readonly Vector3 i = new Vector3(1, 0, 1);
        public static readonly Vector3 j = new Vector3(0, 1, 1);

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public double Length
        {
            get { return System.Math.Sqrt(X*X + Y*Y+Z*Z); }
        }

        public static Vector3 operator +(Vector3 vector, Vector3 other)
        {
            return new Vector3(vector.X + other.X, vector.Y + other.Y, vector.Z + other.Z);
        }

        public static Vector3 operator -(Vector3 vector, Vector3 vector2)
        {
            return vector + vector2.Invert();
        }

        public static Vector3 operator -(Vector3 vector)
        {
            return new Vector3(-vector.X, -vector.Y, -vector.Z);
        }

        public static Vector3 operator*(Vector3 vector, double scalar)
        {
            return new Vector3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        public static Vector3 operator *(double scalar, Vector3 vector)
        {
            return vector*scalar;
        }

        public bool Equals(Vector3 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Math.Abs(X - other.X) < Config.Epsilon && Math.Abs(Y - other.Y) < Config.Epsilon && Math.Abs(Z - other.Z) < Config.Epsilon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector3) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode*397) ^ Y.GetHashCode();
                hashCode = (hashCode*397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("({0};{1};{2})", X, Y, Z);
        }
    }
}