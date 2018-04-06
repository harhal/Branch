using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BuildingInProcess : Building {

    [SerializeField]
    PackagedBuilding nextBuilding;
    public string Description;
    public float BuildingTime;
    public float Timer;
    public bool IsActive;

    [NonSerialized]
    public Building NextBuilding;

    public BuildingInProcess(Building NextBuilding, string Description, float BuildingTime, bool Activity = true)
    {
        this.NextBuilding = NextBuilding;
        this.Description = Description;
        this.BuildingTime = BuildingTime;
        IsActive = Activity;
    }

    void BuildingFinished()
    {
        var controller = PlayerController.MainController;
        if (NextBuilding != null)
        {
            GridComponent.grid.ReplaceBuilding(this, NextBuilding);
            if (controller.IsBuildingSelected(this) &&
                controller.CurrentMode == PlayerController.ControlMode.BuildingControl)
            {
                controller.SetBuildingControlMode(NextBuilding);
            }
            Kanban.Board.BuildingProcessIsOver("Building completed", NextBuilding);
        }
        else
        {
            Kanban.Board.BuildingProcessIsOver("Place cleared", GridComponent.grid.GridToLocation(Location));
            GridComponent.grid.RemoveBuilding(this);
            if (controller.IsBuildingSelected(this) &&
                controller.CurrentMode == PlayerController.ControlMode.BuildingControl)
                controller.SetDefaultMode();
        }
    }

	// Update is called once per frame
	public override void Update () {
        if (!IsActive) return;
        if (Timer < BuildingTime)
            Timer += Time.deltaTime;
        else
            BuildingFinished();

    }

    public float GetProgress()
    {
        return Mathf.Clamp(Timer / BuildingTime, 0f, 1f);
    }

    internal override void PrepareToSave()
    {
        nextBuilding = new PackagedBuilding(NextBuilding);
    }

    internal override void InitAfterLoad()
    {
        NextBuilding = nextBuilding.GetUnpackedBuilding();
    }
}
