using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGameObject : MonoBehaviour
{
    public Renderer BuildingRenderer;

    public Building Building;

    public void SetBuilding(Building building)
    {
        Building = building;
    }

    private void Update()
    {
        if (Building != null)
            Building.Update();
    }
}