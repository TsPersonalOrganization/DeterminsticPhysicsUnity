using UnityEngine;
using System;
using TrueSync.Physics3D;

namespace TrueSync
{
    public class TSPrismaticJoint : TrueSyncBehaviour
    {
        PrismaticJoint3D thisJoint;
        TSRigidBody thisBody;

        [SerializeField]
        FP minimumDistance;

        [SerializeField]
        FP maximumDistance;

        [SerializeField]
        TSCollider connectedBody;
        [SerializeField]
        Vector3 anchor;
        [SerializeField]
        Vector3 Axis;


        [SerializeField]
        FP breakForce = FP.PositiveInfinity;
        TSVector TSWorldAxis;



        public override void OnSyncedStart()
        {
            base.OnSyncedStart();

            thisBody = GetComponent<TSRigidBody>();
            

            IBody3D body1 = GetComponent<TSCollider>().Body;
            IBody3D body2 = connectedBody.Body;

            Vector3 worldPos = transform.TransformPoint(anchor);
            TSVector TSworldPos = worldPos.ToTSVector();

            Vector3 worldAxis = transform.TransformDirection(Axis);
            TSWorldAxis = worldAxis.ToTSVector();
            
            thisJoint = new PrismaticJoint3D(PhysicsWorldManager.instance.GetWorld(), body1, body2, minimumDistance, maximumDistance);
        }

        public override void OnSyncedUpdate()
        {

        }
#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.SphereHandleCap(0, transform.TransformPoint(anchor), Quaternion.identity, 0.1f, EventType.Repaint);

            Vector3 v3 = transform.rotation * Axis;

            if (v3 == Vector3.zero)
            {
                return;
            }
            UnityEditor.Handles.ArrowHandleCap(1, transform.TransformPoint(anchor), Quaternion.LookRotation(90 * v3), 1, EventType.Repaint);
        }
#endif
    }
}
