using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {

    public float seconds = 3f;

	// Use this for initialization
	void Start () {

        Destroy(gameObject, seconds);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
