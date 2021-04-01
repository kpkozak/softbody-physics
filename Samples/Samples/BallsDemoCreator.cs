using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;
using Physics;
using Physics.Bodies;
using Physics.Bodies.Materials;
using Physics.Force.Fields;
using SFML.Graphics;
using SFML.Window;

namespace Samples.Samples
{
    internal class BallsDemoCreator
    {
        public Demo CreateBallsDemo(Window window)
        {
            var rand = new Random();
            var material = new Material(0.9, 0.3);
            var fields = new List<IForceField>
            {
                new HomogenousGravitationField(500,(0, 1)),
                //new AirResistanceField(5)
            };
            IList<Body> objects = new List<Body>();
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((400, 1400))
                .WithShape(new Circle(Vector2.Zero, 900))
                .WithMaterial(material)
                .Build());
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((-50, 500))
                .WithShape(new Circle(Vector2.Zero, 200))
                .WithMaterial(material)
                .Build());
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((850, 500))
                .WithShape(new Circle(Vector2.Zero, 200))
                .WithMaterial(material)
                .Build());
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((-50, 250))
                .WithShape(new Circle(Vector2.Zero, 200))
                .WithMaterial(material)
                .Build());
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((850, 250))
                .WithShape(new Circle(Vector2.Zero, 200))
                .WithMaterial(material)
                .Build());
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((0, 0))
                .WithShape(new Circle(Vector2.Zero, 200))
                .WithMaterial(material)
                .Build());
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((800, 0))
                .WithShape(new Circle(Vector2.Zero, 200))
                .WithMaterial(material)
                .Build());
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((200, -100))
                .WithShape(new Circle(Vector2.Zero, 200))
                .WithMaterial(material)
                .Build());
            objects.Add(RigidBody.CreateStatic()
                .WithLocation((420, -100))
                .WithShape(new Circle(Vector2.Zero, 200))
                .WithMaterial(material)
                .Build());
            for (var i = 1; i < 150; i++)
            {
                objects.Add(RigidBody.Create()
                    .WithMass(20 + rand.NextDouble() * 10)
                    .WithLocation((180 + (i / 7) * 40, 150 + ((i % 7) * 40)))
                    .WithShape(new Circle(Vector2.Zero, 3))
                    .WithMaterial(new Material(0.7, 0))
                    .Build());
            }
            var physics = Create(objects, fields);
            return new Demo(physics, Enumerable.Empty<Drawable>(), () => { });
        }

        private static PhysicsScene Create(IList<Body> objects, List<IForceField> fields)
        {
            return new PhysicsSceneBuilder()
                .WithForceFields(fields)
                .Build(objects);
        }
    }
}