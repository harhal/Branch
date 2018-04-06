using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour {

    //GridComponent grid;
    //ResourceStorage resources;
    //[SerializeField]
    //SoilBlock soil;
    
    public Human[] Scientists;
    public Human[] Agents;

    private void Awake()
    {
        /*GameObject finded = GameObject.Find("ResourceStorage");
        if (finded != null)
            resources = finded.GetComponent<ResourceStorage>();
        finded = GameObject.Find("Base");
        if (finded != null)
            grid = finded.GetComponent<GridComponent>();*/
    }
    // Use this for initialization
    void Start ()
    {
        Initialize();
    }

    void Initialize()
    {
        /*SessionData.Data.ResourceStorage.ChangeReputation(100);
        SessionData.Data.ResourceStorage.AddMoney(100);
        foreach (Human agent in Agents)
            SessionData.Data.ResourceStorage.Agents.Hire(agent);
        foreach (Human scientist in Scientists)
            SessionData.Data.ResourceStorage.Scientists.Hire(scientist);
        Vector2Int gridSize = GridComponent.grid.GetSize();
        SessionData.Data.BranchBase.Buildings = new Building[gridSize.x, gridSize.y];
        Vector2Int loc = new Vector2Int(0, 0);
        for (loc.y = 0; loc.y < gridSize.y; loc.y++)
            for (loc.x = 0; loc.x < gridSize.x; loc.x++)
            {
                SoilBlock newBlock;
                if (loc.y > 32)
                    newBlock = new SoilBlock(10, SoilBlock.SoilType.SoftSoil);
                else if (loc.y > 12)
                    newBlock = new SoilBlock(25, SoilBlock.SoilType.DenseSoil);
                else
                    newBlock = new SoilBlock(50, SoilBlock.SoilType.Rock);
                GridComponent.grid.PlaceBuilding(newBlock, loc);
            }*/
                //grid.PlaceBuilding(GameObject.Instantiate<SoilBlock>(soil), loc);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
