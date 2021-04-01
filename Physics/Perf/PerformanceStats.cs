using System;

namespace Physics.Perf {
    public class PerformanceStats
    {
        public PerformanceStats(TimeSpan total, TimeSpan min, TimeSpan max, TimeSpan average)
        {
            Total = total;
            Min = min;
            Max = max;
            Average = average;
        }
        public TimeSpan Total { get; }
        public TimeSpan Max { get; }
        public TimeSpan Min { get; }
        public TimeSpan Average { get; }
    }
}