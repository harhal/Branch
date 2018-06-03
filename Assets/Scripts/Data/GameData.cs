using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {

    public static PackagedGameData Data;

    public static string DataPath = "Assets/Data/GameData.json";

    public void LoadFromJson(string path)
    {
        string json = System.IO.File.OpenText(path).ReadToEnd();
        var gameData = JsonUtility.FromJson<PackagedGameData>(json);
        Data = gameData;
    }

    public void SaveToJson(string path)
    {
        string json = JsonUtility.ToJson(Data, true);
        System.IO.File.WriteAllText(path, json);
    }
    
    void Awake ()
    {
        if (Data == null)
            LoadFromJson(DataPath);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class PackagedGameData: ISerializationCallbackReceiver
{
    public LevelsData LevelsData;
    [SerializeField]
    AnomalObject[] anomalObjects;
    public FieldOperation[] FieldOperations;
    public HumanGenerator HumanGenerator;
    
    [System.NonSerialized]
    public Dictionary<int, AnomalObject> AnomalObjects;

    public PackagedGameData()
    {
        LevelsData = new LevelsData();
        FieldOperations = new FieldOperation[0];
        anomalObjects = new AnomalObject[0];
        HumanGenerator = new HumanGenerator();
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        AnomalObjects = new Dictionary<int, AnomalObject>();
        if (anomalObjects != null)
            foreach (AnomalObject item in anomalObjects)
            {
                AnomalObjects.Add(item.ID, item);
            }
    }
}