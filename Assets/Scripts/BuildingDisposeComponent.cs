using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDisposeComponent : MonoBehaviour
{

    //public GameObject terrain;
    Camera mainCamera;
    private Building building;
    new Renderer renderer;
    GridComponent grid;
    bool Rotated;
    ResourceStorage resources;
    [SerializeField]
    public Vector3 FlyOffset = new Vector3(0, 1, 0);

    public bool IsFree()
    {
        return building == null;
    }

    public void SetBuilding(Building building)
    {
        this.building = building;
        enabled = true;
    }

    // Use this for initialization
    private void Awake()
    {
        grid = GetComponent<GridComponent>();
        GameObject finded = GameObject.Find("Main Camera");
        print("binded");
        if (finded != null)
            mainCamera = finded.GetComponent<Camera>();
        finded = GameObject.Find("ResourceStorage");
        if (finded != null)
            resources = finded.GetComponent<ResourceStorage>();
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
        renderer = building.BuildingRenderer;
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
        building.transform.position = grid.GridToLocation(CellLocation = grid.LocationToGrid(building, hit.point)) + FlyOffset;
        if (!grid.CheckPlace(building, CellLocation) && renderer != null)
            renderer.material.color = Color.red;
        else
            renderer.material.color = Color.white;
        /*if (Input.GetMouseButtonUp(1))
        {
            Rotate90();
        }*/
        if (Input.GetMouseButtonUp(0))
        {
            if (grid.CheckPlace(building, CellLocation))
            {
                if (building.Cost <= resources.Money)
                {
                    resources.SpendMoney(building.Cost);
                    building.transform.position = grid.GridToLocation(CellLocation = grid.LocationToGrid(building, hit.point));
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
    }
}
