using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour {

    [Range(0, 1)]
    public float Progress;
    public Color Color0;
    public Color Color0_5;
    public Color Color1;
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
        if (Scale != null)
        {
            Scale.fillAmount = Progress;
            if (Progress <= 0.5f)
                Scale.color =  Color.Lerp(Color0, Color0_5, Progress / 0.5f);
            else
                Scale.color = Color.Lerp(Color0_5, Color1, (Progress - 0.5f) / 0.5f);
        }
        if (Purcent != null)
            Purcent.text = (Progress * 100).ToString("0") + "%";
    }
}
