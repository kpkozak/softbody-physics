using System;
using System.Collections.Generic;
using System.Linq;
using Geometry;
using Geometry.Matrix;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies.Materials;
using Physics.Force.Fields;

namespace Physics.Bodies
{
    public class RigidBody : Body
    {
        // todo on mass set recount inertia
        public static RigidBodyBuilder.IMassRigidBodyBuilder Create()
        {
            return new RigidBodyBuilder(UpdateBehavior.PhysicsEnabled);
        }

        public static RigidBodyBuilder.ILocationRigidBodyBuilder CreateStatic()
        {
            return new RigidBodyBuilder(UpdateBehavior.Static)
                .WithMass(Double.PositiveInfinity);
        }

        public override UpdateBehavior BehaviorType { get; }
        public override uint CollisionLayer { get; set; }
        private Shape _shape;
        public Vector2 Velocity { get; set; }
        public override double Mass { get; }

        internal RigidBody(UpdateBehavior behaviorType, double mass, double inertia, Vector2 position, double rotation,
            Vector2 velocity, double angularVelocity, Shape shape, Material material, uint collisionLayer,
            IEnumerable<IForceField> customForceFields)
        {
            BehaviorType = UpdateBehavior.PhysicsEnabled;
            Rotation = rotation;
            _shape = shape;
            BehaviorType = behaviorType;
            Mass = mass;
            MomentOfInertia = inertia;
            Position = position;
            Velocity = velocity;
            AngularVelocity = new Vector3(0, 0, angularVelocity);
            Material = material;
            CollisionLayer = collisionLayer;
            ForceFields = customForceFields.ToList();

        }

        // TODO ensure that only Z is non-zero
        public Vector3 AngularVelocity { get; set; }

        public IList<IForceField> ForceFields { get; set; }

        public override Shape Shape
        {
            get { return _shape; }
            set
            {
                if (value is Polygon)
                {
                    value = (value as Polygon).AdjustToMassCenter();
                }
                _shape = value;
                MomentOfInertia = _shape.CountInertia(Mass, Vector2.Zero);
            }
        }

        public override double InverseMass
        {
            get { return 1 / Mass; }
        }

        public override double InverseMomentOfInertia
        {
            get { return 1 / MomentOfInertia; }
        }

        public override Material Material { get; set; }

        /// <summary>
        /// Applying impulse. Impulse is force*dt (?)
        /// </summary>
        /// <param name="impulse"></param>
        /// <param name="r"></param>
        public override void ApplyImpulse(Vector2 impulse, Vector2 r)
        {
            Velocity += impulse * InverseMass;
            AngularVelocity += r.Cross(impulse) * InverseMomentOfInertia;
        }

        internal override Vector2 GetVelocityOfPoint(Vector2 point)
        {
            return Velocity + (Vector2) AngularVelocity.Cross(point.AsVector3());
        }

        public double GetEffectiveMass(Vector2 r, Vector2 axis)
        {
            var d = r.Cross(axis).Length;
            return Mass + MomentOfInertia * d * d;
        }

        internal override void Update(double deltaT)
        {
            if (BehaviorType != UpdateBehavior.PhysicsEnabled) return;

            var resultantForce = CountResultantForce();

            var acceleration = resultantForce * (1 / Mass);
            Velocity = Velocity + (acceleration * (deltaT)); // + _tempLinearAccelration;
            Position = Position + (Velocity * (deltaT));

            Rotation += AngularVelocity.Z * deltaT;
        }

        private Vector2 CountResultantForce()
        {
            var forceFieldsResult = ForceFields.Select(x => x.GetForce(this).Value)
                .Concat(_globalForceFields.Select(x => x.GetForce(this).Value))
                .Aggregate(Vector2.Zero, (x, y) => x + y);
            return forceFieldsResult;
        }

        public override Matrix3 GetTransformMatrix()
        {
            return Matrix3.Translation(Position) * Matrix3.Rotation(Rotation);
        }

        public override Matrix3 GetRotationMatrix()
        {
            return Matrix3.Rotation(Rotation);
        }

        internal override double GetMass(Vector2 point)
        {
            return Mass;
        }

        internal override double GetInverseMomentOfInertia(Vector2 point, Vector2 axis)
        {
            // todo to chyba jest niepoprawne?
            return Math.Pow(point.Cross(axis).Length, 2) * InverseMomentOfInertia;
        }
    }
}