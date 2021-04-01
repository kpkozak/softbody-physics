using System;
using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;
using GUI;
using Physics;
using Physics.Bodies;
using Physics.Bodies.Materials;
using Physics.Constraints;
using Physics.Force.Fields;
using SFML.Graphics;
using SFML.Window;

namespace Samples.Samples
{
    internal class InteractiveDemoCreator
    {
        public IDemo CreateSoftDemo(RenderWindow window)
        {
            var physics = CreateInteractivePhysics(window);

            var builder = new GuiFactory(window, new Vector2u((uint) (window.Size.X * 0.2), window.Size.Y),
                new Vector2f((float) (window.Size.X * 0.75), 0), 12, 35);
            var softBodySwitch = builder.CreateSwitch(5, 2, 2, 1, "RigidBody", "SoftBody");

            var regularPolygonSwitch = builder.CreateSwitch(5, 6, 2, 1, "Random", "Regular Shape");

            var verticesCountLabel = builder.CreateLabel(0, 8, 5, 1, "Vertices");
            var verticesCountSlider = builder.CreateSlider(3, 8, 5, 8, 5, 1);

            var massLabel = builder.CreateLabel(0, 12, 5, 1, "Mass");
            var massSlider = builder.CreateSlider(10, 50, 5, 12, 5, 1);
            var rotationLabel = builder.CreateLabel(0, 14, 5, 1, "Rotation");
            var rotationSlider = builder.CreateSlider(0, 360, 5, 14, 5, 1);
            var sizeLabel = builder.CreateLabel(0, 16, 5, 1, "Size");
            var sizeSlider = builder.CreateSlider(50, 120, 5, 16, 5, 1);

            var materialLabel = builder.CreateLabel(5, 19, 5, 1, "Material");
            var restitutionLabel = builder.CreateLabel(0, 20, 5, 1, "Restitution");
            var restitutionSlider = builder.CreateSlider(0.01, 0.95, 5, 20, 5, 1);
            var frictionLabel = builder.CreateLabel(0, 22, 5, 1, "Friction");
            var frictionSlider = builder.CreateSlider(0.01, 0.5, 5, 22, 5, 1);
            var dampingLabel = builder.CreateLabel(0, 24, 5, 1, "Damping");
            var dampingSlider = builder.CreateSlider(0.01, 0.8, 5, 24, 5, 1);
            var frequencyLabel = builder.CreateLabel(0, 26, 5, 1, "Frequency");
            var frequencySlider = builder.CreateSlider(80, 140, 5, 26, 5, 1);

            var collisionLayersLabel = builder.CreateLabel(2, 29, 6, 1, "Collision layers");
            var collision1 = builder.CreateCheckbox(2, 31, 1, 1, "", true);
            var collision2 = builder.CreateCheckbox(4, 31, 1, 1, "", true);
            var collision3 = builder.CreateCheckbox(6, 31, 1, 1, "", true);
            var collision4 = builder.CreateCheckbox(8, 31, 1, 1, "", true);
            var collision5 = builder.CreateCheckbox(10, 31, 1, 1, "", true);

            var workingArea = new WorkingArea(new Vector2f(0, 0),
                new Vector2f((float) (window.Size.X * 0.75), window.Size.Y),
                new Color(0, 0, 0, 1));
            var guiWorkingArea = new WorkingArea(new Vector2f((float) (window.Size.X * 0.75), 0),
                new Vector2f((float) (window.Size.X * 0.5), window.Size.Y), new Color(0, 0, 0, 40));
            var createLine = window.Size.Y * 0.1;
            var lastCreatedItem = physics.Objects.First();

            workingArea.MouseButtonPressed += HandleSoftDemoMouse;
            window.MouseButtonPressed += workingArea.MousePressedHandler;

            return new Demo(physics, new Drawable[]
                {
                    guiWorkingArea,
                    workingArea,
                    softBodySwitch,
                    regularPolygonSwitch,
                    verticesCountLabel,
                    verticesCountSlider,
                    massLabel,
                    massSlider,
                    rotationLabel,
                    rotationSlider,
                    sizeLabel,
                    sizeSlider,
                    materialLabel,
                    restitutionLabel,
                    restitutionSlider,
                    frictionLabel,
                    frictionSlider,
                    dampingLabel,
                    dampingSlider,
                    frequencyLabel,
                    frequencySlider,
                    collisionLayersLabel,
                    collision1,
                    collision2,
                    collision3,
                    collision4,
                    collision5
                },
                () => window.MouseButtonPressed -= HandleSoftDemoMouse);

            void HandleSoftDemoMouse(object sender, MouseButtonEventArgs e)
            {
                lastCreatedItem = softBodySwitch.IsChecked ? (Body)CreateSoftBody(e) : CreateRigidBody(e);
            }

            RigidBody CreateRigidBody(MouseButtonEventArgs e)
            {
                var body = RigidBody.Create()
                    .WithMass(massSlider.Value)
                    .WithLocation(GetPosition(e), rotationSlider.Value*180/Math.PI)//rotationSlider.Value/180*Math.PI)
                    .WithShape(GetShape())
                    .WithMaterial(GetMaterial())
                    .OnLayer(GetLayer())
                    .Build();
                physics.Add(body);
                return body;
            }

            SoftBody CreateSoftBody(MouseButtonEventArgs e)
            {
                var body = SoftBody.Create()
                    .WithMass(massSlider.Value)
                    .WithLocation(GetPosition(e), rotationSlider.Value/180*Math.PI)
                    .WithShape(GetShape())
                    .WithMaterial(GetMaterial())
                    .OnLayer(GetLayer())
                    .Build();
                physics.Add(body);
                return body;
            }

            Vector2 GetPosition(MouseButtonEventArgs e)
            {
                return (e.X, lastCreatedItem.Position.Y < createLine
                    ? lastCreatedItem.Position.Y - 100
                    : createLine);
            }

            Polygon GetShape()
            {
                return regularPolygonSwitch.IsChecked
                    ? Polygon.RegularPolygon((int) verticesCountSlider.Value, sizeSlider.Value)
                    : Polygon.RandomPolygon((int) verticesCountSlider.Value, 50, sizeSlider.Value);
            }

            Material GetMaterial()
            {
                return new Material(restitutionSlider.Value, frictionSlider.Value,
                    new Material.Softness(frequencySlider.Value, dampingSlider.Value/3));
            }

            uint GetLayer()
            {
                return ToUInt(collision1.IsChecked) | ToUInt(collision2.IsChecked) << 1 |
                       ToUInt(collision3.IsChecked) << 2 | ToUInt(collision4.IsChecked) << 3 |
                       ToUInt(collision5.IsChecked) << 4;
            }

            uint ToUInt(bool value)
            {
                return (uint) (value ? 1 : 0);
            }

        }

        private static int softBodyVertices = 4;
        private Switch _softBodySwitch;

        public PhysicsScene CreateInteractivePhysics(RenderWindow window)
        {
            var sceneSize = window.Size;
            var objects = new List<Body>();

            objects.Add(RigidBody.CreateStatic()
                .WithLocation((sceneSize.X*0.5, sceneSize.Y*0.9))
                .WithShape(Polygon.AARectangle(sceneSize.X*0.8, sceneSize.Y*0.1))
                .WithMaterial(new Material(0, 0))
                .Build());

            objects.Add(RigidBody.CreateStatic()
                .WithLocation((sceneSize.X*0.1, sceneSize.Y*0.2))
                .WithShape(Polygon.AARectangle(50, 30))
                .WithMaterial(new Material(0, 0))
                .Build());

            objects.Add(RigidBody.CreateStatic()
                .WithLocation((sceneSize.X * 0.3, sceneSize.Y * 0.5))
                .WithShape(Polygon.RandomPolygon(5,50, 30))
                .WithMaterial(new Material(0, 0))
                .Build());

            objects.Add(RigidBody.CreateStatic()
                .WithLocation((sceneSize.X * 0.7, sceneSize.Y * 0.3), -Math.PI/3.5)
                .WithShape(Polygon.AARectangle(sceneSize.X*0.3, sceneSize.Y*0.05))
                .WithMaterial(new Material(0, 0))
                .Build());

            objects.Add(RigidBody.CreateStatic()
                .WithLocation((sceneSize.X * 0.1, sceneSize.Y * 0.7))
                .WithShape(Polygon.AARectangle(sceneSize.X * 0.03, sceneSize.Y * 0.5))
                .WithMaterial(new Material(0, 0))
                .Build());

            var system = PhysicsScene.Create().Build(objects);
            system.GlobalForceFields.Add(new HomogenousGravitationField(1000, (0, 1)));
            return system;
        }

        public IDemo CreateThirdDemo(RenderWindow window)
        {
            var physics = CreateInteractivePhysics(window);

            var createJointNow = false;
            var random = new Random();
            var constraintType=0;
            var ConstraintNames = new[] { "Orientation", "Distance", "Spring", "Spring #2" };

        window.MouseButtonPressed += HandleInteractiveDemoMouse;
            
            return new Demo(physics, Enumerable.Empty<Drawable>(),
                () => window.MouseButtonPressed -= HandleInteractiveDemoMouse);

            void HandleInteractiveDemoMouse(object sender, MouseButtonEventArgs e)
            {
                if (e.Button == Mouse.Button.Right)
                {
                    constraintType = (constraintType + 1) % 4;
                    Console.Out.WriteLine(ConstraintNames[constraintType]);
                    return;
                }
                if (e.Button != Mouse.Button.Left) return;

                var newObject = RigidBody.Create()
                    .WithMass(20)
                    .WithLocation((e.X, e.Y))
                    .WithShape(new Polygon(
                        (-GetNextRandom(), -GetNextRandom()),
                        (GetNextRandom(), -GetNextRandom()),
                        (GetNextRandom(), GetNextRandom()),
                        (-GetNextRandom(), GetNextRandom())))
                    .WithMaterial(new Material(0.2, 0.1))
                    .Build();

                if (createJointNow)
                {
                    var last = physics.Objects.Last() as RigidBody;
                    switch (constraintType)
                    {
                        case 0:
                        {

                            var joint = OrientationConstraint.Create(newObject, last);
                            physics.Add(joint);
                        }
                            break;
                        case 1:
                        {
                            var joint = DistanceConstraint.Create(newObject, last, Vector2.Zero, Vector2.Zero,
                                DistanceConstraint.Type.Rod);
                            physics.Add(joint);
                        }
                            break;
                        case 2:
                        {
                            var joint = SpringConstraint.Create(newObject, last, Vector2.Zero, Vector2.Zero,
                                DistanceConstraint.Type.Rope, 5.001, 0.5);
                            physics.Add(joint);
                        }
                            break;
                        case 3:
                        {
                            var joint = SpringConstraint.Create(newObject, last, Vector2.Zero, Vector2.Zero,
                                DistanceConstraint.Type.Rod, 3, 0.1);
                            physics.Add(joint);
                        }
                            break;
                    }
                }

                physics.Add(newObject);
                createJointNow = !createJointNow;
            }

            int GetNextRandom()
            {
                return random.Next(10, 40);
            }
        }
    }
}