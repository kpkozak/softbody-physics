using System;

namespace Geometry.Matrix
{
    public abstract class Matrix
    {
        private readonly int _dimension;
        // rows
        protected readonly double[] _values;

        protected Matrix(int dimension, params double[] values)
        {
            _dimension = dimension;
            _values = values;

            if (values.Length != _dimension*_dimension)
                throw new ArgumentException("Invalid amount of elements provided. Matrix 3x3 should have 9 values");
        }

        public double this[int row, int column]
        {
            get
            {
                if (row < 0 || column < 0 || row>=_dimension || row>=_dimension)
                    throw new ArgumentOutOfRangeException("invalid index");
                return _values[row * _dimension + column];
            }
            set
            {
                if (row < 0 || column < 0 || row >= _dimension || row >= _dimension)
                    throw new ArgumentOutOfRangeException("invalid index");
                _values[row * _dimension + column] = value;
            }
        }
    }
}