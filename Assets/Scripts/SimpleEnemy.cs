using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour {

    private UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    public GameObject attackCone;
    public int HP = 3;
    public float playerDetectionRange = 20f;
    public GameObject deathEffect;
    public float rateOfAttack = 2f;
    public float attackRange = 3f;
    public float recoveryTime = 2f;


    bool walkToPlayer = false;
    bool canWalk = true;
    bool attakingPlayer = false;
    bool attackPlayer = true;

    // Use this for initialization
    void Start () {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

	}
	
	// Update is called once per frame
	void Update () {

        if(Vector3.Distance(transform.position,GameManager.Instance.player.transform.position) < playerDetectionRange)
        {
            if(Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < attackRange && Vector3.Dot(transform.forward,(GameManager.Instance.player.transform.position - transform.position).normalized) > 0.3f)
            {
                attakingPlayer = true;
                walkToPlayer = false;
            }
            else
            {
                attakingPlayer = false;
                walkToPlayer = true;
            }
        }

        if(attakingPlayer)
        {
            if(attackPlayer)
            {
                StartCoroutine(Attack());
                attackPlayer = false;
            }
        }

        if(walkToPlayer && canWalk)
        {
            agent.SetDestination(GameManager.Instance.player.transform.position);
            anim.SetBool("walk", true);
        }
        else
        {
            //agent.Stop();
            anim.SetBool("walk", false);
        }

        if(HP <= 0)
        {
            Destroy(gameObject);
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

    }

    IEnumerator Attack()
    {
        anim.SetTrigger("attack");
        canWalk = false;
        yield return new WaitForSeconds(rateOfAttack);
        attackPlayer = true;
        canWalk = true;
    }

    IEnumerator GotHit()
    {
        //anim.SetTrigger("attack");
        canWalk = false;
        agent.SetDestination(transform.position);
        anim.SetTrigger("gotHit");
        yield return new WaitForSeconds(recoveryTime);
        canWalk = true;
    }

    public IEnumerator ActivateAttackCone()
    {
        attackCone.SetActive(true);
        yield return new WaitForFixedUpdate();
        attackCone.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerDamage")
        {
            HP -= 1;
            StartCoroutine(GotHit());
        }
    }
}
