using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public delegate void Action(string Discription = "");

public class SessionData : MonoBehaviour {

    public static SessionDataPackage Data;

    public static string ProfileName = "";

    internal void LoadFromJson(string path)
    {
        string json = System.IO.File.OpenText(path).ReadToEnd();
        var gameData = JsonUtility.FromJson<SessionDataPackage>(json);
        Data = gameData;
        Data.InitAfterLoad();
    }

    public void Save()
    {
        SaveToJson("Saves/" + ProfileName + "/OnlySave.json");
    }

    internal void SaveToJson(string path)
    {
        Data.PrepareToSave();
        string json = JsonUtility.ToJson(Data, true);
        System.IO.File.WriteAllText(path, json);
    }

    // Use this for initialization
    void Start () {
        if (Data == null)
            Data = new SessionDataPackage();
        if (ProfileName != "")
            LoadFromJson("Saves/" + ProfileName + "/OnlySave.json");
        else
            LoadFromJson("Assets/Data/InitSessionData.json");
        if (Data.PlayerName != ProfileName)
            Data.PlayerName = ProfileName;
        Data.Bureau.GenerateOperationStack();
    }

    private void Update()
    {
        Data.Bureau.Update();
    }

    //Test functions

    /*public void SpawnAnomalObject()
    {
        Data.Warehouse.SpawnAnomalObject();
        //Data.Warehouse.AddAnomalObject(new AnomalObject(0, "Test Object", "", 100, "Test description", new ResearchRepot[0], new ImpactFactors(0)));
    }*/
}

[System.Serializable]
public class SessionDataPackage
{
    public string PlayerName;
    public Researches Researches;
    public ResourceStorage ResourceStorage;
    public BranchBase BranchBase;
    public Warehouse Warehouse;
    public Bureau Bureau;
    
    public SessionDataPackage()
    {
        Researches = new Researches();
        ResourceStorage = new ResourceStorage();
        BranchBase = new BranchBase(new Vector2Int(24, 39));
        Warehouse = new Warehouse();
        Bureau = new Bureau();
    }

    internal void PrepareToSave()
    {
        Researches.PrepareToSave();
        ResourceStorage.PrepareToSave();
        BranchBase.PrepareToSave();
        Warehouse.PrepareToSave();
        Bureau.PrepareToSave();
    }

    internal void InitAfterLoad()
    {
        Researches.InitAfterLoad();
        ResourceStorage.InitAfterLoad();
        BranchBase.InitAfterLoad();
        Warehouse.InitAfterLoad();
        Bureau.InitAfterLoad();
    }
}
