using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Report : MonoBehaviour {
    public Sprite Photo;
    [TextArea]
    public string Description;
    public VariantButton[] Choises;
    [HideInInspector]
    public FieldOperation operation;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update () {
		
	}
}
