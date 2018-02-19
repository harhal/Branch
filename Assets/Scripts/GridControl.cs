using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridControl : MonoBehaviour {

    Camera mainCamera;
    BuildingDisposeComponent bdc;
    GridComponent grid;

    // Use this for initialization
    void Start ()
    {
        GameObject finded = GameObject.Find("Main Camera");
        if (finded != null)
            mainCamera = finded.GetComponent<Camera>();
        bdc = GetComponent<BuildingDisposeComponent>();
        grid = GetComponent<GridComponent>();
	}

    Vector2Int? GetCellLocation()
    {
        RaycastHit hit;
        if (mainCamera == null) { Debug.LogError("Main Camera is null"); return null; }
        Debug.DrawRay(mainCamera.ScreenPointToRay(Input.mousePosition).origin, mainCamera.ScreenPointToRay(Input.mousePosition).direction * 100, Color.red, 100);
        if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, 1 << 8)) return null;
        if (gameObject != hit.collider.gameObject) return null;
        return grid.LocationToGrid(hit.point);
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2Int? cellLocation = GetCellLocation();
        if (cellLocation == null) return;
        if (bdc != null ? bdc.IsFree() : true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                /*Building CurrentBuilding = grid.GetBuildingAt(cellLocation.Value);
                if (CurrentBuilding is SoilBlock)
                {
                    grid.RemoveBuilding(CurrentBuilding);
                    (CurrentBuilding as SoilBlock).Dig();
                }*/
            }
        }
        else
        {
            if (bdc.CurrentTask == BuildingDisposeComponent.TaskType.Build)
            {
                bdc.Move(cellLocation.Value);
                if (Input.GetMouseButtonDown(0))
                    bdc.Build(cellLocation.Value);
            }
            else if(bdc.CurrentTask == BuildingDisposeComponent.TaskType.Dig)
            {
                bdc.Move(cellLocation.Value);
                if (Input.GetMouseButtonDown(0))
                    bdc.Dig(cellLocation.Value);
            }
        }
	}
}
