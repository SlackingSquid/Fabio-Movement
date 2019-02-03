using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAminationEvents : MonoBehaviour {

    public GameObject attackCone;
    public ParticleSystem slash01;
    public ParticleSystem slash02;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
	}

    public IEnumerator ActivateAttackCone(float time)
    {
        attackCone.SetActive(true);
        //yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(time);
        attackCone.SetActive(false);
    }

    public void ActivateSlashEffect01()
    {
        slash01.Play();
    }
    public void ActivateSlashEffect02()
    {
        slash02.Play();
    }
}
