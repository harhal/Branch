using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStorage : Building {

    public AnomalObject anomalObject;
    [SerializeField]
    ImpactFactors Protection;

    public uint HiredScientists;
    public uint WorkingScientists;
    public uint MaxScientists;
    [SerializeField]
    float ResearchSpeed = 1;
    List<Human> Scientists;

    ResourceStorage resources;

    private void OnEnable()
    {
        if (ResourceStorage.resourceStorage != null)
        {
            resources = ResourceStorage.resourceStorage;
        }
    }

    public float GetPurcentDurability()
    {
        return Durability / MaxDurability * 100;
    }

    public void HireScientist()
    {
        HiredScientists++;
        WorkingScientists = HiredScientists;
        resources.Scientists.Send();
    }

    public void FireScientist()
    {
        HiredScientists--;
        WorkingScientists = HiredScientists;
        resources.Scientists.Return(Scientists[0]);
        Scientists.Remove(Scientists[0]);
    }

    public void KillScientist()
    {
        HiredScientists--;
        WorkingScientists = HiredScientists;
        resources.Scientists.Dead(Scientists[0]);
        Scientists.Remove(Scientists[0]);
    }

    // Update is called once per frame
    public override void Update () {
        if (anomalObject != null)
            anomalObject.Research(WorkingScientists * ResearchSpeed * Time.deltaTime);
    }
}
