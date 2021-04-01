using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    internal class PolygonCollider : ICollider
    {
        private readonly CollisionPointsFinder _collisionPointsFinder;

        private readonly SATInterpenetrationChecker _satChecker;

        public PolygonCollider(SATInterpenetrationChecker satChecker, CollisionPointsFinder collisionPointsFinder)
        {
            _satChecker = satChecker;
            _collisionPointsFinder = collisionPointsFinder;
        }

        public event EventHandler<CollisionArgs> ObjectsColliding;

        public void Collide(Body object1, Body object2)
        {
            Vector2 interpenetration;
            Vector2 collisionNormal;
            var aMatrix = object1.GetTransformMatrix();
            var globalBoundsShape1 = ((Polygon) object1.Shape).Points.Select(x => aMatrix * x).ToArray();
            var bMatrix = object2.GetTransformMatrix();
            var globalBoundsShape2 = ((Polygon) object2.Shape).Points.Select(x => bMatrix * x).ToArray();

            if (_satChecker.AreColliding(globalBoundsShape1, globalBoundsShape2, out interpenetration,
                out collisionNormal))
            {
                var collisionPoints = _collisionPointsFinder.GetCollisionPoints(
                        globalBoundsShape1, globalBoundsShape2)
                    .ToArray();
                if (collisionPoints.Any())
                {
                    RaiseObjectsColliding(new CollisionArgs(object1, object2, interpenetration,
                        collisionNormal, collisionPoints));
                }
            }
        }

        protected virtual void RaiseObjectsColliding(CollisionArgs e)
        {
            ObjectsColliding?.Invoke(this, e);
        }
    }
}