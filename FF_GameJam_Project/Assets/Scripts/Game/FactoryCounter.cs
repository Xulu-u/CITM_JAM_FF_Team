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
    private void Update()
    {
        if (gameObject.GetComponent<Canvas>().worldCamera != null)
        {
            if (gameObject.GetComponent<Canvas>().worldCamera.name == "OrthographicCamera")
            {
                //Quaternion rot = gameObject.GetComponent<RectTransform>().rotation;
                gameObject.transform.LookAt(gameObject.GetComponent<Canvas>().worldCamera.transform);
                gameObject.transform.eulerAngles = new Vector3(-270, 270, gameObject.transform.eulerAngles.z);
            }
            else
                gameObject.transform.LookAt(gameObject.GetComponent<Canvas>().worldCamera.transform);
        }
    }
}
