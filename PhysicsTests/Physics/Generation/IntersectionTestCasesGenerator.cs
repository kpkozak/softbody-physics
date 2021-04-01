using System;
using System.IO;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;
using NUnit.Framework;
using PostScriptImage;

namespace PhysicsTests.Generation
{
    [TestFixture, Explicit]
    // ReSharper disable once TestClassNameSuffixWarning
    // ReSharper disable once TestClassNameSuffixWarning
    public class IntersectionTestCasesGenerator
    {
        private static Random _random = new Random();
        [Test]
        public void GenerateCases()
        {
            if (File.Exists("intersectionTestCases.txt"))
                File.Delete("intersectionTestCases.txt");
            var testCases = 50;
            for (int i = 0; i < testCases; ++i)
            {
                var polygon1 = new Polygon(new Vector2[]
                {
                    new Vector2(-GetNextRandomCoord(),-GetNextRandomCoord()),
                    new Vector2(GetNextRandomCoord(),-GetNextRandomCoord()), 
                    new Vector2(GetNextRandomCoord(),GetNextRandomCoord()), 
                    new Vector2(-GetNextRandomCoord(),GetNextRandomCoord())
                });

                var polygon2 = new Polygon(new Vector2[]
                {
                    new Vector2(-GetNextRandomCoord(),-GetNextRandomCoord()),
                    new Vector2(GetNextRandomCoord(),-GetNextRandomCoord()), 
                    new Vector2(GetNextRandomCoord(),GetNextRandomCoord()), 
                    new Vector2(-GetNextRandomCoord(),GetNextRandomCoord())
                });

                var position1 = GetNextRandomPosition();
                var position2 = -GetNextRandomPosition();
                var imageTranslation = -position2+Vector2.i*120+Vector2.j*120;
                var image = new Image(new Vector2(0,0), new Vector2(350,350));
                image.AddPolygon(new Polygon(polygon1.Points.Select(x=>x+position1+imageTranslation).ToArray()));
                image.AddPolygon(new Polygon(polygon2.Points.Select(x => x + position2+imageTranslation).ToArray()));
                image.Save(i + ".ps");

                var lineToWrite = String.Empty + position1.X + " " + position1.Y;
                foreach (var point in polygon1.Points)
                {
                    lineToWrite = lineToWrite + " " + point.X + " " + point.Y;
                }
                lineToWrite = lineToWrite + " " + position2.X + " " + position2.Y;
                foreach (var point in polygon2.Points)
                {
                    lineToWrite = lineToWrite + " " + point.X + " " + point.Y;
                }
                lineToWrite = lineToWrite + Environment.NewLine;

                File.AppendAllText("intersectionTestCases.txt",lineToWrite);
            }
        }

        private Vector2 GetNextRandomPosition()
        {
            return new Vector2(_random.Next(0, 60), _random.Next(0, 60));
        }

        private double GetNextRandomCoord()
        {
            return _random.Next(0, 100);
        }
    }
}
