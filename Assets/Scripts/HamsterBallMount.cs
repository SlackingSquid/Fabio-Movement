using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterBallMount : Mount
{
    bool playerIsAlreadyOn = false;
    public float ballMaxSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsOn && !playerIsAlreadyOn)
        {
            GameManager.Instance.player.playerAnim.MountToWall();
            RB.velocity = new Vector3(GameManager.Instance.player.lVel.x,RB.velocity.y, GameManager.Instance.player.lVel.z);
            playerIsAlreadyOn = true;
        }
        else if(!playerIsOn && playerIsAlreadyOn)
        {
           GameManager.Instance.player.playerAnim.DismountEverything();
           playerIsAlreadyOn = false;
        }

        if(playerIsAlreadyOn)
        {
            if(RB.velocity.magnitude < ballMaxSpeed)
            {
                //RB.AddForce(GameManager.Instance.player.moveDir * GameManager.Instance.player.moveInputMagnitude * 10f);
                RB.AddTorque((new Vector3(GameManager.Instance.player.moveDir.z,0f, -GameManager.Instance.player.moveDir.x)) * GameManager.Instance.player.moveInputMagnitude * 10f);
            }
        }
        
    }
}
