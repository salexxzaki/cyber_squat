using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryVisualController : MonoBehaviour
{
    public float batteryValue = 0;
    public float chasingSpeed = 0.02f;
    public float chasingValue = 0;
    public Animator batteryAnimator;
    public float fasterChasingSpeed = 0.02f;
    public float fasterChasingValue = 0;
    public Animator batteryFasterLineAnimator;
    private static readonly int Blend = Animator.StringToHash("Blend");
    
    [Header("Visuals")]
    public MeshRenderer batteryMesh;
    public Gradient batteryGradient;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChasingControl();
    }

    private void ChasingControl()
    {
        chasingValue = Mathf.Lerp(chasingValue, fasterChasingValue, chasingSpeed);
        batteryAnimator.SetFloat(Blend, chasingValue);
        // Update battery visuals
        batteryMesh.material.color = batteryGradient.Evaluate(chasingValue);
        // also change the emissive color
        batteryMesh.material.SetColor("_EmissionColor", batteryGradient.Evaluate(chasingValue));
        
        fasterChasingValue = Mathf.Lerp(fasterChasingValue, batteryValue, fasterChasingSpeed);
        batteryFasterLineAnimator.SetFloat(Blend, fasterChasingValue);
    }

    public void UpdateBattery(float newValue)
    {
        batteryValue = newValue;
    }
}
