using System.Collections.Generic;

namespace Physics.Collision.Handling
{
    public class CompositeCollisionHandler : ICollisionHandler
    {
        private readonly ICollection<ICollisionHandler> _collisionHandlers;

        public CompositeCollisionHandler(ICollection<ICollisionHandler> collisionHandlers)
        {
            _collisionHandlers = collisionHandlers;
        }

        public void HandleCollision(object sender, Detection.CollisionArgs e)
        {
            foreach (var collisionHandler in _collisionHandlers)
            {
                collisionHandler.HandleCollision(sender, e);
            }
        }
    }
}