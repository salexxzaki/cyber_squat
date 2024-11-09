using UnityEngine;
using UnityEngine.XR;

public class SquatCounter : MonoBehaviour
{
    private InputDevice headset;
    private float currentHeight;
    private float peakHeight;
    private float troughHeight;
    private bool isTrackingDown;
    private bool isTrackingUp;
    private bool squatDetected;
    public int squatCount;
    public float squatThreshold = 0.2f;
    public float amplitudeThreshold = 0.5f;

    void Start()
    {
        headset = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        isTrackingDown = false;
        isTrackingUp = false;
        squatDetected = false;
        squatCount = 0;
        peakHeight = 0f;
        troughHeight = float.MaxValue;
    }

    void Update()
    {
        if (headset.isValid)
        {
            if (headset.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 headsetPosition))
            {
                currentHeight = headsetPosition.y;

                if (currentHeight > peakHeight)
                {
                    peakHeight = currentHeight;
                    isTrackingUp = true;
                }

                if (currentHeight < troughHeight)
                {
                    troughHeight = currentHeight;
                    isTrackingDown = true;
                }

                if (isTrackingDown && isTrackingUp && (peakHeight - troughHeight >= amplitudeThreshold))
                {
                    if (!squatDetected && currentHeight >= (troughHeight + squatThreshold))
                    {
                        squatCount++;
                        squatDetected = true;
                    }
                }

                if (squatDetected && currentHeight >= peakHeight - squatThreshold)
                {
                    squatDetected = false;
                    peakHeight = currentHeight;
                    troughHeight = float.MaxValue;
                    isTrackingDown = false;
                    isTrackingUp = false;
                }
            }
        }
    }
}
