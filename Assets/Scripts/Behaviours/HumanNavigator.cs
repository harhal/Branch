using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanNavigator : MonoBehaviour {
	
    public Vector3 GetHumanLocation(Human human)
    {
        return Vector3.zero;
    }

    public float GetWalkingDuration(Human human)
    {
        if (human.Destination != null)
            return (Vector3.right - human.Location).magnitude;
        else
            return (Vector3.zero  - human.Location).magnitude;
    }

	void Update () {
		foreach (var item in SessionData.Data.ResourceStorage.People)
        {
            if (item.Value.Activity == Human.ActivityType.Going)
                item.Value.Arrived();
        }
	}
}
