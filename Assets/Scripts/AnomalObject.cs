using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ImpactFactors
{
    public float TermoFactor;         //Угроза: Температурный фактор;                                                   Протеводействие: Система температурного конторля
    public float RealityFactor;       //Угроза: Фактор воздействия на текущую реальность и взаимодействия с другими;    Протеводействие: Фиксирование реальности
    public float MentalFactor;        //Угроза: Фактор воздействия на разум;                                            Протеводействие: Ментальное изолирование
    public float PhisicalFactor;      //Угроза: Фактор физических повреждений;                                          Протеводействие: Укрепление
    public float SpaceFactor;         //Угроза: Фактор воздействия на пространство;                                     Протеводействие: Стабилизация пространства
    public float RadiationFactor;     //Угроза: Фактор излучения;                                                       Протеводействие: Экранирование
    public float WillFactor;          //Угроза: Фактор воли;                                                            Протеводействие: Наблюдение и контроль
    public float ToxicFactor;         //Угроза: Химическое / биологическое заражение;                                   Противодействие: Гермитизация
}

public class AnomalObject : MonoBehaviour {

    public int ID;
    public string Name;
    public Sprite Image;
    public float Progress;
    public string Description;
    public ImpactFactors Properties;

    public ObjectStorage storage;
}
