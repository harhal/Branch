using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStorage : Building {

    public AnomalObject anomalObject;
    [SerializeField]
    ImpactFactors Protection;


    public float GetPurcentDurability()
    {
        return Durability / MaxDurability * 100;
    }
	
	// Update is called once per frame
	public override void Update () {
		
	}
}
