using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridComponent : MonoBehaviour
{
    public static GridComponent grid;

    public Dictionary<Building, BuildingGameObject> Buildings;
    [SerializeField]
    protected Vector2Int Size;
    [SerializeField]
    protected Vector2 CellSize;
    [SerializeField]
    protected Vector2 Offset;

    Renderer BG;

    public Vector2Int GetSize() { return Size; }

    public virtual void SetGridVisibility(bool Visibility) { }

    protected virtual bool OccupyCell(int x, int y, Building building)
    {
        if (x >= 0 && y >= 0 && x < Size.x && y < Size.y)
        {
            SessionData.Data.BranchBase.Buildings[x, y] = building;
            return true;
        }
        else return false;
    }

    protected virtual bool FreeCell(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Size.x && y < Size.y)
        {
            SessionData.Data.BranchBase.Buildings[x, y] = null;
            return true;
        }
        else return false;
    }

    public Vector2Int LocationToGrid(Vector3 place)
    {
        Vector2 LocalPlace = new Vector2((transform.position + place).x, (transform.position + place).y) - Offset;
        Vector2Int result = new Vector2Int((int)(LocalPlace.x / CellSize.x), (int)(LocalPlace.y / CellSize.y));
        if (LocalPlace.x < 0) result.x--;
        if (LocalPlace.y < 0) result.y--;
        return result;
    }

    public Vector3 GridToLocation(Vector2Int Cell)
    {
        Vector2 Pos2D = new Vector2(Cell.x * CellSize.x + CellSize.x / 2, Cell.y * CellSize.y + CellSize.y / 2) + Offset;
        Vector3 Pos2_5D = transform.position + new Vector3(Pos2D.x, Pos2D.y, 0);
        RaycastHit hit;
        if (Physics.Raycast(Pos2_5D, Vector3.up, out hit, float.PositiveInfinity, 1 << 8)) return hit.point;
        if (Physics.Raycast(Pos2_5D, Vector3.down, out hit, float.PositiveInfinity, 1 << 8)) return hit.point;
        return Pos2_5D;
    }

    public Building GetBuildingAt(Vector2Int Cell)
    {
        if (Cell.x < 0 || Cell.x >= Size.x || Cell.y < 0 || Cell.y >= Size.y) return null;
        return SessionData.Data.BranchBase.Buildings[Cell.x, Cell.y];
    }

    public bool CheckPlace(Building building, Vector2Int place)
    {
        if (place.x < 0 || place.y < 0 || place.x + building.Size.x - 1 >= Size.x || place.y + building.Size.y - 1 >= Size.y)
            return false;
        for (int i = place.x; i < place.x + building.Size.x && i < Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y && j < Size.y; j++)
                if (SessionData.Data.BranchBase.Buildings[i, j] != null) return false;
        return true;
    }

    public bool PlaceBuilding(Building building, Vector2Int place)
    {
        if (building == null)
            return false;
        if (!CheckPlace(building, place))
            return false;
        SessionData.Data.BranchBase.BuildingsList.Add(building);
        building.Location = place;
        ObjectStorage storage;
        if ((storage = building as ObjectStorage) != null)
            SessionData.Data.BranchBase.BuildingsDB.Add(storage.ID, storage);
        var newBuilding = Builder.builder.GetNewBuildingGameObject(building);
        if (newBuilding != null)
        {
            newBuilding.SetBuilding(building);
            Buildings.Add(building, newBuilding);
            newBuilding.transform.position = GridToLocation(place);
            newBuilding.transform.SetParent(transform, true);
        }
        for (int i = place.x; i < place.x + building.Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y; j++)
                OccupyCell(i, j, building);
        return true;
    }

    public bool ReplaceBuilding(Building baseBuilding, Building building)
    {
        Vector2Int place = baseBuilding.Location;
        //if (!SessionData.Data.BranchBase.BaseMap.TryGetValue(baseBuilding, out place)) return false;
        RemoveBuilding(baseBuilding);
        PlaceBuilding(building, place);
        return true;
    }

    public bool RemoveBuilding(Building building)
    {
        Vector2Int place = building.Location;
        //if (!SessionData.Data.BranchBase.BaseMap.TryGetValue(building, out place)) return false;
        for (int i = place.x; i < place.x + building.Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y; j++)
                FreeCell(i, j);
        SessionData.Data.BranchBase.BuildingsList.Remove(building);
        BuildingGameObject RemovingGO;
        if (Buildings.TryGetValue(building, out RemovingGO))
            GameObject.Destroy(RemovingGO.gameObject);
        Buildings.Remove(building);
        return true;
    }

    public void LoadBuildings()
    {
        Size = SessionData.Data.BranchBase.Size;
        foreach (var item in SessionData.Data.BranchBase.BuildingsList)
        {
            /*ObjectStorage storage;
            if ((storage = item as ObjectStorage) != null)
                SessionData.Data.BranchBase.BuildingsDB.Add(storage.ID, storage);*/
            var newBuilding = Builder.builder.GetNewBuildingGameObject(item);
            if (newBuilding != null)
            {
                newBuilding.SetBuilding(item);
                Buildings.Add(item, newBuilding);
                newBuilding.transform.position = GridToLocation(item.Location);
                newBuilding.transform.SetParent(transform, true);
            }
            for (int i = item.Location.x; i < item.Location.x + item.Size.x; i++)
                for (int j = item.Location.y; j < item.Location.y + item.Size.y; j++)
                    OccupyCell(i, j, item);
        }
    }

    public void HighLightFocus(Building focusedBuilding)
    {
        foreach (var item in Buildings)
            if (item.Key != focusedBuilding)
            {
                item.Value.BuildingRenderer.material.color = Color.gray;
                item.Value.BuildingRenderer.material.SetColor("_Emission", Color.black);
            }
            else
            {
                item.Value.BuildingRenderer.material.color = Color.white;
                item.Value.BuildingRenderer.material.SetColor("_Emission", Color.black);
            }
        BG.material.color = Color.gray;
        BG.material.SetColor("_Emission", Color.black);
    }

    public void HighLightFreeStorages()
    {
        ObjectStorage storage;
        foreach (var item in Buildings)
            if ((storage = item.Key as ObjectStorage) != null)
                if (storage.anomalObject == null)
                {
                    item.Value.BuildingRenderer.material.color = Color.white;
                    item.Value.BuildingRenderer.material.SetColor("_Emission", Color.green);
                }
                else
                {
                    item.Value.BuildingRenderer.material.color = Color.gray;
                    item.Value.BuildingRenderer.material.SetColor("_Emission", Color.red);
                }
            else
            {
                item.Value.BuildingRenderer.material.color = Color.gray;
                item.Value.BuildingRenderer.material.SetColor("_Emission", Color.black);
            }
        BG.material.color = Color.gray;
        BG.material.SetColor("_Emission", Color.black);
    }

    public void HighLightForBuilding()
    {
        foreach (var item in Buildings)
        {
            item.Value.BuildingRenderer.material.color = Color.gray;
            item.Value.BuildingRenderer.material.SetColor("_Emission", Color.red);
        }
        BG.material.color = Color.white;
        BG.material.SetColor("_Emission", Color.green);
    }

    public void HighLightOff()
    {
        foreach (var item in Buildings)
        {
            item.Value.BuildingRenderer.material.color = Color.white;
            item.Value.BuildingRenderer.material.SetColor("_Emission", Color.black);
        }
        BG.material.color = Color.white;
        BG.material.SetColor("_Emission", Color.black);
    }

    protected void Awake ()
    {
        Buildings = new Dictionary<Building, BuildingGameObject>();
        if (grid == null)
            grid = this;
        BG = GetComponent<Renderer>();
    }

    protected void Start()
    {
    }
}
