using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Warehouse
{
    //public event Action DataChanged;
    [SerializeField]
    int[] usedAnomalObjects;
    [SerializeField]
    AnomalObject[] anomalObjects;

    [System.NonSerialized]
    public HashSet<int> UsedAnomalObjects;
    //[System.NonSerialized]
    //Dictionary<int, AnomalObject> _anomalObjects;
    public Dictionary<int, AnomalObject> AnomalObjects;
    /*{
        get
        {
            if (_anomalObjects == null)
            {
                _anomalObjects = new Dictionary<int, AnomalObject>();
                if (anomalObject != null)
                    foreach (var item in anomalObject)
                    {
                        _anomalObjects.Add(item.ID, item);
                    }
            }
            return _anomalObjects;
        }
    }*/

    public Warehouse()
    {
        UsedAnomalObjects = new HashSet<int>();
        AnomalObjects = new Dictionary<int, AnomalObject>();
    }

    public void AddAnomalObject(int ID)
    {
        AnomalObjects.Add(ID, GameData.Data.AnomalObjects[ID]);
        UsedAnomalObjects.Add(ID);
    }

    public void SpawnAnomalObject() //Debug
    {
        foreach (var item in GameData.Data.AnomalObjects.Keys)
        {
            if (!UsedAnomalObjects.Contains(item))
            {
                AddAnomalObject(item);
                Kanban.Board.NewAnomalObject("Debug object added", item);
                return;
            }
        }
    }

    internal void PrepareToSave()
    {
        anomalObjects = new AnomalObject[AnomalObjects.Count];
        int i = 0;
        foreach (var item in AnomalObjects)
        {
            anomalObjects[i] = item.Value;
            i++;
        }
    }

    internal void InitAfterLoad()
    {
        AnomalObjects = new Dictionary<int, AnomalObject>();
        if (anomalObjects != null)
            foreach (var item in anomalObjects)
            {
                AnomalObjects.Add(item.ID, item);
            }
    }
}
