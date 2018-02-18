using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {
    
    public Building[] Prototype;

    BuildingDisposeComponent bdc;
    //Camera mainCamera;

    public void Start()
    {
        GameObject finded = GameObject.Find("Terrain");
        if (finded != null)
        {
            bdc = finded.GetComponent<BuildingDisposeComponent>();
            print("Connected with BDC on terrain");
        }
        else
        {
            finded = GameObject.Find("Base");
            if (finded != null)
            {
                bdc = finded.GetComponent<BuildingDisposeComponent>();
                print("Connected with base BDC");
            }
        }
        /*finded = GameObject.Find("Main Camera");
        if (finded != null)
            mainCamera = finded.GetComponent<Camera>();*/
    }

    public void Build(int index)
    {
        if (bdc == null) { print("BDC is null"); return; }
        if (!bdc.IsFree()) return;
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
