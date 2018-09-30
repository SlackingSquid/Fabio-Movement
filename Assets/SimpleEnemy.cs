using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour {

    private UnityEngine.AI.NavMeshAgent agent;
    public Animator anim;

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

        if(Vector3.Distance(transform.position,GameManager.Instance.player.transform.position) < 30f)
        {
            if(Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) < 2f)
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

    }

    IEnumerator Attack()
    {
        anim.SetTrigger("attack");
        canWalk = false;
        yield return new WaitForSeconds(2f);
        attackPlayer = true;
        canWalk = true;
    }
}
