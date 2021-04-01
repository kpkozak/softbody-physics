using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    class FlexConcavePolygonCollider : ICollider
    {
        // todo think if not too many nested { }
        private readonly SATInterpenetrationChecker _satChecker;
        private readonly CollisionPointsFinder _collisionPointsFinder;
        private readonly DuplicateCollisionPointsMerger _duplicateCollisionPointsMerger;

        public FlexConcavePolygonCollider(SATInterpenetrationChecker satChecker, CollisionPointsFinder collisionPointsFinder, 
            DuplicateCollisionPointsMerger duplicateCollisionPointsMerger)
        {
            _satChecker = satChecker;
            _collisionPointsFinder = collisionPointsFinder;
            _duplicateCollisionPointsMerger = duplicateCollisionPointsMerger;
        }

        public event EventHandler<CollisionArgs> ObjectsColliding;
        public void Collide(Body object1, Body object2)
        {
            var aMatrix = object1.GetTransformMatrix();
            var bMatrix = object2.GetTransformMatrix();

            var globalBoundsTriangles1 = ((FlexConcavePolygon) object1.Shape).GetTriangles()
                .Select(triangle => aMatrix * triangle).ToArray();
            var globalBoundsTriangles2 = ((FlexConcavePolygon)object2.Shape).GetTriangles().Select(triangle => bMatrix * triangle).ToArray();

            var allCollisionPoints = Enumerable.Empty<CollisionPoint>();

            for (var i = 0; i < globalBoundsTriangles1.Length; ++i)
            {
                for (var j = 0; j < globalBoundsTriangles2.Length; ++j)
                {
                    var shape1 = globalBoundsTriangles1[i].Points;
                    var shape2 = globalBoundsTriangles2[j].Points;
                    if (_satChecker.AreColliding(shape1, shape2,
                        out Vector2 currInterpenetration,
                        out Vector2 currCollisionNormal))
                    {
                        var collisionPoints = _collisionPointsFinder.GetCollisionPoints(shape1, shape2).ToArray();
                        
                        allCollisionPoints = allCollisionPoints.Concat(collisionPoints.Select(x=>new CollisionPoint(x,currInterpenetration,currCollisionNormal)));
                    }
                }
            }
            var allPointsArray = allCollisionPoints.ToArray();
            if (allPointsArray.Any())
            {
                var distinctPoints = _duplicateCollisionPointsMerger.Merge(allPointsArray).ToArray();
               
                foreach (var collisionPoint in distinctPoints)
                {
                    RaiseObjectsColliding(new CollisionArgs(object1,object2,collisionPoint.Interpenetration,collisionPoint.Normal, collisionPoint.Point));
                }
            }
        }

        protected virtual void RaiseObjectsColliding(CollisionArgs e)
        {
            ObjectsColliding?.Invoke(this, e);
        }
    }

    
}
