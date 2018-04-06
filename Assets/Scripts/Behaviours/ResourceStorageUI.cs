using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStorageUI : MonoBehaviour {

    public Text MoneyOutput;
    public Text AgentsOutput;
    public Text ScientistsOutput;
    public Text OperativesOutput;
    public Text D_PersonnelOutput;
    public EntrepotUI entrepotUI;

    public ResourceStorage ResourceStorage;

    // Use this for initialization
    void Start () {
        ResourceStorage = SessionData.Data.ResourceStorage;
    }
	
	// Update is called once per frame
	void Update () {
        if (MoneyOutput != null)
            MoneyOutput.text = ResourceStorage.Money.ToString();
        if (AgentsOutput != null)
            AgentsOutput.text = ResourceStorage.FreeAgents.Count.ToString() + '(' + ResourceStorage.AgentsCount.ToString() + ')';
        if (ScientistsOutput != null)
            ScientistsOutput.text = ResourceStorage.FreeScientists.Count.ToString() + '(' + ResourceStorage.ScientistsCount.ToString() + ')';
        if (OperativesOutput != null)
            OperativesOutput.text = ResourceStorage.FreeOperatives.Count.ToString() + '(' + ResourceStorage.OperativesCount.ToString() + ')';
        if (D_PersonnelOutput != null)
            D_PersonnelOutput.text = ResourceStorage.FreeD_Personnel.Count.ToString() + '(' + ResourceStorage.D_PersonnelCount.ToString() + ')';
    }
}