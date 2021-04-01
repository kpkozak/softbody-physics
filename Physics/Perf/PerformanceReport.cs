using System.Collections.Generic;
using System.Text;

namespace Physics.Perf {
    public class PerformanceReport
    {
        public PerformanceReport(Dictionary<ActionType, PerformanceStats> statistics)
        {
            Statistics = statistics;
        }
        public Dictionary<ActionType, PerformanceStats> Statistics { get; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var performanceStatistic in Statistics)
            {
                stringBuilder.AppendLine(
                    $"{performanceStatistic.Key}: total {performanceStatistic.Value.Total}, avg: {performanceStatistic.Value.Average.TotalMilliseconds} ms, max: {performanceStatistic.Value.Max.TotalMilliseconds} ms, min: {performanceStatistic.Value.Min.TotalMilliseconds} ms");
            }
            return stringBuilder.ToString();
        }
    }
}