using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolder : MonoBehaviour {

    Rigidbody RB;
    public float triggerActivationVelocity = 6f;
    public GameObject damageTriggers;
    bool activateTriggers = true;

	// Use this for initialization
	void Start () {

        RB = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update () {

        if (!activateTriggers)
        {
            damageTriggers.transform.position = transform.position + (RB.velocity.normalized * 0.1f);
        }

        if (activateTriggers && RB.velocity.magnitude >= triggerActivationVelocity)
        {
            activateTriggers = false;
            damageTriggers.SetActive(true);
        }
        else if(!activateTriggers && RB.velocity.magnitude < triggerActivationVelocity)
        {
            activateTriggers = true;
            damageTriggers.SetActive(false);
        }
        
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerDamage")
        {
            RB.velocity = ((transform.position - (GameManager.Instance.player.transform.position + new Vector3(0f,0.5f,0f))).normalized * 8f) + (Vector3.up * 2f);
        }
    }
}
