using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGrid : MonoBehaviour {

    Dictionary<Building, Vector2Int> Buildings;
    [SerializeField]
    private Vector2Int Size;
    [SerializeField]
    private float CellSize;
    [SerializeField]
    private Vector2 Offset;
    bool[,] FreeTerrain;
    //float[] ColorArray;
    BitArray ColorArray;
    Terrain terrain;
    [SerializeField]
    private Material GridMaterial;

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
                ColorArray[j * Size.x + i] = false;
            }
        //RefreshGrid();
        //GridMaterial.SetFloatArray("_CellArray", ColorArray);
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
                ColorArray[j * Size.x + i] = true;
            }
        //RefreshGrid();
        //GridMaterial.SetFloatArray("_CellArray", ColorArray);
        Buildings.Remove(building);
        return true;
    }

    public void SetGridVisibility(bool Visibility)
    {
        GridMaterial.SetFloat("_DrawGrid", Visibility ? 1 : 0);
    }

    void RefreshGrid()
    {
        string outp = "";
        float[] farr = new float[Mathf.CeilToInt(ColorArray.Length / 24) + 1];
        for (int i = 0; i < ColorArray.Length; i++)
        {
            farr[Mathf.CeilToInt(i / 24)] += (ColorArray[i] ? 1 : 0) << (i % 24);
        }
        foreach (float val in farr)
        GridMaterial.SetFloatArray("_CellArray", farr);
    }
    
    void Start () {
        Buildings = new Dictionary<Building, Vector2Int>();
        terrain = GetComponent<Terrain>();
        FreeTerrain = new bool[Size.x, Size.y];
        //ColorArray = new float[Size.x * Size.y];
        ColorArray = new BitArray(Size.x * Size.y, true);
        for (int i = 0; i < Size.x; i++)
            for (int j = 0; j < Size.y; j++)
            {
                FreeTerrain[i, j] = true;
                ColorArray[j * Size.x + i] = true;
            }
        if (terrain == null) return;
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetFloat("_DrawGrid", 0);
        mpb.SetVector("_GridSize", new Vector4(Size.x, Size.y, 0, 0));
        mpb.SetFloat("_GridSpacing", CellSize);
        mpb.SetVector("_GridOffset", new Vector4(Offset.x, Offset.y, 0, 0));
        terrain.SetSplatMaterialPropertyBlock(mpb);
    }
    
    void Update ()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        RefreshGrid();
        terrain.SetSplatMaterialPropertyBlock(mpb);
    }
}
