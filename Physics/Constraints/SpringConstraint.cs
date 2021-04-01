using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Constraints
{
    public class SpringConstraint : IConstraint
    {
        public RigidBody BodyA{get;}
        public RigidBody BodyB { get; }
        public double Length { get; }
        public bool IsRigid { get; }

        private readonly double _damping;
        private readonly double _frequency;

        public Vector2 AAnchorPoint { get; }
        public Vector2 BAnchorPoint { get; }
        private Vector2 _bias;

        private Vector2 _globalRa;
        private Vector2 _globalRb;

        private Vector2 _i;

        private double _kinv;
        private Vector2 _n;
        private double _softness;

        private SpringConstraint(RigidBody bodyA, RigidBody bodyB, Vector2 aAnchorPoint, Vector2 bAnchorPoint, double length, bool isRigid,
            double frequency, double damping)
        {
            BodyA = bodyA;
            BodyB = bodyB;
            AAnchorPoint = aAnchorPoint;
            BAnchorPoint = bAnchorPoint;
            Length = length;
            IsRigid = isRigid;
            _frequency = frequency;
            _damping = damping;
        }

        public void Prepare()
        {
            var ma = BodyA.GetRotationMatrix();
            var mb = BodyB.GetRotationMatrix();

            _globalRa = ma * AAnchorPoint;
            _globalRb = mb * BAnchorPoint;

            var distanceVec = CountDistance(BodyA.Position, _globalRa, BodyB.Position, _globalRb);
            var distance = distanceVec.Length;

            _n = distanceVec.Normalize();

            var gran = _globalRa.Cross(_n).Z;
            var grbn = _globalRb.Cross(_n).Z;

            // TODO remove duplicate
            var k = BodyA.InverseMass + BodyB.InverseMass + BodyA.InverseMomentOfInertia * gran * gran +
                    BodyB.InverseMomentOfInertia * grbn * grbn;
            _kinv = 1 / k;

            var x = distance - Length;
            var springk = _kinv * _frequency * _frequency;
            var springc = 2 * _kinv * _damping * _frequency;

            _softness = 1 / (Constants.DeltaT * (springc + Constants.DeltaT * springk));
            _bias = x * Constants.DeltaT * springk * _softness * _n;

            _kinv = 1 / (k + _softness);
            _i = Vector2.Zero;
        }

        public void Resolve()
        {
            // todo duplicated code
            var vab = (BodyB.Velocity.AsVector3() + BodyB.AngularVelocity.Cross(_globalRb.AsVector3()) - BodyA.Velocity.AsVector3() -
                       BodyA.AngularVelocity.Cross(_globalRa.AsVector3())).Dot(_n.AsVector3()) *
                      _n;
            var j = -_kinv * (vab + _bias + _i * _softness);

            if (!IsRigid && j.Dot(_n) > 0)
                return;

            BodyA.ApplyImpulse(-j, _globalRa);
            BodyB.ApplyImpulse(j, _globalRb);

            _i = _i + j;
        }

        public static SpringConstraint Create(RigidBody bodyA, Body bodyB, Vector2 aAnchorPoint,
            Vector2 bAnchorPoint, DistanceConstraint.Type type, double freq, double damp)
        {
            var rotatedRa = bodyA.GetRotationMatrix() * aAnchorPoint;
            var rotatedRb = bodyB.GetRotationMatrix() * bAnchorPoint;
            var springLength = CountDistance(bodyA.Position, rotatedRa, bodyB.Position, rotatedRb);
            return new SpringConstraint(bodyA, bodyB as RigidBody, aAnchorPoint, bAnchorPoint, springLength.Length,
                type == DistanceConstraint.Type.Rod, freq, damp);
        }


        private static Vector2 CountDistance(Vector2 positionA, Vector2 ra, Vector2 positionB, Vector2 rb)
        {
            return positionB + rb - positionA - ra;
        }
    }
}