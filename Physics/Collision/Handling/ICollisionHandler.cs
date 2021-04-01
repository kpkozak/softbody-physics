using Physics.Collision.Detection;

namespace Physics.Collision.Handling
{
    public interface ICollisionHandler
    {
        void HandleCollision(object sender, Detection.CollisionArgs e);
    }
}