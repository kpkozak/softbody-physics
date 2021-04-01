using System;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    public interface ICollider
    {
        event EventHandler<CollisionArgs> ObjectsColliding;
        void Collide(Body object1, Body object2);
    }
}