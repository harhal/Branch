using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeLine: ISerializationCallbackReceiver
{
    [SerializeField]
    System.DateTime worldTime;
    
    [SerializeField]
    TimeEventData[] EventData;

    [System.NonSerialized]
    public Dictionary<System.DateTime, TimeEventData> EventObjects;

    public void OnBeforeSerialize()
    {
        EventData = new TimeEventData[EventObjects.Count];
        int i = 0;
        foreach (var item in EventObjects)
        {
            EventData[i] = item.Value;
            i++;
        }
    }

    public void OnAfterDeserialize()
    {
        EventObjects = new Dictionary<System.DateTime, TimeEventData>();
        foreach (var item in EventData)
        {
            EventObjects.Add(item.Time, item);
        }
    }

    public static bool isValid()
    {
        if (SessionData.Data == null) return false;
        if (SessionData.Data.TimeLine == null) return false;
        if (SessionData.Data.TimeLine.EventObjects == null) return false;
        return true;
    }

    public static System.DateTime WorldTime
    {
        get
        {
            return SessionData.Data.TimeLine.worldTime;
        }
    }

    public static void Add(iTimeEventReason Investigator, string Description)
    {
        if (!isValid()) return;
        SessionData.Data.TimeLine.EventObjects.Add(WorldTime, new TimeEventData(WorldTime, Investigator, Description));
    }
}
