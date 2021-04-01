using System.Collections.Generic;
using Geometry.Shapes;
using Geometry.Vector;
using NUnit.Framework;

namespace PhysicsTests.Geometry
{
    [TestFixture]
    public class LinesIntersectionTests
    {
        // todo distanceto tests
        // todo parallel calculating tests
        // todo perpendicular calculating tests

        [Test]
        [TestCaseSource(nameof(IntersectionTestCases))]
        public void IntersectionPointIsCalculatedProperly(Line a, Line b, Vector2 expected)
        {
            var intersectionPoint = a.GetIntersectionPoint(b);
            Assert.That(intersectionPoint, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(NaNIntersectionTestCases))]
        public void PerpendicularIntersectionTest(Line a, Line b)
        {
            var result = a.GetIntersectionPoint(b);

            Assert.That(result.X, Is.NaN);
            Assert.That(result.Y, Is.NaN);
        }

        private IEnumerable<TestCaseData> IntersectionTestCases()
        {
            var ab = Line.Builder.CreateFromPoints(A, B);
            var bc = Line.Builder.CreateFromPoints(C, B);
            var ed = Line.Builder.CreateFromPoints(E, D);
            var fh = Line.Builder.CreateFromPoints(F,H);
            var gf = Line.Builder.CreateFromPoints(G, F);

            yield return new TestCaseData(ab, bc, B).SetName("ab, bc, B");
            yield return new TestCaseData(bc, ab, B).SetName("bc, ab, B");

            yield return new TestCaseData(ed, bc, new Vector2(4, 3.81771)).SetName("ed, bc, new Vector2(4, 3.82)");
            yield return new TestCaseData(bc, ed, new Vector2(4, 3.81771)).SetName("bc, ed, new Vector2(4, 3.82)");

            yield return new TestCaseData(fh, ed, new Vector2(4, 0)).SetName("fh, ed, new Vector2(4, 0)");
            yield return new TestCaseData(ed, fh, new Vector2(4, 0)).SetName("ed, fh, new Vector2(4, 0)");

            yield return new TestCaseData(gf, fh, F).SetName("gf, fh, F");
            yield return new TestCaseData(fh, gf, F).SetName("fh, gf, F");
        }

        private IEnumerable<TestCaseData> NaNIntersectionTestCases()
        {
            //perpendicular
            yield return new TestCaseData(Line.Builder.CreateFromPoints(E, D), Line.Builder.CreateFromPoints(G, F));
            yield return new TestCaseData(Line.Builder.CreateFromPoints(E, D), Line.Builder.CreateFromPoints(E, D));
            yield return new TestCaseData(Line.Builder.CreateFromPoints(new Vector2(0, 0), new Vector2(0, 1)),
                Line.Builder.CreateFromPoints(new Vector2(2, 0), new Vector2(2, 2)));

            yield return new TestCaseData(Line.Builder.CreateFromPoints(A, B),
                Line.Builder.CreateFromPoints(A + C, B + C));
        }

        private Vector2 A = new Vector2(2, 2);
        private Vector2 B = new Vector2(5.08, 2.46);
        private Vector2 C = new Vector2(2.28, 5.98);
        private Vector2 D = new Vector2(4, 1);
        private Vector2 E = new Vector2(4, 6);
        private Vector2 F = Vector2.Zero;
        private Vector2 G = new Vector2(0, 4);
        private Vector2 H = new Vector2(2, 0);
    }
}