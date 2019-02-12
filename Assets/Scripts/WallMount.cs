using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMount : Mount
{
    bool playerIsAlreadyOn = false;
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

            playerIsAlreadyOn = true;
        }
        else if(!playerIsOn && playerIsAlreadyOn)
        {
           GameManager.Instance.player.playerAnim.DismountEverything();
           playerIsAlreadyOn = false;
        }
        
    }
}
