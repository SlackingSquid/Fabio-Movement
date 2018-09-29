﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour {

    CharacterMovement playerMovement;
    public int maxHP = 3;
    public int currentHP;
    public float invincibilityTime = 1f;
    bool canTakeDamage = false;

    public Image hpBar;
    public Image hpBarBG;

    // Use this for initialization
    void Start () {

        playerMovement = GetComponent<CharacterMovement>();
        currentHP = maxHP;
        StartCoroutine(SetInvincibleForTime(invincibilityTime));
        hpBarBG.rectTransform.localScale = new Vector3(maxHP,1,1);
        //hpBarBG.rectTransform.localScale = new Vector3(maxHP, 1, 1);
    }
	
	// Update is called once per frame
	void Update () {

        if(currentHP >= 0)
            hpBar.rectTransform.localScale = new Vector3(currentHP, 1, 1);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Damage" && canTakeDamage)
        {
            currentHP -= 1;
            //StartCoroutine(playerMovement.ApplyKnockBack((transform.position - (playerMovement.transform.position + (Vector3.up * 0.5f))).normalized, 100f, 0.5f));
            playerMovement.ApplyForce((transform.position - other.transform.position).normalized *15f, 0f);
            GameManager.Instance.cameraShake.Shake(0.4f, 0.2f, 0.6f);
            StartCoroutine(SetInvincibleForTime(invincibilityTime));
        }
    }

    public IEnumerator SetInvincibleForTime(float time)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(time);
        canTakeDamage = true;
    }
}
