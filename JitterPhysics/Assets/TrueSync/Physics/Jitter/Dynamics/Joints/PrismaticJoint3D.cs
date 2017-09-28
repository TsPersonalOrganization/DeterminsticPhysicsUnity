/////////////////////////////////////////////////////////////////////////////
// Copyright (C) <2017> <Tiger Shan>
// this is a custom prismatic Joint implemented into jitter Physics.
// the original version was erased, and now it is goig to be reimplemented.
/////////////////////////////////////////////////////////////////////////////

namespace TrueSync.Physics3D
{
    public class PrismaticJoint3D : Joint3D
    {
        // form prismatic joint
        FixedAngle fixedAngle;

        PointOnLine pointOnLine;

        PointPointDistance minDistance = null;
        PointPointDistance maxDistance = null;

        public PrismaticJoint3D(IWorld world, IBody3D body1, IBody3D body2, FP minimumDistance, FP maximumDistance)
            : base((World)world)
        {
            fixedAngle = new FixedAngle((RigidBody)body1, (RigidBody)body2);
            pointOnLine = new PointOnLine((RigidBody)body1, (RigidBody)body2, ((RigidBody)body1).position , ((RigidBody)body2).position);

            //minDistance = new PointPointDistance((RigidBody)body1, (RigidBody)body2, ((RigidBody)body1).position, ((RigidBody)body2).position);
            //minDistance.Behavior = PointPointDistance.DistanceBehavior.LimitMinimumDistance;
            //minDistance.Distance = minimumDistance;

            //maxDistance = new PointPointDistance((RigidBody)body1, (RigidBody)body2, ((RigidBody)body1).position, ((RigidBody)body2).position);
            //maxDistance.Behavior = PointPointDistance.DistanceBehavior.LimitMaximumDistance;
            //maxDistance.Distance = maximumDistance;

            StateTracker.AddTracking(fixedAngle);
            StateTracker.AddTracking(pointOnLine);
            //StateTracker.AddTracking(minDistance);
            //StateTracker.AddTracking(maxDistance);

            Activate();
        }

        public override void Activate()
        {            
            //World.AddConstraint(maxDistance);
            //World.AddConstraint(minDistance);

            World.AddConstraint(fixedAngle);
            World.AddConstraint(pointOnLine);
        }

        public override void Deactivate()
        {
            //World.RemoveConstraint(maxDistance);
            //World.RemoveConstraint(minDistance);

            World.RemoveConstraint(fixedAngle);
            World.RemoveConstraint(pointOnLine);
        }
    }
}
