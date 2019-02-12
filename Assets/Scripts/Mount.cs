using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : MonoBehaviour {
    [HideInInspector] public Rigidbody RB;
    public GameObject mountTrigger;
    [HideInInspector] public bool playerIsOn = false;

	// Use this for initialization
	void Start () {
        RB = GetComponent<Rigidbody>();
	}
	
	void Update () {

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.transform.parent != null)
            {
                if (other.transform.parent.gameObject.GetComponent<CharacterMovement>() != null)
                {
                    other.transform.parent.gameObject.GetComponent<CharacterMovement>().MountPlayer(this);
                }
            }
        }
        if((other.gameObject.tag == "Damage" || other.gameObject.tag == "DeathZone") && playerIsOn)
        {
            GameManager.Instance.player.Jump();
        }
    }
    public void PlayerDismounting()
    {
        StartCoroutine(DisableMountTrigger());
        playerIsOn = false;
    }
    IEnumerator DisableMountTrigger()
    {
        mountTrigger.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        mountTrigger.SetActive(true);
    }
    
}
