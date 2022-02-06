using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerManager : MonoBehaviour
{

    public AudioMixer mixer;



    public void SetVolumeMixerMaster()
    {

        mixer.SetFloat("VolumeMaster", gameObject.GetComponent<Slider>().value);

    }

    public void SetVolumeMixerMusic()
    {

        mixer.SetFloat("VolumeMusic", gameObject.GetComponent<Slider>().value);

    }

    public void SetVolumeMixerEffects()
    {

        mixer.SetFloat("VolumeEffects", gameObject.GetComponent<Slider>().value);

    }



}
