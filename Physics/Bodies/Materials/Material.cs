using System;

namespace Physics.Bodies.Materials
{
    public class Material
    {
        public static readonly Material Default = new Material(0,1, new Softness(0,1));

        /// <exception cref="ArgumentException">Coefficient of restitution should be between 0 and 1</exception>
        public Material(double restitution, double friction, Softness flexibility = null)
        {
            if(!(restitution>=0 && restitution<=1))
                throw new ArgumentException("Coefficient of restitution should be between 0 and 1");
            Restitution = restitution;
            Friction = friction;
            Flexibility = flexibility ?? Softness.Default;
        }

        public double Restitution { get; private set; }
        public double Friction { get; private set; }
        public Softness Flexibility { get; private set; }

        public class Softness
        {
            public static readonly Softness Default = new Softness(100, 0);

            public Softness(double frequency, double damping)
            {
                Frequency = frequency;
                Damping = damping;
            }

            public double Frequency { get; }
            public double Damping { get; }
        }
    }
}