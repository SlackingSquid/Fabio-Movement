using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShotMount : Mount {

    public GameObject fromPoint;
    public GameObject toPoint;
    public float toVelocity = 20f;
    public float fromVelocity = 6f;
    float vel = 0f;
    Vector3 fromVector;
    Vector3 toVector;
    Vector3 vec;
    bool going = false;
    bool playerGotOn = false;

	// Use this for initialization
	void Start () {
        RB = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update () {

        if (playerIsOn)
        {
            if(!playerGotOn)
            {
                playerGotOn = true;
                going = true;
                RB.isKinematic = false;
            }
                
        }
        else
        {
            if (going)
            {
                going = false;
            }
            playerGotOn = false;
        }

        fromVector = (fromPoint.transform.position - transform.position).normalized;
        toVector = (toPoint.transform.position - transform.position).normalized;

        if (going)
        {
            vec = toVector;
            vel = toVelocity;

            if (Vector3.Distance(transform.position, toPoint.transform.position) < 0.2f)
            {
                GameManager.Instance.player.Jump();
                GameManager.Instance.player.ApplyForce(RB.velocity, 0f);
                going = false;
            }
        }
        else
        {
            vec = fromVector;
            vel = fromVelocity;
            if (Vector3.Distance(transform.position, fromPoint.transform.position) < 0.2f)
            {
                going = true;
                if(!playerIsOn)
                    RB.isKinematic = true;
            }
        }
        RB.velocity = vec * vel;

    }

}
