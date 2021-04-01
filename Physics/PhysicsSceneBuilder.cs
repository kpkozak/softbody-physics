using System.Collections.Generic;
using Geometry.Vector;
using Physics.Bodies;
using Physics.Collision.Detection;
using Physics.Collision.Handling;
using Physics.Force.Fields;

namespace Physics
{
    public class PhysicsSceneBuilder
    {
        private readonly List<IForceField> _globalForceFields = new List<IForceField>();
        // todo make something with this
        public PhysicsScene Build(IList<Body> objects)
        {
            var collider = new ColliderFactory().Create();

            var collisionHandler = new ImpulseCollisionHandler(new DistanceConstraintResolver());
            collider.ObjectsColliding += collisionHandler.HandleCollision;

            foreach (var body in objects)
            {
                body._globalForceFields = _globalForceFields;
            }

            return new PhysicsScene(objects, collider, new DeferredCollisionHandler(collisionHandler),
                _globalForceFields);
        }

        public PhysicsSceneBuilder WithGravity(double g, Vector2 direction)
        {
            _globalForceFields.Add(new HomogenousGravitationField(g, direction));
            return this;
        }

        public PhysicsSceneBuilder WithForceField(IForceField field)
        {
            _globalForceFields.Add(field);
            return this;
        }

        public PhysicsSceneBuilder WithForceFields(IEnumerable<IForceField> fields)
        {
            _globalForceFields.AddRange(fields);
            return this;
        }
    }
}