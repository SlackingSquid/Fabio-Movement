using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackableTrigger : MonoBehaviour {

    public float triggerCooldown = 2f;
    bool canTrigger = true;
    public UnityEvent myEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (canTrigger && other.tag == "PlayerDamage")
        {
            myEvent.Invoke();
            StartCoroutine(SetTriggerCooldown());
        }
    }

    IEnumerator SetTriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }
}
