using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Force.Fields
{
    public class HomogenousGravitationField : IForceField
    {
        private readonly Vector2 _forceValuePerMassUnit;

        public HomogenousGravitationField(double g, Vector2 direction)
        {
            _forceValuePerMassUnit = direction * g;
        }

        public Force GetForce(Body body)
        {
            return new Force(_forceValuePerMassUnit*body.GetMass((0, 0)));
        }
    }
}