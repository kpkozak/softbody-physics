using System;
using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Force.Fields
{
    public class AirResistanceField : IForceField
    {
        public Force GetForce(Body body)
        {
            var velocity = body.GetVelocityOfPoint((0, 0));

            return new Force(-(Math.Pow(velocity.Length, 2) * velocity.Normalize()) * _resistanceFactor);
        }

        private readonly double _resistanceFactor;

        public AirResistanceField(double resistanceFactor)
        {
            _resistanceFactor = resistanceFactor;
        }
    }
}