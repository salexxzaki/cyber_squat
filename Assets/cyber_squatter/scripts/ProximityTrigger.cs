using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProximityTrigger : MonoBehaviour
{
    public Transform target;
    public float triggerDistance = 0.2f;
    public float resetThreshold = 0.3f;
    public float lastTriggerTime = 0;
    public float minTimeTriggerThreshold = .3f;
    public bool isTriggered = false;
   
    public UnityEvent onTriggered;

    // Update is called once per frame
    void Update()
    {
        TriggerControl();
    }

    private void TriggerControl()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        
        if (distance < triggerDistance)
        {
            if (!isTriggered && lastTriggerTime + minTimeTriggerThreshold < Time.time)
            {
                isTriggered = true; 
                lastTriggerTime = Time.time;
                onTriggered?.Invoke();
            }
        }
        else if (distance > resetThreshold)
        {
            isTriggered = false;
        }
    }
}
