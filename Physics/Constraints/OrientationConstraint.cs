using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Constraints
{
    public class OrientationConstraint:IConstraint
    {
        public RigidBody BodyA { get; }
        public RigidBody BodyB { get; }
        private double _efficientMass;
        private double _bias;
        private readonly double _relativeRotation;

        public static OrientationConstraint Create(RigidBody bodyA, RigidBody bodyB)
        {
            return new OrientationConstraint(bodyA, bodyB, bodyB.Rotation - bodyA.Rotation);
        }

        private OrientationConstraint(RigidBody bodyA, RigidBody bodyB, double relativeRotation)
        {
            BodyA = bodyA;
            BodyB = bodyB;
            _relativeRotation = relativeRotation;
        }

        public void Prepare()
        {
            _efficientMass = 1 / (BodyA.InverseMomentOfInertia + BodyB.InverseMomentOfInertia);
            _bias = Constants.ConstraintBias / Constants.DeltaT * (BodyB.Rotation - BodyA.Rotation-_relativeRotation); 
        }
        public void Resolve()
        {
            Prepare();

            var relativeAngularVelocity = BodyB.AngularVelocity - BodyA.AngularVelocity;
            var j = -_efficientMass * (relativeAngularVelocity + Vector3.ZeroCoords * _bias);

            BodyA.AngularVelocity -= BodyA.InverseMomentOfInertia * j;
            BodyB.AngularVelocity += BodyB.InverseMomentOfInertia * j;
        }
    }
}
