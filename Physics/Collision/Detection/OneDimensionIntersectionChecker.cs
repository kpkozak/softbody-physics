using System;

namespace Physics.Collision.Detection
{
    class OneDimensionIntersectionChecker
    {
        public bool AreIntersecting(double min1, double max1, double min2, double max2)
        {
            if (min1 > max1 || min2>max2)
                throw new ArgumentException("Min cannot be greater than max");
            if (max2 < min1 || max1 < min2)
                return false;
            return true;
        }

        public double CountInterpenetration(double min1, double max1, double min2, double max2)
        {
            if (min1 > max1 || min2 > max2)
                throw new ArgumentException("Min cannot be greater than max");
            var min = Math.Max(min1, min2);
            var max = Math.Min(max1, max2);
            return max - min;
        }
    }
}