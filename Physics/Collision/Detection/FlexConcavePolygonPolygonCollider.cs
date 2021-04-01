using System;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    internal sealed class FlexConcavePolygonPolygonCollider : ICollider
    {
        private readonly CollisionPointsFinder _collisionPointsFinder;
        private readonly SATInterpenetrationChecker _satChecker;
        // todo use duplicatecollisionpointsmerger
        public FlexConcavePolygonPolygonCollider(SATInterpenetrationChecker satChecker,
            CollisionPointsFinder collisionPointsFinder)
        {
            _satChecker = satChecker;
            _collisionPointsFinder = collisionPointsFinder;
        }

        public event EventHandler<CollisionArgs> ObjectsColliding;

        public void Collide(Body object1, Body object2)
        {
            // todo think of too many nested { }
            if (!(object1.Shape is FlexConcavePolygon))
            {
                Collide(object2, object1);
            }
            else
            {
                var aMatrix = object1.GetTransformMatrix();
                var bMatrix = object2.GetTransformMatrix();

                var flexPolygon = (FlexConcavePolygon) object1.Shape;
                var globalBoundsPolygon = ((Polygon) object2.Shape).Points.Select(x => bMatrix * x).ToArray();

                foreach (var globalBoundstriangle in flexPolygon.GetTriangles()
                    .Select(triangle => (aMatrix * triangle).Points))
                {
                    Vector2 interpenetration;
                    Vector2 collisionNormal;
                    if (_satChecker.AreColliding(globalBoundsPolygon,
                        globalBoundstriangle,
                        out interpenetration, out collisionNormal))
                    {
                        var collisionPoints = _collisionPointsFinder.GetCollisionPoints(
                            globalBoundsPolygon, globalBoundstriangle).ToArray();
                        if (collisionPoints.Any())
                        {
                            RaiseObjectsColliding(new CollisionArgs(object1, object2, -interpenetration,
                                -collisionNormal, collisionPoints));
                        }
                    }
                }
            }
        }

        private void RaiseObjectsColliding(CollisionArgs e)
        {
            ObjectsColliding?.Invoke(this, e);
        }
    }
}