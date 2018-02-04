using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGrid : MonoBehaviour {

    Dictionary<Building, Vector2Int> Buildings;
    public Vector2Int Size;
    public float CellSize;
    public Vector2 Offset;
    bool[,] FreeTerrain;
    Texture2D ColorArray;
    Terrain terrain;

    public Vector2Int LocationToGrid(Building building, Vector3 place)
    {
        Vector2 LocalPlace = new Vector2((transform.position + place).x, (transform.position + place).z) - Offset;
        Vector2Int result = new Vector2Int((int)(LocalPlace.x / CellSize), (int)(LocalPlace.y / CellSize));
        if (LocalPlace.x < 0) result.x--;
        if (LocalPlace.y < 0) result.y--;
        return result;
    }

    public Vector3 GridToLocation(Vector2Int Cell)
    {
        Vector2 Pos2D = new Vector2(Cell.x * CellSize + CellSize / 2, Cell.y * CellSize + CellSize / 2) + Offset;
        Vector3 Pos2_5D = transform.position + new Vector3(Pos2D.x, 0, Pos2D.y);
        RaycastHit hit;
        if (Physics.Raycast(Pos2_5D, Vector3.up, out hit, float.PositiveInfinity, 1 << 8)) return hit.point;
        if (Physics.Raycast(Pos2_5D, Vector3.down, out hit, float.PositiveInfinity, 1 << 8)) return hit.point;
        return Pos2_5D;
    }

    public bool CheckPlace(Building building, Vector2Int place)
    {
        if (place.x < 0 || place.y < 0 || place.x + building.Size.x - 1 >= Size.x || place.y + building.Size.y - 1 >= Size.y)
            return false;
        for (int i = place.x; i < place.x + building.Size.x && i < Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y && j < Size.y; j++)
                if (!FreeTerrain[i, j]) return false;
        return true;
    }

    public bool PlaceBuilding(Building building, Vector2Int place)
    {
        if (!CheckPlace(building, place))
            return false;
        Buildings.Add(building, place);
        for (int i = place.x; i < place.x + building.Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y; j++)
            {
                FreeTerrain[i, j] = false;
                ColorArray.SetPixel(i, j, Color.red);
            }
        ColorArray.Apply();
        return true;
    }

        public bool RemoveBuilding(Building building)
    {
        Vector2Int place;
        if (!Buildings.TryGetValue(building, out place)) return false;
        for (int i = place.x; i < place.x + building.Size.x; i++)
            for (int j = place.y; j < place.y + building.Size.y; j++)
            {
                FreeTerrain[i, j] = true;
                ColorArray.SetPixel(i, j, Color.green);
            }
        Buildings.Remove(building);
        ColorArray.Apply();
        return true;
    }

    public void SetGridVisibility(bool Visibility)
    {
        if (terrain == null) return;
        terrain.materialTemplate.SetFloat("_DrawGrid", Visibility ? 1 : 0);
    }

    // Use this for initialization
    void Start () {
        Buildings = new Dictionary<Building, Vector2Int>();
        terrain = GetComponent<Terrain>();
        FreeTerrain = new bool[Size.x, Size.y];
        ColorArray = new Texture2D(FreeTerrain.GetLength(0), FreeTerrain.GetLength(1));
        for (int i = 0; i < Size.x; i++)
            for (int j = 0; j < Size.y; j++)
            {
                FreeTerrain[i, j] = true;
                ColorArray.SetPixel(i, j, Color.green);
            }
        ColorArray.Apply();
        ColorArray.filterMode = FilterMode.Point;
        if (terrain == null) return;
        terrain.materialTemplate.SetTexture("_GridColor", ColorArray);
        terrain.materialTemplate.SetVector("_GridSize", new Vector4(Size.x, Size.y, 0, 0));
        terrain.materialTemplate.SetFloat("_GridSpacing", CellSize);
        terrain.materialTemplate.SetVector("_GridOffset", new Vector4(Offset.x, Offset.y, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
    }
}
