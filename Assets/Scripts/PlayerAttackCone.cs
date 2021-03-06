﻿using System.Collections;
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
            GameManager.Instance.player.ApplyForce(-GameManager.Instance.player.transform.forward * 5f, 0.1f);
            GameManager.Instance.cameraShake.Shake(0.05f, 0.1f, 0.2f);
            StartCoroutine(GameManager.Instance.player.playerAnim.PauseAttackForSec());
        }
        if(other.gameObject.tag == "Enemy")
        {
            StartCoroutine(GameManager.Instance.player.playerAnim.PauseAttackForSec());
        }
    }
}
