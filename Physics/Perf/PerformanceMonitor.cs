using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Physics.Perf
{
    public class PerformanceMonitor
    {
        private static Dictionary<ActionType, List<long>> _ticksMeasurements;

        static PerformanceMonitor()
        {
            Reset();
        }

        public static void Reset()
        {
            _ticksMeasurements = new Dictionary<ActionType, List<long>>();
            foreach (ActionType enumValue in Enum.GetValues(typeof(ActionType)))
            {
                _ticksMeasurements[enumValue] = new List<long>();
            }
        }

        public static void Measure(ActionType actionType, Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            _ticksMeasurements[actionType].Add(stopwatch.ElapsedTicks);
        }

        public static PerformanceReport GetPerformanceReport()
        {
            return new PerformanceReport(_ticksMeasurements.ToDictionary(x => x.Key, x => CountStats(x.Value)));
        }

        private static PerformanceStats CountStats(List<long> measurements)
        {
            return new PerformanceStats(
                TimeSpan.FromTicks(measurements.Sum()),
                TimeSpan.FromTicks(measurements.Min()),
                TimeSpan.FromTicks(measurements.Max()),
                TimeSpan.FromTicks((long) measurements.Average()));
        }
    }
}
