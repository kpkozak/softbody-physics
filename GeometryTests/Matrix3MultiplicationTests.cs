using System.Collections.Generic;
using Geometry.Matrix;
using NUnit.Framework;

namespace PhysicsTests
{
    [TestFixture]
    public class Matrix3MultiplicationTests
    {
        private IEnumerable<TestCaseData> TestCases()
        {
            var a = Matrix3.Create(
                1, 2, 3,
                4, 5, 6,
                7, 8, 9);
            var b = Matrix3.Create(
                10,11,12,
                13,14,15,
                16,17,18);
            var result = Matrix3.Create(
                84, 90, 96,
                201, 216, 231,
                318, 342, 366);
            yield return new TestCaseData(a, b, result);

            a = Matrix3.Create(
                1.2, 2.8, 11.6, 
                4, 18, 72.5, 
                6.33, 8, 12);
            b = Matrix3.Create(
                8,1,22,
                13.3,62,41.5,
                69,1.5,1.8);
            result = Matrix3.Create(
                847.24,192.2,163.48,
                5273.9,1228.75,965.5,
                985.04,520.32999999,492.86);
            yield return new TestCaseData(a, b, result);
        }

        [Test,TestCaseSource(nameof(TestCases))]
        public void MatricesAreMultiplicatedCorrectly(Matrix3 matrix, Matrix3 other, Matrix3 expected)
        {
            var result = matrix * other;

            Assert.That(result,Is.EqualTo(expected));
        }
    }
}