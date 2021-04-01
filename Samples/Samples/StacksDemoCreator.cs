using System.Collections.Generic;
using Geometry.Shapes;
using Physics;
using Physics.Bodies;
using Physics.Bodies.Materials;
using Physics.Force.Fields;

namespace Samples.Samples
{
    internal class StacksDemoCreator
    {
        public PhysicsScene CreateStackDemo()
        {
            var field = new HomogenousGravitationField(20,(0, 1));
            var objects = new List<Body>();
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((400, 500))
                .WithShape(Polygon.AARectangle(800, 100))
                .WithMaterial(new Material(0, 0.9))
                .Build());

            for (int i = 0; i < 10; ++i)
            {
                objects.Add(RigidBody.Create()
                    .WithMass(30)
                    .WithLocation((200, 200 + i * 26))
                    .WithShape(Polygon.AARectangle(25, 25))
                    .WithMaterial(new Material(0, 0.3))
                    .Build());
            }

            return PhysicsScene.Create()
                .WithForceField(field).Build(objects);
        }
    }
}