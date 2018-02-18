using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    public Vector2Int Size;
    public Renderer BuildingRenderer;
    public uint Cost { get { return cost; } private set { cost = value; } }
    [SerializeField]
    private uint cost;
}
