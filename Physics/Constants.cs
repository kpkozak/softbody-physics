namespace Physics
{
    public static class Constants
    {
        /// <summary>
        /// Floating point comparison epsilon
        /// </summary>
        public const double Epsilon = 1e-3;
        
        /// <summary>
        /// Size of const time step
        /// </summary>
        public const double DeltaT = 0.008;

        /// <summary>
        /// Bias factor used to Baumgarte Stabilization for constraints
        /// </summary>
        public const double ConstraintBias = 0.1;
    }
}