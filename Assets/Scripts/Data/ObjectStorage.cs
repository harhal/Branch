using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectStorage : Building {

    public int ID;
    [SerializeField]
    int level;
    public int UpgradePoints;
    [SerializeField]
    bool anomalObjectIDIsValid = false;
    [SerializeField]
    int anomalObjectID;
    public ImpactFactors Protection;
    public int WorkingScientists;
    public List<int> Scientists;
    public int WorkingOperatives;
    public List<int> Guard;

    public int Level{ get { return level; } private set { level = value; } }
    [System.NonSerialized]
    AnomalObject _anomalObject;
    public AnomalObject anomalObject
    {
        get
        {
            if (_anomalObject == null && anomalObjectIDIsValid)
                _anomalObject = SessionData.Data.Warehouse.AnomalObjects[anomalObjectID];
            return _anomalObject;
        }

        set
        {
            if (value != null)
            {
                _anomalObject = value;
                anomalObjectIDIsValid = true;
                anomalObjectID = value.ID;
            }
            else
            {
                _anomalObject = null;
                anomalObjectIDIsValid = false;
            }
        }
    }

    public int HiredScientists
    {
        get
        {
            return (int)Scientists.Count;
        }
    }

    public int HiredOperatives
    {
        get
        {
            return (int)Guard.Count;
        }
    }

    public ObjectStorage()
    {
        Scientists = new List<int>();
        Guard = new List<int>();
    }

    public void LevelUp()
    {
        Level++;
        UpgradePoints += 4;
    }/*

    public void HireScientist()
    {
        //HiredScientists++;
        WorkingScientists++;// = HiredScientists;
        var scientist = SessionData.Data.ResourceStorage.GetFreeScientist();
        SessionData.Data.ResourceStorage.Send(scientist, this);
        Scientists.Add(scientist.ID);
    }

    public void FireScientist()
    {
        //HiredScientists--;
        WorkingScientists--;// = HiredScientists;
        SessionData.Data.ResourceStorage.Return(SessionData.Data.ResourceStorage.People[Scientists[0]]);
        Scientists.Remove(Scientists[0]);
    }

    public void KillScientist()
    {
        //HiredScientists--;
        WorkingScientists--;// = HiredScientists;
        SessionData.Data.ResourceStorage.Kill(SessionData.Data.ResourceStorage.People[Scientists[0]]);
        Scientists.Remove(Scientists[0]);
    }

    public void HireGuard()
    {
        //HiredOperatives++;
        WorkingOperatives++;// = HiredOperatives;
        var guard = SessionData.Data.ResourceStorage.GetFreeOperative();
        SessionData.Data.ResourceStorage.Send(guard, this);
        Guard.Add(guard.ID);
    }

    public void FireGuard()
    {
        //HiredOperatives--;
        WorkingOperatives--;// = HiredOperatives;
        SessionData.Data.ResourceStorage.Return(SessionData.Data.ResourceStorage.People[Guard[0]]);
        Guard.Remove(Guard[0]);
    }

    public void KillGuard()
    {
        //HiredOperatives--;
        WorkingOperatives--;// = HiredOperatives;
        SessionData.Data.ResourceStorage.Kill(SessionData.Data.ResourceStorage.People[Guard[0]]);
        Guard.Remove(Guard[0]);
    }*/
    
    public override void Update () {
        if (anomalObject != null ? anomalObject.Progress < 1 : false)
        {
            foreach (var scientist in Scientists)
                if (SessionData.Data.ResourceStorage.People[scientist].Activity == Human.ActivityType.Working)
                    anomalObject.Research(GameData.Data.LevelsData.GetScientistPointsAtLevel(SessionData.Data.ResourceStorage.People[scientist].Level) * Time.deltaTime);
        }
    }
}
