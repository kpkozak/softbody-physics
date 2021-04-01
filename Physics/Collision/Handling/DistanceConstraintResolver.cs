using Geometry.Vector;
using Physics.Bodies;

namespace Physics.Collision.Handling
{
    internal class DistanceConstraintResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <param name="resolveDirection">vector along which constraint has to be resolved (normalized vector of length 1)</param>
        /// <param name="distanceToResolve">Distance which bodies have to be moved along resolveDirection</param>
        public void ResolveDistanceConstraint(Body object1, Body object2, Vector2 resolveDirection, double distanceToResolve)
        {
            if (object1.BehaviorType == UpdateBehavior.Static)
            {
                ResolveByMovingSingleObject(object2, resolveDirection, distanceToResolve);
            }
            else if (object2.BehaviorType == UpdateBehavior.Static)
            {
                ResolveByMovingSingleObject(object1, -resolveDirection, distanceToResolve);
            }
            else
            {
                var totalInverseMass = (object1.InverseMass) + (object2.InverseMass);
                var movePerInverseMassUnit = resolveDirection *
                                             (-distanceToResolve / totalInverseMass);

                object1.Position += movePerInverseMassUnit * (object1.InverseMass);
                object2.Position -= movePerInverseMassUnit * (object2.InverseMass);
            }
        }

        private void ResolveByMovingSingleObject(Body body, Vector2 collisionNormal, double interpenetration)
        {
            body.Position += collisionNormal*interpenetration;
        }
    }
}