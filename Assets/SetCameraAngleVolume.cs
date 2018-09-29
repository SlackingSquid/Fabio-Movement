using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraAngleVolume : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.Instance.cameraFollow.KeepCameraFacingDirection((-transform.right - (Vector3.up * 0.5f)).normalized, 200f);
        }
    }
}
