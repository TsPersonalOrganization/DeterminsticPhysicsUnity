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

    public TSRigidBody spring;
    public TSRigidBody ball;

    public TSRigidBody rightLeg;
    public TSRigidBody leftLeg;
    public TSRigidBody rightFoot;
    public TSRigidBody leftFoot;


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

    int counter = 0;
    public override void OnSyncedUpdate()
    {
        base.OnSyncedUpdate();

        counter++;

        if (counter > 10)
        {
            //Debug.Log("testing spine and pevlis");
            head.AddForce(TSVector.up * 300);
            spine.AddForce(TSVector.up * 300);
            pelvis.AddForce(TSVector.up * 300);

            leftLeg.AddForce(TSVector.down * 300);
            rightLeg.AddForce(TSVector.down * 300);

            leftFoot.AddForce(TSVector.down * 150);
            rightFoot.AddForce(TSVector.down * 150);

            //spring.AddForce(TSVector.down * 0);
            //ball.AddForce(TSVector.down * 10);

            JitterPhysicsTools.AlignToVector(spine, spine.tsTransform.forward, pelvis.tsTransform.forward, 0.1f, 6.0f);
            JitterPhysicsTools.AlignToVector(head, head.tsTransform.forward, pelvis.tsTransform.forward, 0.1f, 6.0f);

            //ball.AddTorque(TSVector.right * 100);
        }
        //pelvis.AddForce(TSVector.up * 20, ForceMode.VelocityChange);
        //head.AddForce(TSVector.up * 20, ForceMode.VelocityChange);

       // cube.AddForce(new Vector3(0, 2000000,0).ToTSVector());
    }

}
