using System;
using System.Collections.Generic;
using Geometry;
using Geometry.Shapes;
using Geometry.Vector;
using NUnit.Framework;

namespace GeometryTests
{
    [TestFixture]
    class PolygonIntegralTests
    {
        [Test]
        [TestCaseSource(nameof(TriangleTestCases))]
        public void TriangleIntegralTest(Vector2 A, Vector2 B, Vector2 C, double expectedArea)
        {
            var area = PolygonIntegral.IntegralOnTriangle(A, B, C, (dPhi, h) => Math.Tan(dPhi) * h * h / 2);
            Assert.That(area, Is.EqualTo(expectedArea).Within(0.1).Percent);
        }

        // todo tests on other polygons
        [Test]
        [TestCaseSource(nameof(TriangleTestCases))]
        public void PolygonIntegralTestOnTriangles(Vector2 A, Vector2 B, Vector2 C, double expectedArea)
        {
            var area = PolygonIntegral.IntegralOnPolygon(new Polygon(A, B, C), 1d / 3 * (A + B + C),
                (dPhi, h) => Math.Tan(dPhi) * h * h / 2);
            Assert.That(area, Is.EqualTo(expectedArea).Within(0.1).Percent);
        }

        [Test]
        [TestCaseSource(nameof(PolygonsTestCases))]
        public void PolygonIntegralTest(Polygon polygon, Vector2 center, double expected)
        {
            var area = PolygonIntegral.IntegralOnPolygon(polygon, center, (dPhi, h) => 1d / 2 * h * Math.Tan(dPhi) * h);

            Assert.That(area, Is.EqualTo(expected).Within(3).Percent);
        }
        
        private IEnumerable<TestCaseData> PolygonsTestCases()
        {
            yield return new TestCaseData(Polygon.RegularPolygon(6, 20), Vector2.Zero, 3 * 20 * 20 * Math.Sqrt(3) / 2)
                .SetName("Polygon - 6 - 1");
            yield return new TestCaseData(Polygon.RegularPolygon(6, 20), new Vector2(2, 2),
                3 * 20 * 20 * Math.Sqrt(3) / 2).SetName("Polygon - 6 - 2");
        }

        //integral calculation does not work on concave polygons
        //            yield return new TestCaseData(
        //                new Polygon(
        //                    (0, 0),
        //                    (0, 2),
        //                    (2, 3),
        //                    (2, 2),
        //                    (1, 1.5),
        //                    (2, 1),
        //                    (2, 0)),
        //                new Vector2(1, 1),
        //                4.5).SetName("Concave polygon 1");

        private IEnumerable<TestCaseData> TriangleTestCases()
        {
            yield return new TestCaseData(new Vector2(0, 0), new Vector2(2, 2), new Vector2(3, 0), 3);
            yield return new TestCaseData(new Vector2(6, 4), new Vector2(10, 6), new Vector2(14, 2), 12);
        }

    }
}
