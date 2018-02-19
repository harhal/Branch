using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDisposeComponent : MonoBehaviour
{
    private Building building;
    new Renderer renderer;
    GridComponent grid;
    ResourceStorage resources;
    [SerializeField]
    public Vector3 FlyOffset = new Vector3(0, 1, 0);

    public enum TaskType { Build, Dig }

    public TaskType CurrentTask;

    public bool IsFree()
    {
        return building == null && CurrentTask == TaskType.Build;
    }

    public void SetBuilding(Building building)
    {
        this.building = building;
        CurrentTask = TaskType.Build;
        enabled = true;
    }

    public void EnableToDig()
    {
        CurrentTask = TaskType.Dig;
        enabled = true;
    }

    private void Awake()
    {
        grid = GetComponent<GridComponent>();
        GameObject finded = GameObject.Find("ResourceStorage");
        if (finded != null)
            resources = finded.GetComponent<ResourceStorage>();
    }

    private void OnEnable()
    {
        if ((CurrentTask == TaskType.Build ? building == null : false) || grid == null)
        {
            enabled = false;
            return;
        }
        grid.SetGridVisibility(true);
        if (CurrentTask == TaskType.Build)
            renderer = building.BuildingRenderer;
    }

    private void OnDisable()
    {
        if (grid != null)
            grid.SetGridVisibility(false);
        else
            print("terraim is null");
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (building != null)
            {
                GameObject.Destroy(building.gameObject);
                building = null;
            }
            CurrentTask = TaskType.Build;
            enabled = false;
        }
    }

    public void Move(Vector2Int CellLocation)
    {
        if (CurrentTask == TaskType.Build)
        {
            if (building == null) return;
            building.transform.position = grid.GridToLocation(CellLocation) + FlyOffset;
            if (!grid.CheckPlace(building, CellLocation) && renderer != null)
                renderer.material.color = Color.red;
            else
                renderer.material.color = Color.white;
        }
        else if (CurrentTask == TaskType.Dig)
        {
            /*SoilBlock soil = grid.GetBuildingAt(CellLocation) as SoilBlock;
            if (soil == null) return;
            soil.re*/
        }
    }

    public void Build(Vector2Int CellLocation)
    {
        if (grid.CheckPlace(building, CellLocation))
        {
            if (building.Cost <= resources.Money)
            {
                resources.SpendMoney(building.Cost);
                grid.PlaceBuilding(building, CellLocation);
                building.GetComponent<BuildProcessComponent>().enabled = true;
                building = null;
                enabled = false;
            }
            else
            {
                print("Нужно больше золота"); //ToDo Make output to user
            }
        }
    }

    public void Dig(Vector2Int CellLocation)
    {
        SoilBlock soil = grid.GetBuildingAt(CellLocation) as SoilBlock;
        if (soil == null) return;
        if (soil.Cost <= resources.Money)
        {
            resources.SpendMoney(soil.Cost);
            grid.RemoveBuilding(soil);
            CurrentTask = TaskType.Build;
            building = null;
            enabled = false;
        }
        else
        {
            print("Нужно больше золота"); //ToDo Make output to user
        }
    }
}