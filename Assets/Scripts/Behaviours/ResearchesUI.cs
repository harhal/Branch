using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchesUI : OverlayWindow
{
    public static ResearchesUI UI;

    public Researches Researches;
    public ProgressBar[] Reserches;
    public ProgressBar[] Techs;

    private void Awake()
    {
        if (UI == null)
            UI = this;
        Hide();
    }

    public void OnEnable()
    {
        for (int i = 0; i < ImpactFactors.FieldsCount; i++)
        {
            Reserches[i].Progress = Researches.GetPurcentProgress(i);
            Techs[i].Progress = Researches.ResearchedTechs[i] / ImpactFactors.MaxValue;
        }
    }

    // Use this for initialization
    void Start () {
        Researches = SessionData.Data.Researches;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
