using System.Collections.Generic;
using Physics.Collision.Detection;

namespace Physics.Collision.Handling
{
    internal class DeferredCollisionHandler : ICollisionHandler
    {
        private readonly List<(object Sender, CollisionArgs Args)> _collisions;
        private readonly ICollisionHandler _handler;

        public DeferredCollisionHandler(ICollisionHandler handler)
        {
            _handler = handler;
            _collisions = new List<(object Sender, CollisionArgs Args)>();
        }

        public void HandleCollision(object sender, CollisionArgs e)
        {
            _collisions.Add((sender,e));
        }

        public void HandleAllCollisions()
        {
            foreach (var collision in _collisions)
            {
                _handler.HandleCollision(collision.Sender, collision.Args);
            }
        }
    }
}