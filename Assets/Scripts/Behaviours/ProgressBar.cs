using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour {

    [Range(0, 1)]
    public float Progress;
    public Color Color0;
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
            Scale.color = Color.Lerp(Color0, Color1, Progress);
        }
        if (Purcent != null)
            Purcent.text = (Progress * 100).ToString("0") + "%";
    }
}
