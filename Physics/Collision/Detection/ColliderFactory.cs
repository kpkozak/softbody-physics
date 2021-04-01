using System;
using System.Collections.Generic;
using Geometry.Shapes;
using Logging;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    public class ColliderFactory
    {
        public ICollider Create()
        {
            var satChecker = new SATInterpenetrationChecker(new OneDimensionIntersectionChecker());
            var collisionFinder = new CollisionPointsFinder(new ConsoleLogger());

            var circlesCollider = new CirclesCollider();
            var polygonsCollider = new PolygonCollider(satChecker,collisionFinder);

            var circlePolygonCollider = new CirclePolygonCollider(satChecker);
            var flexPolygonCollider = new FlexConcavePolygonPolygonCollider(satChecker, collisionFinder);
            var flexFlexCollider =
                new FlexConcavePolygonCollider(satChecker, collisionFinder, new DuplicateCollisionPointsMerger());
            var flexCircleCollider = new FlexConcavePolygonCircleCollider(
                new CirclePolygonCollider(satChecker));

            var circle = typeof(Circle);
            var polygon = typeof(Polygon);
            var flex = typeof(FlexConcavePolygon);

            var colliders = new Dictionary<Type, IDictionary<Type, ICollider>>
            {
                [circle] = new Dictionary<Type, ICollider>
                {
                    {circle, circlesCollider},
                    {polygon, circlePolygonCollider},
                    {flex, flexCircleCollider}
                },
                [polygon] = new Dictionary<Type, ICollider>
                {
                    {circle, circlePolygonCollider},
                    {polygon, polygonsCollider},
                    {flex, flexPolygonCollider}
                },
                [flex] = new Dictionary<Type, ICollider>
                {
                    {circle, flexCircleCollider},
                    {polygon, flexPolygonCollider},
                    {flex, flexFlexCollider}
                }
            };

            var newCollider = new CompositeCollider(colliders);
            var broadPhase = new BroadPhase.CircleBroadPhaseCollider(newCollider);

            newCollider.ObjectsColliding += broadPhase.RaiseObjectsColliding;

            circlesCollider.ObjectsColliding += newCollider.RaiseObjectsColliding;
            polygonsCollider.ObjectsColliding += newCollider.RaiseObjectsColliding;
            circlePolygonCollider.ObjectsColliding += newCollider.RaiseObjectsColliding;
            flexPolygonCollider.ObjectsColliding += newCollider.RaiseObjectsColliding;
            flexFlexCollider.ObjectsColliding += newCollider.RaiseObjectsColliding;
            flexCircleCollider.ObjectsColliding += newCollider.RaiseObjectsColliding;

            return broadPhase;
        }
    }
}