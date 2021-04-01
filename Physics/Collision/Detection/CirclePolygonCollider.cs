using System;
using System.Linq;
using Geometry;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    internal class CirclePolygonCollider:ICollider
    {
        public CirclePolygonCollider(SATInterpenetrationChecker satChecker)
        {
        }

        public event EventHandler<CollisionArgs> ObjectsColliding;

        public void Collide(Body object1, Body object2)
        {
            if (object1.Shape is Circle)
            {  Collide(object2, object1);}
            else { 

            var matrix = object1.GetTransformMatrix();

            var polygon = ((Polygon) object1.Shape).Points.Select(x=>matrix*x).ToArray();
            var circle = ((Circle) object2.Shape).Translate(object2.Position);

                foreach (var edge in polygon.GetEdges())
                {
                    var direction = edge.Segment.Line.Direction;
                    // to chyba jest zle
                    var a = edge.Segment.A.Dot(direction);
                    var b = edge.Segment.B.Dot(direction);

                    var center = circle.Center.Dot(direction);
                    if (IsBetween(center, a, b))
                    {
                        // sprawdzac odleglosc do boku
                        var distance = edge.Segment.Line.DistanceTo(circle.Center);
                        if (distance < circle.Radius)
                        {
                            var interpenetration = direction.GetNormalVector() * (circle.Radius - distance);
                            var collision = new CollisionArgs(object1, object2, interpenetration,
                                interpenetration.Normalize(), new[] {center * direction});
                            RaiseObjectsColliding(collision);
                            return;
                        }
                    }
                    else
                    {
//                    // odleglosc od wierzcholka
//                    var distance = (edge.Segment.A - circle.Center).Length;
//                    if (distance < circle.Radius)
//                    {
//                        var interpenetration = circle.Radius - distance;
//                        var normal = (circle.Center - edge.Segment.A).Normalize();
//
//                        RaiseObjectsColliding(new CollisionArgs(object1, object2, interpenetration * normal, normal,
//                            new[] {edge.Segment.A}));
//                    }
//
//                    distance = (edge.Segment.B - circle.Center).Length;
//                    if (distance < circle.Radius)
//                    {
//                        var interpenetration = circle.Radius - distance;
//                        var normal = (circle.Center - edge.Segment.B).Normalize();
//
//                        RaiseObjectsColliding(new CollisionArgs(object1, object2, interpenetration * normal, normal,
//                            new[] { edge.Segment.B }));
//                    }
                    }
                }
            }
        }

        private bool IsBetween(double center, double a, double b)
        {
            if (a > b)
                (a, b) = (b, a);

            return a < center && b > center;
        }

        protected virtual void RaiseObjectsColliding(CollisionArgs e)
        {
            ObjectsColliding?.Invoke(this, e);
        }
    }
}