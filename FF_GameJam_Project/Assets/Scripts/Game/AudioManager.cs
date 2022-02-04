using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{


  
   public AudioMixer mixer;


    string masterVolume = "VolumeMaster";
    string effectVolume = "VolumeEffects";
    string musicVolume = "VolumeMusic";

    private void Start()
    {

        DontDestroyOnLoad(gameObject);
    }


    public void SetMute(bool state)
    {
        if (state == true)
        {
            mixer.SetFloat(masterVolume, 0);
            mixer.SetFloat(effectVolume, 0);
            mixer.SetFloat(musicVolume, 0);
        }
        else
        {
            mixer.ClearFloat(masterVolume);
            mixer.ClearFloat(effectVolume);
            mixer.ClearFloat(musicVolume);
           
        }
    }





}
