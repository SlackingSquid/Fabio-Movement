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

        anim.SetFloat("running", GameManager.Instance.player.RB.velocity.magnitude / GameManager.Instance.player.walkSpeed * 3f);

        accelSecDerLerp = Mathf.Lerp(accelSecDerLerp, accelSecDer, Time.deltaTime * 5f);
        anim.SetFloat("runTilt", accelSecDerLerp * 5f);


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
            anim.SetFloat("jumpVel", GameManager.Instance.player.RB.velocity.y);
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
}
