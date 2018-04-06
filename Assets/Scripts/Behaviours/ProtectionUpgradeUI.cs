using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtectionUpgradeUI : MonoBehaviour {
    
    public ProgressBar BaseProtection;
    public ProgressBar UpgradedProtection;
    public ProgressBar ResearchedTech;
    public Button ButtonAdd;

    public static int MaxValue;
    public int DefaultValue { get; private set; }
    public int Value { get; private set; }
    public int Researched;
    public static int FreePoints;

    public void Add()
    {
        if (Value < Researched && FreePoints > 0)
        {
            Value++;
            FreePoints--;
            UpgradedProtection.Progress = Value / (float)MaxValue;
        }
    }

    public void SetBaseValue(int Value, int Researched)
    {
        DefaultValue = Value;
        this.Researched = Researched;
        BaseProtection.Progress = DefaultValue / (float)MaxValue;
        ResearchedTech.Progress = Researched / (float)MaxValue;
        ResetValue();
    }

    public void ResetValue()
    {
        Value = DefaultValue;
        UpgradedProtection.Progress = Value / (float)MaxValue;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
