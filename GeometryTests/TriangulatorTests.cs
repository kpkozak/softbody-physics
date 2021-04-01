using System.Collections.Generic;
using System.Linq;
using Geometry;
using Geometry.Shapes;
using Geometry.Vector;
using NUnit.Framework;
using PostScriptImage;

namespace GeometryTests
{
    [TestFixture, Explicit]
    public class TriangulatorTests
    {
        [Test, TestCaseSource(nameof(TestCases))]
        
        public void SaveTestImages(Vector2[] vertices, string filename)
        {
            var polygon = new Polygon(vertices);
            var trianglesIndices = polygon.Points.GetTriangulationIndices();

            SaveImage(filename, polygon, trianglesIndices);
        }

        private static void SaveImage(string filename, Polygon polygon, IEnumerable<(int indexA, int indexB, int indexC)> trianglesIndices)
        {
            var image = new Image(Vector2.Zero,
                new Vector2(polygon.Points.Max(point => point.X)+25, polygon.Points.Max(point => point.Y)+25));

            image.AddPolygon(polygon);

            image.SetLineColor(1, 0, 0);
            foreach (var diagonal in trianglesIndices)
            {
                image.AddSegment(polygon.Points[diagonal.indexA], polygon.Points[diagonal.indexB]);
                image.AddSegment(polygon.Points[diagonal.indexA], polygon.Points[diagonal.indexC]);
                image.AddSegment(polygon.Points[diagonal.indexB], polygon.Points[diagonal.indexC]);
            }
            image.Save(filename);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(rectangle, "rectangle.ps").SetName("rectangle");
            yield return new TestCaseData(complexPolygon, "complexPolygon.ps").SetName("complexPolygon");
        }

        private static Vector2[] complexPolygon =
        {
            (200, 600),
            (400, 400),
            (495, 198),
            (725, 82),
            (950,82),
            (1000, 200),
            (1185, 264),
            (1296, 394),
            (1219, 544),
            (998,573),
            (1139,711),
            (1224,886),
            (1073,968),
            (812,936),
            (742,736),
            (660,832),
            (529,985),
            (309,840),
            (333,707),
            (188,738)
        };

        private static Vector2[] rectangle =
        {
            (120, 140),
            (160, 140),
            (160, 180),
            (120, 180)
        };
    }
}
