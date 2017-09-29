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

using System;

namespace TrueSync.Physics3D
{

    /// <summary>
    /// Connects to bodies with a hinge joint.
    /// </summary>
    public class BasicJoint3D : Joint3D
    {

        private FixedPoint[] worldPointConstraint;

        private IBody3D firstBody;
        private IBody3D secondBody;

        private TSVector hingeA;

        /// <summary>
        /// Initializes a new instance of the HingeJoint class.
        /// </summary>
        /// <param name="world">The world class where the constraints get added to.</param>
        /// <param name="body1">The first body connected to the second one.</param>
        /// <param name="body2">The second body connected to the first one.</param>
        /// <param name="position">The position in world space where both bodies get connected.</param>
        /// <param name="hingeAxis">The axis if the hinge.</param>
        /// 
        public static void Create(IWorld world, IBody3D body1, IBody3D body2, TSVector anchorPosition, TSVector anchorPosition2)
        {
            new BasicJoint3D(world, body1, body2, anchorPosition, anchorPosition2);
        }

        public BasicJoint3D(IWorld world, IBody3D body1, IBody3D body2, TSVector anchorPosition, TSVector anchorPosition2) : base((World)world)
        {
            firstBody = body1;
            secondBody = body2;
            //hingeA = hingeAxis;
            worldPointConstraint = new FixedPoint[2];
            ///hingeAxis *= FP.Half;
            TSVector anchor = anchorPosition;
            //TSVector.Add(ref anchor, ref hingeAxis, out anchor);
            TSVector anchor2 = anchorPosition2;
            //TSVector.Subtract(ref anchor2, ref hingeAxis, out anchor2);
    
            //pointLock = new PointPointDistance((RigidBody)body1, (RigidBody)body2, position, position);
            //pointLock.Distance = 0;

            //pointLock.Behavior = PointPointDistance.DistanceBehavior.LimitMaximumDistance;


            worldPointConstraint[0] = new FixedPoint((RigidBody)body1, (RigidBody)body2, ((RigidBody)body1).position, anchorPosition);
            worldPointConstraint[1] = new FixedPoint((RigidBody)body2, (RigidBody)body1, ((RigidBody)body2).position, anchorPosition);
            

            StateTracker.AddTracking(worldPointConstraint[0]);
            StateTracker.AddTracking(worldPointConstraint[1]);

            Activate();
        }

        public FixedPoint PointOnPointConstraint1 { get { return worldPointConstraint[0]; } }

        public FixedPoint PointOnPointConstraint2 { get { return worldPointConstraint[1]; } }

        //public virtual FP AppliedImpulse { get { return worldPointConstraint[0].AppliedImpulse + worldPointConstraint[1].AppliedImpulse; } }

        public FP getHingeAngle()
        {
            return (firstBody.TSOrientation.eulerAngles - secondBody.TSOrientation.eulerAngles).magnitude;
        }

        public FP getAngularVel()
        {
            return (firstBody.TSAngularVelocity - secondBody.TSAngularVelocity).magnitude;
        }


        /// <summary>
        /// Adds the internal constraints of this joint to the world class.
        /// </summary>
        public override void Activate()
        {
            World.AddConstraint(worldPointConstraint[0]);
            World.AddConstraint(worldPointConstraint[1]);
           // World.AddConstraint(pointLock);
        }

        /// <summary>
        /// Removes the internal constraints of this joint from the world class.
        /// </summary>
        public override void Deactivate()
        {
            World.RemoveConstraint(worldPointConstraint[0]);
            World.RemoveConstraint(worldPointConstraint[1]);
            //World.RemoveConstraint(pointLock);
        }

    }
}
