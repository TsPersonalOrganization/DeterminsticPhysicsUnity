using TrueSync;
using UnityEngine;

public class NewBehaviourScript : TrueSyncBehaviour
{

	public TSRigidBody rigidBody;

	public Vector3 torque;

	public Vector3 force;

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Input.GetKeyDown (KeyCode.T))
		{
			rigidBody.AddTorque (torque.ToTSVector ());
		}
		if (Input.GetKeyDown (KeyCode.F))
		{
			rigidBody.AddForce (force.ToTSVector ());
		}
	}
}
