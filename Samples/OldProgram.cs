using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;
using GUI;
using Physics;
using Physics.Bodies;
using Physics.Collision.Detection;
using Physics.Constraints;
using Samples.Drawables;
using Samples.Samples;
using SFML.Graphics;
using SFML.Window;

namespace Samples {
    internal class OldProgram
    {
        static string _selected;
        private static PhysicsScene _physics;
        private static List<Drawable> sprites;
        private static void Main2(string[] args)
        {
            var dict = new Dictionary<string, Func<PhysicsScene>>();
//            dict["balls"] = new BallsDemoCreator().CreateBallsPhysics;
            dict["polygons"] = new PolygonsDemoCreator().CreatePolygonsPhysics;
//            dict["third"] = () => new InteractiveDemoCreator().CreateInteractivePhysics();
            dict["stacks"] = new StacksDemoCreator().CreateStackDemo;
//            dict["soft"] = new InteractiveDemoCreator().CreateInteractivePhysics;
//            dict["parasoft"] = () => new InteractiveDemoCreator().CreateInteractivePhysics();
            dict["wall"] = () =>
            {
                var builder = new WallDemoBuilder();
                var scene = builder.CreateScene();
                
                scene.Add(builder.CreateBenchmarkObjects());
                return scene;
            };

            var mouseHandlers= new Dictionary<string, EventHandler<MouseButtonEventArgs>>();
            mouseHandlers["balls"] = mouseHandlers["polygons"] = mouseHandlers["stacks"] = mouseHandlers["wall"] = (obj, eventargs) => { };
//            mouseHandlers["third"] = HandleInteractiveDemoMouse;
//            mouseHandlers["soft"] = HandleSoftDemoMouse;
            mouseHandlers["parasoft"] = HandleParasoftDemoMouse;
            var workingArea = new WorkingArea(new Vector2f(0,0),new Vector2f(800,600), new Color(127,127,127,127));
            var slider = new Slider(200, 600, 200, new Vector2f(800, 700), new Vector2f(200, 50));
            while (true)
            {
                try
                {
                    do
                    {
                        Console.Out.WriteLine($"Choose demo ({dict.Keys.Aggregate((x, y) => x + "/" + y)}):");
                        _selected = Console.In.ReadLine();
                    } while (!dict.ContainsKey(_selected));


                    _physics = dict[_selected]();
                    var window = new RenderWindow(new VideoMode(1366, 768), "Physics Samples", Styles.Close);
                    window.SetVerticalSyncEnabled(true);
                    window.MouseButtonPressed += slider.MousePressedHandler;
                    window.MouseButtonReleased += slider.MouseReleasedHandler;
                    window.MouseButtonPressed += workingArea.MousePressedHandler;
                    window.MouseMoved += slider.MouseDragHandler;

                    sprites = _physics.Objects.Select(item => item.ToDrawable()).ToList();

                    var watch = new Stopwatch();
                    var close = false;
                    window.Closed += (sender, eventArgs) => close = true;
                    workingArea.MouseButtonPressed += mouseHandlers[_selected];
                    
//                    _createJointNow = false;
                    while (!close)
                    {
                        window.DispatchEvents();
                        var elapsedTime = watch.Elapsed.TotalSeconds;
                        try
                        {
                            _physics.Update(elapsedTime);
                        }
                        catch (Exception e) when (e is InvalidOperationException || e is NoCollisionFoundException) { }
                        watch.Restart();
                        window.Clear(Color.White);
                        window.Draw(slider);
                        window.Draw(workingArea);
                        foreach (var sprite in sprites)
                        {
                            window.Draw(sprite);
                            
                        }
                        window.Display();
                    }
                    window.MouseButtonPressed -= mouseHandlers[_selected];
                    window.Close();
                }
                catch (Exception) { }
            }
        }

        private const double DIF = 50;
        private const double MAS = 25;
        private const double SS = 25;
        private const double FR = 50;
        private const double DMP = 0.2;

        private static void HandleParasoftDemoMouse(object sender, MouseButtonEventArgs e)
        {
            if (e.Button != Mouse.Button.Left)
                return;
            var centerPosition = new Vector2(e.X, e.Y);
            var otherPositions = new[]
            {
                new Vector2(e.X - DIF, e.Y - DIF),
                new Vector2(e.X - DIF, e.Y),
                new Vector2(e.X - DIF, e.Y + DIF),
                new Vector2(e.X, e.Y + DIF),
                new Vector2(e.X + DIF, e.Y + DIF),
                new Vector2(e.X + DIF, e.Y),
                new Vector2(e.X + DIF, e.Y - DIF),
                new Vector2(e.X, e.Y - DIF),
            };
            var newCenter = RigidBody.Create()
                .WithMass(MAS)
                .WithLocation(centerPosition)
                .WithShape(Polygon.AARectangle(SS, SS))
                .Build();

            var objects = otherPositions.Select(x => RigidBody.Create()
                    .WithMass(MAS)
                    .WithLocation(x)
                    .WithShape(Polygon.AARectangle(SS, SS))
                    .Build())
                .ToArray();

            var joints = objects.Select(obj => SpringConstraint.Create(newCenter, obj, Vector2.Zero, Vector2.Zero,
                DistanceConstraint.Type.Rod, FR, DMP)).Concat(GetRods(objects)).ToArray();

            foreach (var rigidBody in objects)
            {
                _physics.Add(rigidBody);
                sprites.Add(new DrawableRigidBody(rigidBody));
            }
            _physics.Add(newCenter);
            sprites.Add(new DrawableRigidBody(newCenter));
            sprites.Add(new ParasoftDrawableItem(objects));
            foreach (var constraint in joints)
            {
                _physics.Add(constraint);
            }
        }

        private static IEnumerable<IConstraint> GetRods(RigidBody[] bodies)
        {
            yield return SpringConstraint.Create(bodies.First(), bodies.Last(), Vector2.Zero, Vector2.Zero,
                DistanceConstraint.Type.Rod, FR, DMP);
            for (var i = 1; i < bodies.Length; ++i)
                yield return SpringConstraint.Create(bodies[i - 1], bodies[i], Vector2.Zero, Vector2.Zero,
                    DistanceConstraint.Type.Rod, FR, DMP);
        }
    }
}