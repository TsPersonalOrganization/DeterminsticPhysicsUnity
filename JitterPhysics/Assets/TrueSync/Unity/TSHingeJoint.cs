using UnityEngine;
using System;
using TrueSync.Physics3D;

namespace TrueSync
{
    [Serializable]
    public class SpringElements
    {
        public FP spring;
        public FP damper;
        public FP tagetPosition;
    }

    [Serializable]
    public class LimitElements
    {
        public FP Min;
        public FP Max;
        //TODO: Bouciness, Bounce Mi Vel, Contact Distance...
    }


    public class TSHingeJoint : TrueSyncBehaviour
    {
        HingeJoint3D thisJoint;
        TSRigidBody thisBody;


        [SerializeField]
        TSCollider connectedBody;
        [SerializeField]
        Vector3 anchor;
        [SerializeField]
        Vector3 Axis;
        //[SerializeField]
        //bool useLimits = false;
        [SerializeField]
        LimitElements Limits;
        [SerializeField]
        bool useSpring;
        [SerializeField]
        SpringElements Spring;
        [SerializeField]
        FP breakForce = FP.PositiveInfinity;
        TSVector TSWorldAxis;



        public override void OnSyncedStart()
        {
            thisBody = GetComponent<TSRigidBody>();
            IBody3D body1 = GetComponent<TSCollider>().Body;
            IBody3D body2 = connectedBody.Body;

            Vector3 worldPos = transform.TransformPoint(anchor);
            TSVector TSworldPos = worldPos.ToTSVector();

            Vector3 worldAxis = transform.TransformDirection(Axis);
            TSWorldAxis = worldAxis.ToTSVector();


            //if (useLimits)
            //    thisJoint = new LimitedHingeJoint(PhysicsWorldManager.instance.GetWorld(), body1, body2, TSworldPos, TSWorldAxis, -Limits.Min, Limits.Max);
            //else
                thisJoint = new HingeJoint3D(PhysicsWorldManager.instance.GetWorld(), body1, body2, TSworldPos, TSWorldAxis);
           
        }

        public override void OnSyncedUpdate()
        {
            if (useSpring)
            {
                //Adding a spring and damper Term to the Equation of Motion 
                thisBody.AddTorque((-1) * TSWorldAxis * ((thisJoint.getHingeAngle() - Spring.tagetPosition) * Spring.spring + thisJoint.getAngularVel() * Spring.damper));
            }

            if (TSMath.Abs(thisJoint.AppliedImpulse) >= breakForce)//@TODO: Add break torque
            {
                thisJoint.Deactivate();
                Destroy(this);
            }

        }

    }
}
