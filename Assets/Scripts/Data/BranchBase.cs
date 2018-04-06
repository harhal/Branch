using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BranchBase
{
    [SerializeField]
    int LastID = 574;
    public Vector2Int Size;
    [SerializeField]
    PackagedBuilding[] buildings;

    [System.NonSerialized]
    public Building[,] Buildings;
    /*[System.NonSerialized]
    public Dictionary<Building, Vector2Int> BaseMap;*/
    [System.NonSerialized]
    public List<Building> BuildingsList;
    [System.NonSerialized]
    public Dictionary<int, ObjectStorage> BuildingsDB;

    public BranchBase(Vector2Int Size)
    {
        this.Size = Size;
        Buildings = new Building[Size.x, Size.y];
        BuildingsList = new List<Building>();
        //BaseMap = new Dictionary<Building, Vector2Int>();
        BuildingsDB = new Dictionary<int, ObjectStorage>();
    }    

    internal void PrepareToSave()
    {
        buildings = new PackagedBuilding[BuildingsList.Count];
        int i = 0;
        foreach (var item in BuildingsList)
        {
            item.PrepareToSave();
               buildings[i] = new PackagedBuilding(item);
            i++;
        }
    }

    internal void InitAfterLoad()
    {
        BuildingsList = new List<Building>();
        BuildingsDB = new Dictionary<int, ObjectStorage>();
        foreach (var item in buildings)
        {
            var building = item.GetUnpackedBuilding();
            building.InitAfterLoad();
            BuildingsList.Add(building);
            var storage = building as ObjectStorage;
            if (storage != null)
                BuildingsDB.Add(storage.ID, storage);
        }
        GridComponent.grid.LoadBuildings();
    }

    public int GetNewStorageID()
    {
        return LastID += (int)Random.Range(1, 20);
    }
}
