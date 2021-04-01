using Physics.Integrator;
using Physics.Items;

namespace Physics.Configure
{
    public class DummyUpdater : IUpdater
    {
        public void Update(double elapsedTime, IParticle updatedObject)
        {
        }
    }
}