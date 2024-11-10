using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SmoothLookAt : MonoBehaviour
{
      public Transform target; 
      public float smoothTime = 0.3f;

      private void LateUpdate()
      {
            LookAtSmooth();
      }

      private void LookAtSmooth()
      {
            Vector3 direction = transform.position - target.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, smoothTime);
      }
}
