using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class AudioManagerEffects : MonoBehaviour
{


    public AudioMixer mixer;

    public AudioClip placeRoad;
    public AudioClip originDestinationSound;
    public AudioClip clickSound;
    public AudioClip errorSound;

   
    public AudioSource audioSourceEffects;


    public void PlayClickUI()
    {
        audioSourceEffects.clip = clickSound;
        audioSourceEffects.Play();
    }

    public void PlayDestinationOrigin()
    {
        audioSourceEffects.clip = originDestinationSound;
        audioSourceEffects.Play();
    }

    public void PlayRoadBuild()
    {
        
        audioSourceEffects.clip = placeRoad;
        audioSourceEffects.Play();
    }

    public void PlayError()
    {
        if (audioSourceEffects.isPlaying != true)
        {
            audioSourceEffects.clip = errorSound;
            audioSourceEffects.Play();
        }
    }

}
