using System;
using System.Collections.Generic;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Collision.Detection.BroadPhase
{
    internal class CircleBroadPhaseCollider:ICollider
    {
        public event EventHandler<CollisionArgs> ObjectsColliding;

        private readonly Dictionary<Body, double> _circles;
        private readonly ICollider _collider;

        public CircleBroadPhaseCollider(ICollider collider)
        {
            _collider = collider;
            _circles = new Dictionary<Body, double>();
        }

        public void Collide(Body object1, Body object2)
        {
            if (!_circles.ContainsKey(object1))
                AddCircle(object1);
            if (!_circles.ContainsKey(object2))
                AddCircle(object2);

            var distance = (object2.Position - object1.Position).Length;

            if (distance < _circles[object1] + _circles[object2])
            {
                _collider.Collide(object1, object2);
            }
        }

        private void AddCircle(Body body)
        {
            switch (body.Shape)
            {
                case Polygon polygon:
                    _circles[body] = CreateCircle(polygon);
                    break;
                case Circle circle:
                    _circles[body] = CreateCircle(circle);
                    break;
                case FlexConcavePolygon flex:
                    _circles[body] = CreateCircle(flex);
                    break;
                default: throw new NotSupportedException();
            }
        }

        private double CreateCircle(FlexConcavePolygon flex)
        {
            CountMinAndMax(flex.Points,out var minX, out var maxX, out var minY, out var maxY);

            var maxDist = (new Vector2(maxX, maxY) - (minX, minY)).Length;
            return maxDist * 1.3;
        }

        private void CountMinAndMax(IEnumerable<Vector2> points, out double minX, out double maxX, out double minY, out double maxY)
        {
            maxX = double.NegativeInfinity;
            minX = double.PositiveInfinity;

            maxY = double.NegativeInfinity;
            minY = double.PositiveInfinity;

            foreach (var point in points)
            {
                if (point.X > maxX) maxX = point.X;
                if (point.X < minX) minX = point.X;
                if (point.Y > maxY) maxY = point.Y;
                if (point.Y < minY) minY = point.Y;
            }
        }

        private double CreateCircle(Circle circle)
        {
            return circle.Radius;
        }

        private double CreateCircle(Polygon polygon)
        {
            CountMinAndMax(polygon.Points, out var minX, out var maxX, out var minY, out var maxY);
            var maxDist = (new Vector2(maxX, maxY) - (minX, minY)).Length;
            return maxDist;
        }

        internal void RaiseObjectsColliding(object sender, CollisionArgs e)
        {
            ObjectsColliding?.Invoke(this, e);
        }
    }
}
