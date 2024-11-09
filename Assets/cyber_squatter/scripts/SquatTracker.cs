using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum SquatState
{
    None,
    GoingDown,
    GoingUp
}

public class SquatCounter : MonoBehaviour
{
    [Header("Config")]
    private InputDevice _headset;
    public int squatCount;
    public float directionThreshold = 0.02f;
    public float squatThreshold = 0.2f;
    public float amplitudeThreshold = 0.5f;
    
    [Header("Tracking")]
    public float currentHeight;
    public List<float> lastStillValues = new List<float>();
    public List<Vector3> lastPositions;
    public float averageLastHeight;
    public float lowestHeight;
    public float highestHeight;
    public float amplitudeTravelled;
    
    [Header("Debug")]
    public bool isGoingDown;
    public bool isGoingUp;
    public SquatState squatState = SquatState.None;
    public bool isDebug = false;
    public Transform debugHeadset;
    

    void Start()
    {
        _headset = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        squatCount = 0;
    }

    void FixedUpdate()
    {
        if (isDebug && debugHeadset != null)
        {
            DetectSquat(debugHeadset.position);
        }
        else if (_headset.isValid && _headset.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 headsetPosition))
        {
            DetectSquat(headsetPosition);
        }
    }
    
    private void LastPositionsControl(Vector3 pos)
    {
        if (lastPositions.Count >= 10)
        {
            lastPositions.RemoveAt(0);
        }
        
        lastPositions.Add(pos);
        
        var totalY = 0f;
        
        for (int i = 0; i < lastPositions.Count; i++)
        {
            totalY += lastPositions[i].y;
        }
        
        averageLastHeight = totalY / lastPositions.Count;
        
        var posYDiff = lastPositions[0].y - pos.y;
        amplitudeTravelled += posYDiff;
    }
    
    private void SquatControl()
    {
        var heightDiff = averageLastHeight - currentHeight;

        if (heightDiff > directionThreshold)
        {
            UpdateSquatState(SquatState.GoingDown);

            if (currentHeight < lowestHeight)
            {
                lowestHeight = currentHeight;
                highestHeight = currentHeight + squatThreshold;
            }
            
        } else if (heightDiff < -directionThreshold)
        {
            UpdateSquatState(SquatState.GoingUp);

            if (currentHeight > highestHeight)
            {
                highestHeight = currentHeight;
                lowestHeight = currentHeight - squatThreshold;
            }
        }
    }
    
    private void UpdateSquatState(SquatState state)
    {
        if(squatState == state) return;
        squatState = state;
        ResetAmplitude();
        Debug.Log("New state: " + state);
    }

    private void ResetAmplitude()
    {
        amplitudeTravelled = 0;
    }

    private void DetectSquat(Vector3 headsetPosition)
    {
        currentHeight = headsetPosition.y;
        SquatControl();
        LastPositionsControl(headsetPosition);
    }
}
