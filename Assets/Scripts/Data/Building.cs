using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HighlightType { None, Aviable, Selected, Forbided };

[System.Serializable]
public class Building
{
    public Vector2Int Size = Vector2Int.one;
    [SerializeField]
    private int cost;
    public Vector2Int Location;

    public int Cost { get { return cost; } protected set { cost = value; } }
    [System.NonSerialized]
    public BuildingGameObject container;

    public virtual void HighLight(HighlightType highlightType, Renderer renderer) { }
    public virtual void Update() { }

    internal virtual void PrepareToSave() { }

    internal virtual void InitAfterLoad() { }
}

[System.Serializable]
public class PackagedBuilding
{
    [SerializeField]
    string Type;
    [SerializeField]
    string Building;

    public PackagedBuilding(Building building)
    {
        if (building != null)
        {
            Type = building.GetType().Name;
            Building = JsonUtility.ToJson(building);
        }
        else Type = "null";
    }

    public Building GetUnpackedBuilding()
    {
        if (Type != "null")
        {
            return JsonUtility.FromJson(Building, System.Type.GetType(Type)) as Building;
        }
        else
            return null;
    }
}