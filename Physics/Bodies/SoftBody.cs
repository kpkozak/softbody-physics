using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Matrix;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies.Materials;
using Physics.Constraints;
using Physics.Force.Fields;

namespace Physics.Bodies
{
    public class SoftBody : Body
    {
        internal readonly RigidBody[] _objects;
        internal readonly RigidBody _center;
        public readonly IConstraint[] _joints;
        private readonly FlexConcavePolygon _shape;

        internal override List<IForceField> _globalForceFields
        {
            get { return _center._globalForceFields; }
            set
            {
                _center._globalForceFields = value;
                foreach (var particle in _objects)
                {
                    particle._globalForceFields = value;
                }
            }
        }

        internal SoftBody(FlexConcavePolygon shape, RigidBody center, RigidBody[] objects, IConstraint[] joints, uint collisionLayer)
        {
            _center = center;
            _objects = objects;
            _joints = joints;

            _shape = shape;
            CollisionLayer = collisionLayer;
        }

        public override void ApplyImpulse(Vector2 impulse, Vector2 r)
        {
            var closestObjects = FindClosestObjects(3, r);
            if (closestObjects[0].Distance < double.Epsilon)
            {
                closestObjects[0].Body.ApplyImpulse(impulse, Vector2.Zero);
            }
            else
            {
                var inverseDistanceSum = closestObjects.Sum(x => 1 / x.Distance);
                foreach (var particle in closestObjects)
                {
                    var impulseForParticle = impulse * (1 / particle.Distance / inverseDistanceSum);
                    particle.Body.ApplyImpulse(impulseForParticle, Vector2.Zero);
                }
            }
        }

        internal override Vector2 GetVelocityOfPoint(Vector2 point)
        {
            // todo no raczej nie 
            return FindClosestObject(point).Velocity;
        }

        public double GetEffectiveMass(Vector2 r, Vector2 axis)
        {
            // todo maybe use something
            return FindClosestObjects(1, r).First().Body.GetEffectiveMass(Vector2.Zero, axis);
        }

        internal override void Update(double dt)
        {
            _center.Update(dt);
            foreach (var particle in _objects)
            {
                particle.Update(dt);
            }
        }

        /// <param name="point">in local bounds</param>
        private (RigidBody Body, double Distance)[] FindClosestObjects(int num, Vector2 point)
        {
            if (num > _objects.Length)
                throw new ArgumentException("Not enough points");
            var closest = new List<(RigidBody Body, double Distance)> {(null, Double.PositiveInfinity)};

            foreach (var rigidBody in _objects)
            {
                var distance = (rigidBody.Position-_center.Position - point).Length;
                if (distance < closest.Last().Distance)
                {
                    InsertCloseBody(closest, (rigidBody, distance), num);
                }
            }
            return closest.ToArray();
        }


        /// <param name="vector2">in local bounds</param>
        private RigidBody FindClosestObject(Vector2 vector2)
        {
            var closest=_objects[0];
            var minDistance = double.PositiveInfinity;
            foreach (var rigidBody in _objects)
            {
                var distance = (vector2+_center.Position - rigidBody.Position).Length;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = rigidBody;
                }
            }
            return closest;
        }

                private static void InsertCloseBody(IList<(RigidBody Body, double Distance)> closestBodies, (RigidBody Body, double Distance) newClose, int num)
        {
            var index = closestBodies.Count - 1;
            while (index >= 0 && closestBodies[index].Distance > newClose.Distance)
                index--;

            closestBodies.Insert(index+1, newClose);
            if (closestBodies.Count > num)
                closestBodies.RemoveAt(num);
        }

        public override Vector2 Position
        {
            get { return _center.Position; }
            set
            {
                //                throw new NotSupportedException();
                var diff = value - _center.Position;
                foreach (var obj in _objects)
                {
                    obj.Position += diff;
                }
                _center.Position += diff;
            }
        }

        public override double Rotation
        {
            get { return 0; }
            set
            {
                var matrix = Matrix3.Translation(Position) * Matrix3.Rotation(value) * Matrix3.Translation(-Position);
                foreach (var particle in _objects)
                {
                    particle.Position = matrix * particle.Position;
                }
            }
        }

        public override Shape Shape { get { return _shape; }
            set { throw new NotSupportedException(); }
        }
        public override double InverseMass { get { return 1/(_center.Mass); } }
        public override double InverseMomentOfInertia { get { return 1.0d/MomentOfInertia; } }

        public override Material Material
        {
            get { return _center.Material; }
            set
            {
                _center.Material = value;
                foreach (var particle in _objects)
                {
                    particle.Material = value;
                }
            }
        }

        public override UpdateBehavior BehaviorType { get {return UpdateBehavior.PhysicsEnabled;} }
        public override uint CollisionLayer { get; set; }
        public override double Mass { get { return 1 / InverseMass; }}

        public override Matrix3 GetTransformMatrix()
        {
            return Matrix3.Identity();
        }

        public override Matrix3 GetRotationMatrix()
        {
            throw new NotSupportedException();
        }

        internal override double GetMass(Vector2 point)
        {
            return FindClosestObjects(1, point).First().Body.Mass;
        }

        internal override double GetInverseMomentOfInertia(Vector2 point, Vector2 axis)
        {
            return 0;
        }

        /// <param name="point">in global bounds</param>
        internal RigidBody GetClosestBody(Vector2 point)
        {
            var local = point - Position;
            return FindClosestObjects(1,local).Single().Body;
        }

        public static SoftBodyBuilder.IMassSoftBodyBuilder Create()
        {
            return new SoftBodyBuilder();
        }
    }
}