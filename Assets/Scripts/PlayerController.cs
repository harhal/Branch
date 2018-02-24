using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum ControlMode { BaseControl, Build, Dig, StorageControl, AnomalObjectControl, EntrepointControl, MissionControl }

    GridComponent grid;
    ResourceStorage resources;
    Camera mainCamera;
    public ControlMode CurrentMode { get; private set; }
    GameObject CurrentSelection;
    public Vector3 FlyOffset = new Vector3(0, 1, 0);

    public ObjectStorageUI objectStorageUI;
    public AnomalObjectUI anomalObjectUI;

    public void SetBuildMode(Building building)
    {
        CurrentSelection = building.gameObject;
        CurrentMode = ControlMode.Build;
    }

    void ResetMode()
    {
        print("ControlReset");
        switch (CurrentMode)
        {
            case (ControlMode.Build):
                {
                    if (CurrentSelection != null)
                        GameObject.Destroy(CurrentSelection);
                    break;
                }
            case (ControlMode.StorageControl):
                {
                    objectStorageUI.Hide();
                    anomalObjectUI.Hide();
                    break;
                }
            case (ControlMode.AnomalObjectControl):
                {
                    anomalObjectUI.Hide();
                    break;
                }
        }
        CurrentSelection = null;
    }

    public void SetDefaultMode()
    {
        ResetMode();
        CurrentMode = ControlMode.BaseControl;
    }

    public void SetDigMode()
    {
        ResetMode();
        CurrentMode = ControlMode.Dig;
    }

    public void SetStorageMode(ObjectStorage storage)
    {
        ResetMode();
        CurrentSelection = storage.gameObject;
        CurrentMode = ControlMode.StorageControl;
    }

    public void SetSAnomalObjectMode(AnomalObject anomalObject)
    {
        ResetMode();
        CurrentSelection = anomalObject.gameObject;
        CurrentMode = ControlMode.AnomalObjectControl;
    }

    private void Start()
    {
        GameObject finded = GameObject.Find("ResourceStorage");
        if (finded != null)
            resources = finded.GetComponent<ResourceStorage>();
        finded = GameObject.Find("Base");
        if (finded != null)
            grid = finded.GetComponent<GridComponent>();
        finded = GameObject.Find("Main Camera");
        if (finded != null)
            mainCamera = finded.GetComponent<Camera>();
        /*finded = GameObject.Find("StorageInfo");
        if (finded != null)
            objectStorageUI = finded.GetComponent<ObjectStorageUI>();
        finded = GameObject.Find("AnomalObjectInfo");
        if (finded != null)
            anomalObjectUI = finded.GetComponent<AnomalObjectUI>();*/
    }

    Vector2Int? GetCellLocation()
    {
        RaycastHit hit;
        if (mainCamera == null) { Debug.LogError("Main Camera is null"); return null; }
        if (grid == null) { Debug.LogError("Grid is null"); return null; }
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 100);
        if (!Physics.Raycast(ray, out hit, float.PositiveInfinity, 1 << 8)) return null;
        if (grid.gameObject != hit.collider.gameObject) return null;
        return grid.LocationToGrid(hit.point);
    }

    void Update()
    {
        Vector2Int? cellLocation = GetCellLocation();
        if (cellLocation == null) return;
        if (Input.GetAxis("Cancel") != 0)
        {
            ResetMode();
            CurrentMode = ControlMode.BaseControl;
        }
        switch (CurrentMode)
        {
            case ControlMode.Build:
                {
                    Move(cellLocation.Value);
                    if (Input.GetMouseButtonDown(0))
                        Build(cellLocation.Value);
                    break;
                }
            case ControlMode.Dig:
                {
                    Move(cellLocation.Value);
                    if (Input.GetMouseButtonDown(0))
                        Dig(cellLocation.Value);
                    break;
                }
            default:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        var storage = grid.GetBuildingAt(cellLocation.Value) as ObjectStorage;
                        if (storage != null)
                        {
                            objectStorageUI.SetObjectStorage(storage);
                            CurrentMode = ControlMode.StorageControl;
                        }
                    }
                    break;
                }
        }
    }

    void Move(Vector2Int CellLocation)
    {
        if (CurrentMode == ControlMode.Build)
        {
            if (CurrentSelection == null) return;
            var building = CurrentSelection.GetComponent<Building>();
            if (building == null) return;
            CurrentSelection.transform.position = grid.GridToLocation(CellLocation) + FlyOffset;
            if (!grid.CheckPlace(building, CellLocation) && building.BuildingRenderer != null)
                building.BuildingRenderer.material.color = Color.red;
            else
                building.BuildingRenderer.material.color = Color.white;
        }
        else if (CurrentMode == ControlMode.Dig)
        {
            SoilBlock soil = grid.GetBuildingAt(CellLocation) as SoilBlock;
            if (soil == null) return;
            soil.HighLightType = 1;
        }
    }

    void Build(Vector2Int CellLocation)
    {
        if (CurrentSelection == null) return;
        var building = CurrentSelection.GetComponent<Building>();
        if (building == null) return;
        if (grid.CheckPlace(building, CellLocation))
        {
            if (building.Cost <= resources.Money)
            {
                resources.SpendMoney(building.Cost);
                grid.PlaceBuilding(building, CellLocation);
                building.GetComponent<BuildProcessComponent>().enabled = true;
                CurrentSelection = null;
                SetDefaultMode();
            }
            else
            {
                print("Нужно больше золота"); //ToDo Make output to user
            }
        }
    }

    void Dig(Vector2Int CellLocation)
    {
        SoilBlock soil = grid.GetBuildingAt(CellLocation) as SoilBlock;
        if (soil == null) return;
        if (soil.Cost <= resources.Money)
        {
            resources.SpendMoney(soil.Cost);
            grid.RemoveBuilding(soil);
            CurrentMode = ControlMode.BaseControl;
            CurrentSelection = null;
            SetDefaultMode();
        }
        else
        {
            print("Нужно больше золота"); //ToDo Make output to user
        }
    }
}