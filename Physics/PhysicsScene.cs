using System;
using System.Collections.Generic;
using System.Linq;
using Physics.Bodies;
using Physics.Collision.Detection;
using Physics.Collision.Handling;
using Physics.Constraints;
using Physics.Force.Fields;
using Physics.Perf;

namespace Physics
{
    public sealed class PhysicsScene
    {
        public static PhysicsSceneBuilder Create()
        {
            return new PhysicsSceneBuilder();
        }

        private readonly ICollider _collider;

        private readonly IList<Body> _objects;
        private readonly List<IConstraint> _constraints;
        private double _dtRest;
        // stored only to avoid garbage collecting
        private readonly DeferredCollisionHandler _collisionHandler;
        public List<IForceField> GlobalForceFields { get; }
        public event EventHandler<Body> BodyAdded;
        public event EventHandler<IConstraint> ConstraintAdded;

        internal PhysicsScene(IList<Body> objects, ICollider collider, DeferredCollisionHandler collisionHandler, List<IForceField> globalForceFields)
        {
            GlobalForceFields = globalForceFields;
            _collider = collider;
            _collisionHandler = collisionHandler;
            _objects = objects;
            _constraints = new List<IConstraint>();
        }

        public void Add(RigidBody body)
        {
            _objects.Add(body);
            body._globalForceFields = GlobalForceFields;

            RaiseBodyAdded(body);
        }

        public void Add(SoftBody body)
        {
            _objects.Add(body);
            body._globalForceFields = GlobalForceFields;
            _constraints.AddRange(body._joints);

            RaiseBodyAdded(body);
        }

        public void Update(double dt)
        {
            dt += _dtRest;
            while (dt > Constants.DeltaT)
            {
                FixedStepUpdate();
                dt -= Constants.DeltaT;
            }
            _dtRest = dt;
        }

        private void FixedStepUpdate()
        {
            PerformanceMonitor.Measure(ActionType.Total, () =>
            {
                PerformanceMonitor.Measure(ActionType.CollisionDetection, () =>
                {
                    foreach (var item in _objects)
                    {
                        for (int i = _objects.IndexOf(item) + 1; i < _objects.Count(); i++)
                        {
                            _collider.Collide(item, _objects[i]);
                        }
                    }
                });

                PerformanceMonitor.Measure(ActionType.CollisionHandling, () =>
                {
                    _collisionHandler.HandleAllCollisions();
                });

                PerformanceMonitor.Measure(ActionType.BodiesUpdate, () =>
                {
                    for (var i = _objects.Count - 1; i >= 0; --i)
                    {
                        var body = _objects[i];
                        body.Update(Constants.DeltaT);
                    }
                });

                PerformanceMonitor.Measure(ActionType.JointsUpdate, () =>
                {
                    foreach (var joint in _constraints)
                    {
                        joint.Prepare();
                    }
                    for (int i = 0; i < 5; ++i)
                    {
                        foreach (var joint in _constraints)
                        {
                            joint.Resolve();
                        }
                    }
                });
            });
        }

        public IEnumerable<Body> Objects { get { return _objects; } }

        public void Add(IConstraint constraint)
        {
            _constraints.Add(constraint);
            RaiseConstraintAdded(constraint);
        }

        public void Add(IList<Body> bodies)
        {
            foreach (var body in bodies)
            {
                if (body is RigidBody)
                    Add(body as RigidBody);
                else
                    Add(body as SoftBody);
            }
        }

        private void RaiseBodyAdded(Body body)
        {
            BodyAdded?.Invoke(this, body);
        }

        private void RaiseConstraintAdded(IConstraint constraint)
        {
            ConstraintAdded?.Invoke(this, constraint);
        }
    }
}