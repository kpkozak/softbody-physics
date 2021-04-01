using System;
using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Collision.Detection
{
    public class CollisionArgs: EventArgs
    {
        public Body Object1 { get; private set; }
        public Body Object2 { get; private set; }
        public Vector2 Interpenetration { get; private set; }
        public Vector2 Normal { get; private set; }
        public Vector2[] Points { get; private set; }

        public CollisionArgs(Body object1, Body object2, Vector2 interpenetration, Vector2 normal, params Vector2[] points)
        {
            Object1 = object1;
            Object2 = object2;
            Interpenetration = interpenetration;
            Normal = normal;
            Points = points;
        }
    }
}