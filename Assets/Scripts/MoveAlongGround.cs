using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongGround : MonoBehaviour {

    public Vector3 directionAndSpeed;
    public float offsetFromGround = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Translate(directionAndSpeed*Time.deltaTime, Space.Self);

        RaycastHit hit;

        if(Physics.Raycast(transform.position,Vector3.down,out hit,10f))
        {
            if(hit.collider.gameObject.isStatic)
            {
                transform.position = new Vector3(transform.position.x, hit.point.y+ offsetFromGround, transform.position.z);
            }
        }
	}
}
