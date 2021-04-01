using System;
using System.Collections.Generic;
using Physics.Integrator;
using Physics.Items;

namespace Physics.Configure
{


    public class PhysicsBuilder
    {
        private readonly List<GroupBuilder> _groups;


        private PhysicsBuilder()
        {
            _groups = new List<GroupBuilder>();
            
        }

        public static PhysicsBuilder ConfigurePhysics()
        {
            return new PhysicsBuilder();
        }

        public PhysicsBuilder AddGroup(string groupName, Func<GroupBuilder, GroupBuilder> func)
        {
            var builder = new GroupBuilder(groupName);
            _groups.Add(func(builder));
            return this;
        }

        public IPhysicsSystem Build()
        {
            var objects = new Dictionary<string, IItem[]>();
            foreach (var groupBuilder in _groups)
            {
                objects.Add(groupBuilder._groupId, groupBuilder._objects);
                IUpdater updater;
                if (groupBuilder._isStatic)
                {
                    updater = groupBuilder._manualUpdater ?? new DummyUpdater();
                }
                else
                {
                    updater = new PhysicsBasedUpdater();
                }
            }

        }

        public class Group
        {
            string Identifier { get; set; }
            public IList<IItem> Items { get; set; }
            public bool SelfCollision { get; set; }
            public IList<string> CollisionWithGroups { get; set; }
            public IList<ICollisionHandler> CollisionHandlers { get; set; }
            public IUpdater Updater { get; set; }
        }

        public class GroupBuilder
        {
            internal readonly string _groupId;
            internal bool _isStatic = false;
            internal IUpdater _manualUpdater;
            internal IItem[] _objects;
            internal List<string> _gravityWithGroups;
            internal double _airResistanceFactor;
            internal List<string> _collisionWithGroups;
            private readonly List<ICollisionHandler> _customCollisionHandlers;
            private readonly bool _selfCollision;

            public GroupBuilder(string groupId)
            {
                _customCollisionHandlers = new List<ICollisionHandler>();
                _collisionWithGroups = new List<string>();
                _airResistanceFactor = 0;
                _gravityWithGroups = new List<string>();
                _groupId = groupId;
                _objects = new IItem[0];
                _selfCollision = false;
            }

            public GroupBuilder AsStaticObjects()
            {
                _isStatic = true;
                return this;
            }

            public GroupBuilder UpdatedBy(IUpdater updater)
            {
                _manualUpdater = updater;
                return this;
            }

            public GroupBuilder AddObjects(params IParticle[] objects)
            {
                _objects = objects;
                return this;
            }

            public GroupBuilder WithGravity(string groupId)
            {
                _gravityWithGroups.Add(groupId);
                return this;
            }

            public GroupBuilder WithAirResistance(double airResistanceFactor)
            {
                _airResistanceFactor = airResistanceFactor;
                return this;
            }

            public GroupBuilder WithElasticBodyCollision(string groupId)
            {
                _collisionWithGroups.Add(groupId);
                return this;
            }
            public GroupBuilder WithCollisionHandlers(params ICollisionHandler[] customCollisionHandlers)
            {
                foreach (var handler in customCollisionHandlers)
                {
                    _customCollisionHandlers.Add(handler);
                }
                return this;
            }

            public Group Build()
            {
                _customCollisionHandlers.Insert(0, new SpringReboundCollisionHandler());
                return new Group()
                {
                    Items = _objects,
                    SelfCollision = _selfCollision,
                    CollisionWithGroups = _collisionWithGroups,
                    CollisionHandlers = _customCollisionHandlers
                };
            }
        }
    }
}
