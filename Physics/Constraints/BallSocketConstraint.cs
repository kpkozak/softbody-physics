using Geometry.Matrix;
using Geometry.Vector;

namespace Physics.Constraints
{
//    public class BallSocketConstraint : IConstraint
//    {
//        private Vector2 bias;
//        private readonly RigidBody bodyA;
//        private readonly RigidBody bodyB;
//
//        private Vector2 gra;
//        private Vector2 grb;
//
//        private Matrix2 kinv;
//        private readonly Vector2 ra;
//        private readonly Vector2 rb;
//
//        public BallSocketConstraint(RigidBody bodyA, RigidBody bodyB, Vector2 ra, Vector2 rb)
//        {
//            //var posRaLine = new Segment(bodyA.Position, aAnchorPoint).Line;
//
//            var globalRa = bodyA.GetRotationMatrix() * ra;
//            var globalRb = bodyB.GetRotationMatrix() * rb;
//
//            this.bodyA = bodyA;
//            this.bodyB = bodyB;
//            this.ra = ra;
//            this.rb = rb;
//        }
//
//        public void Resolve()
//        {
//            Prepare();
//            var vab = bodyB.Velocity + (Vector2) bodyA.AngularVelocity.Cross(grb.AsVector3()) -
//                      bodyA.Velocity - (Vector2) bodyA.AngularVelocity.Cross(gra.AsVector3());
//            var j = -kinv * (vab + bias);
//
//            bodyA.Velocity -= bodyA.InverseMass * j;
//            bodyB.Velocity += bodyB.InverseMass * j;
//            bodyA.AngularVelocity -= bodyA.InverseMomentOfInertia * gra.Cross(j);
//            bodyB.AngularVelocity += bodyB.InverseMomentOfInertia * grb.Cross(j);
//        }
//
//
//        public void Prepare()
//        {
//            var ma = bodyA.GetTransformMatrix();
//            var mb = bodyB.GetTransformMatrix();
//
//            gra = ra; // (ma * aAnchorPoint);
//            grb = rb; // (mb * bAnchorPoint);
//
//            var k = Matrix2.Diagonal(bodyA.InverseMass + bodyB.InverseMass) +
//                    Matrix2.Diagonal(bodyA.InverseMomentOfInertia) *
//                    Matrix2.Create(gra.Y * gra.Y, -gra.X * gra.Y, -gra.X * gra.Y, gra.X * gra.X) +
//                    Matrix2.Diagonal(bodyB.InverseMomentOfInertia) *
//                    Matrix2.Create(grb.Y * grb.Y, -grb.X * grb.Y, -grb.X * grb.Y, grb.X * grb.X);
//            kinv = k.Inverse();
//            bias = Constants.ConstraintBias / Constants.DeltaT * (bodyB.Position + grb - bodyA.Position - gra);
//        }
//    }
}