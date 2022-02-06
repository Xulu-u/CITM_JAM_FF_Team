using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class AudioManagerMusic : MonoBehaviour
{

   public AudioMixer mixer;


    string masterVolume = "VolumeMaster";
    string effectVolume = "VolumeEffects";
    string musicVolume = "VolumeMusic";


    public AudioClip gameplayMusic;
    public AudioClip menuMusic;


    public AudioSource audioSourceMusic;
  

    private void Start()
    {
       
        if (SceneManager.GetActiveScene().name.ToString() == "MainScene")
        {
            audioSourceMusic.clip = gameplayMusic;
            audioSourceMusic.Play();
         
        }
        else if (SceneManager.GetActiveScene().name.ToString() == "MainMenu")
        {
            audioSourceMusic.clip = menuMusic;
            audioSourceMusic.Play();

        }
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
