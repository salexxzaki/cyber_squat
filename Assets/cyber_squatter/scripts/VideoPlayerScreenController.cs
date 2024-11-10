using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public enum VideoTypes
{
    study,
    music,
    fun,
    none
}

[System.Serializable]
public class VideoObject
{
    public VideoTypes type;
    public VideoClip clip;
}

public class VideoPlayerScreenController : MonoBehaviour
{
    public GameObject enabledScreen;
    public GameObject disabledScreen;
    public GameObject pausedScreen;
    public GameObject controlsScreen;
    public VideoPlayer videoPlayer;
    
    public List<VideoObject> videoObjects = new List<VideoObject>();
    public int currentVideoIndex = 0;
    private double _pausedTime = 0;
    [Header("Debug")]
    public bool isTurnedOn = false;


    private void Start()
    {
        // load video but not play
        videoPlayer.clip = videoObjects[currentVideoIndex].clip;
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += (vp) => videoPlayer.Pause();
    }


    public void SetTheTheme(VideoTypes type)
    {
        for (int i = 0; i < videoObjects.Count; i++)
        {
            if (videoObjects[i].type == type)
            {
                currentVideoIndex = i;
                videoPlayer.clip = videoObjects[currentVideoIndex].clip;
                break;
            }
        }
        
        TurnOnScreen();
    }

    public void TurnOnScreen()
    {
        if(isTurnedOn) return;
        enabledScreen.SetActive(true);
        disabledScreen.SetActive(false);
        controlsScreen.SetActive(true);
        isTurnedOn = true;
        PlayVideo();
    }
    
    public void NextVideo()
    {
        _pausedTime = 0;
        currentVideoIndex++;
        StopVideo();
        if (currentVideoIndex >= videoObjects.Count)
        {
            currentVideoIndex = 0;
        }
        
        videoPlayer.clip = videoObjects[currentVideoIndex].clip;
        PlayVideo();
    }
    
    public void PreviousVideo()
    {
        _pausedTime = 0;
        currentVideoIndex--;
        StopVideo();
        if (currentVideoIndex < 0)
        {
            currentVideoIndex = videoObjects.Count - 1;
        }
        
        videoPlayer.clip = videoObjects[currentVideoIndex].clip;
        PlayVideo();
    }

    public void PlayVideo()
    {
        pausedScreen.SetActive(false);
        videoPlayer.time = _pausedTime;
        if (videoPlayer.isPrepared)
        {
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += (vp) =>
            {
                videoPlayer.Play();
            };
        }
    }
    
    private void InitialPlay()
    {
        PlayVideo();
    }

    public void PauseVideo()
    {
        pausedScreen.SetActive(true);
        _pausedTime = videoPlayer.time;
        videoPlayer.Pause();
    }
    
    public void StopVideo()
    {
        videoPlayer.Stop();
    }
    
    public void TurnOffScreen()
    {
        if(!isTurnedOn) return;
        PauseVideo();
        enabledScreen.SetActive(false);
        disabledScreen.SetActive(true);
        controlsScreen.SetActive(false);
        isTurnedOn = false;
    }
}
