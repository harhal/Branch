using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {
    
    public Building[] Prototype;

    PlayerController controller;

    public void Start()
    {
        GameObject finded = GameObject.Find("PlayerController");
        if (finded != null)
        {
            controller = finded.GetComponent<PlayerController>();
        }
    }

    public void Dig()
    {
        if (controller == null) { print("controller is null"); return; }
        controller.SetDigMode();
        /*if (!bdc.IsFree()) return;
        bdc.EnableToDig();*/
    }

    public void Build(int index)
    {
        if (controller == null) { print("controller is null"); return; }
        //if (!bdc.IsFree()) return;
        if (index < 0 || index >= Prototype.Length) { print("Wrong prototype index");  return; }
        if (Prototype[index] == null)
        {
            print("WrongBuildingType");
            return;
        }
        Building newBuild = Instantiate(Prototype[index]);
        controller.SetBuildMode(newBuild);
    }

}
