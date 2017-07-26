using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	public TrueSync.TSRigidBody rigid;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.J))
		{
			rigid.AddForce (new TrueSync.TSVector (0, 200, 0), ForceMode.Force);
		}
	}
}
