using System;
using NUnit.Framework;
using Physics.Collision.Detection;

namespace PhysicsTests.Collision.Detection
{
    [TestFixture]
    public class IntersectionCheckerTests
    {
        [Test]
        [TestCase(100,200,180,300,true)]
        [TestCase(1, 2, 1.8, 3.0,true)]
        [TestCase(1, 1.1,0.999,1.001, true)]
        [TestCase(1, 1.01, 1.0001, 1.001, true)]
        [TestCase(1, 1.01, 0.99999, 1.010001, true)]
        [TestCase(1, 1.01, 1.0101, 1.0111, false)]
        [TestCase(1, 1.01, 0.99, 0.999, false)]
        public void ResultsAreCorrect(double min1, double max1, double min2, double max2, bool expectedResult)
        {
            var checker = new OneDimensionIntersectionChecker();

            Assert.That(checker.AreIntersecting(min1, max1, min2, max2), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(1,0.99, 1,1.1)]
        [TestCase(1, 1.1, 1.11, 1.1)]
        [TestCase(1, 0.99, 1.11, 1.1)]
        public void ExceptionThrownOnBadArguments(double min1, double max1, double min2, double max2)
        {
            var checker = new OneDimensionIntersectionChecker();

            Assert.Throws<ArgumentException>(() => checker.AreIntersecting(min1, max1, min2, max2));
        }

        [Test]
        [TestCase(1,2,1.5,2.5,0.5)]
        [TestCase(1,2,1.96,2.05,0.04)]
        [TestCase(1,2,0.96,1.09,0.09)]
        [TestCase(1,2,0.00096,1.00001,0.00001)]
        [TestCase(1,2,1.1,1.14,0.04)]
        [TestCase(1,2,1.0012,1.0014,0.0002)]
        [TestCase(1,2,0,3,1)]
        [TestCase(1,2,0.0000099,2.0000001,1)]
        public void ProperInterpenetrationIsCount(double min1, double max1, double min2, double max2, double exectedResult)
        {
            var checker = new OneDimensionIntersectionChecker();

            var result = checker.CountInterpenetration(min1, max1, min2, max2);

            Assert.That(result, Is.EqualTo(exectedResult).Within(0.00001));
        }

        [Test]
        [TestCase(1, 1.01, 1.0101, 1.0111)]
        [TestCase(1, 1.01, 0.99, 0.999)]
        public void NotAPositiveNumberWhenNotIntersecting(double min1, double max1, double min2, double max2)
        {
            var checker = new OneDimensionIntersectionChecker();

            var result = checker.CountInterpenetration(min1, max1, min2, max2);

            AssertThatNotPositiveNumber(result);
        }

        private void AssertThatNotPositiveNumber(double result)
        {
            Assert.That(result, Is.Not.Positive);
        }

        [Test]
        [TestCase(1, 0.99, 1, 1.1)]
        [TestCase(1, 1.1, 1.11, 1.1)]
        [TestCase(1, 0.99, 1.11, 1.1)]
        public void ExceptionThrownOnBadArgumentsForInterpenetrationCount(double min1, double max1, double min2, double max2)
        {
            var checker = new OneDimensionIntersectionChecker();

            Assert.Throws<ArgumentException>(() => checker.CountInterpenetration(min1, max1, min2, max2));
        }
    }
}
