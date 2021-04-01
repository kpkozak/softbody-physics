using System;
using SFML.Graphics;

namespace Samples.Samples
{
    internal interface IDemo: Drawable, IDisposable
    {
        void Update(double elapsedTime);
    }
}