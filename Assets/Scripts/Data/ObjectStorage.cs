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
    public List<int> Liquidators;
    [SerializeField] float ChaosLevel;


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

    public int HiredGuard
    {
        get
        {
            return (int)Guard.Count;
        }
    }

    public int HiredLiquidators
    {
        get
        {
            return (int)Liquidators.Count;
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
    }

    public void RefreshWillProtection()
    {
        float ControlLevel = 0;
        foreach (var item in Guard)
            ControlLevel += GameData.Data.LevelsData.GetOperativePointsAtLevel(SessionData.Data.ResourceStorage.People[item].Level);
        Protection.WillFactor = Mathf.Clamp(Mathf.Floor(ControlLevel / (4 * GameData.Data.LevelsData.GetOperativePointsAtLevel(10)) * 10), 0, 10);
    }

    public void ContaintementBreach()
    {
        ChaosLevel = GameData.Data.LevelsData.MaxChaosLevel / 2;
        Liquidators.AddRange(Guard.ToArray());
        Kanban.Board.ContaintmentBreach(string.Format("Object {0} {1} out of control", anomalObject.ID, anomalObject.Name), this);
        if (PlayerController.MainController.CurrentMode == PlayerController.ControlMode.BuildingControl &&
            PlayerController.MainController.IsBuildingSelected(this))
            ObjectStorageUI.UI.RefreshData();
    }

    public float GetChaosLevel()
    {
        return ChaosLevel / GameData.Data.LevelsData.MaxChaosLevel;
    }

    public void ContaintementRestored()
    {
        ChaosLevel = 0;
        anomalObject.RestoreStability();
        List<int> NewGuard = new List<int>();
        foreach (var liquidator in Liquidators)
        {
            if (Guard.Contains(liquidator))
                NewGuard.Add(liquidator);
            else
                SessionData.Data.ResourceStorage.People[liquidator].Fire();
        }
        Kanban.Board.ContaintmentRestored(string.Format("Control of the Object {0} {1} restored", anomalObject.ID, anomalObject.Name), this);
        RefreshWillProtection();
        Liquidators.Clear();
        if (PlayerController.MainController.CurrentMode == PlayerController.ControlMode.BuildingControl &&
            PlayerController.MainController.IsBuildingSelected(this))
            ObjectStorageUI.UI.RefreshData();
    }

    /*

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
        if (anomalObject == null) return;
        if (anomalObject.Progress < 1 && anomalObject.IsStable())
        {
            foreach (var scientist in Scientists)
                if (SessionData.Data.ResourceStorage.People[scientist].Activity == Human.ActivityType.Working)
                    anomalObject.Research(GameData.Data.LevelsData.GetScientistPointsAtLevel(SessionData.Data.ResourceStorage.People[scientist].Level) * Time.deltaTime);
        }
        if (!anomalObject.IsStable())
        {
            if (ChaosLevel <= 0)
                ContaintementBreach();
            float UnprotectedProperties = (float)(anomalObject.Properties - anomalObject.Storage.Protection);
            if (ChaosLevel <= GameData.Data.LevelsData.MaxChaosLevel)
                ChaosLevel += (GameData.Data.LevelsData.BaseChaosLevelTick +
                    GameData.Data.LevelsData.UnprotectedPointChaosLevelTick * UnprotectedProperties) * Time.deltaTime;
            else
                ChaosLevel = GameData.Data.LevelsData.MaxChaosLevel;
            foreach (var liquidator in Liquidators)
            {
                if (SessionData.Data.ResourceStorage.People[liquidator].Activity == Human.ActivityType.Working)
                    ChaosLevel -= GameData.Data.LevelsData.GetOperativePointsAtLevel(SessionData.Data.ResourceStorage.People[liquidator].Level) * Time.deltaTime;
            }
            if (ChaosLevel <= 0)
                ContaintementRestored();
        }
    }
}
