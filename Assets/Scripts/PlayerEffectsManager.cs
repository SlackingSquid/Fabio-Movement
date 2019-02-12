using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour {

    public CharacterMovement player;
    public GameObject landingEffect;
    public ParticleSystem jumpEffect;
    bool playJumpEffect = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (player.hasJumped && playJumpEffect)
        {
            jumpEffect.Play();
            playJumpEffect = false;
        }
        if (player.leftground && player.isGrounded)
        {
            //if (player.jumpCounter < 0.1f)
            Instantiate(landingEffect, transform.position, landingEffect.transform.rotation);
            playJumpEffect = true;
        }
        

	}

}
