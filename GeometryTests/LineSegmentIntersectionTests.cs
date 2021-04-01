using System.Collections.Generic;
using Geometry.Shapes;
using Geometry.Vector;
using NUnit.Framework;

namespace PhysicsTests.Geometry
{
    [TestFixture]
    public class LineSegmentIntersectionTests
    {
        [Test, TestCaseSource(nameof(IntersectedCases))]
        public void IntersectionPointIsCalculatedProperly(Line line, Segment segment, Vector2 expected)
        {
            var intersectionPoint = line.GetIntersectionPoint(segment);
            Assert.That(intersectionPoint, Is.EqualTo(expected));
        }

        [Test, TestCaseSource(nameof(NoIntersection))]
        public void NoIntersectionReturnsInvalidPoint(Line line, Segment segment)
        {
            var intersectionPoint = line.GetIntersectionPoint(segment);
            Assert.False(intersectionPoint.IsValid);
        }

        private IEnumerable<TestCaseData> IntersectedCases()
        {
            yield return new TestCaseData(AB.Line, CD, I);
            yield return new TestCaseData(CD.Line, AB, I);
        }

        private IEnumerable<TestCaseData> NoIntersection()
        {
            yield return new TestCaseData(AB.Line, EF);
            yield return new TestCaseData(AB.Line, GH);
            yield return new TestCaseData(CD.Line, EF);
            yield return new TestCaseData(EF.Line, CD);
            yield return new TestCaseData(GH.Line, AB);
        }

        private static Vector2 A = (-5.36, -2.09);
        private static Vector2 B = (-2.88, -3.25);
        private static Vector2 C = (-5.6, -4.31);
        private static Vector2 D = (-2.36, -2.51);
        private static Vector2 E = (-3.94, -6.59);
        private static Vector2 F = (-2.76, -4.71);
        private static Vector2 G = (-2, -1);
        private static Vector2 H = (-4, -1);
        private static Vector2 I = (-3.32084, -3.0438003);

        private Segment AB = new Segment(A, B);
        private Segment CD = new Segment(C, D);
        private Segment EF = new Segment(E, F);
        private Segment GH = new Segment(G, H);

    }
}