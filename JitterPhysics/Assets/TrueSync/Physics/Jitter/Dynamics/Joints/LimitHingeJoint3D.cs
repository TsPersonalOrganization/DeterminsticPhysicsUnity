/* Copyright (C) <2009-2011> <Thorben Linneweber, Jitter Physics>
* 
*  This software is provided 'as-is', without any express or implied
*  warranty.  In no event will the authors be held liable for any damages
*  arising from the use of this software.
*
*  Permission is granted to anyone to use this software for any purpose,
*  including commercial applications, and to alter it and redistribute it
*  freely, subject to the following restrictions:
*
*  1. The origin of this software must not be misrepresented; you must not
*      claim that you wrote the original software. If you use this software
*      in a product, an acknowledgment in the product documentation would be
*      appreciated but is not required.
*  2. Altered source versions must be plainly marked as such, and must not be
*      misrepresented as being the original software.
*  3. This notice may not be removed or altered from any source distribution. 
*/

namespace TrueSync.Physics3D
{
    public class LimitHingeJoint3D : HingeJoint3D
    {

        public PointPointDistance distance { get; private set; }


        public override FP AppliedImpulse
        {
            get
            {
                return base.AppliedImpulse + distance.AppliedImpulse;
            }
        }


        public LimitHingeJoint3D(IWorld world, IBody3D body1, IBody3D body2, TSVector position, TSVector hingeAxis, FP minLimit, FP maxLimit) : base(world,body1,body2,position,hingeAxis)
        {

            TSVector perpDir = TSVector.up;

            if (TSVector.Dot(perpDir, hingeAxis) > 0.1f) perpDir = TSVector.right;

            TSVector sideAxis = TSVector.Cross(hingeAxis, perpDir);
            perpDir = TSVector.Cross(sideAxis, hingeAxis);
            perpDir.Normalize();

            FP len = 15;

            TSVector hingeRelAnchorPos0 = perpDir * len;

            FP angleToMiddle = FP.Half * (minLimit - maxLimit);
            TSMatrix outMatrix;
            TSMatrix.CreateFromAxisAngle(ref hingeAxis, -angleToMiddle * FP.Deg2Rad, out outMatrix);

            TSVector hingeRelAnchorPos1 = TSVector.Transform(hingeRelAnchorPos0, outMatrix);

            FP hingeHalfAngle = FP.Half * (minLimit + maxLimit);
            FP allowedDistance = len * 2 * FP.Sin(hingeHalfAngle * FP.Half * FP.Deg2Rad);

            TSVector hingePos = body1.TSPosition;
            TSVector relPos0c = hingePos + hingeRelAnchorPos0;
            TSVector relPos1c = hingePos + hingeRelAnchorPos1;


            distance = new PointPointDistance((RigidBody)body1, (RigidBody)body2, relPos0c, relPos1c);
            distance.Distance = allowedDistance;
            distance.Behavior = PointPointDistance.DistanceBehavior.LimitMaximumDistance;

            StateTracker.AddTracking(distance);

        }


        public override void Activate()
        {
            base.Activate();
            World.AddConstraint(distance);
        }


        public override void Deactivate()
        {
            base.Deactivate();
            World.RemoveConstraint(distance);
        }
    }
}