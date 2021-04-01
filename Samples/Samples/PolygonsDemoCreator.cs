using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;
using Physics;
using Physics.Bodies;
using Physics.Bodies.Materials;
using Physics.Force.Fields;

namespace Samples.Samples
{
    internal class PolygonsDemoCreator
    {
        private static readonly Random Rand = new Random();

        public PhysicsScene CreatePolygonsPhysics()
        {
            var objects = new List<Body>
            {
                RigidBody.Create()
                    .WithMass(10).WithLocation((100 + (25 % 10) * 50, 100 + (25 / 10) * 45))
                    .WithShape(new Polygon((-10, -10), (10, -10), (10, 10), (-10, 10)))
                    .Build()
            };
            var fields = new List<IForceField>
            {
                new CentralGravitationField(objects.First(), 6670),
                new AirResistanceField(0.3)
            };

            for (int i = 0; i < 50; ++i)
            {
                if (i == 25)
                    continue;
                objects.Add(RigidBody.Create().WithMass(10)
                    .WithLocation((100 + (i % 10) * 50, 100 + (i / 10) * 45))
                    .WithShape(new Polygon(new[]
                    {
                        new Vector2(-GetNextRandom(), -GetNextRandom()),
                        new Vector2(GetNextRandom(), -GetNextRandom()),
                        new Vector2(GetNextRandom(), GetNextRandom()),
                        new Vector2(-GetNextRandom(), GetNextRandom()),
                    }))
                    .WithMaterial(new Material(0.99, 0.3))
                    .Build());
            }
            return PhysicsScene.Create()
                .WithForceFields(fields)
                .Build(objects);
        }

        private static int GetNextRandom()
        {
            return (Rand.Next(0, 5) * 2 + 10);
        }
    }
}