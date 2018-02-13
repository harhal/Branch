using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilboardComponent : MonoBehaviour {

    public new Camera camera;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (camera != null)
        {
            Quaternion rotation = Quaternion.LookRotation(camera.transform.forward, Vector3.up);
            transform.rotation = rotation;
        }
    }
}
