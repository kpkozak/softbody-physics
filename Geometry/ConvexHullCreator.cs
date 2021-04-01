using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;

namespace Geometry
{
    public class ConvexHullCreator
    {
        private Vector3 pol_min;

        public Polygon GrahamScan(IList<Vector2> initialPoints)
        {
            return new Polygon(GrahamScanCompute(initialPoints).ToArray());
        }

        public static IList<Vector2> GrahamScanCompute(IList<Vector2> initialPoints)
        {
            var points = SortPoints(initialPoints);
 
            var m = 1;
            for (var i = 2; i < points.Count; i++)
            {
                while (Ccw(points[m - 1], points[m], points[i]))
                {
                    if (m > 1)
                    {
                        points.RemoveAt(m);
                        m -= 1;
                        i--;
                    }
                    else if (i == points.Count - 1)
                        break;
                    else
                        i += 1;
                }
                m += 1;
            }
            return points;
        }

        public static List<Vector2> SortPoints(IList<Vector2> initialPoints)
        {
            var iMin =
                Enumerable.Range(0, initialPoints.Count).Aggregate((jMin, jCur) => initialPoints[jCur].Y < initialPoints[jMin].Y
                    ? jCur
                    : (initialPoints[jCur].Y > initialPoints[jMin].Y
                        ? jMin
                        : (initialPoints[jCur].X < initialPoints[jMin].X ? jCur : jMin)));

            var sorted = Enumerable.Range(0, initialPoints.Count)
                .Where(i => (i != iMin)) 
                .Select(
                    i =>
                        new KeyValuePair<double, Vector2>(
                            Math.Atan2(initialPoints[i].Y - initialPoints[iMin].Y,
                                initialPoints[i].X - initialPoints[iMin].X), initialPoints[i]))
                .OrderBy(pair => pair.Key)
                .Select(pair => pair.Value);

            var points = new List<Vector2>(initialPoints.Count);
            points.Add(initialPoints[iMin]);
            points.AddRange(sorted);
            return points;
        }

        public static bool Ccw(Vector2 a, Vector2 b, Vector2 c)
        {
            return AreaCalculator.Area2(a, b, c) < 0;
        }
    }
}