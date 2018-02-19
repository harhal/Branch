using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    GridComponent grid;
    ResourceStorage resources;
    [SerializeField]
    SoilBlock soil;

    private void Awake()
    {
        GameObject finded = GameObject.Find("ResourceStorage");
        if (finded != null)
            resources = finded.GetComponent<ResourceStorage>();
        finded = GameObject.Find("Base");
        if (finded != null)
            grid = finded.GetComponent<GridComponent>();
    }
    // Use this for initialization
    void Start ()
    {
        Initialize();
    }

    void Initialize()
    {
        resources.AddMoney(100);
        Vector2Int gridSize = grid.GetSize();
        Vector2Int loc = new Vector2Int(0, 0);
        for (loc.y = 0; loc.y < gridSize.y; loc.y++)
            for (loc.x = 0; loc.x < gridSize.x; loc.x++)
                grid.PlaceBuilding(GameObject.Instantiate<SoilBlock>(soil), loc);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
