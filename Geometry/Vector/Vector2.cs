using System;

namespace Geometry.Vector
{
    public class Vector2 : IEquatable<Vector2>
    {
        public static readonly Vector2 i = new Vector2(1, 0);
        public static readonly Vector2 j = new Vector2(0, 1);
        public static readonly Vector2 Zero = new Vector2(0, 0);
        

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; protected set; }
        public double Y { get; protected set; }

        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public bool IsValid
        {
            get { return IsCoordinateValid(X) && IsCoordinateValid(Y); }
        }

        private bool IsCoordinateValid(double value)
        {
            return !(double.IsNaN(value) || double.IsInfinity(value));
        }

        public static Vector2 operator +(Vector2 vector, Vector2 other)
        {
            return new Vector2(vector.X + other.X, vector.Y + other.Y);
        }

        public static Vector2 operator -(Vector2 vector, Vector2 vector2)
        {
            return vector + vector2.Invert();
        }

        public static Vector2 operator -(Vector2 vector)
        {
            return new Vector2(-vector.X, -vector.Y);
        }

        public static explicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.X,v.Y);
        }

        public static Vector2 operator *(Vector2 vector, double scalar)
        {
            return new Vector2(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector2 operator *(double scalar, Vector2 vector)
        {
            return vector * scalar;
        }

        public Vector2 GetNormalVector()
        {
            return new Vector2(Y, -X);
        }

        public static implicit operator Vector2((double X, double Y) tuple)
        {
            return new Vector2(tuple.X, tuple.Y);
        }

        public bool Equals(Vector2 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Math.Abs(X - other.X) < Config.Epsilon && Math.Abs(Y - other.Y) < Config.Epsilon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public Vector3 AsVector3()
        {
            return new Vector3(X, Y, 1);
        }

        public override string ToString()
        {
            return $"({X};{Y})";
        }
    }
}