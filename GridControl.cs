using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour {

    Camera mainCamera;
    BuildingDisposeComponent bdc;
    GridComponent grid;

    // Use this for initialization
    void Start ()
    {
        GameObject 
        bdc = GetComponent<BuildingDisposeComponent>();
        grid = GetComponent<GridComponent>();
	}

    // Update is called once per frame
    void Update ()
    {
	}
}
