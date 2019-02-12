using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

    public bool forceJump = true;
    public bool relativeUpBounce = false;
    public bool bump = true;
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
            if(bump)
                collision.gameObject.GetComponent<CharacterMovement>().ApplyForce((collision.gameObject.transform.position - transform.position).normalized * force, friction);
            if (forceJump)
                collision.gameObject.GetComponent<CharacterMovement>().RB.AddForce(Vector3.up * 30f, ForceMode.Impulse);
            if(relativeUpBounce)
            {
                float yVel = -collision.gameObject.GetComponent<CharacterMovement>().jumpVel*0.8f;
                if(yVel < force)
                {
                    yVel = force;
                }
                //collision.gameObject.GetComponent<CharacterMovement>().RB.AddForce(Vector3.up * yVel, ForceMode.Impulse);
                collision.gameObject.GetComponent<CharacterMovement>().RB.velocity = new Vector3(collision.gameObject.GetComponent<CharacterMovement>().RB.velocity.x, yVel, collision.gameObject.GetComponent<CharacterMovement>().RB.velocity.z);
            }
        }
    }

}
