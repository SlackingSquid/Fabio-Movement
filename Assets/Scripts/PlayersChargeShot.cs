using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersChargeShot : MonoBehaviour {

    public GameObject muzzlePos;
    public LineRenderer LR;
    public GameObject impactExplosion;
    public ParticleSystem chargePS;
    public float chargeTime = 1f;
    float chargeCounter = 0f;
    bool canCharge = true;


	// Use this for initialization
	void Start () {
        LR.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.Instance.player.rolling)
        {
            if (Input.GetButtonDown("Fire3") && canCharge)
            {
                chargeCounter = 0f;
                LR.enabled = true;
                GameManager.Instance.player.EnterChargeShootingMode();
                chargePS.Play();
            }

            if (Input.GetButton("Fire3") && canCharge)
            {
                RaycastHit hit;
                Vector3 explosionPos = Vector3.zero;
                if (Physics.Raycast(muzzlePos.transform.position, transform.forward, out hit, 20f))
                {
                    LR.SetPosition(0, muzzlePos.transform.position);
                    LR.SetPosition(1, hit.point);
                    explosionPos = hit.point;
                }
                else
                {
                    LR.SetPosition(0, muzzlePos.transform.position);
                    LR.SetPosition(1, muzzlePos.transform.position + (transform.forward * 20f));
                    explosionPos = muzzlePos.transform.position + (transform.forward * 20f);
                }

                if (chargeCounter >= chargeTime)
                {
                    EmitExplosion(explosionPos - (transform.forward * 0.2f));
                }
                else
                {
                    chargeCounter += Time.deltaTime;
                }
            }

            if (Input.GetButtonUp("Fire3"))
            {
                canCharge = true;
                LR.enabled = false;
                GameManager.Instance.player.ExitChargeShootingMode();
                chargePS.Stop();
            }
        }

    }

    void EmitExplosion(Vector3 Pos)
    {
        if(impactExplosion != null)
            Instantiate(impactExplosion, Pos, Quaternion.identity);

        chargeCounter = 0f;
        LR.enabled = false;
        canCharge = false;
        GameManager.Instance.player.ExitChargeShootingMode();
    }
}
