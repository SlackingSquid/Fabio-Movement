using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour {

    public bool maxHp = false;
    public GameObject pickUpEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameManager.Instance.playerHP.GainHP(maxHp);
            if (pickUpEffect != null)
            {
                Instantiate(pickUpEffect, transform.position, transform.rotation);
            }
        }
    }
}
