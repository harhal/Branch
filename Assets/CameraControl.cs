using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float Speed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 offset = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Speed * Time.deltaTime;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        offset = Quaternion.Euler(rotation) * offset;
        transform.position += offset;
	}
}
