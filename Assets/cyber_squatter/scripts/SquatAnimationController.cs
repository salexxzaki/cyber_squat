using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.ImmersiveDebugger.UserInterface.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SquatAnimationController : MonoBehaviour
{
    public GameObject[] objectToToggle;
    public int currentIndex = 0;
    public float interval = 1f;
    private float _timer = 0f;


    private void Update()
    {
        TickToggle();
    }

    private void TickToggle()
    {
        _timer += Time.deltaTime;

        if (_timer >= interval)
        {
            _timer = 0f; // Reset the timer
            ImageToggle();
        }
    }

    private void ImageToggle()
    {
        objectToToggle[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % objectToToggle.Length;
        objectToToggle[currentIndex].SetActive(true);
    }
}
