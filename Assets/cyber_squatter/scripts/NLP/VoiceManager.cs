using UnityEngine;
using Oculus.Voice;
using Meta.WitAi.Json;
using UnityEngine.Events;

public class VoiceManager : MonoBehaviour
{
    [Header("Wit Configuration")]
    [SerializeField]
    private AppVoiceExperience appVoiceExperience;

    [Header("Audio")]
    [SerializeField]
    private AudioClip assistantSound;
    private AudioSource audioSource;

    public UnityEvent onPlayCommand;
    public UnityEvent onPauseCommand;
    public UnityEvent onNextCommand;
    public UnityEvent onPreviousCommand;
    public UnityEvent onLearnCommand;
    public UnityEvent onMusicCommand;
    public UnityEvent onFunCommand;

    private void Awake()
    {
        if (appVoiceExperience == null)
        {
            Debug.LogError("AppVoiceExperience is not assigned.");
            return;
        }

        // Subscribe to voice events
        appVoiceExperience.VoiceEvents.OnResponse.AddListener(OnWitResponse);
        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(OnRequestCompleted);
        appVoiceExperience.VoiceEvents.OnError.AddListener(OnError);

        // Initialize audio source
        audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();

        // Start listening
        appVoiceExperience.Activate();
    }

    private void OnDestroy()
    {
        if (appVoiceExperience != null)
        {
            // Unsubscribe from voice events
            appVoiceExperience.VoiceEvents.OnResponse.RemoveListener(OnWitResponse);
            appVoiceExperience.VoiceEvents.OnRequestCompleted.RemoveListener(OnRequestCompleted);
            appVoiceExperience.VoiceEvents.OnError.RemoveListener(OnError);
        }
    }

    private void OnRequestCompleted()
    {
        // Reactivate the voice experience to continue listening
        Debug.Log("Request completed, reactivating voice experience.");
        appVoiceExperience.Activate();
    }

    private void OnError(string error, string message)
    {
        Debug.LogError($"Voice Error: {error} - {message}");
        // Reactivate the voice experience in case of an error
        appVoiceExperience.Activate();
    }

    private void OnWitResponse(WitResponseNode response)
    {
        if (response == null)
        {
            Debug.LogError("Received null response from Wit.ai.");
            return;
        }

        if (response["error"] != null && !string.IsNullOrEmpty(response["error"].Value))
        {
            string errorMessage = response["error"].Value;
            Debug.LogError($"Wit Error: {errorMessage}");
            return;
        }

        // Parse intents
        var intents = response["intents"];
        if (intents != null && intents.Count > 0)
        {
            string topIntentName = intents[0]["name"].Value.ToLower();
            float topIntentConfidence = intents[0]["confidence"].AsFloat;

            // Optional: Set a confidence threshold
            float confidenceThreshold = 0.6f; // Adjust as needed

            if (topIntentConfidence >= confidenceThreshold)
            {
                Debug.Log($"Detected intent: {topIntentName} with confidence {topIntentConfidence}");
                HandleIntent(topIntentName);
            }
            else
            {
                Debug.Log($"Intent confidence {topIntentConfidence} below threshold, ignoring.");
            }
        }
        else
        {
            Debug.Log("No intents detected, ignoring input.");
        }
    }

    private void HandleIntent(string detectedIntent)
    {
        switch (detectedIntent)
        {
            case "play":
                onPlayCommand?.Invoke();
                Debug.Log("Play command executed.");
                break;
            case "pause":
                onPauseCommand?.Invoke();
                Debug.Log("Pause command executed.");
                break;
            case "next":
                onNextCommand?.Invoke();
                Debug.Log("Next command executed.");
                break;
            case "previous":
                onPreviousCommand?.Invoke();
                Debug.Log("Previous command executed.");
                break;
            case "learn":
                onLearnCommand?.Invoke();
                Debug.Log("Learn command executed.");
                break;
            case "song":
                onMusicCommand?.Invoke();
                Debug.Log("Music command executed.");
                break;
            case "fun":
                onFunCommand?.Invoke();
                Debug.Log("Fun command executed.");
                break;
            default:
                Debug.LogWarning($"Unknown intent detected: {detectedIntent}");
                break;
        }
    }
}
