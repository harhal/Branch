using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDisposeComponent : MonoBehaviour
{

    //public GameObject terrain;
    public Camera mainCamera;
    Building building;
    new Renderer renderer;

    // Use this for initialization
    void Start()
    {
        building = GetComponent<Building>();
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (building == null) return;
        RaycastHit hit;
        if (mainCamera == null) return;
        Debug.DrawRay(mainCamera.ScreenPointToRay(Input.mousePosition).origin, mainCamera.ScreenPointToRay(Input.mousePosition).direction * 100, Color.red, 100);
        if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, float.PositiveInfinity, 1 << 8)) return;
        TerrainGrid terrain;
        if (null == (terrain = hit.collider.gameObject.GetComponent<TerrainGrid>())) return;
        Vector2Int CellLocation;
        transform.position = terrain.GridToLocation(CellLocation = terrain.LocationToGrid(building, hit.point));
        if (!terrain.CheckPlace(building, CellLocation))
            renderer.material.color = Color.red;
        else
            renderer.material.color = Color.white;
        if (Input.GetMouseButtonUp(0))
            if (terrain.CheckPlace(building, CellLocation))
            {
                terrain.PlaceBuilding(building, CellLocation);
                //GameObject newBuilding = GameObject.Instantiate(gameObject);
                //terrain.PlaceBuilding(newBuilding.GetComponent<Building>(), CellLocation);
                Destroy(this);
            }
    }
}
