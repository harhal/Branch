using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    public Vector2Int Size;
    public Renderer BuildingRenderer;
    public uint Cost { get { return cost; } private set { cost = value; } }
    [SerializeField]
    private uint cost;
    [SerializeField]
    protected float MaxDurability;
    public float Durability { get; protected set; }

    public byte HighLightType = 0;
    public bool ResetHighLight = true;

    public void FullRepair()
    {
        Durability = MaxDurability;
    }

    public virtual void Update()
    {

    }
}