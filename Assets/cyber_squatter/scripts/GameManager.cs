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
   public VideoPlayerScreenController videoControllah;
   
   [Header("Audiosources")]
   public AudioSource lostTvSound;
   public AudioSource lostCatSound;
   public AudioSource maxxedSound;
   public AudioSource squatUp;
   public AudioSource squatDown;
   public AudioSource squatCombo;
   public AudioSource goodMood;
   public AudioSource badMood;
   
   
   
   [Header("Config")]
   public float maxTime = 30f;
   public float timeReward = 5f;
   public float interval = 1f;
   private float _timer = 0f;
   public int comboCounter = 0;
   public int comboStreak = 5;
   
   [Header("Debug")]
   public bool onMaxxedStage = false;
   public float timeLeft = 10f;
   public UnityEvent onTimeFinished;
   public UnityEvent onTimeMaxed;
   public UnityEvent outOfTimeIdle;
   public UnityEvent onTimeUpdated;

   public CoachMood lastMoodDebug;
   public bool isPaused = true;

   private void Start()
   {
      UpdateBatteryValue();
      Invoke(nameof(InitialPlay), 1.5f);
   }

   private void InitialPlay()
   {
      isPaused = false;
      videoControllah.PlayVideo();
   }

   public void OnPauseHandler()
   {
      if (timeLeft <= 0)
      {
         videoControllah.TurnOffScreen();
         return;
      }
      
      isPaused = true;
      videoControllah.PauseVideo();
      coachController.MeowSound();
   }
   
   public void OnPlayHandler()
   {
      if (timeLeft <= 0)
      {
         videoControllah.TurnOffScreen();
         return;
      }
      
      isPaused = false;
      videoControllah.PlayVideo();
      coachController.MeowSound();
   }
   
   public void OnNextHandler()
   {
      if (timeLeft <= 0)
      {
         videoControllah.TurnOffScreen();
         return;
      }
      
      videoControllah.NextVideo();
      coachController.MeowSound();
   }
   
   public void OnPreviousHandler()
   {
      if (timeLeft <= 0)
      {
         videoControllah.TurnOffScreen();
         return;
      }
      
      videoControllah.PreviousVideo();
      coachController.MeowSound();
   }
   
   public void OnLearnHandler()
   {
      if (timeLeft <= 0)
      {
         videoControllah.TurnOffScreen();
         return;
      }
      
      videoControllah.SetTheTheme(VideoTypes.study);
      coachController.MeowSound();
   }
   
   public void OnMusicHandler()
   {
      if (timeLeft <= 0)
      {
         videoControllah.TurnOffScreen();
         return;
      }
      
      videoControllah.SetTheTheme(VideoTypes.music);
      coachController.MeowSound();
   }
   
   public void OnFunHandler()
   {
      if (timeLeft <= 0)
      {
         videoControllah.TurnOffScreen();
         return;
      }
      
      videoControllah.SetTheTheme(VideoTypes.fun);
      coachController.MeowSound();
   }
   
   public void ProvideReward()
   {
      var newTime = timeLeft + timeReward;

      if (newTime >= (maxTime * 0.96f) && !onMaxxedStage)
      {
         PlayOnShot(maxxedSound);
         onTimeMaxed?.Invoke();
         onMaxxedStage = true;
      }
      
      if(newTime > maxTime)
      {
         timeLeft = maxTime;
      }
      else
      {
         timeLeft = newTime;
      }
      
      onTimeUpdated?.Invoke();
      comboCounter++;
      ComboStreakHandler();
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

      if (isPaused)
      {
         return;
      }
      
      MaxxedOutControl();

      timeLeft -= interval;
      onTimeUpdated?.Invoke();
      
      if (timeLeft <= 0)
      {
         onTimeFinished?.Invoke();
         timeLeft = 0;
         comboCounter = 0;

         FailedAudioSequence();
      }
      
      UpdateBatteryValue();
   }

   private void MaxxedOutControl()
   {
      if (timeLeft < maxTime * 0.95f && onMaxxedStage)
      {
         onMaxxedStage = false;
      }
   }

   private void FailedAudioSequence()
   {
      PlayOnShot(lostTvSound);
      Invoke(nameof(LostCatSound), .4f);
   }

   private void LostCatSound()
   {
      PlayOnShot(lostCatSound);
   }

   private void ComboStreakHandler()
   {
      if(comboCounter % comboStreak == 0)
      {
         coachController.SetMood(CoachMood.Happy);
         
         var pitch = (90f + (comboCounter / 100f * 50f)) / 100f;
         PlayOnShot(squatCombo, pitch);
         Debug.Log($"Combo Streak! Pitch: {pitch}");
      }
   }
   
   private void UpdateBatteryValue()
   {
      var timeRatio = timeLeft / maxTime;
      var batteryValue = Mathf.Clamp(timeRatio, 0f, 1f);
      batteryControl.UpdateBattery(batteryValue);
      
      var nextMood = CoachMood.Idle;

      if (lastMoodDebug == CoachMood.Angry && timeRatio <= 0.2f)
      {
         nextMood = lastMoodDebug;
      }
      
      if (timeLeft <= 0)
      {
         nextMood = CoachMood.Sad;
      } else if (timeRatio <= 0.10f)
      {
         nextMood = CoachMood.Angry;
      }
      
      if(nextMood == lastMoodDebug)
      {
         return;
      }
      
      if(nextMood == CoachMood.Idle)
      {
         PlayOnShot(goodMood);
      }
      else if(nextMood == CoachMood.Angry)
      {
         PlayOnShot(badMood);
      }
      
      coachController.SetMood(nextMood);
      lastMoodDebug = nextMood;
   }

   public void SquatDownSound()
   {
      PlayOnShot(squatDown);
   }
   
   public void SquatUpSound()
   {
      PlayOnShot(squatUp);
   }

   private void PlayOnShot(AudioSource audioS, float pitchOrdered = 0f)
   {
      if (pitchOrdered <= 0f)
      {
         audioS.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
      }
      else
      {
         audioS.pitch = pitchOrdered;
      }

      audioS.PlayOneShot(audioS.clip);
   }
}
