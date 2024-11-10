using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum CoachMood
{
    Idle,
    Angry,
    Happy,
    Sad,
}

[System.Serializable]
public class MoodObject
{
    public CoachMood mood;
    public GameObject moodObject;
}

public class CoachController : MonoBehaviour
{
    public List<MoodObject> moods;
    public CoachMood currentMood = CoachMood.Idle;
    public int currentMoodIndex = 0;

    public void SetMood(CoachMood mood)
    {
        currentMood = mood;
        foreach (var moodObject in moods)
        {
            moodObject.moodObject.SetActive(moodObject.mood == mood);
        }
    }

    private void Update()
    {
        // when space is pressed, change the mood
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentMoodIndex++;
            if (currentMoodIndex >= moods.Count)
            {
                currentMoodIndex = 0;
            }
            SetMood(moods[currentMoodIndex].mood);
        }
    }
}
