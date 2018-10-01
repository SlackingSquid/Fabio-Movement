using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    public Animator anim;
    bool playRollAnim = true;

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

    }

    public void Attack()
    {
        anim.SetTrigger("attack");
    }
}
