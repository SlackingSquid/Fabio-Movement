using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyInitialVelocity : MonoBehaviour {

    public Vector3 localVector = new Vector3(0f,0f,1f);
    public float velocity = 5f;
    Rigidbody RB;

	// Use this for initialization
	void Start () {

        if(GetComponent<Rigidbody>() != null)
        {
            RB = GetComponent<Rigidbody>();
            RB.velocity = transform.TransformVector(localVector) * velocity;
        }
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
