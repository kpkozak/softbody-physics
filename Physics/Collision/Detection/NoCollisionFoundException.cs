using System;

namespace Physics.Collision.Detection
{
    public class NoCollisionFoundException : Exception
    {
        public NoCollisionFoundException(string message): base(message)
        {
        }
    }
}