using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public float upForce = 2f;
    public float downForce = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log(other.gameObject.tag);
            // GameManager.Instance.player.RB.AddForce(transform.up * force);
            /*if(GameManager.Instance.player.RB.velocity.y < 2f)
            {
                GameManager.Instance.player.RB.velocity += Vector3.up * force;
            }*/
            //GameManager.Instance.player.RB.velocity += Vector3.up*force;
            if (Input.GetButton("Jump"))
            {
                if(GameManager.Instance.player.RB.velocity.y < 5f)
                    GameManager.Instance.player.RB.velocity += Vector3.up * upForce;
            }
            else
            {
                if (GameManager.Instance.player.RB.velocity.y < -2f)
                    GameManager.Instance.player.RB.velocity += Vector3.up * downForce;
            }
        }
        
    }
}
