using System;
using System.IO;
using Physics.Perf;
using Samples.Samples;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO Consider using some structs instead of classes
            // todo Candidates:
            // Vector2
            // Line
            // Segment
            Console.Out.WriteLine("Wall benchmark:");
            var builder = new WallDemoBuilder();
            var physics = builder.CreateScene();
            physics.Add(builder.CreateBenchmarkObjects());

            physics.Update(10);

            var performanceReport = PerformanceMonitor.GetPerformanceReport();

            Console.Out.WriteLine(performanceReport);
            File.AppendAllText("perf-report.txt", $"----------\r\n{DateTime.Now}\r\nWall\r\n{performanceReport}");
        }
    }
}
