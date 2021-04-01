using System;

namespace Physics.Bodies.Materials
{
    public static class MaterialExtensions
    {
        // todo remove staticity
        public static double GetResultingRestitution(Material material, Material other)
        {
            return Math.Max(material.Restitution,other.Restitution);
        }

        public static double GetResultingFriction(Material material, Material other)
        {
            return Math.Sqrt(Math.Pow(material.Friction, 2) + Math.Pow(other.Friction, 2));
        }
    }
}