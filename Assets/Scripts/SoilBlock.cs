using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilBlock : Building {

    public Material baseMaterial;
    public Material highLightedMaterial;

    public override void Update()
    {
        if (HighLightType != 0)
            BuildingRenderer.material = highLightedMaterial;
        else
            BuildingRenderer.material = baseMaterial;
        if (ResetHighLight)
            HighLightType = 0;
    }

    /*public override void Select()
    {
        Dig();
    }*/

    /*public void Dig()
    {
        if (this == null) return;
        if (gameObject == null) return;
        GameObject.Destroy(gameObject);
    }*/
}
