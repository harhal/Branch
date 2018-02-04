using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Building building;
    public Camera mainCamera;

    public void OnClick()
    {
        Building newBuild = Instantiate(building);
        BuildingDisposeComponent bdc = newBuild.GetComponent<BuildingDisposeComponent>();
        if (bdc == null) return;
        bdc.mainCamera = mainCamera;
    }

}
