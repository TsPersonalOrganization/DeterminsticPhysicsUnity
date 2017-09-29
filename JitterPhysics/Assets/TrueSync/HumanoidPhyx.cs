using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;
using Dynamics;

public class HumanoidPhyx : MonoBehaviour
{
    public Rigidbody head;
    public Rigidbody spine;
    public Rigidbody pelvis;

    public Rigidbody leftArm;
    public Rigidbody rightArm;

    public Rigidbody spring;
    public Rigidbody ball;

    public Rigidbody rightLeg;
    public Rigidbody leftLeg;
    public Rigidbody rightFoot;
    public Rigidbody leftFoot;
    
	// Update is called once per frame
	void FixedUpdate ()
    {
        head.AddForce(TSVector.up.ToVector() * 400);
        spine.AddForce(TSVector.up.ToVector() * 400);
        pelvis.AddForce(TSVector.up.ToVector() * 400);

        leftLeg.AddForce(TSVector.down.ToVector() * 400);
        rightLeg.AddForce(TSVector.down.ToVector() * 400);

        leftFoot.AddForce(TSVector.down.ToVector() * 200);
        rightFoot.AddForce(TSVector.down.ToVector() * 200);

        JitterPhysicsTools.PhyxAlignToVector(spine, spine.transform.forward, pelvis.transform.forward, 0.1f, 6.0f);
        JitterPhysicsTools.PhyxAlignToVector(head, head.transform.forward, pelvis.transform.forward, 0.1f, 6.0f);
    }
}
