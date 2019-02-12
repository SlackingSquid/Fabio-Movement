using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    public Animator anim;
    bool playRollAnim = true;


    float accelSecDer = 0f;

    float accelSecDerLerp;
    float prevVel = 0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if (GameManager.Instance.player.rolling)
        {
            if (playRollAnim)
            {
                playRollAnim = false;
                anim.SetBool("rolling",true);
            }
        }
        else
        {
            if (!playRollAnim)
            {
                playRollAnim = true;
                anim.SetBool("rolling", false);
            }
        }

        anim.SetBool("invincible", !GameManager.Instance.playerHP.canTakeDamage);

        anim.SetFloat("running", GameManager.Instance.player.RB.velocity.magnitude / GameManager.Instance.player.walkSpeed * 3f);

        accelSecDerLerp = Mathf.Lerp(accelSecDerLerp, accelSecDer, Time.deltaTime * 5f);
        anim.SetFloat("runTilt", accelSecDerLerp * 5f);

        anim.SetBool("sliding", GameManager.Instance.player.sliding || GameManager.Instance.player.afterSlide);

        if(GameManager.Instance.player.isGrounded)
        {
            if (!anim.GetBool("onGround"))
            {
                anim.SetBool("onGround", true);
            }
        }
        else
        {
            if (anim.GetBool("onGround"))
            {
                anim.SetBool("onGround", false);
            }
            float horMoveMag = new Vector3(GameManager.Instance.player.RB.velocity.x, 0f, GameManager.Instance.player.RB.velocity.z).magnitude / GameManager.Instance.player.walkSpeed;
            anim.SetFloat("jumpVel", (GameManager.Instance.player.RB.velocity.y*0.1f) * horMoveMag);
        }
        

    }

    private void FixedUpdate()
    {
        accelSecDer = prevVel - GameManager.Instance.player.RB.velocity.magnitude;
        prevVel = GameManager.Instance.player.RB.velocity.magnitude;
    }

    public void Attack()
    {
        anim.SetTrigger("attack");
    }

    public void MountToWall()
    {
        anim.SetBool("mWall", true);
    }

    public void DismountEverything()
    {
        anim.SetBool("mWall", false);
    }
}
