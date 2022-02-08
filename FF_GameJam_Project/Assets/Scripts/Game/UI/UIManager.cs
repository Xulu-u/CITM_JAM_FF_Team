using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text timerText;
    public Text countText;
    public Text gameTimerText; // The one which counts like this --> 00:00

    public GameManager manager;




    private void Update()
    {
        timerText.text = manager.timePerRound.ToString();

        countText.text = manager.completedTravel.ToString();

        
    }





}
