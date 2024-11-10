using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   [Header("Config")]
   public float maxTime = 30f;
   public float timeReward = 5f;
   [Header("Debug")]
   public float timeLeft = 10f;


   public void ProvideReward()
   {
      var newTime = timeLeft + timeReward;
      
      if(timeReward > maxTime)
      {
         timeLeft = maxTime;
      }
      else
      {
         timeLeft = newTime;
      }
   }
}
