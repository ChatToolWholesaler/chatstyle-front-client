using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenControl : MonoBehaviour
{
    public void SetFullScreen(bool is_fullscreen)
    {
        if (!is_fullscreen)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Resolution[] resolutions = Screen.resolutions;
            Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
            Screen.fullScreen = true;
        }
    }
}
