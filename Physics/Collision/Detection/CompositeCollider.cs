using System;
using System.Collections.Generic;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    public class CompositeCollider : ICollider
    {
        private readonly IDictionary<Type, IDictionary<Type, ICollider>> _colliders;

        public CompositeCollider(IDictionary<Type, IDictionary<Type, ICollider>> colliders)
        {
            _colliders = colliders;
        }

        public void Collide(Body object1, Body object2)
        {
            if (object1.BehaviorType == UpdateBehavior.Static && object2.BehaviorType == UpdateBehavior.Static)
                return;

            if ((object1.CollisionLayer & object2.CollisionLayer) == 0)
                return;

            var type1 = object1.Shape.GetType();
            var type2 = object2.Shape.GetType();

            _colliders[type1][type2].Collide(object1, object2);
        }

        public event EventHandler<CollisionArgs> ObjectsColliding;

        internal void RaiseObjectsColliding(object sender, CollisionArgs e)
        {
            ObjectsColliding?.Invoke(sender, e);
        }
    }
}