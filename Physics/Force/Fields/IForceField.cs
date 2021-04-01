using Physics.Bodies;

namespace Physics.Force.Fields
{
    public interface IForceField
    {
        Force GetForce(Body body);
    }
}