using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanelSwitch : MonoBehaviour {

    public static BottomPanelSwitch UI;

    public Button BuildSwitch;
    public Button WarehouseSwitch;

    public void SwitchToBuild()
    {
        BuildSwitch.interactable = false;
        Builder.builder.Show();
        WarehouseSwitch.interactable = true;
        EntrepotUI.UI.Hide();
    }

    public void SwitchToWarehouse()
    {
        BuildSwitch.interactable = true;
        Builder.builder.Hide();
        WarehouseSwitch.interactable = false;
        EntrepotUI.UI.Show();
    }
    
    void Awake ()
    {
        if (UI == null)
            UI = this;
    }

    private void Start()
    {
        SwitchToBuild();
    }
}
