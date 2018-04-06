using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMessageUI : OverlayWindow
{
    public static TextMessageUI UI;

    public Text Message;

    public void SetMessage(string Message)
    {
        if (this.Message != null)
            this.Message.text = Message;
    }

	void Awake ()
    {
        if (UI == null)
            UI = this;
        Hide();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
