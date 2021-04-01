using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DemoGame.GUI;
using GUI;
using Newtonsoft.Json;
using Samples;
using Samples.Samples;
using SFML.Graphics;
using SFML.Window;

namespace Samples
{
    public class Program
    {
        private const int SizeUnits = 60;
        private static Configuration _configuration;
        private static bool _inDemo = false;
        private static SampleType _selectedSample;

        public static void Main(string[] args)
        {
            var configContent = File.Exists("config.json") ? File.ReadAllText("config.json"):string.Empty;
            _configuration = JsonConvert.DeserializeObject<Configuration>(configContent) ?? Configuration.Default;

            var window = CreateWindow();
            var demoCreators = GetDemoCreators();
            var demoSelectingMenu = GetDemoSelectingMenu(window, demoCreators);
            while (window.IsOpen())
            {
                var selectedDemo = ShowDemosMenu(window, demoSelectingMenu);
                if (selectedDemo == SampleType.None) return;

                using (var demo = demoCreators[selectedDemo](window))
                {
                    PerformDemo(window, demo);
                }
            }
        }

        private static List<Button> GetDemoSelectingMenu(RenderWindow window, Dictionary<SampleType, Func<RenderWindow, IDemo>> demoCreators)
        {
            var builder = new GUI.GuiFactory(window, window.Size, new Vector2f(0, 0), 5, demoCreators.Count*2+1);

            var buttons =
                demoCreators.Select((x, index) =>
                {
                    var button = builder.CreateButton(2, index * 2 + 1, 1, 1, x.Key.ToString().Replace('_', ' '));
                    button.ButtonPressed += (sender, args) => _selectedSample = x.Key;
                    return button;
                }).ToList();
            return buttons;
        }

        private static void PerformDemo(RenderWindow window, IDemo demo)
        {
            var watch = new Stopwatch();
            window.KeyPressed += HandleExit;
            _inDemo = true;
            while (_inDemo)
            {
                var elapsedTime = watch.Elapsed.TotalSeconds;
                demo.Update(elapsedTime);
                watch.Restart();

                window.Draw(demo);
                window.Display();
                window.DispatchEvents();
            }
            window.KeyPressed -= HandleExit;
        }

        private static void HandleExit(object sender, KeyEventArgs args)
        {
            if(args.Code == Keyboard.Key.Escape) _inDemo = false;
        }

        private static Dictionary<SampleType, Func<RenderWindow, IDemo>> GetDemoCreators()
        {
            return new Dictionary<SampleType, Func<RenderWindow, IDemo>>()
            {
                {SampleType.Balls, new BallsDemoCreator().CreateBallsDemo},
//                {SampleType.Polygons, new PolygonsDemoCreator().CreatePolygonsPhysics},
                {SampleType.Polygons, new InteractiveDemoCreator().CreateThirdDemo},
//                {SampleType.Stacks, new StacksDemoCreator().CreateStackDemo},
                {SampleType.Soft_Body, new InteractiveDemoCreator().CreateSoftDemo},
//                {SampleType.Parasoft, new InteractiveDemoCreator().CreateParasoftPhysics},
//                {
//                    SampleType.Wall, (window) =>
//                    {
//                        var builder = new WallDemoBuilder();
//                        var scene = builder.CreateScene();
//
//                        scene.Add(builder.CreateBenchmarkObjects());
//                        return scene;
//                    }
//                }
            };
        }

        private static RenderWindow CreateWindow()
        {
            var videoMode = SelectVideoMode(_configuration);

            var window = new RenderWindow(videoMode, "Physics.Samples",
                _configuration.Mode != Mode.Windowed ? Styles.Fullscreen : Styles.Close);
            window.SetVerticalSyncEnabled(true);
            return window;
        }

        private static SampleType ShowDemosMenu(RenderWindow window, List<Button> demoSelectingMenu)
        {
            _selectedSample = SampleType.None;
            EventHandler<KeyEventArgs> handler = (sender, args) =>
            {
                if (args.Code == Keyboard.Key.Escape) window.Close();
            };

            window.KeyPressed += handler;
            while (_selectedSample == SampleType.None && window.IsOpen())
            {
                window.Clear(Color.White);
                foreach (var button in demoSelectingMenu)
                {
                    window.Draw(button);
                }
                window.Display();
                window.DispatchEvents();
            }
            window.KeyPressed -= handler;
            return _selectedSample;
        }

        private static void HandleDemoKeys(object sender, KeyEventArgs args)
        {
            if (args.Code == Keyboard.Key.Escape) ((RenderWindow) sender).Close();
            else { _selectedSample = SampleType.Soft_Body;}
        }

        private static EventHandler<KeyEventArgs> ExitOnEscape(RenderWindow window)
        {
            return (sender, args) =>
            {
                if (args.Code == Keyboard.Key.Escape) window.Close();
            };
        }

        private static VideoMode SelectVideoMode(Configuration config)
        {
            if (config.Mode != Mode.Auto)
            {
                var videoMode = new VideoMode((uint) config.ScreenWidth, (uint) config.ScreenHeight);
                if (videoMode.IsValid()) return videoMode;
            }
            return VideoMode.DesktopMode;
        }
    }
}