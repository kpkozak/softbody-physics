using System;
using System.Collections.Generic;
using System.Linq;
using Geometry;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies.Materials;
using Physics.Constraints;
using Physics.Force.Fields;

namespace Physics.Bodies {
    public class SoftBodyBuilder : SoftBodyBuilder.IMassSoftBodyBuilder, SoftBodyBuilder.IShapeSoftBodyBuilder,
        SoftBodyBuilder.ILocationSoftBodyBuilder,
        SoftBodyBuilder.IDefaultsSoftBodyBuilder
    {
        private double _mass = 0;
        private Polygon _shape;
        private Vector2 _position = Vector2.Zero;
        private Material _material = Material.Default;
        private Vector2 _velocity = Vector2.Zero;
        private double _angularVelocity = 0;
        private uint _collisionLayer = 0xFFFFFFFF;
        private double _rotation;
        private IEnumerable<IForceField> _customForceFields = Enumerable.Empty<IForceField>();
        private double _maxVerticesDistance;

        internal SoftBodyBuilder()
        {
        }

        public ILocationSoftBodyBuilder WithMass(double mass)
        {
            _mass = mass;
            return this;
        }

        public IDefaultsSoftBodyBuilder WithShape(Polygon shape, double maxVerticesDistance=40)
        {
            _shape = shape;
            _maxVerticesDistance = maxVerticesDistance;
            return this;
        }

        public IShapeSoftBodyBuilder WithLocation(Vector2 position, double rotation = 0)
        {
            _position = position;
            _rotation = rotation;
            return this;
        }

        public IDefaultsSoftBodyBuilder WithMaterial(Material material)
        {
            _material = material;
            return this;
        }

        public IDefaultsSoftBodyBuilder WithVelocity(Vector2 linearVelocity, double angularVelocity)
        {
            _velocity = linearVelocity;
            _angularVelocity = angularVelocity;
            return this;
        }

        public IDefaultsSoftBodyBuilder OnLayer(uint layer)
        {
            _collisionLayer = layer;
            return this;
        }

        public IDefaultsSoftBodyBuilder ApplyCustomForceFields(IEnumerable<IForceField> fields)
        {
            _customForceFields = _customForceFields.Concat(fields);
            return this;
        }

        public IDefaultsSoftBodyBuilder ApplyCustomForceField(IForceField forceField)
        {
            _customForceFields = _customForceFields.Concat(Enumerable.Repeat(forceField, 1));
            return this;
        }

        public SoftBody Build()
        {
            var customForceFields = _customForceFields.ToList();

            _shape = _shape.AdjustToMassCenter(); // (1)

            var shapePoints = CreateOutlinePositions(_shape, _maxVerticesDistance).ToArray(); // (2)
            var massPerSingleParticle = _mass / (shapePoints.Length + 1);
            var midPoints = new List<RigidBody>();
            var objects = shapePoints.Select(x =>
                {
                    var body = RigidBody.Create()
                        .WithMass(massPerSingleParticle)
                        .WithLocation(x.Point)
                        .WithShape(Shape.Default)
                        .WithMaterial(_material)
                        .ApplyCustomForceFields(customForceFields)
                        .Build();

                    if (x.IsMidPoint) midPoints.Add(body);

                    return body;
                })
                .ToArray(); // (3)

            var center = RigidBody.Create()
                .WithMass(_mass)
                .WithLocation(_position)
                .WithShape(Shape.Default)
                .WithMaterial(_material)
                .ApplyCustomForceFields(customForceFields)
                .Build(); // (4)

            var joints = GetCentralSprings(objects, center)
                .Concat(GetRods(objects))
                .Concat(GetRods(midPoints))
                .ToArray(); // (5)

            var triangles = shapePoints.Select(x=>x.Point).ToArray().GetTriangulationIndices().ToArray();

            var flexPolygon = new FlexConcavePolygon(triangles,objects); // (6)

            var softBody = new SoftBody(flexPolygon, center, objects,
                joints, _collisionLayer);
            softBody.Rotation = _rotation;
            return softBody;
        }


        private IEnumerable<(Vector2 Point, bool IsMidPoint)> CreateOutlinePositions(Polygon shape, double maxPointsDistance = 100)
        {
            var edges = shape.Points.Select(x => x + _position).ToArray().GetEdges();
            foreach (var edge in edges)
            {
                yield return (edge.Segment.A,false);

                var lineDirection = (edge.Segment.B - edge.Segment.A).Normalize();
                var segmentLength = edge.Segment.Length;

                var splits = (int)Math.Floor(segmentLength / maxPointsDistance);
                var subsegmentLength = (segmentLength / (splits + 1));

                for (var i = 1; i <= splits; i++)
                {
                    yield return (edge.Segment.A + lineDirection * subsegmentLength * i, i==(splits+1)/2);
                }
            }
        }

        private IEnumerable<IConstraint> GetCentralSprings(RigidBody[] objects, RigidBody center)
        {
            return objects.Select(obj => SpringConstraint.Create(center, obj, Vector2.Zero, Vector2.Zero,
                DistanceConstraint.Type.Rod, _material.Flexibility.Frequency, _material.Flexibility.Damping));
        }

        private IEnumerable<IConstraint> GetDiagonalSprings(RigidBody[] objects)
        {
            for (int i = 0; i < objects.Length - 2; ++i)
            {
                for (int j = i + 2; j < objects.Length; ++j)
                {
                    if (i == 0 && j == objects.Length - 1)
                        break;

                    yield return SpringConstraint.Create(objects[i], objects[j], Vector2.Zero, Vector2.Zero,
                        DistanceConstraint.Type.Rod, _material.Flexibility.Frequency, _material.Flexibility.Damping);
                }
            }
        }

        private IEnumerable<IConstraint> GetRods(IList<RigidBody> bodies)
        {

            yield return SpringConstraint.Create(bodies.First(), bodies.Last(), Vector2.Zero, Vector2.Zero,
                DistanceConstraint.Type.Rod, _material.Flexibility.Frequency, _material.Flexibility.Damping);
            for (var i = 1; i < bodies.Count; ++i)
                yield return SpringConstraint.Create(bodies[i - 1], bodies[i], Vector2.Zero, Vector2.Zero,
                    DistanceConstraint.Type.Rod, _material.Flexibility.Frequency, _material.Flexibility.Damping);
        }

        public interface IShapeSoftBodyBuilder
        {
            IDefaultsSoftBodyBuilder WithShape(Polygon shape, double maxVerticesDistance=40);
        }

        public interface IDefaultsSoftBodyBuilder
        {
            IDefaultsSoftBodyBuilder WithMaterial(Material material);
            IDefaultsSoftBodyBuilder WithVelocity(Vector2 linearVelocity, double angularVelocity);
            IDefaultsSoftBodyBuilder OnLayer(uint layer);
            IDefaultsSoftBodyBuilder ApplyCustomForceFields(IEnumerable<IForceField> fields);
            IDefaultsSoftBodyBuilder ApplyCustomForceField(IForceField forceField);
            SoftBody Build();
        }

        public interface ILocationSoftBodyBuilder
        {
            IShapeSoftBodyBuilder WithLocation(Vector2 position, double rotation = 0);
        }

        public interface IMassSoftBodyBuilder
        {
            ILocationSoftBodyBuilder WithMass(double mass);
        }
    }
}