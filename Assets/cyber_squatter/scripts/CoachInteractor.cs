using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;

public class CoachInteractor : MonoBehaviour
{
    public List<string> welcomeInteractions = new List<string>
    {
        "Well, look what the cat dragged in! Let's get squatting!"
    };

    public List<string> onTimeFinishedPhrases = new List<string>
    {
        "Are you kitten me?? Keep squatting!",
        "Less lounge, more lunge! Go!!",
        "I'm not mad, just... severely unimpressed.",
        "Every time you stop, a unicorn loses its horn. Think about that.",
        "Chop-chop! My nap schedule is counting on you!",
        "I've seen snails move faster than this!"
    };

    public List<string> onTimeMaxedPhrases = new List<string>
    {
        "You're purr-fect! Keep going!",
        "Fur real, you're doing great!",
        "Me-WOW! You're on a roll!",
        "Wow! You're fur-midable!!",
        "Squat-tastic! You're making those muscles meow!"
    };

    private TTSSpeaker ttsSpeaker;

    void Start()
    {
        ttsSpeaker = GetComponent<TTSSpeaker>();
        if (ttsSpeaker == null)
        {
            Debug.LogError("TTSSpeaker component not found on the GameObject.");
            return;
        }

        if (welcomeInteractions != null && welcomeInteractions.Count > 0)
        {
            PlayRandomPhrase(welcomeInteractions);
        }
    }

    void OnDestroy()
    {
    }

    public void HandleOnTimeFinished()
    {
        PlayRandomPhrase(onTimeFinishedPhrases);
    }

    public void HandleOnTimeMaxed()
    {
        PlayRandomPhrase(onTimeMaxedPhrases);
    }

    private void PlayRandomPhrase(List<string> phrases)
    {
        if (phrases != null && phrases.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, phrases.Count);
            string phraseToSpeak = phrases[randomIndex];
            ttsSpeaker.Speak(phraseToSpeak);
        }
    }
}
