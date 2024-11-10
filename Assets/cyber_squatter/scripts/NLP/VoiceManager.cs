using UnityEngine;
using Oculus.Voice;
using Meta.WitAi.Json;

public class VoiceManager : MonoBehaviour
{
    [Header("Wit Configuration")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;

    [Header("Audio")]
    [SerializeField] private AudioClip assistantSound;

    private AudioSource audioSource;
    private bool _voiceCommandReady = false;

    private void Awake()
    {
        if (appVoiceExperience == null)
        {
            return;
        }

        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);
        appVoiceExperience.VoiceEvents.OnResponse.AddListener(OnWitResponse);
        appVoiceExperience.VoiceEvents.OnError.AddListener(OnError);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        appVoiceExperience.Activate();
    }

    private void OnDestroy()
    {
        if (appVoiceExperience != null)
        {
            appVoiceExperience.VoiceEvents.OnRequestCompleted.RemoveListener(ReactivateVoice);
            appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);
            appVoiceExperience.VoiceEvents.OnResponse.RemoveListener(OnWitResponse);
            appVoiceExperience.VoiceEvents.OnError.RemoveListener(OnError);
        }
    }

    private void OnError(string error, string message)
    {
        // Handle error
    }

    private void ReactivateVoice()
    {
        _voiceCommandReady = true;
        appVoiceExperience.Activate();
    }

    private void OnFullTranscription(string transcription)
    {
        _voiceCommandReady = false;
    }

    private void OnWitResponse(WitResponseNode response)
    {
        if (response["error"] != null && !string.IsNullOrEmpty(response["error"].Value))
        {
            string errorMessage = response["error"].Value;
            return;
        }

        // Parse intents
        var intents = response["intents"];
        if (intents != null && intents.Count > 0)
        {
            string detectedIntent = intents[0]["name"].Value.ToLower();
            OnIntentDetected(new string[] { detectedIntent });
        }
    }

    private void OnIntentDetected(string[] args)
    {
        string detectedIntent = args.Length > 0 ? args[0].ToLower() : "";

        switch (detectedIntent)
        {
            case "wake_word":
                StartListeningForPrompt();
                break;
            case "play":
                HandlePlayIntent();
                break;
            case "pause":
                HandlePauseIntent();
                break;
            case "next":
                HandleNextIntent();
                break;
            case "previous":
                HandlePreviousIntent();
                break;
            case "learn":
                HandleLearnIntent();
                break;
            case "song":
                HandleMusicIntent();
                break;
            default:
                // Handle unknown intents if necessary
                break;
        }
    }

    private void StartListeningForPrompt()
    {
        // Play the assistant sound
        if (assistantSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(assistantSound);
        }

        appVoiceExperience.Activate();
    }

    private void HandlePlayIntent()
    {
        // Play video logic
    }

    private void HandlePauseIntent()
    {
        // Pause video logic
    }

    private void HandleNextIntent()
    {
        // Next video logic
    }

    private void HandlePreviousIntent()
    {
        // Previous video logic
    }

    private void HandleLearnIntent()
    {
        // Educational video logic
    }

    private void HandleMusicIntent()
    {
        // Music video logic
    }
}
