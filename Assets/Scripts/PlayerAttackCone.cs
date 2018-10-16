using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCone : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Damage" && other.gameObject.tag != "SoftObject")
        {
            GameManager.Instance.player.ApplyForce(-transform.parent.forward * 6f, 0.1f);
            GameManager.Instance.cameraShake.Shake(0.1f, 0.1f, 0.2f);
        }
    }
}
