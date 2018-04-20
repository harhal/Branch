using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : OverlayWindow {

    public static PauseMenu Menu;

    public Button Save;

    // Use this for initialization
    void Awake () {
        if (Menu == null)
            Menu = this;
        Hide();
        Save.interactable = SessionData.ProfileName != "";
	}

	// Update is called once per frame
	void Update () {
		
	}
}
