using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerScreenController : MonoBehaviour
{
    public GameObject enabledScreen;
    public GameObject disabledScreen;
    
    
    // public 
    
    public void TurnOnScreen()
    {
        enabledScreen.SetActive(true);
        disabledScreen.SetActive(false);
    }
    
    public void TurnOffScreen()
    {
        enabledScreen.SetActive(false);
        disabledScreen.SetActive(true);
    }
}
