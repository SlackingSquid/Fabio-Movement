using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    public float force = 50f;
    public float friction = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.GetComponent<CharacterMovement>())
        {
            collision.gameObject.GetComponent<CharacterMovement>().ApplyForce((collision.gameObject.transform.position - transform.position).normalized * force, friction);
        }
    }
}
