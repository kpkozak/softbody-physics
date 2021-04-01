using System;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;

namespace Geometry.Matrix
{
    // tOdo https://www.codeproject.com/Articles/3354/Efficient-Matrix-Programming-in-C
    // TODO make matrix immutable
    public class Matrix3 : Matrix, IEquatable<Matrix3>
    {
        private Matrix3(params double[] values) : base(3, values)
        {
        }

        public bool Equals(Matrix3 other)
        {
            for (var i = 0; i < 9; ++i)
                if (Math.Abs(_values[i] - other._values[i]) > Config.Epsilon)
                    return false;
            return true;
        }

        public static Matrix3 Diagonal(double value)
        {
            return new Matrix3(
                value, 0, 0,
                0, value, 0,
                0, 0, value);
        }

        public static Matrix3 Translation(Vector2 vector)
        {
            return new Matrix3(
                1, 0, vector.X,
                0, 1, vector.Y,
                0, 0, 1);
        }

        public static Matrix3 Scale(Vector2 vector)
        {
            return new Matrix3(
                vector.X, 0, 0,
                0, vector.Y, 0,
                0, 0, 1
            );
        }

        public static Matrix3 Rotation(double phi)
        {
            var cos = Math.Cos(phi);
            var sin = Math.Sin(phi);
            return new Matrix3(
                cos, -sin, 0,
                sin, cos, 0,
                0, 0, 1);
        }

        public static Matrix3 Create(params double[] rows)
        {
            if (rows.Length != 9)
                throw new ArgumentException("3x3 matrix must have 9 values");

            return new Matrix3(rows);
        }

        public static Matrix3 Create(params Vector3[] rows)
        {
            if (rows.Length != 3)
                throw new ArgumentException("Matrix must have 3 rows");

            return new Matrix3(
                rows[0].X, rows[0].Y, rows[0].Z,
                rows[1].X, rows[1].Y, rows[1].Z,
                rows[2].X, rows[2].Y, rows[2].Z);
        }

        public static Matrix3 operator *(Matrix3 matrix, Matrix3 other)
        {
            // a0b0+a1b3+a2b6, a0b1+a1b4+a2b7, a0b2+a1b5+a2b8,
            // a3b0+a4b3+a5b6, a3b1+a4b4+a5b7, a3b2+a4b5+a5b8,
            // a6b0+a7b3+a8b6, a6b1+a7b4+a8b7, a6b2+a7b5+a8b8

            // find (a(\d)b(\d)) replace a[$2]*b[$3]
            var a = matrix._values;
            var b = other._values;
            return new Matrix3(
                a[0] * b[0] + a[1] * b[3] + a[2] * b[6], a[0] * b[1] + a[1] * b[4] + a[2] * b[7],
                a[0] * b[2] + a[1] * b[5] + a[2] * b[8],
                a[3] * b[0] + a[4] * b[3] + a[5] * b[6], a[3] * b[1] + a[4] * b[4] + a[5] * b[7],
                a[3] * b[2] + a[4] * b[5] + a[5] * b[8],
                a[6] * b[0] + a[7] * b[3] + a[8] * b[6], a[6] * b[1] + a[7] * b[4] + a[8] * b[7],
                a[6] * b[2] + a[7] * b[5] + a[8] * b[8]
            );
        }

        public static Vector2 operator *(Matrix3 matrix, Vector2 vector)
        {
            var a = matrix._values;
            var z = a[6] * vector.X + a[7] * vector.Y + a[8] * 1;
            return new Vector2(
                (a[0] * vector.X + a[1] * vector.Y + a[2] * 1) / z,
                (a[3] * vector.X + a[4] * vector.Y + a[5] * 1) / z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Matrix3) obj);
        }

        public override int GetHashCode()
        {
            return _values[0].GetHashCode();
        }

        public override string ToString()
        {
            return
                $"[{_values[0]:000.00} {_values[1]:000.00} {_values[2]:000.00}]{Environment.NewLine}[{_values[3]:000.00} {_values[4]:000.00} {_values[5]:000.00}]{Environment.NewLine}[{_values[6]:000.00} {_values[7]:000.00} {_values[8]:000.00}]";
        }

        public static Matrix3 Identity()
        {
            return new Matrix3(
                1, 0, 0,
                0, 1, 0,
                0, 0, 1);
        }
    }
}