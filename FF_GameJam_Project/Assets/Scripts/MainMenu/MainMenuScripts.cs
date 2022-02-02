using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour
{

    public void PlayButton()
    {
        //Change scene and start the game.
    }

    public void SettingsButton()
    {
        //Change scene to settings scene and allow some modifications
        SceneManager.LoadScene("SettingsMenu"); //Settings Scene
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
