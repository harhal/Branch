using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

    public static TimeController timeController;

    public Button Pause;
    public Button Play;
    public Button X2;
    public Button X5;
    public Text PauseText;

    private void Awake()
    {
        if (timeController == null)
            timeController = this;
    }

    public void SetTimeScale(float TimeScale)
    {
        Time.timeScale = TimeScale;
    }

    void Update ()
    {
        if (SessionData.Data.ResourceStorage.Reputation <= 0)
        {
            PauseText.text = "Defeat";
            Time.timeScale = 0;
        }
        PauseText.gameObject.SetActive(Time.timeScale == 0);
        if (Time.timeScale == 0)
        {
            Pause.interactable = false;
            Play.interactable = true;
            X2.interactable = true;
            X5.interactable = true;
        }
        if (Time.timeScale == 1)
        {
            Pause.interactable = true;
            Play.interactable = false;
            X2.interactable = true;
            X5.interactable = true;
        }
        if (Time.timeScale == 2)
        {
            Pause.interactable = true;
            Play.interactable = true;
            X2.interactable = false;
            X5.interactable = true;
        }
        if (Time.timeScale == 5)
        {
            Pause.interactable = true;
            Play.interactable = true;
            X2.interactable = true;
            X5.interactable = false;
        }
    }
}
