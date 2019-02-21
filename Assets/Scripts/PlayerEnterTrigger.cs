using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEnterTrigger : MonoBehaviour
{
    public float triggerCooldown = 2f;
    public bool oneTimeUse = false;
    bool canTrigger = true;
    public UnityEvent myEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTrigger && other.tag == "Player")
        {
            myEvent.Invoke();
            if(oneTimeUse)
            {
                canTrigger = false;
            }
            else
            {
                StartCoroutine(SetTriggerCooldown());
            }
        }
    }

    IEnumerator SetTriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }
}
