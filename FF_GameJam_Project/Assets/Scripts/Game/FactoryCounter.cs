using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryCounter : MonoBehaviour
{
    public float packetTime;
    public float packetTimer;
    public int   packetCount = 0;
    public Collider collider;

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
                gameObject.transform.eulerAngles = new Vector3(90, -90, gameObject.transform.eulerAngles.z);
            }
            else
            {
                gameObject.transform.LookAt(gameObject.GetComponent<Canvas>().worldCamera.transform);
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, 180 + gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
            }
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Car")
    //    {
    //        packetCount--;
    //    }
    //}
}
