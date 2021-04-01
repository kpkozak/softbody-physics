using System.Collections.Generic;
using System.Linq;
using Geometry.Shapes;
using Geometry.Vector;

namespace Physics.Bodies
{
    public class FlexConcavePolygon : Shape
    {
        private readonly (int IndexA, int IndexB, int IndexC)[] _trianglesIndices;
        private readonly RigidBody[] _particles;
        
        public FlexConcavePolygon((int IndexA, int IndexB, int IndexC)[] trianglesIndices, RigidBody[] particles)
        {
            _trianglesIndices = trianglesIndices;
            _particles = particles;
        }

        public IEnumerable<Polygon> GetTriangles()
        {
            foreach (var triangle in _trianglesIndices)
            {
                yield return new Polygon(_particles[triangle.IndexA].Position, _particles[triangle.IndexB].Position,
                    _particles[triangle.IndexC].Position);
            }
        }

        public IEnumerable<Vector2> Points
        {
            get { return _particles.Select(x => x.Position); }
        }
    }
}