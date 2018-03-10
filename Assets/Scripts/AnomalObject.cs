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
    public float BiohazardFactor;         //Угроза: Химическое / биологическое заражение;                                   Противодействие: Гермитизация
}

[System.Serializable]
public struct ResearchRepot
{
    public float ProgressPoints;
    [TextArea]
    public string Report;
}

public class AnomalObject : MonoBehaviour {

    public int ID;
    public string Name;
    public Sprite Image;
    public float Progress { get { return Mathf.Clamp(CurrentResearchPoints / FullResearchPoints, 0, 1); } }
    public float FullResearchPoints;
    public float CurrentResearchPoints;
    [TextArea]
    public string Description;
    public ResearchRepot[] Reports;
    int currentReport = 0;
    public ImpactFactors Properties;

    public ObjectStorage storage;

    public delegate void Message();
    public event Message ResearchIsOver;
    public event Message ResearchUpdated;

    public void Research(float ResearchPoints)
    {
        CurrentResearchPoints += ResearchPoints;
        if (currentReport < Reports.Length)
        {
            if (CurrentResearchPoints >= Reports[currentReport].ProgressPoints)
            {
                Description += '\n' + Reports[currentReport].Report;
                currentReport++;
                if (ResearchUpdated != null)
                    ResearchUpdated();
            }
        }
        if (CurrentResearchPoints >= FullResearchPoints && ResearchIsOver != null)
            ResearchIsOver();
    }
}
