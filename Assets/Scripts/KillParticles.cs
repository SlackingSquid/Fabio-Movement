using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParticles : MonoBehaviour {

	ParticleSystem PS;

	// Use this for initialization
	void Start () {

		PS = GetComponent<ParticleSystem>();
		
	}
	
	// Update is called once per frame
	void Update () {

		if(!PS.IsAlive())
		{
			Destroy(gameObject);
		}
		
	}
}
