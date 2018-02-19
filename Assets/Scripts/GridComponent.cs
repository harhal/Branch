using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridComponent : MonoBehaviour
{
    Dictionary<Building, Vector2Int> Buildings;
    [SerializeField]
    protected Vector2Int Size;
    [SerializeField]
    protected Vector2 CellSize;
    [SerializeField]
    protected Vector2 Offset;
    Building[,] OccupiedСells;

    public Vector2Int GetSize() { return Size; }

    public virtual void SetGridVisibility(bool Visibility) { }

    protected virtual bool OccupyCell(int x, int y, Building building)
    {
        if (x >= 0 && y >= 0 && x < Size.x && y < Size.y)
        {
            OccupiedСells[x, y] = building;
            return true;
        }
        else return false;
    }

    protected virtual bool FreeCell(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < Size.x && y < Size.y)
        {
            OccupiedСells[x, y] = null;
            return true;
        }
        else return false;
    }

    public Vector2Int LocationToGrid(/*Building building, */Vector3 place)
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
        return OccupiedСells[Cell.x, Cell.y];
    }

    public bool CheckPlace(Building building, Vector2Int place)
    {
        if (place.x < 0 || place.y < 0 || place.x + building.Size.x - 1 >= Size.x || place.y + building.Size.y - 1 >= Size.y)
            return false;
        for (int i = place.x; i < place.x + building.Size.x && i < Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y && j < Size.y; j++)
                if (OccupiedСells[i, j] != null) return false;
        return true;
    }

    public bool PlaceBuilding(Building building, Vector2Int place)
    {
        if (!CheckPlace(building, place))
            return false;
        Buildings.Add(building, place);
        building.transform.position = GridToLocation(place);
        for (int i = place.x; i < place.x + building.Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y; j++)
                OccupyCell(i, j, building);
        return true;
    }

    public bool RemoveBuilding(Building building)
    {
        Vector2Int place;
        if (!Buildings.TryGetValue(building, out place)) return false;
        for (int i = place.x; i < place.x + building.Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y; j++)
                FreeCell(i, j);
        Buildings.Remove(building);
        GameObject.Destroy(building.gameObject);
        return true;
    }
    
    protected void Awake ()
    {
        Buildings = new Dictionary<Building, Vector2Int>();
        OccupiedСells = new Building[Size.x, Size.y];
        for (int i = 0; i < Size.x; i++)
            for (int j = 0; j < Size.y; j++)
                FreeCell(i, j);
    }
}
