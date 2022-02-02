using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScripts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        //Change scene and start the game.
    }

    public void SettingsButton()
    {
        //Change scene to settings scene and allow some modifications
    }

    public void CreditsButtons()
    {
        //Change to credits scene
    }

    public void QuitButton()
    {
        //close application.
        Application.Quit();
    }
}
