using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerControl : MonoBehaviour {

    public AudioMixer audioMixer;

    public void SetBGMVolume(float volume)    // 控制背景音乐音量的函数    
    {
        audioMixer.SetFloat("BGM_volume", volume);        // MusicVolume为暴露出来的Music的参数    
    }
    public void SetEffectVolume(float volume)    // 控制音效音量的函数    
    {
        audioMixer.SetFloat("effects_volume", volume);        // SoundEffectVolume为暴露出来的Effect的参数    
    }
}
