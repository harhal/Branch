using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeEventType { Human, Bilding, Report, AnomalObject }

[System.Serializable]
public class TimeEventData : ISerializationCallbackReceiver
{
    [SerializeField]
    TimeEventType ReasonType;
    [SerializeField]
    int ID;
    public System.DateTime Time;
    public string Description;

    [System.NonSerialized]
    public iTimeEventReason AssignedObject;

    public TimeEventData(System.DateTime Time, iTimeEventReason AssignedObject, string Description)
    {
        this.Time = Time;
        this.AssignedObject = AssignedObject;
        this.Description = Description;
    }

    iTimeEventReason GetObject()
    {
        switch (ReasonType)
        {
            case (TimeEventType.Human): return SessionData.Data.ResourceStorage.People[ID];
            case (TimeEventType.Bilding): return SessionData.Data.BranchBase.BuildingsDB[ID];
            case (TimeEventType.Report): return SessionData.Data.Bureau.Archive[ID]; //Change to reports archive
            case (TimeEventType.AnomalObject): return SessionData.Data.Warehouse.AnomalObjects[ID];
        }
        return null;
    }

    public void OnBeforeSerialize()
    {
        ID = AssignedObject.GetID();
        ReasonType = AssignedObject.GetReasonType();
    }

    public void OnAfterDeserialize()
    {
        AssignedObject = GetObject();
    }
}

public interface iTimeEventReason
{
    bool IsActual();
    int GetID();
    TimeEventType GetReasonType();
    void OpenUI();
}
