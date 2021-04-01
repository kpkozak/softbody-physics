using System;
using System.Collections.Generic;
using System.Linq;
using Physics;
using Physics.Bodies;
using Physics.Constraints;
using Samples.Drawables;
using SFML.Graphics;

namespace Samples.Samples {
    internal class Demo : IDemo, IDisposable
    {
        private readonly List<Drawable> _physicsDrawables;
        private readonly IEnumerable<Drawable> _guiItems;
        private readonly Action _eventUnsubscriber;
        public PhysicsScene Physics { get; }

        public Demo(PhysicsScene physics, IEnumerable<Drawable> guiItems, Action eventUnsubscriber)
        {
            Physics = physics;
            Physics.BodyAdded += UpdateSprites;
            Physics.ConstraintAdded += UpdateSprites;

            _physicsDrawables = physics.Objects.Select(body => Extensions.ToDrawable(body)).ToList();
            _guiItems = guiItems;
            _eventUnsubscriber = eventUnsubscriber;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Clear(Color.White);
            foreach (var physicsDrawable in _physicsDrawables)
            {
                physicsDrawable.Draw(target, states);
            }
            foreach (var guiItem in _guiItems)
            {
                guiItem.Draw(target, states);
            }
        }

        public void Update(double elapsedTime)
        {
            Physics.Update(elapsedTime);
        }

        public void Dispose()
        {
            Physics.BodyAdded -= UpdateSprites;
            _eventUnsubscriber();
        }

        private void UpdateSprites(object sender, Body body)
        {
            _physicsDrawables.Add(body.ToDrawable());
        }

        private void UpdateSprites(object sender, IConstraint constraint)
        {
            _physicsDrawables.Add(constraint.ToDrawable());
        }
    }
}