using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Researches
{
    public ImpactFactors ResearchesProgress;
    public ImpactFactors ResearchedTechs;

    public void Research(ImpactFactors Researches, AnomalObject ResearchedObject)
    {
        ResearchesProgress += Researches;
        for (int i = 0; i < ImpactFactors.FieldsCount; i++)
        {
            if (i != 6)
            {
                if (ResearchedTechs[i] >= ImpactFactors.MaxValue)
                {
                    ResearchesProgress[i] = 0;
                    ResearchedTechs[i] = ImpactFactors.MaxValue;
                }
                if (ResearchesProgress[i] > GameData.Data.LevelsData.GetResearchesPointsToNextLevel((int)ResearchedTechs[i]))
                {
                    ResearchesProgress[i] = ResearchesProgress[i] - GameData.Data.LevelsData.GetResearchesPointsToNextLevel((int)ResearchedTechs[i]);
                    ResearchedObject.Description += '\n' + GameData.Data.LevelsData.GetDiscoveryDescription(i, (int)ResearchedTechs[i]);
                    Kanban.Board.ResearchesUpdated("New technogy discovered", ResearchedObject);
                    ResearchedTechs[i]++;
                }
            }
        }
    }

    public float GetPurcentProgress(int i)
    {
        if (i < 0 || i > ImpactFactors.FieldsCount) return 1;
        return ResearchesProgress[i] / GameData.Data.LevelsData.GetResearchesPointsToNextLevel((int)ResearchedTechs[i]);
    }

    public ImpactFactors AnomalObjectPropertiesToResearches(ImpactFactors Properties)
    {
        ImpactFactors result = new ImpactFactors(0);
        for (int i = 0; i < ImpactFactors.FieldsCount; i++)
        {
            result[i] = GameData.Data.LevelsData.GetResearchesPointsByProperty((int)Properties[i]);
        }
        return result;
    }

    internal void PrepareToSave()
    {
    }

    internal void InitAfterLoad()
    {
    }
}