using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogItem : MonoBehaviour {

    public Image image;
    public Image isNewIndicator;
    public bool isNew;
    public Button ActionButton;

    public void Set(Sprite sprite, UnityEngine.Events.UnityAction action)
    {
        isNew = true;
        image.sprite = sprite;
        ActionButton.onClick.AddListener(action);
        ActionButton.onClick.AddListener(Read);
    }

    public void Read()
    {
        isNew = false;
        EventStack.CommonStack.Refresh();
        Refresh();
    }

    public void Refresh()
    {
        if (isNew)
            isNewIndicator.color = Color.yellow;
        else
            isNewIndicator.color = Color.white;
        EventStack.CommonStack.Refresh();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
