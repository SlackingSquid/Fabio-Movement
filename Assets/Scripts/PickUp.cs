using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public GameObject pickUpEffect;
    Animator anim;
    public float animOffsetPower = 0.1f;

    // Use this for initialization
    void Start()
    {

        if (GetComponent<Animator>() != null)
        {
            anim = GetComponent<Animator>();
            anim.SetFloat("animOffset", Mathf.Sin(transform.position.x + transform.position.z * animOffsetPower)/2f+0.5f);
        }

        LayerMask.NameToLayer("Ignore Raycast");
        
        
    }       

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.GetComponentInChildren<CharacterMovement>() == true)
        {
            GameManager.Instance.cameraShake.Shake(0.1f, 0.1f, 0.5f);
            Destroy(gameObject);
            if (pickUpEffect != null)
            {
                Instantiate(pickUpEffect, transform.position, transform.rotation);
            }
        }
    }


}
