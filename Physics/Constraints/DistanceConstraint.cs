using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Constraints
{
    public class DistanceConstraint : IConstraint
    {
        public enum Type
        {
            Rod,
            Rope
        }
    
        public Vector2 AAnchorPoint { get; }
        public Vector2 BAnchorPoint { get; }
        public Body BodyA { get; }
        public Body BodyB { get; }
        public bool IsRigid { get; }
        public double MaxDistance { get; }

        private Vector2 _bias;
        private double _efficientInverseMass;

        private Vector2 _forbiddenMoveDir;
        private Vector2 _ra;
        private Vector2 _rb;

        private DistanceConstraint(Body bodyA, Body bodyB, Vector2 aAnchorPoint, Vector2 bAnchorPoint,
            double maxDistance, bool isRigid)
        {
            BodyA = bodyA;
            BodyB = bodyB;
            AAnchorPoint = aAnchorPoint;
            BAnchorPoint = bAnchorPoint;
            MaxDistance = maxDistance;
            IsRigid = isRigid;
        }

        public void Prepare()
        {
            // Barnas, str 91

            // ramiona
            _ra = BodyA.GetRotationMatrix() * AAnchorPoint;
            _rb = BodyB.GetRotationMatrix() * BAnchorPoint;

            var anchorPointsDistance = CountDistance(BodyA.Position, _ra, BodyB.Position, _rb);
            var distance = anchorPointsDistance.Length;
            _forbiddenMoveDir = anchorPointsDistance.Normalize();

            // effective mass
            var globalRan = _ra.Cross(_forbiddenMoveDir).Z;
            var globalRbn = _rb.Cross(_forbiddenMoveDir).Z;

            var k = BodyA.InverseMass + BodyB.InverseMass +
                    BodyA.InverseMomentOfInertia * globalRan * globalRan +
                    BodyB.InverseMomentOfInertia * globalRbn * globalRbn;

            _efficientInverseMass = 1 / k;
            _bias = Constants.ConstraintBias / Constants.DeltaT * (distance - MaxDistance) * _forbiddenMoveDir;
        }

        public void Resolve()
        {
            var vab = (BodyB.GetVelocityOfPoint(BAnchorPoint)-
                       BodyB.GetVelocityOfPoint(AAnchorPoint)).AsVector3()
                      .Dot(_forbiddenMoveDir.AsVector3()) *
                      _forbiddenMoveDir;
            var impulsePerInverseMass = -_efficientInverseMass * (vab + _bias);

            if (!IsRigid && impulsePerInverseMass.Dot(_forbiddenMoveDir) > 0)
                return;

            BodyA.ApplyImpulse(-impulsePerInverseMass, _ra);
            BodyB.ApplyImpulse(impulsePerInverseMass, _rb);
        }

        public static DistanceConstraint Create(RigidBody bodyA, Body bodyB, Vector2 aAnchorPoint,
            Vector2 bAnchorPoint, Type type)
        {
            var rotatedRa = bodyA.GetRotationMatrix() * aAnchorPoint;
            var rotatedRb = bodyB.GetRotationMatrix() * bAnchorPoint;
            var maxDistance = CountDistance(bodyA.Position, rotatedRa, bodyB.Position, rotatedRb);
            return new DistanceConstraint(bodyA, bodyB, aAnchorPoint, bAnchorPoint, maxDistance.Length,
                type == Type.Rod);
        }

        private static Vector2 CountDistance(Vector2 positionA, Vector2 ra, Vector2 positionB, Vector2 rb)
        {
            return positionB + rb - positionA - ra;
        }
    }
}