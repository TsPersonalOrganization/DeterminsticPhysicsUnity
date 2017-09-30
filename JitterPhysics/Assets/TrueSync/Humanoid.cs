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

    private TSVector direction;
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


            direction = new TSVector(hor, 0, ver);
        }


        //counter++;

        //if (counter > 10)
        //{
        //Debug.Log("testing spine and pevlis");
        

        if (allowInput)
        {
            //spring.AddForce(TSVector.down * 0);
            ball.AddForce(TSVector.down * 100);
            TSVector a = new TSVector(ver, 0, -hor);

            ball.angularVelocity = a * speedRatio;

            //Debug.Log(ball.angularVelocity + " a: " + a);

            if (ball.angularVelocity.magnitude > 50)
                ball.angularVelocity = previousAngularVelocity;


            previousAngularVelocity = ball.angularVelocity;

            UpdateRunPose();
        }
        else
        {
            ball.angularVelocity = TSVector.zero;
            UpdateStandingPose();
        }

        ////pelvis.angularVelocity = TSVector.zero;
        //pelvis.velocity = TSVector.zero;
        //pelvis.MovePosition(new TSVector(ball.position.x, pelvis.position.y, ball.position.z));
    
        
    }


    private void UpdateStandingPose()
    {
        JitterPhysicsTools.AlignToVector(spine, spine.tsTransform.forward, direction.normalized, 0.001f, 12.0f);
        JitterPhysicsTools.AlignToVector(head, head.tsTransform.forward, direction.normalized, 0.001f, 12.0f);
        JitterPhysicsTools.AlignToVector(pelvis, pelvis.tsTransform.forward, direction.normalized, 0.001f, 12.0f);

        head.AddForce(TSVector.up * 3000);
        spine.AddForce(TSVector.up * 3000);
        pelvis.AddForce(TSVector.up * 3000);

        leftLeg.AddForce(TSVector.down * 3000);
        rightLeg.AddForce(TSVector.down * 3000);

        leftFoot.AddForce(TSVector.down * 1500);
        rightFoot.AddForce(TSVector.down * 1500);
    }


    private void UpdateRunPose()
    {
        spine.angularVelocity = TSVector.zero;
        head.angularVelocity = TSVector.zero;
        pelvis.angularVelocity = TSVector.zero;

        spine.velocity = TSVector.zero;
        head.velocity = TSVector.zero;
        pelvis.velocity = TSVector.zero;


        head.AddForce(TSVector.up * 300);
        spine.AddForce(TSVector.up * 300);
        pelvis.AddForce(TSVector.up * 300);

        leftLeg.AddForce(TSVector.down * 300);
        rightLeg.AddForce(TSVector.down * 300);

        leftFoot.AddForce(TSVector.down * 150);
        rightFoot.AddForce(TSVector.down * 150);

        Debug.Log(spine.angularVelocity);
        Debug.Log(head.angularVelocity);
        Debug.Log(pelvis.angularVelocity);

        JitterPhysicsTools.AlignToVector(spine, spine.tsTransform.forward, direction.normalized, 0.001f, 20.0f);
        JitterPhysicsTools.AlignToVector(head, head.tsTransform.forward, direction.normalized, 0.001f, 20.0f);
        JitterPhysicsTools.AlignToVector(pelvis, pelvis.tsTransform.forward, direction.normalized, 0.001f, 20.0f);
    }

}
