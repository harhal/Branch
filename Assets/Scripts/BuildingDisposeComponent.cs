using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDisposeComponent : MonoBehaviour
{

    //public GameObject terrain;
    public Camera mainCamera;
    private Building building;
    new Renderer renderer;
    TerrainGrid grid;
    bool Rotated;

    public void SetBuilding(Building building)
    {
        this.building = building;
        enabled = true;
    }

    // Use this for initialization
    private void Awake()
    {
        grid = GetComponent<TerrainGrid>();
    }

    private void OnEnable()
    {
        if (building == null || grid == null)
        { 
            enabled = false;
            return;
        }
        Rotated = false;
        grid.SetGridVisibility(true);
        renderer = building.GetComponent<Renderer>();
    }

    private void OnDisable()
    {
        if (grid != null)
            grid.SetGridVisibility(false);
        else
            print("terraim is null");
    }

    public void Rotate90()
    {
        building.Size = new Vector2Int(building.Size.y, building.Size.x);
        if (Rotated)
            building.transform.Rotate(0, 90, 0);
        else
            building.transform.Rotate(0, -90, 0);
        Rotated = !Rotated;
    }

    // Update is called once per frame
    void Update()
    {
        if (building == null) return;
        RaycastHit hit;
        if (mainCamera == null) { Debug.LogError("Main Camera is null"); return; }
        Debug.DrawRay(mainCamera.ScreenPointToRay(Input.mousePosition).origin, mainCamera.ScreenPointToRay(Input.mousePosition).direction * 100, Color.red, 100);
        if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, 1 << 8)) return;
        if (gameObject != hit.collider.gameObject) return;
        Vector2Int CellLocation;
        building.transform.position = grid.GridToLocation(CellLocation = grid.LocationToGrid(building, hit.point));
        if (!grid.CheckPlace(building, CellLocation))
            renderer.material.color = Color.red;
        else
            renderer.material.color = Color.white;
        if (Input.GetMouseButtonUp(1))
        {
            Rotate90();
        }
            if (Input.GetMouseButtonUp(0))
        {
            if (grid.CheckPlace(building, CellLocation))
            {
                grid.PlaceBuilding(building, CellLocation);
                building.GetComponent<BuildProcessComponent>().enabled = true;
                building = null;
                enabled = false;
            }
        }
    }
}
