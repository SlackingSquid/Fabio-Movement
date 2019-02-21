using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour {


    CharacterMovement playerMovement;
    public int maxHP = 3;
    public int currentHP;
    public float invincibilityTime = 1f;
    public bool canTakeDamage = false;

    public Image hpBar;
    public Image hpBarBG;

    bool playerAreadyDied = false;
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


        //for testing only!
        if(currentHP <= 0 && !playerAreadyDied)
        {
            //GameManager.Instance.ResetFromCheckPoint();
            playerAreadyDied = true;
            StartCoroutine(GameManager.Instance.ResetFromCheckPoint());
        }

        if(currentHP > 0)
        {
            playerAreadyDied = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Damage" && canTakeDamage)
        {
            TakeDamage(other);
        }
        if (other.gameObject.tag == "DeathZone" && canTakeDamage)
        {
            KillPlayer();
        }
    }

    public void TakeDamage(Collider other)
    {
        if(currentHP > 0)
            currentHP -= 1;
        playerMovement.ApplyForce((transform.position - other.transform.position).normalized * 15f, 0f);
        GameManager.Instance.cameraShake.Shake(0.4f, 0.2f, 0.6f);
        StartCoroutine(SetInvincibleForTime(invincibilityTime));
    }

    public void KillPlayer()
    {
        currentHP = 0;
    }

    public IEnumerator SetInvincibleForTime(float time)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(time);
        canTakeDamage = true;
    }

    public void GainHP(bool fullHP = false)
    {
        if(currentHP < maxHP)
        {
            if (fullHP)
                currentHP = maxHP;
            else
                currentHP++;
        }
    }
}
