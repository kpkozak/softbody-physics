using System.Collections.Generic;
using System.Linq;
using Geometry.Vector;

namespace Geometry.Shapes
{
    public static class PolygonUtils
    {
        // TODO tutaj juz zostalo zoptymalizowane - lazy enumerable
        public static IEnumerable<Vector2> GetEdgesNormals(this Polygon polygon)
        {
            var points = polygon.Points;
            foreach (var vector2 in GetEdgesNormals(points)) yield return vector2;
        }

        public static IEnumerable<Vector2> GetEdgesNormals(this Vector2[] points)
        {
            yield return (points[0] - points[points.Count() - 1]).GetNormalVector().Normalize();
            for (int i = 1; i < points.Count(); ++i)
            {
                yield return (points[i] - points[i - 1]).GetNormalVector().Normalize();
            }
        }

        public static IEnumerable<(Segment Segment, int AIndex, int BIndex)> GetEdges(this Polygon polygon)
        {
            var points = polygon.Points;
            return GetEdges(points);
        }

        public static IEnumerable<(Segment Segment, int AIndex, int BIndex)> GetEdges(this Vector2[] points)
        {
            yield return (Segment: new Segment(points[points.Length - 1], points[0]), AIndex: points.Length - 1, BIndex: 0);
            for (int i = 1; i < points.Length; ++i)
            {
                yield return (Segment: new Segment(points[i - 1], points[i]), AIndex: i - 1, BIndex: i);
            }
        }
    }
}