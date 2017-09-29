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

    public FP speedRatio;
    public TSVector previousAngularVelocity = TSVector.one;
    /**
    * @brief Key to set/get horizontal position from {@link TrueSyncInput}.
    **/
    private const byte INPUT_KEY_HORIZONTAL = 0;

    /**
    * @brief Key to set/get vertical position from {@link TrueSyncInput}.
    **/
    private const byte INPUT_KEY_VERTICAL = 1;

    /**
    * @brief Key to set/get jump state from {@link TrueSyncInput}.
    **/
    private const byte INPUT_KEY_CREATE = 2;
    

    public override void OnSyncedStart()
    {
        
    }

    /**
    * @brief Sets player inputs.
    **/
    public override void OnSyncedInput()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        bool space = Input.GetKey(KeyCode.Space);

        TrueSyncInput.SetInt(INPUT_KEY_HORIZONTAL, (int)(hor * 100));
        TrueSyncInput.SetInt(INPUT_KEY_VERTICAL, (int)(ver * 100));
        TrueSyncInput.SetBool(INPUT_KEY_CREATE, space);
    }

    /**
    * @brief Updates ball's movements and instantiates new ball objects when player press space.
    **/

    int counter = 0;
    public override void OnSyncedUpdate()
    {
        base.OnSyncedUpdate();

        FP hor = (FP)TrueSyncInput.GetInt(INPUT_KEY_HORIZONTAL) / 100;
        FP ver = (FP)TrueSyncInput.GetInt(INPUT_KEY_VERTICAL) / 100;
        bool currentCreateState = TrueSyncInput.GetBool(INPUT_KEY_CREATE);

        bool allowInput = false;

        if(hor != 0 || ver != 0)
        {
            allowInput = true;
            //Debug.Log("allow input is true: " + hor + " : " + ver);
        }


        //counter++;

        //if (counter > 10)
        //{
        //Debug.Log("testing spine and pevlis");
        head.AddForce(TSVector.up * 200);
        spine.AddForce(TSVector.up * 200);
        pelvis.AddForce(TSVector.up * 200);

        leftLeg.AddForce(TSVector.down * 200);
        rightLeg.AddForce(TSVector.down * 200);

        leftFoot.AddForce(TSVector.down * 100);
        rightFoot.AddForce(TSVector.down * 100);

        if (allowInput)
        {
            //spring.AddForce(TSVector.down * 0);
            ball.AddForce(TSVector.down * 100);
            TSVector a = new TSVector(ver, 0, -hor);

            ball.angularVelocity = a * speedRatio;

            Debug.Log(ball.angularVelocity + " a: " + a);

            if (ball.angularVelocity.magnitude > 50)
                ball.angularVelocity = previousAngularVelocity;


            previousAngularVelocity = ball.angularVelocity;
        }
        else
        {
            ball.angularVelocity = TSVector.zero;
            //ball.MovePosition(pelvis.position);
        }
        //ball.AddTorque(a * speedRatio * 10);
        pelvis.angularVelocity = TSVector.zero;
        pelvis.velocity = TSVector.zero;
        pelvis.MovePosition(new TSVector(ball.position.x, pelvis.position.y, ball.position.z));
    

        JitterPhysicsTools.AlignToVector(spine, spine.tsTransform.forward, pelvis.tsTransform.forward, 0.1f, 6.0f);
        JitterPhysicsTools.AlignToVector(head, head.tsTransform.forward, pelvis.tsTransform.forward, 0.1f, 6.0f);

        //ball.AddTorque(TSVector.right * 100);
        //}
        //pelvis.AddForce(TSVector.up * 20, ForceMode.VelocityChange);
        //head.AddForce(TSVector.up * 20, ForceMode.VelocityChange);

        // cube.AddForce(new Vector3(0, 2000000,0).ToTSVector());
    }

}
