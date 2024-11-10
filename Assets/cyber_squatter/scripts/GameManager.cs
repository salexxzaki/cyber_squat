using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
   [Header("Reference")]
   public BatteryVisualController batteryControl;
   
   [Header("Config")]
   public float maxTime = 30f;
   public float timeReward = 5f;
   public float interval = 1f;
   private float _timer = 0f;
   [Header("Debug")]
   public float timeLeft = 10f;

   public UnityEvent onTimeFinished;
   public UnityEvent onTimeMaxed;
   public UnityEvent onTimeUpdated;

   private void Start()
   {
      UpdateBatteryValue();
   }

   public void ProvideReward()
   {
      var newTime = timeLeft + timeReward;
      
      if(timeReward > maxTime)
      {
         onTimeMaxed?.Invoke();
         timeLeft = maxTime;
      }
      else
      {
         timeLeft = newTime;
      }
      
      onTimeUpdated?.Invoke();
   }

   private void Update()
   {
      TimeControl();
   }

   private void TimeControl()
   {
      _timer += Time.deltaTime;

      if (_timer >= interval)
      {
         _timer = 0f; // Reset the timer
         TimeTick();
      }
   }

   private void TimeTick()
   {
      if(timeLeft <= 0)
      {
         return;
      }

      timeLeft -= interval;
      onTimeUpdated?.Invoke();
      
      UpdateBatteryValue();
      
      if (timeLeft <= 0)
      {
         onTimeFinished?.Invoke();
         timeLeft = 0;
      }
   }

   private void UpdateBatteryValue()
   {
      var timeRatio = timeLeft / maxTime;
      var batteryValue = Mathf.Clamp(timeRatio, 0f, 1f);
      batteryControl.UpdateBattery(batteryValue);
   }
}
