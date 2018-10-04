using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHomingEnemy : MonoBehaviour {

    public int HP = 3;
    public float speed = 8f;
    public float rotationSpeed = 10f;
    public float playerDetectionRange = 20f;
    bool attakingOlayer = false;
    Vector3 vectorToPlayer;
    Rigidbody RB;

    // Use this for initialization
    void Start () {

        RB = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < playerDetectionRange)
        {
            //Quaternion.RotateTowards(Quaternion.LookRotation(transform.forward), Quaternion.LookRotation((transform.position - GameManager.Instance.player.transform.position).normalized),500f);
            if(RB.useGravity)
                vectorToPlayer = (new Vector3(GameManager.Instance.player.transform.position.x,transform.position.y, GameManager.Instance.player.transform.position.z) - transform.position).normalized;
            else
                vectorToPlayer = ((GameManager.Instance.player.transform.position + (Vector3.up*0.5f)) - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, vectorToPlayer, Time.deltaTime * rotationSpeed);
            RB.velocity = transform.forward * speed;
            
        }

        if (HP <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerDamage")
        {
            HP -= 1;
            Vector3 dir = -transform.forward;
            transform.forward = dir;
        }
    }
}
