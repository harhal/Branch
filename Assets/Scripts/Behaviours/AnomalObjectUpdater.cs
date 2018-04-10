using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalObjectUpdater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var item in SessionData.Data.Warehouse.AnomalObjects)
        {
            item.Value.Update();
        }
	}
}
