using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    [Tooltip("must be at least 1")]
    public int HP = 1;
    [Tooltip("optional")]
    public GameObject hitEffect;
    public GameObject deathEffect;
    public Vector3 effectsOffset = new Vector3(0, 0, 0);
    public bool canBeDamagedByEnemies = true;
    [Header("ITEM (optional)")]
    [Tooltip("Will spawn when destroyed (optional)")]
    public GameObject itemHolding;
    public Vector3 spawnOffset = new Vector3(0, 0, 0);


	// Use this for initialization
	void Start () {

        LayerMask.NameToLayer("Ignore Raycast");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerDamage")
        {
            onHit();
        }
        if (other.tag == "Damage" && canBeDamagedByEnemies)
        {
            onHit();
        }
    }

    void onHit()
    {
        HP--;
        if(HP <= 0)
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        if(hitEffect != null)
        {
            Instantiate(hitEffect, transform.position + effectsOffset, transform.rotation);
        }
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position + effectsOffset, transform.rotation);
        }
        if (itemHolding != null)
        {
            Instantiate(itemHolding, transform.position + spawnOffset, transform.rotation);
        }
        Destroy(gameObject);
    }
}
