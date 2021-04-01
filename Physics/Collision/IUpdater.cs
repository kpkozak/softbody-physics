using Physics.Items;

namespace Physics.Integrator
{
    public interface IUpdater
    {
        void Update(double elapsedTime, IParticle updatedObject);
    }

    class PhysicsBasedUpdater : IUpdater
    {
        public void Update(double elapsedTime, IParticle updatedObject)
        {
            updatedObject.Update(elapsedTime);
        }
    }
}