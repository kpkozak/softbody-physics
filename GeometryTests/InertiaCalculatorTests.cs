using System;
using System.Collections.Generic;
using Geometry;
using Geometry.Shapes;
using Geometry.Vector;
using NUnit.Framework;

namespace GeometryTests
{
    [TestFixture]
    class InertiaCalculatorTests
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void InertiaIsCalculatedProperly(Polygon polygon, double expected, Vector2 massCenter)
        {
            var inertia = polygon.CountGeometricInertia();
            Assert.That(inertia, Is.EqualTo(expected).Within(1).Percent);
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void MassCenterIsCalculatedProperly(Polygon polygon, double inertia, Vector2 expected)
        {
            var massCenter = polygon.CountCenterMass();

            Assert.That(massCenter, Is.EqualTo(expected).Using(TestVectorComparer.Default));
        }

        private IEnumerable<TestCaseData> TestCases()
        {
            var r1 = 10;
            yield return new TestCaseData(Polygon.RegularPolygon(6, r1), 5 * Math.Sqrt(3) / 8 * Math.Pow(r1, 4), Vector2.Zero);
            yield return new TestCaseData(Polygon.RegularPolygon(4, 50 / Math.Sqrt(2)), Math.Pow(50, 4) / 6,
                Vector2.Zero);

            var joinedHexagonsInertia = 7 * Math.Pow(r1, 4) * Math.Sqrt(3) / 2;
            yield return new TestCaseData(joinedHexagons, joinedHexagonsInertia, joinedHexagonsMassCenter);
        }

        private static readonly Vector2 joinedHexagonsMassCenter = (22.5,14.98);
        private static readonly Polygon joinedHexagons = new Polygon(
            (5,10.66),
            (10,2),
            (20,2),
            (25,10.66),
            (35,10.66),
            (40,19.32),
            (35,27.98),
            (25,27.98),
            (20,19.32),
            (10,19.32));
    }
}