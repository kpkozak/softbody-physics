using System.Collections.Generic;
using System.Linq;
using Geometry;
using Geometry.Shapes;
using Geometry.Vector;
using Physics.Bodies.Materials;
using Physics.Force.Fields;

namespace Physics.Bodies {
    public class RigidBodyBuilder : RigidBodyBuilder.IMassRigidBodyBuilder, RigidBodyBuilder.IShapeRigidBodyBuilder,
        RigidBodyBuilder.ILocationRigidBodyBuilder,
        RigidBodyBuilder.IDefaultsRigidBodyBuilder
    {
        private readonly UpdateBehavior _updateBehavior = UpdateBehavior.PhysicsEnabled;
        private double _mass = 0;
        private Shape _shape;
        private Vector2 _position = Vector2.Zero;
        private Material _material = Material.Default;
        private Vector2 _velocity = Vector2.Zero;
        private double _angularVelocity = 0;
        private uint _collisionLayer = 0xFFFFFFFF;
        private double _rotation;
        private IEnumerable<IForceField> _customForceFields = Enumerable.Empty<IForceField>();

        internal RigidBodyBuilder(UpdateBehavior updateBehavior)
        {
            _updateBehavior = updateBehavior;
        }

        public ILocationRigidBodyBuilder WithMass(double mass)
        {
            _mass = mass;
            return this;
        }

        public IDefaultsRigidBodyBuilder WithShape(Shape shape)
        {
            _shape = shape;
            return this;
        }

        public IShapeRigidBodyBuilder WithLocation(Vector2 position, double rotation = 0)
        {
            _position = position;
            _rotation = rotation;
            return this;
        }

        public IDefaultsRigidBodyBuilder WithMaterial(Material material)
        {
            _material = material;
            return this;
        }

        public IDefaultsRigidBodyBuilder WithVelocity(Vector2 linearVelocity, double angularVelocity)
        {
            _velocity = linearVelocity;
            _angularVelocity = angularVelocity;
            return this;
        }

        public IDefaultsRigidBodyBuilder OnLayer(uint layer)
        {
            _collisionLayer = layer;
            return this;
        }

        public IDefaultsRigidBodyBuilder ApplyCustomForceFields(IEnumerable<IForceField> fields)
        {
            _customForceFields = _customForceFields.Concat(fields);
            return this;
        }

        public IDefaultsRigidBodyBuilder ApplyCustomForceField(IForceField forceField)
        {
            _customForceFields = _customForceFields.Concat(Enumerable.Repeat(forceField, 1));
            return this;
        }

        public RigidBody Build()
        {
            _shape = _shape is Polygon ? (_shape as Polygon).AdjustToMassCenter() : _shape;
            var inertia = _shape.CountInertia(_mass, (0, 0));
            return new RigidBody(_updateBehavior, _mass, inertia, _position, _rotation, _velocity, _angularVelocity,
                _shape, _material, _collisionLayer, _customForceFields);
        }

        public interface IShapeRigidBodyBuilder
        {
            IDefaultsRigidBodyBuilder WithShape(Shape shape);
        }

        public interface IDefaultsRigidBodyBuilder
        {
            IDefaultsRigidBodyBuilder WithMaterial(Material material);
            IDefaultsRigidBodyBuilder WithVelocity(Vector2 linearVelocity, double angularVelocity);
            IDefaultsRigidBodyBuilder OnLayer(uint layer);
            IDefaultsRigidBodyBuilder ApplyCustomForceFields(IEnumerable<IForceField> fields);
            IDefaultsRigidBodyBuilder ApplyCustomForceField(IForceField forceField);
            RigidBody Build();
        }

        public interface ILocationRigidBodyBuilder
        {
            IShapeRigidBodyBuilder WithLocation(Vector2 position, double rotation = 0);
        }

        public interface IMassRigidBodyBuilder
        {
            ILocationRigidBodyBuilder WithMass(double mass);
        }
    }
}