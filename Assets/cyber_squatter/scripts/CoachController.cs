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
    public Animator animator;
}

public class CoachController : MonoBehaviour
{
    public List<MoodObject> moods;
    public CoachMood currentMood = CoachMood.Idle;
    public int currentMoodIndex = 0;
    public Animator cachedAnimator;
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int Cry = Animator.StringToHash("cry");
    
    [Header("References")]
    public AudioSource audioSource;

    private void Start()
    {
        cachedAnimator = moods.Find(moodObject => moodObject.mood == currentMood).animator;
    }

    public void SetMood(CoachMood mood)
    {
        currentMood = mood;
        foreach (var moodObject in moods)
        {
            moodObject.moodObject.SetActive(moodObject.mood == mood);
        }
        cachedAnimator = moods.Find(moodObject => moodObject.mood == mood).animator;
        
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        switch (currentMood)
        {
            case CoachMood.Idle:
                cachedAnimator.SetTrigger(Idle);
                break;
            case CoachMood.Angry:
                cachedAnimator.SetTrigger(Run);
                break;
            case CoachMood.Sad:
                cachedAnimator.SetTrigger(Cry);
                break;
        }
    }

    public void MeowSound()
    {
        // shift a pitch a little bit
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioSource.clip);
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
