using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTracker : MonoBehaviour
{
    [Header("References")]
    public Transform referenceObject;
    public Transform followObject;
    
    private List<Vector3> _lastPositions = new List<Vector3>();
    public Vector3 averageLastPosition;
    
    private List<Quaternion> _lastRotations = new List<Quaternion>();
    public Quaternion averageLastRotation;
    
    [Header("Config")]
    [Range(1, 50)]
    public int samples = 15;
    
    void Update()
    {
        TrackingControl();
        FollowControl();
    }

    private void FollowControl()
    {
        followObject.position = averageLastPosition;
        followObject.rotation = averageLastRotation;
    }

    private void TrackingControl()
    {
        // Update position history and average position
        _lastPositions.Add(referenceObject.position);
        if (_lastPositions.Count > samples)
        {
            _lastPositions.RemoveAt(0);
        }

        float xTemp = 0;
        float yTemp = 0;
        float zTemp = 0;

        foreach (var pos in _lastPositions)
        {
            xTemp += pos.x;
            yTemp += pos.y;
            zTemp += pos.z;
        }

        averageLastPosition = new Vector3(xTemp / _lastPositions.Count, yTemp / _lastPositions.Count, zTemp / _lastPositions.Count);
        
        // Update rotation history and average rotation
        _lastRotations.Add(referenceObject.rotation);
        if (_lastRotations.Count > samples)
        {
            _lastRotations.RemoveAt(0);
        }

        averageLastRotation = AverageQuaternion(_lastRotations);
    }

    // Helper function for averaging quaternions
    private Quaternion AverageQuaternion(List<Quaternion> quaternions)
    {
        Quaternion avgRotation = quaternions[0];
        for (int i = 1; i < quaternions.Count; i++)
        {
            avgRotation = Quaternion.Slerp(avgRotation, quaternions[i], 1.0f / (i + 1));
        }
        return avgRotation;
    }
}
