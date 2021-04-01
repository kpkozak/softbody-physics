using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;
using Logging;
using PostScriptImage;

namespace Physics.Collision.Detection
{
    class CollisionPointsFinder
    {
        private readonly ILogger _logger;

        public CollisionPointsFinder(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<Vector2> GetCollisionPoints(Vector2[] globalBoundsShape1, Vector2[] globalBoundsShape2)
        {
            // todo wydajnosc List, array itd.
            //  TODO może dałoby sie to zoptymalizowac do lepszego czasu niz to co jest ... ?
            var points = new List<Vector2>();
            foreach (var point in globalBoundsShape1)
            {
                if (IsInPolygon(point, globalBoundsShape2))
                {
                    points.Add(point);
                }
            }
            foreach (var point in globalBoundsShape2)
            {
                if (IsInPolygon(point, globalBoundsShape1))
                    points.Add(point);
            }
            return points;
        }

        private void LogErrorDetails(Vector2[] translatedPolygon, Vector2[] translatedOther)
        {
            var maxX = translatedPolygon.Concat(translatedOther).Max(x => x.X);
            var maxY = translatedPolygon.Concat(translatedOther).Max(y => y.Y);
            var image = new Image(Vector2.Zero, new Vector2(maxX, maxY));
            image.AddPolygon(new Polygon(translatedPolygon));
            image.AddPolygon(new Polygon(translatedOther));
            image.Save(string.Format("{0}.ps", DateTime.Now.ToString("ddMMyyyyHHmmss")));
        }

        private bool IsInPolygon(Vector2 point, Vector2[] other)
        {
            //http://csharphelper.com/blog/2014/07/determine-whether-a-point-is-inside-a-polygon-in-c/
            return pnpoly(other.Length, other.Select(x => x.X).ToArray(), other.Select(x => x.Y).ToArray(), point.X,
                point.Y);
        }

        /// <summary>
        /// https://www.codeproject.com/Tips/84226/Is-a-Point-inside-a-Polygon
        /// </summary>
        /// <returns></returns>
        bool pnpoly(int nvert, double[] vertx, double[] verty, double testx, double testy)
        {
            int i, j;
            bool c = false;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((verty[i] > testy) != (verty[j] > testy)) &&
                 (testx < (vertx[j] - vertx[i]) * (testy - verty[i]) / (verty[j] - verty[i]) + vertx[i]))
                    c = !c;
            }
            return c;
        }
    }
}