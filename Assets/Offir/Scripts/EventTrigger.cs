using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public string playerTag = "Player";

    public UnityEvent triggeredEvent;
    public bool triggerOnce;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            triggeredEvent.Invoke();
            if(triggerOnce)
            {
                Destroy(this.gameObject);
            }
        }    
    }

}
