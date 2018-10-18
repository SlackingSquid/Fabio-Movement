using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour {

    public bool maxHp = false;
    public GameObject pickUpEffect;
    public float extraHpTimeWindow = 0.4f;
    public ParticleSystem extraBling_PS;
    bool extraHP = false;

	// Use this for initialization
	void Start () {

        LayerMask.NameToLayer("Ignore Raycast");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameManager.Instance.playerHP.GainHP(maxHp);
            if(extraHP)
                GameManager.Instance.playerHP.GainHP(maxHp);
            if (pickUpEffect != null)
            {
                Instantiate(pickUpEffect, transform.position, transform.rotation);
            }
        }
    }

    public void ExtraBling()
    {
        if (extraBling_PS != null)
        {
            extraBling_PS.Play();
        }
        StartCoroutine(extraHPtimer());
    }

    IEnumerator extraHPtimer()
    {
        extraHP = true;
        yield return new WaitForSeconds(extraHpTimeWindow);
        extraHP = false;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Destroy(gameObject);
    //        GameManager.Instance.playerHP.GainHP(maxHp);
    //        if (pickUpEffect != null)
    //        {
    //            Instantiate(pickUpEffect, transform.position, transform.rotation);
    //        }
    //    }
    //}
}
