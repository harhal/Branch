using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoilUI : UICollabsiblePanel {

    public static SoilUI UI;

    public Text Type;
    public Text Cost;

    SoilBlock Soil;

    private void Awake()
    {
        if (UI == null)
            UI = this;
    }

    private void Start()
    {
        Hide();
    }

    void Refresh()
    {
        switch (Soil.Type)
        {
            case SoilBlock.SoilType.SoftSoil:
                {
                    Type.text = "Soft soil";
                    break;
                }
            case SoilBlock.SoilType.DenseSoil:
                {
                    Type.text = "Dense soil";
                    break;
                }
            case SoilBlock.SoilType.Rock:
                {
                    Type.text = "Rock";
                    break;
                }
        }
        Cost.text = Soil.Cost.ToString();
    }

    public void Dig()
    {
        Builder.builder.Dig(Soil);
        //PlayerController.MainController.Dig(Soil);
    }

    public void SetData(SoilBlock Soil)
    {
        this.Soil = Soil;
        Show();
        Refresh();
    }
}
