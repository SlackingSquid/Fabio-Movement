using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTriggerDisabler : MonoBehaviour {

    public float activeTime = 0.2f;

	void Start () {

        StartCoroutine(ActivateTrigger());
	}
	
    public IEnumerator ActivateTrigger()
    {
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);
    }
}
