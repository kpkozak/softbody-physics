using System;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    public class CirclesCollider : ICollider
    {
        public event EventHandler<CollisionArgs> ObjectsColliding;

        public void Collide(Body object1, Body object2)
        {
            var sphere1 = (Circle) object1.Shape;
            var sphere2 = (Circle) object2.Shape;

            var centersDistance = Math.Abs((object1.Position - object2.Position).Length);
            var radiusSum = sphere1.Radius + sphere2.Radius;

            if (radiusSum > centersDistance)
            {
                var interpenetration =
                    (object2.Position - object1.Position)
                    .Normalize()
                    .Multiply(Math.Abs(radiusSum - centersDistance));
                var collisionNormal = (object2.Position - object1.Position).Normalize();
                var collisionPoint = (object1.Position * sphere2.Radius + object2.Position * sphere1.Radius) *
                                     (1.0d / radiusSum);
                RaiseObjectsColliding(object1, object2, interpenetration, collisionNormal, collisionPoint);
            }
        }

        // todo dopisać testy, w szczególności obliczanie punktu kolizji
        private void RaiseObjectsColliding(Body object1, Body object2, Vector2 collideVector, Vector2 direction,
            Vector2 collisionPoints)
        {
            ObjectsColliding?.Invoke(this,
                new CollisionArgs(object1, object2, collideVector, direction, new[] {collisionPoints}));
        }
    }
}