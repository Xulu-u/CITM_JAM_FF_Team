using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisionnnsnsns : MonoBehaviour
{
    FactoryCounter factoryCounter;
    private void Start()
    {
        factoryCounter = gameObject.GetComponentInChildren<FactoryCounter>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Car")
        //{
        if (factoryCounter.packetCount > 0)
        {
            factoryCounter.packetCount--;
        }
        //}
    }
}
