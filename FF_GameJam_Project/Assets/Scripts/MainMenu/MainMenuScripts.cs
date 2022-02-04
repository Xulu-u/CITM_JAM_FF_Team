using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour
{

    public void PlayButton()
    {
        SceneManager.LoadScene("MainScene"); //Play Scene
    }

    public void SettingsButton()
    {
        //Change scene to settings scene and allow some modifications
        SceneManager.LoadScene("SettingsMenu"); //Settings Scene
    }

    public void CreditsButtons()
    {
        SceneManager.LoadScene("CreditsMenu"); //Credits Scene
    }

    public void QuitButton()
    {
        //close application.
        Application.Quit();
    }
}
