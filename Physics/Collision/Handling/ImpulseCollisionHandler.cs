using System.Diagnostics;
using System.Linq;
using Geometry;
using Geometry.Vector;
using Physics.Bodies;
using Physics.Bodies.Materials;
using Physics.Collision.Detection;

namespace Physics.Collision.Handling
{
    internal class ImpulseCollisionHandler : ICollisionHandler
    {
        private static double MinimalVelocityToBounce = 10;
        private readonly DistanceConstraintResolver _distanceConstraintResolver;

        public ImpulseCollisionHandler(DistanceConstraintResolver distanceConstraintResolver)
        {
            _distanceConstraintResolver = distanceConstraintResolver;
        }

        public void HandleCollision(object sender, CollisionArgs e)
        {
            var r1 = Vector2.Zero;
            var r2 = Vector2.Zero;
            foreach (var collisionPoint in e.Points)
            {
                r1 = r1 + e.Points.Last() - e.Object1.Position;    
                r2 = r2 + e.Points.Last() - e.Object2.Position;
                ResolveBounce(e, collisionPoint);
            }

            r1 *= 1d / e.Points.Length;
            r2 *= 1d / e.Points.Length;
            ResolveFriction(e, r1, r2);

            ResolveInterpenetration(e);
        }

        private static Vector2 CalculateRelativeVelocity(Body body, Body other, Vector2 collisionPoint)
        {
            var ra = collisionPoint - body.Position;
            var rb = collisionPoint - other.Position;

            var v2 = other.GetVelocityOfPoint(rb);
            var v1 = body.GetVelocityOfPoint(ra);
            return v2 - v1;
        }

        private static void ResolveBounce(CollisionArgs e, Vector2 point)
        {
            var r1 = point - e.Object1.Position;
            var r2 = point - e.Object2.Position;
            var totalInverseMass = CountTotalInverseMass(e, r1, r2); // (1)

            var relativeVelocity = CalculateRelativeVelocity(e.Object2, e.Object1, point); 
            var normalEnclosingVelocity = relativeVelocity.Dot(e.Normal); // (2)
            if (normalEnclosingVelocity < 0)
                return;

            var restitution = normalEnclosingVelocity > MinimalVelocityToBounce
                ? MaterialExtensions.GetResultingRestitution(e.Object1.Material, e.Object2.Material)
                : 0; // (3)
            var newSeparationVelocity = -normalEnclosingVelocity * restitution;
            var deltaVelocity = newSeparationVelocity - normalEnclosingVelocity;

            var impulse = deltaVelocity / totalInverseMass;
            var impulsePerInverseMassUnit = e.Normal * impulse; // (4)

            e.Object1.ApplyImpulse(impulsePerInverseMassUnit, r1); // (5)
            e.Object2.ApplyImpulse(-impulsePerInverseMassUnit, r2);
        }

        private static double CountTotalInverseMass(CollisionArgs e, Vector2 r1, Vector2 r2)
        {
            var totalInverseMass = 1 / e.Object1.GetMass(r1) + 1 / e.Object2.GetMass(r2) +
                                   e.Object1.GetInverseMomentOfInertia(r1, e.Normal) +
                                   e.Object2.GetInverseMomentOfInertia(r2, e.Normal);
            return totalInverseMass;
        }

        private static void ResolveFriction(CollisionArgs e,
            Vector2 r1, Vector2 r2)
        {
            
            var totalInverseMass = CountTotalInverseMass(e, r1, r2);
            var relativeVelocity = CalculateRelativeVelocity(e.Object2, e.Object1, e.Points.First());
            var frictionFactor = MaterialExtensions.GetResultingFriction(e.Object1.Material, e.Object2.Material);
            var tangentVelocity = relativeVelocity - relativeVelocity.Dot(e.Normal) * e.Normal;
            var frictionImpulse = -tangentVelocity * frictionFactor;
            
            var frictionPerInverseMassUnit = frictionImpulse * (1 / totalInverseMass);
            e.Object1.ApplyImpulse(frictionPerInverseMassUnit, r1);
            e.Object2.ApplyImpulse(-frictionPerInverseMassUnit, r2);
        }

        private void ResolveInterpenetration(CollisionArgs eventArgs)
        {
            if (eventArgs.Interpenetration.Length < Config.Epsilon)
                return;

            var body1 = (eventArgs.Object1 as SoftBody)?.GetClosestBody(eventArgs.Points.First()) ?? eventArgs.Object1;
            var body2 = (eventArgs.Object2 as SoftBody)?.GetClosestBody(eventArgs.Points.First()) ?? eventArgs.Object2;

            _distanceConstraintResolver.ResolveDistanceConstraint(body1, body2,
                eventArgs.Normal, eventArgs.Interpenetration.Length);
        }
    }
}