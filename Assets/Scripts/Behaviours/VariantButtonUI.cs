using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantButtonUI : MonoBehaviour {

    public Text text;
    public Button button;

    public void Set(VariantButton variantButton)// bool IsActive, string Text, UnityEngine.Events.UnityAction OnClick)
    {
        text.text = variantButton.Text;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(variantButton.OnClick);
        button.interactable = variantButton.CheckActivity();
        /*text.text = Text;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
        button.interactable = IsActive;*/
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
