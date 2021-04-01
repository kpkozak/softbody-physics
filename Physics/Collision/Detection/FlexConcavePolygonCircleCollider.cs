using System;
using Physics.Bodies;

namespace Physics.Collision.Detection {
    internal class FlexConcavePolygonCircleCollider : ICollider
    {
        private CirclePolygonCollider circlePolygonCollider;

        public FlexConcavePolygonCircleCollider(CirclePolygonCollider circlePolygonCollider)
        {
            this.circlePolygonCollider = circlePolygonCollider;
        }

        public event EventHandler<CollisionArgs> ObjectsColliding;
        public void Collide(Body object1, Body object2)
        {
            throw new NotImplementedException();
        }
    }
}