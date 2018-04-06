using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilboardComponent : MonoBehaviour {

    new Camera camera;

    // Use this for initialization
    void Start () {
        GameObject finded = GameObject.Find("Main Camera");
        if (finded != null)
            camera = finded.GetComponent<Camera>();

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
