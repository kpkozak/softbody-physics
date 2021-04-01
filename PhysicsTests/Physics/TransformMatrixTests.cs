using System;
using System.Collections.Generic;
using Geometry.Shapes;
using Geometry.Vector;
using NUnit.Framework;
using Physics.Bodies;

namespace PhysicsTests.Physics
{
    [TestFixture]
    public class TransformMatrixTests
    {
        private static IEnumerable<TestCaseData> TestCases()
        {
            // translation only
            yield return new TestCaseData(new Vector2(100, 0), 0, new Vector2(0, 0), new Vector2(100, 0))
                .SetName("Translation 1");
            yield return new TestCaseData(new Vector2(0, 120), 0, new Vector2(0, 0), new Vector2(0, 120))
                .SetName("Translation 2");
            yield return new TestCaseData(new Vector2(-78, 10), 0, new Vector2(26, 53), new Vector2(-52, 63))
                .SetName("Translation 3");

            // rotation only
            yield return new TestCaseData(new Vector2(0, 0), Math.PI, new Vector2(112, 0), new Vector2(-112, 0))
                .SetName("Rotation 1");
            yield return new TestCaseData(new Vector2(0, 0), Math.PI / 2, new Vector2(86, 0), new Vector2(0, 86))
                .SetName("Rotation 2");
            yield return new TestCaseData(new Vector2(0, 0), -Math.PI / 2, new Vector2(20, 0),
                new Vector2(0, -20)).SetName("Rotation 3");
            yield return new TestCaseData(new Vector2(0, 0), -Math.PI / 2, new Vector2(0, -12),
                new Vector2(-12, 0)).SetName("Rotation 4");
            yield return new TestCaseData(new Vector2(0, 0), -Math.PI / 2, new Vector2(14.1421, 14.1421),
                new Vector2(14.1421, -14.1421)).SetName("Rotation 7");
            yield return new TestCaseData(new Vector2(0, 0), -Math.PI / 4, new Vector2(0, 20),
                new Vector2(14.1421, 14.1421)).SetName("Rotation 8");
            yield return new TestCaseData(new Vector2(0, 0), Math.PI / 4, new Vector2(-20, 0),
                new Vector2(-14.1421, -14.1421)).SetName("Rotation 9");

            // rotation & translation
            yield return new TestCaseData(new Vector2(100, 100), Math.PI, new Vector2(0, 0), new Vector2(100, 100))
                .SetName("Rotate&Transform");
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TransformationIsDoneCorrectly(Vector2 position, double rotation, Vector2 point,
            Vector2 expectedTranslatedPoint)
        {
            var body = RigidBody.Create()
                .WithMass(0)
                .WithLocation(position, rotation)
                .WithShape(Shape.Default)
                .Build();

            var matrix = body.GetTransformMatrix();

            var result = matrix * point;

            Assert.That(result, Is.EqualTo(expectedTranslatedPoint));
        }
    }
}