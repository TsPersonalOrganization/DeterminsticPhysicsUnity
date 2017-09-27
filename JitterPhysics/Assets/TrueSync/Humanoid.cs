using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;
using Dynamics;


public class Humanoid : TrueSyncBehaviour
{
    public TSRigidBody head;
    public TSRigidBody spine;
    public TSRigidBody pelvis;

    public TSRigidBody leftArm;
    public TSRigidBody rightArm;

    public TSRigidBody leftLeg;
    public TSRigidBody rightLeg;

    public TSRigidBody leftFoot;
    public TSRigidBody rightFoot;

    public override void OnSyncedStart()
    {
        
    }

    /**
    * @brief Sets player inputs.
    **/
    public override void OnSyncedInput()
    {
    }

    /**
    * @brief Updates ball's movements and instantiates new ball objects when player press space.
    **/
    public override void OnSyncedUpdate()
    {
        base.OnSyncedUpdate();

        //Debug.Log("testing spine and pevlis");
        head.AddForce(TSVector.up * 100);
        spine.AddForce(TSVector.up * 100);
        pelvis.AddForce(TSVector.up * 200);

        leftLeg.AddForce(TSVector.down * 150);
        rightLeg.AddForce(TSVector.down * 150);

        leftFoot.AddForce(TSVector.down * 50);
        rightFoot.AddForce(TSVector.down * 50);

        JitterPhysicsTools.AlignToVector(spine, spine.tsTransform.forward.ToVector(), pelvis.transform.forward, 0.1f, 6.0f);
        JitterPhysicsTools.AlignToVector(head, head.tsTransform.forward.ToVector(), pelvis.transform.forward, 0.1f, 6.0f);

        
        //pelvis.AddForce(TSVector.up * 20, ForceMode.VelocityChange);
        //head.AddForce(TSVector.up * 20, ForceMode.VelocityChange);

       // cube.AddForce(new Vector3(0, 2000000,0).ToTSVector());
    }

}
