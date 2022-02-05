using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Camera ourCamera;


    //Movement
    public float cameraSpeed = 1;



    //Zoom
    public float zoomSpeed = 1;
    public float zoomLimitLow = 100;
    public float zoomLimitHigh = 200;


    private float zoomValue=0;


    private void LateUpdate()
    {


        MoveCamera();

        SetCameraZoom();
        ApplyZoomCamera();
      
        

    }

    void MoveCamera()
    {
        float xAxis = Input.GetAxis("Horizontal") * cameraSpeed;
        float zAxis = Input.GetAxis("Vertical") * cameraSpeed;


        ourCamera.transform.Translate(new Vector3(xAxis, zAxis, 0.0f));
    }

    void SetCameraZoom()
    {

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && ourCamera.orthographicSize < zoomLimitHigh)
        {

           zoomValue += zoomSpeed;

           
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && ourCamera.orthographicSize > zoomLimitLow)
        {
            zoomValue -= zoomSpeed;

           
        }
    }

    void ApplyZoomCamera()
    {

        if (zoomValue != 0)
        {
            float prevVal = ourCamera.orthographicSize;

            ourCamera.orthographicSize = Mathf.Lerp(ourCamera.orthographicSize, ourCamera.orthographicSize+zoomValue, zoomSpeed*Time.deltaTime);


            zoomValue = zoomValue - (ourCamera.orthographicSize- prevVal);

            if (zoomValue < 0.1 && zoomValue > -0.1)
            {
                zoomValue = 0;
            }
        }



    }



}
