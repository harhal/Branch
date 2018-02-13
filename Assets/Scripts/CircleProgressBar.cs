using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CircleProgressBar : MonoBehaviour {

    [Range(0, 1)]
    public float Progress;
    Image Scale;
    Text Purcent;

	// Use this for initialization
	void Start () {
        Scale = GetComponentInChildren<Image>();
        Purcent = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Scale.fillAmount = Progress;
        Scale.color = new Color(1 - Progress, Progress, 0);
        Purcent.text = (Progress * 100).ToString("0") + "%";
    }
}
