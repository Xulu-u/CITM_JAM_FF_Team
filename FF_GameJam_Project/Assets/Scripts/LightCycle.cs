using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCycle : MonoBehaviour
{
    LightingManager lightingManager;
    // Start is called before the first frame update
    void Start()
    {
        lightingManager = GameObject.Find("Day_NightCycle").GetComponent<LightingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightingManager.TimeOfDay >= 6 && lightingManager.TimeOfDay <= 16)
        {
            gameObject.GetComponent<Light>().intensity = 0;
        }
        else gameObject.GetComponent<Light>().intensity = 10;
    }
}
