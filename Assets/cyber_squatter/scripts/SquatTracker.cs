using System.Collections.Generic;
using TMPro;
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
    [Header("References")]
    public Transform referencePoint;
    public TMP_Text squatCountText;
    [Header("Config")]
    private InputDevice _headset;
    public int squatCount;
    public float directionThreshold = 0.05f;
    public float squatThreshold = 0.2f;
    [Header("Tracking")]
    public float currentHeight;
    public List<Vector3> lastPositions;
    public float averageLastHeight;
    public float lowestHeight;
    public float highestHeight;
    public float amplitudeThreshold = 0.5f;
    public float amplitudeTravelled;
    
    [Header("Debug")]
    public bool lowerAmplitudeReached;
    public bool upperAmplitudeReached;
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
        
        var posYDiff = referencePoint.position.y - pos.y;
        amplitudeTravelled = Mathf.Abs(posYDiff);
        
        AmplitudeCopletionControl();
    }

    private void AmplitudeCopletionControl()
    {
        if (amplitudeTravelled >= amplitudeThreshold)
        {
            if (squatState == SquatState.GoingDown)
            {
                lowerAmplitudeReached = true;
            }
            else if (squatState == SquatState.GoingUp)
            {
                if(!lowerAmplitudeReached) return;
                upperAmplitudeReached = true;
                
                Debug.Log("Done!");
                squatCount++;
                lowerAmplitudeReached = false;
                upperAmplitudeReached = false;
                squatCountText.text = "SQUAT COUNTER: " + squatCount;
            }
        }
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
        UpdateReferencePointPosition();
        Debug.Log("New state: " + state);
    }

    private void UpdateReferencePointPosition()
    {
        if (lastPositions.Count > 0)
        {
            referencePoint.transform.position = lastPositions[0];
        }
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
