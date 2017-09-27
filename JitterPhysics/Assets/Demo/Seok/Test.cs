using TrueSync;
using UnityEngine;

public class Test : TrueSyncBehaviour
{
    public TSRigidBody spine;
    

    // Update is called once per frame
    public override void OnSyncedUpdate()
    {
        
	    spine.AddForce (new Vector3(0,500,0).ToTSVector());
		
	}
}
