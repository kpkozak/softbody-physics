using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Force.Fields
{
    public class CentralGravitationField : IForceField
    {
        private readonly Body _gravitationSource;

        private readonly double G;

        public CentralGravitationField(Body gravitationSource, double g)
        {
            _gravitationSource = gravitationSource;
            G = g;
        }

        public Force GetForce(Body body)
        {
            var centralPoint = _gravitationSource.Position;

            if (centralPoint.Equals(body.Position)) return Force.Zero;

            var radius2 = centralPoint.CountRadius2(body.Position);
            var forceValue = G * body.GetMass((0, 0)) * _gravitationSource.Mass / radius2;
            return new Force(body.Position.CountDistance(centralPoint).Normalize()*forceValue);
        }
    }
}