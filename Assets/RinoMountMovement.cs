using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoMountMovement : Mount {
    public float runSpeed = 10f;
    public float playerControl = 2f;
    float playerControlMul = 1f;
    float pMul = 1f;
    float wallCorrection = 0f;
    bool centerCamera = true;
    public GameObject PlayerDamage;
    Vector3 vel;
	// Use this for initialization
	void Start () {

        RB = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update () {
		
        if(playerIsOn)
        {
            PlayerDamage.SetActive(true);

            if (centerCamera)
            {
                // StartCoroutine(GameManager.Instance.cameraFollow.SetCameraDirection((transform.forward - (Vector3.up*0.5f)).normalized, 200f));
                centerCamera = false;
            }
            GameManager.Instance.cameraFollow.KeepCameraFacingDirection((transform.forward - (Vector3.up * 0.5f)).normalized, 200f);


            pMul = Mathf.Lerp(pMul, playerControlMul, Time.deltaTime * 0.1f);

            if (RB.velocity.magnitude < 1f)
            {
                wallCorrection = 10f;
            }

            transform.RotateAround(transform.position, Vector3.up, ((Mathf.PerlinNoise(Time.time*5f, Time.time * 2f) - 0.5f)*5f) + (Input.GetAxis("Horizontal")*playerControl* pMul) + wallCorrection);

            RaycastHit hit;
            if(Physics.Raycast(transform.position+(Vector3.up*0.5f),transform.forward,out hit, 10f))
            {
                Vector3 nWall = new Vector3(hit.normal.x, 0f, hit.normal.z).normalized;
                Vector3 cross = Vector3.Cross(nWall, Vector3.up);
                if(RB.velocity.magnitude < 1f)
                {
                    wallCorrection = 10f;
                }
                else
                {
                    wallCorrection = Vector3.Dot((hit.point - transform.position).normalized, cross) * 10f;
                }
                playerControlMul = 0f;
            }
            else
            {
                wallCorrection = 0f;
                playerControlMul = 1f;
                vel = new Vector3(transform.forward.x * runSpeed, RB.velocity.y, transform.forward.z * runSpeed);
                RB.velocity = vel;
            }
        }
        else
        {
            if (!centerCamera)
            {
                centerCamera = true;
            }
            PlayerDamage.SetActive(false);
        }

	}
}
