using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectStorage : Building, iTimeEventReason, ISerializationCallbackReceiver
{

    public int ID;
    [SerializeField]
    int level;
    public int UpgradePoints;
    [SerializeField]
    bool anomalObjectIDIsValid = false;
    [SerializeField]
    int anomalObjectID;
    public ImpactFactors Protection;
    //public int WorkingScientists;
    public List<int> Scientists;
    //public int WorkingOperatives;
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

    public Human GetHumanByID(int id)
    {
        return SessionData.Data.ResourceStorage.People[id];
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
            ControlLevel += GameData.Data.LevelsData.GetOperativePointsAtLevel(GetHumanByID(item).Level);
        Protection.WillFactor = Mathf.Clamp(Mathf.Floor(ControlLevel / (4 * GameData.Data.LevelsData.GetOperativePointsAtLevel(10)) * 10), 0, 10);
    }

    public void ContaintementBreach()
    {
        ChaosLevel = GameData.Data.LevelsData.MaxChaosLevel / 2;
        Liquidators.AddRange(Guard.ToArray());
        Kanban.Board.ContaintmentBreach(string.Format("Object {0} {1} out of control", anomalObject.ID, anomalObject.Name), this);
        foreach (var item in Scientists)
        {
            if (GetHumanByID(item).Activity == Human.ActivityType.Working)
                GetHumanByID(item).Activity = Human.ActivityType.Blocked;
            else
                GetHumanByID(item).Fire();
        }
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
                GetHumanByID(liquidator).Fire();
        }
        Kanban.Board.ContaintmentRestored(string.Format("Control of the Object {0} {1} restored", anomalObject.ID, anomalObject.Name), this);
        RefreshWillProtection();
        Liquidators.Clear();
        foreach (var item in Scientists)
            GetHumanByID(item).Activity = Human.ActivityType.Working;
        if (PlayerController.MainController.CurrentMode == PlayerController.ControlMode.BuildingControl &&
            PlayerController.MainController.IsBuildingSelected(this))
            ObjectStorageUI.UI.RefreshData();
    }

    public override void Update () {
        if (anomalObject == null) return;
        if (anomalObject.Progress < 1 && anomalObject.IsStable())
        {
            foreach (var scientist in Scientists)
                if (GetHumanByID(scientist).Activity == Human.ActivityType.Working)
                    anomalObject.Research(GameData.Data.LevelsData.GetScientistPointsAtLevel(GetHumanByID(scientist).Level) * Time.deltaTime);
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
                if (GetHumanByID(liquidator).Activity == Human.ActivityType.Working)
                    ChaosLevel -= GameData.Data.LevelsData.GetOperativePointsAtLevel(GetHumanByID(liquidator).Level) * Time.deltaTime;
            }
            if (ChaosLevel <= 0)
                ContaintementRestored();
        }
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        foreach (var item in Scientists)
            GetHumanByID(item).Destination = this;
        foreach (var item in Guard)
            GetHumanByID(item).Destination = this;
        foreach (var item in Liquidators)
            GetHumanByID(item).Destination = this;
    }

    public bool IsActual()
    {
        throw new NotImplementedException();
    }

    public void OpenUI()
    {
        throw new NotImplementedException();
    }

    public int GetID()
    {
        return ID;
    }
}
