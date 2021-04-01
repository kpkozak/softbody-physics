using System.Collections.Generic;
using Geometry.Shapes;
using Geometry.Vector;
using NUnit.Framework;
using Physics.Bodies;
using Physics.Collision.Detection;

namespace PhysicsTests.Physics.Collision.Detection
{
    [TestFixture]
    public class CirclePolygon2DColliderTests
    {
        [Test]
        [TestCaseSource(nameof(CollisionCases))]
        public void CollisionTest(Body body1, Body body2)
        {
            var collider = CreateCollider();
            CollisionArgs collision = null;
            collider.ObjectsColliding += (sender, args) => collision = args;

            collider.Collide(body1, body2);

            Assert.That(collision, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(nameof(NoCollisionCases))]
        public void NoCollisionTest(Body body1, Body body2)
        {
            var collider = CreateCollider();
            CollisionArgs collision = null;
            collider.ObjectsColliding += (sender, args) => collision = args;

            collider.Collide(body1, body2);

            Assert.That(collision, Is.Null);
        }

        private ICollider CreateCollider()
        {
            return new CirclePolygonCollider(new SATInterpenetrationChecker(new OneDimensionIntersectionChecker()));
        }

        private IEnumerable<TestCaseData> CollisionCases()
        {
            var circle1 = RigidBody.Create()
                .WithMass(0)
                .WithLocation((4, 2))
                .WithShape(new Circle(1.1))
                .Build();

            var circle2 = RigidBody.Create()
                .WithMass(0)
                .WithLocation((4, 4))
                .WithShape(new Circle(1.77))
                .Build();

            var circle3 = RigidBody.Create()
                .WithMass(0)
                .WithLocation((4, 4))
                .WithShape(new Circle(1.43))
                .Build();

            var circle4 = RigidBody.Create()
                .WithMass(0)
                .WithLocation((1.21, 3.67))
                .WithShape(new Circle(0.69))
                .Build();

            yield return new TestCaseData(square, circle1).SetName("1");
            yield return new TestCaseData(square, circle2).SetName("2");
            yield return new TestCaseData(square, circle3).SetName("3");
            yield return new TestCaseData(square, circle4).SetName("4");

            yield return new TestCaseData(square2, circle1).SetName("5");
            yield return new TestCaseData(square2, circle2).SetName("6");
            yield return new TestCaseData(square2, circle3).SetName("7");
            yield return new TestCaseData(square2, circle4).SetName("8");
        }

        private IEnumerable<TestCaseData> NoCollisionCases()
        {
            var circle1 = RigidBody.Create()
                .WithMass(0)
                .WithLocation((4, 2))
                .WithShape(new Circle(0.98))
                .Build();
            var circle2 = RigidBody.Create()
                .WithMass(0)
                .WithLocation((4, 4))
                .WithShape(new Circle(1.40))
                .Build();
            var circle3 = RigidBody.Create()
                .WithMass(0)
                .WithLocation((4, 4))
                .WithShape(new Circle(1.412))
                .Build();
            var circle4 = RigidBody.Create()
                .WithMass(0)
                .WithLocation((1.21, 3.67))
                .WithShape(new Circle(0.66))
                .Build();

            yield return new TestCaseData(square, circle1).SetName("1");
            yield return new TestCaseData(square, circle2).SetName("2");
            yield return new TestCaseData(square, circle3).SetName("3");
            yield return new TestCaseData(square, circle4).SetName("4");

            yield return new TestCaseData(square2, circle1).SetName("5");
            yield return new TestCaseData(square2, circle2).SetName("6");
            yield return new TestCaseData(square2, circle3).SetName("7");
            yield return new TestCaseData(square2, circle4).SetName("8");
        }

        private static readonly Body square = RigidBody.Create()
            .WithMass(0)
            .WithLocation((3, 3))
            .WithShape(Polygon.AARectangle(2, 2))
            .Build();

        private static readonly Body square2 = RigidBody.Create()
            .WithMass(0)
            .WithLocation((3, 3))
            .WithShape(new Polygon(new Vector2(-1, 1), new Vector2(-1, -1),
                new Vector2(1, -1),
                new Vector2(1, 1)
            ))
            .Build();
    }
}