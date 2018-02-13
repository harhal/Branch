using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {
    
    public Building[] Prototype;

    public BuildingDisposeComponent bdc;
    public Camera mainCamera;

    public void Build(int index)
    {
        if (index < 0 || index >= Prototype.Length) { print("Wrong prototype index");  return; }
        if (Prototype[index] == null)
        {
            print("WrongBuildingType");
            return;
        }
        Building newBuild = Instantiate(Prototype[index]);
        bdc.SetBuilding(newBuild);
    }

}
