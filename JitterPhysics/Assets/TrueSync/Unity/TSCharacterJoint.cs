using UnityEngine;
using System;
using TrueSync.Physics3D;

namespace TrueSync
{

	public class TSCharacterJoint : TrueSyncBehaviour
	{
		CharacterJoint3D thisJoint;
		TSRigidBody thisBody;

		[SerializeField]
		TSCollider connectedBody;

		[SerializeField]
		Vector3 anchor;

		[SerializeField]
		Vector3 Axis;
	
		[SerializeField]
		FP breakForce = FP.PositiveInfinity;

		TSVector TSWorldAxis;

		public override void OnSyncedStart ()
		{
			thisBody = GetComponent<TSRigidBody> ();


			Vector3 worldPos = transform.TransformPoint (anchor);
			TSVector TSworldPos = worldPos.ToTSVector ();

			Vector3 worldAxis = transform.TransformDirection (Axis);
			TSWorldAxis = worldAxis.ToTSVector ();

			thisJoint = new CharacterJoint3D (PhysicsWorldManager.instance.GetWorld (), GetComponent<TSCollider> ().Body, connectedBody.Body, TSworldPos, TSWorldAxis);

		}

		public override void OnSyncedUpdate ()
		{
			//if (useSpring)
			//{
			//	//Adding a spring and damper Term to the Equation of Motion 
			//	thisBody.AddTorque ((-1) * TSWorldAxis * ((thisJoint.getHingeAngle () - Spring.tagetPosition) * Spring.spring + thisJoint.getAngularVel () * Spring.damper));
			//}

			if (Input.GetKeyDown (KeyCode.T))
			{
				thisBody.AddTorque (new Vector3 (0, 90, 0).ToTSVector ());
			}

			Debug.Log (thisJoint.getHingeAngle () + " : " + thisJoint.getAngularVel ());

			//thisBody.tsTransform.rotation = TSQuaternion.Euler (TSMath.Clamp (thisBody.tsTransform.eulerAngles.x, -30, 30), thisBody.tsTransform.eulerAngles.y, thisBody.tsTransform.eulerAngles.z);

			//CharacterJoint;

			if (TSMath.Abs (thisJoint.AppliedImpulse) >= breakForce)//@TODO: Add break torque
			{
				thisJoint.Deactivate ();
				Destroy (this);
			}

		}
#if UNITY_EDITOR

        protected virtual void OnDrawGizmosSelected ()
		{
			UnityEditor.Handles.SphereHandleCap (0, transform.TransformPoint (anchor), Quaternion.identity, 0.1f, EventType.Repaint);
			Vector3 v3 = transform.rotation * Axis;
			if (v3 == Vector3.zero)
			{
				return;
			}
			UnityEditor.Handles.ArrowHandleCap (1, transform.TransformPoint (anchor), Quaternion.LookRotation (90 * v3), 1, EventType.Repaint);
		}
#endif
    }
}
