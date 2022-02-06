using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryCounter : MonoBehaviour
{
    public float packetTime;
    public float packetTimer;
    public int   packetCount = 0;

    private void Start()
    {
        packetTimer = packetTime;
    }
}
