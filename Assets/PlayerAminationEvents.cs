using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAminationEvents : MonoBehaviour {

    public GameObject attackCone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator ActivateAttackCone()
    {
        attackCone.SetActive(true);
        yield return new WaitForFixedUpdate();
        attackCone.SetActive(false);
    }
}
