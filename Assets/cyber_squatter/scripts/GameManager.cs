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
   public CoachController coachController;
   
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

   public CoachMood lastMoodDebug;

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
      UpdateBatteryValue();
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
      
      
      if (timeLeft <= 0)
      {
         onTimeFinished?.Invoke();
         timeLeft = 0;
      }
      
      UpdateBatteryValue();
   }

   private void UpdateBatteryValue()
   {
      var timeRatio = timeLeft / maxTime;
      var batteryValue = Mathf.Clamp(timeRatio, 0f, 1f);
      batteryControl.UpdateBattery(batteryValue);
      
      var nextMood = CoachMood.Idle;
      if (timeRatio <= 0.10f)
      {
         nextMood = CoachMood.Angry;
      } else if (timeLeft == 0)
      {
         nextMood = CoachMood.Sad;
      }
      
      if(nextMood == lastMoodDebug)
      {
         return;
      }
      
      coachController.SetMood(nextMood);
      lastMoodDebug = nextMood;
   }
}
