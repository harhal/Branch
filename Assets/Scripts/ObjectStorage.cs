using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStorage : Building {

    AnomalObject anomalObject;
    [SerializeField]
    ImpactFactors Protection;
    [SerializeField]
    float MaxDurability;
    public float Durability { get; private set; }

    public override void Select()
    {

    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
