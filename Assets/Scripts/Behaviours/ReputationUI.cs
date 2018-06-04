using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationUI : MonoBehaviour {

    public ProgressBar Reputation;
	
	// Update is called once per frame
	void Update () {
        Reputation.Progress = SessionData.Data.ResourceStorage.Reputation / 100;
	}
}
