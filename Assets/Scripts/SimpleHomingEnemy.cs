using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHomingEnemy : MonoBehaviour {

    public int HP = 3;
    public float speed = 8f;
    public float rotationSpeed = 10f;
    public float playerDetectionRange = 20f;
    Vector3 vectorToPlayer;
    Rigidbody RB;
    Vector3 vel;

    // Use this for initialization
    void Start () {

        RB = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < playerDetectionRange)
        {
            if (RB.useGravity)
            {
                vectorToPlayer = (new Vector3(GameManager.Instance.player.transform.position.x, transform.position.y, GameManager.Instance.player.transform.position.z) - transform.position).normalized;
                vel = new Vector3(transform.forward.x * speed, RB.velocity.y, transform.forward.z * speed);
            }
            else
            {
                vectorToPlayer = ((GameManager.Instance.player.transform.position + (Vector3.up * 0.5f)) - transform.position).normalized;
                vel = transform.forward * speed;
            }
            
            RB.velocity = vel;
            
        }

        if(vectorToPlayer != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, vectorToPlayer, Time.deltaTime * rotationSpeed);
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
