using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TechDiscoveries
{
    [SerializeField] string[] Discoveries;

    public string GetDiscovery(int Level)
    {
        if (Level < 0 || Level >= Discoveries.Length) return "";
        else return Discoveries[Level];
    }
}

[System.Serializable]
public class LevelsData {
    [SerializeField] float[] HumanExperience;
    [SerializeField] float[] TechProgress;
    [SerializeField] float[] AgentPoints;
    [SerializeField] float[] OperativePoints;
    [SerializeField] float[] ScientistPoints;
    [SerializeField] float[] AgentDeathChance;
    [SerializeField] float[] OperativeDeathChance;
    [SerializeField] float[] ScientistDeathChance;
    [SerializeField] float[] PropertyToResearches;
    [SerializeField] TechDiscoveries[] Discoveries;
    public float FreeAnomalObjectBreachTime;
    public float OnePointAnomalObjectBreachTime;
    public float ProtectedAnomalObjectBreachTime;
    public float BaseChaosLevelTick;
    public float UnprotectedPointChaosLevelTick;
    public float MaxChaosLevel;

    float GetArrayValue(float[] array, int index)
    {
        return array[Mathf.Clamp(index, 1, array.Length) - 1];
    }

    public string GetDiscoveryDescription(int Tech, int Level)
    {
        if (Tech < 0 || Tech >= Discoveries.Length) return "";
        return Discoveries[Tech].GetDiscovery(Level);
    }

    public float GetExperienceToNextLevel(int Level)
    {
        return GetArrayValue(HumanExperience, Level);
    }

    public float GetResearchesPointsToNextLevel(int Level)
    {
        return GetArrayValue(TechProgress, Level);
    }

    public float GetAgentPointsAtLevel(int Level)
    {
        return GetArrayValue(AgentPoints, Level);
    }

    public float GetOperativePointsAtLevel(int Level)
    {
        return GetArrayValue(OperativePoints, Level);
    }

    public float GetScientistPointsAtLevel(int Level)
    {
        return GetArrayValue(ScientistPoints, Level);
    }

    public float GetAgentDeathChanceAtLevel(int Level)
    {
        return GetArrayValue(AgentDeathChance, Level);
    }

    public float GetOperativeDeathChanceAtLevel(int Level)
    {
        return GetArrayValue(OperativeDeathChance, Level);
    }

    public float GetScientistDeathChanceAtLevel(int Level)
    {
        return GetArrayValue(ScientistDeathChance, Level);
    }

    public float GetResearchesPointsByProperty(int Level)
    {
        return GetArrayValue(PropertyToResearches, Level);
    }
}
