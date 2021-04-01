using System;
using System.Collections.Generic;
using Geometry.Matrix;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies.Materials;
using Physics.Force.Fields;

namespace Physics.Bodies
{
    public abstract class Body
    {
        public abstract UpdateBehavior BehaviorType { get; }
        public abstract uint CollisionLayer { get; set; }

        public virtual Vector2 Position { get; set; }
        public virtual double Rotation
        {
            get { return _rotation; }
            set { _rotation = value % (Math.PI * 2); }
        }

        public abstract Shape Shape { get; set; }
        public abstract double Mass { get; }
        public double MomentOfInertia { get; protected set; }
        public abstract double InverseMass { get; }
        public abstract double InverseMomentOfInertia { get; }

        public abstract Material Material { get; set; }

        public abstract void ApplyImpulse(Vector2 impulse, Vector2 r);
        internal abstract void Update(double dt);
        
        public abstract Matrix3 GetTransformMatrix();
        public abstract Matrix3 GetRotationMatrix();

        internal abstract double GetMass(Vector2 point);
        internal abstract double GetInverseMomentOfInertia(Vector2 point, Vector2 axis);
        /// <param name="point">In local bounds</param>
        internal abstract Vector2 GetVelocityOfPoint(Vector2 point);
        internal virtual List<IForceField> _globalForceFields { get; set; }

        private double _rotation;
    }
}