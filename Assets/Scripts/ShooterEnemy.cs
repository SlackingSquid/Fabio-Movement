using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour {

    private UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;
    public GameObject muzzlePoint;
    public GameObject projctile;
    public int HP = 3;
    public float playerDetectionRange = 20f;
    public float shootingRange = 10f;
    public float attackAnimTime = 1f;
    public float ROF = 2f;
    public GameObject deathEffect;

    bool walkToPlayer = false;
    bool canWalk = true;
    bool attakingPlayer = false;
    bool attackPlayer = true;

    // Use this for initialization
    void Start()
    {

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.stoppingDistance = shootingRange;

    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < playerDetectionRange)
        {
            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < shootingRange)// && Vector3.Dot(transform.forward, (GameManager.Instance.player.transform.position - transform.position).normalized) > 0.3f)
            {
                attakingPlayer = true;
                walkToPlayer = false;
            }
            else 
            {
                attakingPlayer = false;
                walkToPlayer = true;
            }

            if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < 4f && canWalk)
            {
                agent.SetDestination(GameManager.Instance.player.transform.position + ((transform.position - GameManager.Instance.player.transform.position).normalized * 20f));
            }
        }

        if (attakingPlayer)
        {
            if (attackPlayer && agent.velocity.magnitude < 0.5f)
            {
                Vector3 vecToPlayer = new Vector3(GameManager.Instance.player.transform.position.x, transform.position.y, GameManager.Instance.player.transform.position.z);
                if (Vector3.Dot((vecToPlayer - transform.position).normalized, transform.forward) < 0.995)
                {
                    transform.forward = Vector3.Lerp(transform.forward, (vecToPlayer - transform.position).normalized, Time.deltaTime * 10f);
                }
                else
                {
                    StartCoroutine(Attack());
                }
            }
        }

        if (walkToPlayer && canWalk)
        {
            agent.SetDestination(GameManager.Instance.player.transform.position);
            anim.SetBool("walk", true);
        }
        else
        {
            //agent.Stop();
            anim.SetBool("walk", false);
        }

        if (HP <= 0)
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
        attackPlayer = false;
        yield return new WaitForSeconds(attackAnimTime);
        canWalk = true;
        yield return new WaitForSeconds(ROF);
        attackPlayer = true;
        
    }

    IEnumerator GotHit()
    {
        //anim.SetTrigger("attack");
        canWalk = false;
        agent.SetDestination(transform.position);
        anim.SetTrigger("gotHit");
        yield return new WaitForSeconds(2f);
        canWalk = true;
    }

    public void Shoot()
    {
        if(projctile != null)
        {
            Instantiate(projctile, muzzlePoint.transform.position, muzzlePoint.transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerDamage")
        {
            HP -= 1;
            StartCoroutine(GotHit());
        }
    }
}
