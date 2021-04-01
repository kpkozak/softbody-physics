using System.Collections.Generic;
using Geometry;
using Geometry.Vector;
using NUnit.Framework;

namespace PhysicsTests
{
    [TestFixture]
    public class VectorTests
    {
        [Test, TestCaseSource("TestCases")]
        public void MultipliedLinearCourseEqualMinus1(Vector2 vector)
        {
            
            var normal = vector.GetNormalVector();

            //a1*a2 = -1 (vectors are perpendicular)
            Assert.That((normal.Y/normal.X)*(vector.Y/vector.X), Is.NaN.Or.EqualTo(-1));
        }

        [Test, TestCaseSource("TestCases")]
        public void DotProductEqualToZero(Vector2 vector)
        {
            var normal = vector.GetNormalVector();

            // Dot product (iloczyn skalarny) is equal to zero
            Assert.That(vector.Dot(normal), Is.EqualTo(0.0));
        }

        private IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData(new Vector2(1,0));
            yield return new TestCaseData(new Vector2(1, 1));
            yield return new TestCaseData(new Vector2(1, 2));
            yield return new TestCaseData(new Vector2(2, 1));
            yield return new TestCaseData(new Vector2(1.11, 3.85));
            yield return new TestCaseData(new Vector2(28.5, 325.12));
        }
    }
}
