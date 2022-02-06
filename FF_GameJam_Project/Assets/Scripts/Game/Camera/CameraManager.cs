using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Camera ourCameraOrthographic;
    public Camera ourCameraPerspective;
    public Canvas GameUI;
    public GameObject gameManager;




    //orthographic
    [Header("Orthographic Camera")]
    //Movement
    public float cameraSpeedOrtho = 1;
    public float maxXPosOrtho = 0;
    public float maxZPosOrtho = 0;
    //Zoom
    public float zoomSpeedOrtho = 1;
    public float zoomLimitLowOrtho = 100;
    public float zoomLimitHighOrtho = 200;
    private float zoomValueOrtho = 0;


    //perspective
    [Header("Perspective Camera")]
    public float cameraSensitivity = 90;
    public float verticalSpeed = 75;
    public float MoveSpeedPers = 75;
    public float turboMultiplierSpeed = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private void Start()
    {
        ourCameraOrthographic.enabled = true;
        ourCameraPerspective.enabled = false;
        

    }

    private void LateUpdate()
    {
        if (ourCameraOrthographic.enabled == true)
        {
            MoveCameraOrth();
            SetCameraZoomOrth();
            ApplyCameraZoomOrth();
        }
        else
        {
            MoveCameraPers();
            CameraRotPers();
            
        }

        SwapCameraType();

    }

    void SwapCameraType()
    {

        if (Input.GetKeyDown(KeyCode.Q)) 
        {

            if (ourCameraOrthographic.enabled == true)
            {
                ourCameraOrthographic.enabled = false;
                ourCameraPerspective.enabled = true;
                GameUI.worldCamera = ourCameraPerspective;

                foreach (GameObject obj in gameManager.GetComponent<GameManager>().factoryCanvas)
                {
                    obj.GetComponentInChildren<Canvas>().worldCamera = ourCameraPerspective;
                }
            }
            else
            {
                ourCameraOrthographic.enabled = true;
                ourCameraPerspective.enabled = false;
                GameUI.worldCamera = ourCameraOrthographic;

                foreach (GameObject obj in gameManager.GetComponent<GameManager>().factoryCanvas)
                {
                    obj.GetComponentInChildren<Canvas>().worldCamera = ourCameraOrthographic;
                }
            }

        }

    }

    void CameraRotPers()
    {

        float yAxis = Input.GetAxis("Mouse Y");
        float xAxis = Input.GetAxis("Mouse X");



        
        rotationX += xAxis * cameraSensitivity * Time.deltaTime;
        rotationY += yAxis * cameraSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90, 90);

        ourCameraPerspective.transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
        ourCameraPerspective.transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
    }


    void MoveCameraPers()
    {


        float xAxis = Input.GetAxis("Vertical");
        float zAxis = Input.GetAxis("Horizontal");


        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
            ourCameraPerspective.transform.position += ourCameraPerspective.transform.forward * MoveSpeedPers* turboMultiplierSpeed * xAxis * Time.deltaTime;
            ourCameraPerspective.transform.position += ourCameraPerspective.transform.right * MoveSpeedPers* turboMultiplierSpeed * zAxis * Time.deltaTime;
        }
        else {
            ourCameraPerspective.transform.position += ourCameraPerspective.transform.forward * MoveSpeedPers * xAxis * Time.deltaTime;
            ourCameraPerspective.transform.position += ourCameraPerspective.transform.right * MoveSpeedPers * zAxis * Time.deltaTime; 
        }

        if (ourCameraPerspective.transform.position.y < 35)
        {
            ourCameraPerspective.transform.position = new Vector3(ourCameraPerspective.transform.position.x, 35,ourCameraPerspective.transform.position.z);
        }

       

        
        if (Input.GetKey(KeyCode.Z)) { 
            ourCameraPerspective.transform.position += ourCameraPerspective.transform.up * verticalSpeed * Time.deltaTime; 
        }
        else if (Input.GetKey(KeyCode.X)) { 
            ourCameraPerspective.transform.position -= ourCameraPerspective.transform.up * verticalSpeed * Time.deltaTime; 
        }

    }

 


    //ORTHOPEDIC
    void MoveCameraOrth()
    {
        float xAxis = Input.GetAxis("Horizontal") * cameraSpeedOrtho * Time.deltaTime;

        if (xAxis != 0)
        {
            if (ourCameraOrthographic.transform.position.z < maxXPosOrtho && xAxis > 0)
            {
                ourCameraOrthographic.transform.Translate(new Vector3(xAxis, 0.0f, 0.0f));
            }
            else if (ourCameraOrthographic.transform.position.z > -maxXPosOrtho && xAxis < 0)
            {
                ourCameraOrthographic.transform.Translate(new Vector3(xAxis, 0.0f, 0.0f));
            }
        }
       

        float zAxis = Input.GetAxis("Vertical") * cameraSpeedOrtho * Time.deltaTime;

        if (zAxis != 0)
        {

           if (ourCameraOrthographic.transform.position.x < maxZPosOrtho && zAxis < 0)
           {
               ourCameraOrthographic.transform.Translate(new Vector3(0.0f, zAxis, 0.0f));
           
           }
           else if (ourCameraOrthographic.transform.position.x > -maxZPosOrtho && zAxis > 0)
           {
               ourCameraOrthographic.transform.Translate(new Vector3(0.0f, zAxis, 0.0f));
           }
        }



    }

    void SetCameraZoomOrth()
    {

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && ourCameraOrthographic.orthographicSize < zoomLimitHighOrtho)
        {

            zoomValueOrtho += zoomSpeedOrtho;

           
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && ourCameraOrthographic.orthographicSize > zoomLimitLowOrtho)
        {
            zoomValueOrtho -= zoomSpeedOrtho;

           
        }
    }

    void ApplyCameraZoomOrth()
    {

        if (zoomValueOrtho != 0)
        {
            float prevVal = ourCameraOrthographic.orthographicSize;

            ourCameraOrthographic.orthographicSize = Mathf.Lerp(ourCameraOrthographic.orthographicSize, ourCameraOrthographic.orthographicSize+ zoomValueOrtho, zoomSpeedOrtho * Time.deltaTime);


            zoomValueOrtho = zoomValueOrtho - (ourCameraOrthographic.orthographicSize- prevVal);

            if (zoomValueOrtho < 0.1 && zoomValueOrtho > -0.1)
            {
                zoomValueOrtho = 0;
            }
        }



    }



}
