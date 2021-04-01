using Geometry.Vector;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryTests
{
    [TestFixture]
    public class VectorShowcase
    {
        [Test]
        public void Showcase()
        {
            var expected = new Vector2(5, 7);
            var result = Vector2.i + (1,2) + (3,4) + Vector2.j;

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
