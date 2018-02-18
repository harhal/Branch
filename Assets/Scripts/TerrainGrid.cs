using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGrid : GridComponent {
    Terrain terrain;
    MaterialPropertyBlock mpb;
    BitArray ColorArray;

    public override void SetGridVisibility(bool Visibility)
    {
        mpb.SetFloat("_DrawGrid", Visibility ? 1 : 0);
        terrain.SetSplatMaterialPropertyBlock(mpb);
    }

    protected override bool OccupyCell(int x, int y)
    {
        if (base.OccupyCell(x, y))
        {
            ColorArray[y * Size.x + x] = false;
            return true;
        }
        return false;
    }

    protected override bool FreeCell(int x, int y)
    {
        if (base.FreeCell(x, y))
        {
            ColorArray[y * Size.x + x] = true;
            return true;
        }
        return false;
    }

    void RefreshGrid()
    {
        float[] farr = new float[Mathf.CeilToInt(ColorArray.Length / 24) + 1];
        for (int i = 0; i < ColorArray.Length; i++)
        {
            farr[Mathf.CeilToInt(i / 24)] += (ColorArray[i] ? 1 : 0) << (i % 24);
        }
        //foreach (float val in farr)
        mpb.SetFloatArray("_CellArray", farr);
        terrain.SetSplatMaterialPropertyBlock(mpb);
    }
    
    new void Start ()
    {
        ColorArray = new BitArray(Size.x * Size.y, true);
        base.Start();
        terrain = GetComponent<Terrain>();
        if (terrain == null) return;
        mpb = new MaterialPropertyBlock();
        mpb.SetFloat("_DrawGrid", 0);
        mpb.SetVector("_GridSize", new Vector4(Size.x, Size.y, 0, 0));
        mpb.SetVector("_GridSpacing", new Vector4(CellSize.x, CellSize.y, 0, 0));
        mpb.SetVector("_GridOffset", new Vector4(Offset.x, Offset.y, 0, 0));
        terrain.SetSplatMaterialPropertyBlock(mpb);
    }
    
    void Update ()
    {
        RefreshGrid();
    }
}
