using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geometry.Matrix;
using Geometry.Vector;

namespace Geometry.Shapes
{
    public class Polygon :Shape
    {
        public readonly Vector2[] Points; 
        // TODO Assure immutability by getting IEnumerable and ToArray (or something similar)
        public Polygon(params Vector2[] points)
        {
            if(points.Length<3)
                throw new ArgumentException("Polygon must have at least 3 points");
            Points = points;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("Polygon2d: [");
            foreach (var point in Points)
            {
                builder.Append(point);
                builder.Append(", ");
            }
            builder.Append("]");
            return builder.ToString();
        }

        /// <summary> Create Axis Aligned rectangle </summary>
        public static Polygon AARectangle(double width, double height)
        {
            var xSize2 = width / 2;
            var ySize2 = height / 2;
            var points = new[]
            {
                new Vector2(-xSize2, -ySize2),
                new Vector2(xSize2, -ySize2),
                new Vector2(xSize2, ySize2),
                new Vector2(-xSize2, ySize2)
            };
            return new Polygon(points);
        }

        /// <summary> Creates regular (equiangular and equilateral) polygon  </summary>
        /// <param name="verticesCount">number of vertices</param>
        /// <param name="radius">distance between center and vertice</param>
        /// <exception cref="ArgumentException">Cannot create polygon from less than 3 vertices</exception>
        public static Polygon RegularPolygon(int verticesCount, double radius)
        {
            if (verticesCount < 3)
                throw new ArgumentException("Cannot create polygon from less than 3 vertices");

            return new Polygon(RegularPolygonVertices(verticesCount,radius).ToArray());
        }

        public static Polygon RandomPolygon(int verticesCount, double minRadius, double maxRadius)
        {
            if(verticesCount < 3)
                throw new ArgumentException("Cannot create polygon from less than 3 vertices");

            return new Polygon(RandomVertices(verticesCount, minRadius, maxRadius).ToArray());
        }

        private static Random _random = new Random();
        private static IEnumerable<Vector2> RandomVertices(int verticesCount, double minRadius, double maxRadius)
        {
            var matrix = Matrix3.Identity();
            
            for (var i = 0; i < verticesCount; ++i)
            {
                var radius = _random.NextDouble() * (maxRadius - minRadius) + minRadius;
                var vertex = new Vector2(radius, 0);
                yield return matrix*vertex;
                matrix = matrix * Matrix3.Rotation(2 * Math.PI / verticesCount);
            }
        }

        private static IEnumerable<Vector2> RegularPolygonVertices(int n, double r)
        {
            var matrix = Matrix3.Rotation(2 * Math.PI / n);
            var vertex = new Vector2(r,0);
            for (var i = 0; i < n; ++i)
            {
                yield return vertex;
                vertex = matrix * vertex;
            }
        }

        public static Polygon operator *(Matrix3 matrix, Polygon polygon)
        {
            return new Polygon(polygon.Points.Select(point => matrix * point).ToArray());
        }
    }
}