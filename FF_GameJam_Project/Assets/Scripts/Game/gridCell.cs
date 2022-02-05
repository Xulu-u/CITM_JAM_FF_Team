using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class gridCell : MonoBehaviour
{
    private int posX;
    private int posY;

    public GameObject objectInthisGridSpace = null;

    public bool isOcupied = false;

    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }
}
