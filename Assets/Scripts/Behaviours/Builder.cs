using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Going to remove
public class Builder : UICollabsiblePanel {
    public static Builder builder;

    public BuildingGameObject SoilPrototype;
    public BuildingGameObject VoidPrototype;
    public BuildingGameObject BuildingProcessPrototype;
    public BuildingGameObject[] BuildingPrototype;

    [SerializeField]
    Button ResetButton;
    [SerializeField]
    RectTransform MainList;

    public void Awake()
    {
        if (builder == null)
            builder = this;
    }

    //public void HighLight  

    public void Dig(SoilBlock Soil)
    {
        //SoilBlock soil = grid.GetBuildingAt(CellLocation) as SoilBlock;
        if (Soil == null) return;
        if (Soil.Cost <= SessionData.Data.ResourceStorage.Money)
        {
            SessionData.Data.ResourceStorage.SpendMoney(Soil.Cost);
            BuildingInProcess newProcess = new BuildingInProcess(null, "Clearing place", 5);
            GridComponent.grid.ReplaceBuilding(Soil, newProcess);
            Kanban.Board.BuildingProcessStarted("Clearing place", newProcess);
            PlayerController.MainController.SetBuildingControlMode(newProcess);
        }
        else
        {
            PlayerController.MainController.ShowMessage("You don't have enought money");
        }
    }

    public void SetEnabledResetButton(bool Enabled)
    {
        ResetButton.gameObject.SetActive(Enabled);
        if (Enabled)
            MainList.offsetMax = new Vector2( -170, -10);
        //MainList.anchoredPosition = new Vector2(20 + 150, 0);
        else
            MainList.offsetMax = new Vector2(-10, -10);
    }

    public void Build(int index)
    {
        if (index < 0 || index >= BuildingPrototype.Length) { print("Wrong prototype index");  return; }
        if (BuildingPrototype[index] == null)
        {
            print("WrongBuildingType");
            return;
        }
        var prototype = Instantiate(BuildingPrototype[index]);
        switch (index)
        {
            case 0:
                {
                    prototype.Building = new BuildingInProcess(new ObjectStorage(), "Building object storage", 10, false);
                    break;
                }
        }
        PlayerController.MainController.SetBuildMode(prototype);
    }

    public BuildingGameObject GetNewBuildingGameObject(Building building)
    {
        if (building == null)
            return Instantiate(VoidPrototype);
        Type type = building.GetType();
        if (type == typeof(SoilBlock))
            return Instantiate(SoilPrototype);
        if (type == typeof(BuildingInProcess))
            return Instantiate(BuildingProcessPrototype);
        if (type == typeof(ObjectStorage))
        {
            if (building.Size == new Vector2Int(1, 1))
                return Instantiate(BuildingPrototype[0]);
        }
        return null;
    }
}
