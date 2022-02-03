using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameGrid gameGrid;

    [SerializeField] private LayerMask whatIsAGridLayer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject freeCam;
    [SerializeField] private GameObject editorCamera;

    // Start is called before the first frame update
    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();

    }

    // Update is called once per frame
    void Update()
    {
        if(editorCamera.active == true)
        {
            gridCell cellMouseIsOver = IsMouseOverAGridSpace();
            if (cellMouseIsOver != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    cellMouseIsOver.GetComponentInChildren<SpriteRenderer>().material.color = Color.black;
                }
            }
        }


        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(editorCamera.active == true)
            {
                freeCam.SetActive(true);
                editorCamera.SetActive(false);
            }
            else
            {
                freeCam.SetActive(false);
                editorCamera.SetActive(true);
            }

        }
    }

    private gridCell IsMouseOverAGridSpace()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 250f, whatIsAGridLayer))
        {
            return hitInfo.transform.GetComponent<gridCell>();
        }
        else
        {
            return null;
        }
    }
   
}
