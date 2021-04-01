using System;
using System.Collections.Generic;
using Geometry.Shapes;
using Geometry.Vector;
using Physics;
using Physics.Bodies;
using Physics.Bodies.Materials;
using Physics.Force.Fields;

namespace Samples.Samples
{
    public class WallDemoBuilder
    {
        public PhysicsScene CreateScene()
        {
            // 1366x768
            var verticalBorder = Polygon.AARectangle(50, 780);
            var verticalBorderPositionLeft = new Vector2(0, 384);
            var verticalBorderPositionRight = new Vector2(1366, 384);

            var borderBottom = Polygon.AARectangle(1316, 85);
            var borderBottomPosition = new Vector2(683, 768);

            var bodies = new List<Body>
            {
                RigidBody.CreateStatic()
                    .WithLocation(verticalBorderPositionLeft)
                    .WithShape(verticalBorder)
                    .Build(),
                RigidBody.CreateStatic()
                    .WithLocation(verticalBorderPositionRight)
                    .WithShape(verticalBorder)
                    .Build(),
                RigidBody.CreateStatic()
                    .WithLocation(borderBottomPosition)
                    .WithShape(borderBottom)
                    .Build()
            };

            var obstacle = Polygon.AARectangle(30, 30);
            for (int y = 100; y < 800; y += 150)
            {
                for (int x = 75; x < 1400; x += 100)
                {
                    bodies.Add(RigidBody.CreateStatic()
                        .WithLocation((x, y), Math.PI / 4)
                        .WithShape(obstacle)
                        .Build());
                }
            }

            for (int y = 175; y < 700; y += 150)
            {
                for (int x = 25; x < 1400; x += 100)
                {
                    bodies.Add(RigidBody.CreateStatic()
                        .WithLocation((x, y), Math.PI / 4)
                        .WithShape(obstacle)
                        .Build());
                }
            }
            var gravity = new HomogenousGravitationField(40, (0, 1));

            var physicsSystem = new PhysicsSceneBuilder().Build(bodies);
            physicsSystem.GlobalForceFields.Add(gravity);
            return physicsSystem;
        }

        public IList<Body> CreateBenchmarkObjects()
        {
            var material = new Material(0.5, 0.2, new Material.Softness(60, 0.4));
            var shape = Polygon.RegularPolygon(5, 15);
            var bodies = new List<Body>();
            for (int x = 150; x < 1350; x += 100)
            {
                for (int y = 200; y < 300; y += 100)
                {
                    bodies.Add(SoftBody.Create()
                        .WithMass(20)
                        .WithLocation((x, y))
                        .WithShape(shape)
                        .WithMaterial(material)
                        .Build());
                    bodies.Add(SoftBody.Create()
                        .WithMass(20)
                        .WithLocation((x + 25, y - 45))
                        .WithShape(shape)
                        .WithMaterial(material)
                        .Build());
                    bodies.Add(SoftBody.Create()
                        .WithMass(20)
                        .WithLocation((x + 50, y - 90))
                        .WithShape(shape)
                        .WithMaterial(material)
                        .Build());
                }
            }
            return bodies;
        }
    }
}