using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;

namespace Geometry
{
    public static class Triangulator
    {
        public static IEnumerable<Segment> GetTriangulationSegments(this Polygon polygon)
        {
            var polygonCopy = new Polygon((Vector2[]) polygon.Points.Clone());
            var triangles = Triangulate(polygonCopy.Points.Length, polygonCopy.Points);
            triangles.RemoveAt(triangles.Count - 1); // remove last because is not connected with diagonal
            return triangles.Select(x => new Segment(x.Points[0], x.Points[2]));
        }

        public static IEnumerable<(int indexA, int indexB, int indexC)> GetTriangulationIndices(this Vector2[] polygon)
        {
            var polygonCopy = new Polygon(polygon.ToArray());
            var triangles = Triangulate(polygonCopy.Points.Length, polygonCopy.Points);

            var vertexToIndexMap = new Dictionary<Vector2, int>();

            for (var i = 0; i < polygon.Length; i++)
                vertexToIndexMap[polygon[i]] = i;

            foreach (var triangle in triangles)
                yield return (vertexToIndexMap[triangle.Points[0]], vertexToIndexMap[triangle.Points[1]],
                    vertexToIndexMap[triangle.Points[2]]);
        }

        private static List<Polygon> Triangulate(int n, Vector2[] polygon)
        {
            var triangles = new List<Polygon>();
            if (n == 3)
                return new List<Polygon>
                {
                    new Polygon(polygon[0], polygon[1], polygon[2])
                }; // last three points. No need to split anymore

            if (n < 3) return triangles;

            for (var i = 0; i < n; i++)
            {
                var i1 = (i + 1) % n;
                var i2 = (i + 2) % n;
                if (!Diagonal(i, i2, n, polygon)) continue;

                triangles.Add(new Polygon(polygon[i], polygon[(i + 1) % n],
                    polygon[i2])); // (i, i2) - diagonal that split polygon

                ClipEar(i1, n, polygon);
                triangles.AddRange(Triangulate(n - 1, polygon));

                break;
            }
            return triangles;
        }

        private static bool Left(Vector2 a, Vector2 b, Vector2 c)
        {
            return AreaCalculator.Area2(a, b, c) > Config.Epsilon;
        }

        private static bool LeftOn(Vector2 a, Vector2 b, Vector2 c)
        {
            return AreaCalculator.Area2(a, b, c) >= 0;
        }

        private static bool Collinear(Vector2 a, Vector2 b, Vector2 c)
        {
            return Math.Abs(AreaCalculator.Area2(a, b, c)) < Config.Epsilon;
        }

        private static bool IntersectProp(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            return !Collinear(a, b, c) && !Collinear(a, b, d) && !Collinear(c, d, a) && !Collinear(c, d, b) &&
                   Left(a, b, c) ^ Left(a, b, d) && Left(c, d, a) ^ Left(c, d, b);
        }

        private static bool Between(Vector2 a, Vector2 b, Vector2 c)
        {
            return Collinear(a, b, c) && (Math.Abs(a.X - b.X) > Config.Epsilon
                       ? a.X <= c.X && c.X <= b.X || a.X >= c.X && c.X >= b.X
                       : a.Y <= c.Y && c.Y <= b.Y || a.Y >= c.Y && c.Y >= b.Y);
        }

        private static bool Intersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            return IntersectProp(a, b, c, d) || Between(a, b, c) || Between(a, b, d) || Between(c, d, a) ||
                   Between(c, d, b);
        }

        private static bool DiagonalIntExt(int i, int j, int n, Vector2[] polygon)
        {
            for (var k = 0; k < n; k++)
            {
                var k1 = (k + 1) % n;
                if (!(k == i || k1 == i || k == j || k1 == j))
                    if (Intersect(polygon[i], polygon[j], polygon[k], polygon[k1]))
                        return false;
            }
            return true;
        }

        private static bool InCone(int i, int j, int n, Vector2[] polygon)
        {
            var i1 = (i + 1) % n;
            var in1 = (i + n - 1) % n;
            return LeftOn(polygon[in1], polygon[i], polygon[i1])
                ? Left(polygon[i], polygon[j], polygon[in1]) &&
                  Left(polygon[j], polygon[i], polygon[i1])
                : !(LeftOn(polygon[i], polygon[j], polygon[i1]) &&
                    LeftOn(polygon[j], polygon[i], polygon[in1]));
        }

        private static bool Diagonal(int i, int j, int n, Vector2[] polygon)
        {
            return InCone(i, j, n, polygon) && DiagonalIntExt(i, j, n, polygon);
        }

        private static void ClipEar(int i, int n, Vector2[] polygon)
        {
            for (var k = i; k < n - 1; k++)
                polygon[k] = polygon[k + 1];
        }
    }
}