using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkButton : MonoBehaviour
{
    public string link = "Insert Your Link Here";

    public void OpenLinkInBrowser()
    {
        Application.OpenURL(link);
    }
}
