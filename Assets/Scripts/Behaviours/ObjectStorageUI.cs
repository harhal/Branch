using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStorageUI : UICollabsiblePanel {

    public static ObjectStorageUI UI;

    public ObjectStorage objectStorage;
    public Text Level;
    public GameObject NoStoringObject;
    public GameObject UsualInfoContainer;
    public GameObject ContaintementBreachInfoContainer;
    public Text ScientistsCount;
    public Text GuardCount;
    public Text LiquidatorsCount;
    public Button ScientistsButton;
    public Button GuardButton;
    public ProgressBar[] Factors;
    public Button UpgradeButton;
    public ProgressBar ChaosLevel;

    public void Awake()
    {
        UI = this;
    }
    
    void Start ()
    {
        Hide();
    }
	
	void Update () {
        if (objectStorage == null) return;
        if (objectStorage.anomalObject == null) return;
        if (!objectStorage.anomalObject.IsStable())
            if (ChaosLevel != null)
                ChaosLevel.Progress = objectStorage.GetChaosLevel();
    }

    public void RefreshData()
    {
        if (objectStorage == null) return;
        if (Level != null)
            Level.text = objectStorage.Level.ToString();
        if (ScientistsButton) ScientistsButton.interactable = objectStorage.anomalObject != null;
        if (GuardButton) GuardButton.interactable = objectStorage.anomalObject != null;
        if (ScientistsCount != null)
        {
            ScientistsCount.text = objectStorage.HiredScientists.ToString();
        }
        if (GuardCount != null)
        {
            GuardCount.text = objectStorage.HiredGuard.ToString();
        }
        if (LiquidatorsCount != null)
        {
            LiquidatorsCount.text = objectStorage.HiredLiquidators.ToString();
        }
        if (ChaosLevel != null)
            ChaosLevel.Progress = objectStorage.GetChaosLevel();
        if (Factors.Length >= ImpactFactors.FieldsCount)
        {
            for (int i = 0; i < ImpactFactors.FieldsCount; i++)
            Factors[i].Progress = objectStorage.Protection[i] / ImpactFactors.MaxValue;
        }
        if (objectStorage.anomalObject != null)
        {
            NoStoringObject.SetActive(false);
            AnomalObjectUI.FirstUI.SetAnomalObject(objectStorage.anomalObject);
            if (objectStorage.anomalObject.IsStable())
            {
                UsualInfoContainer.SetActive(true);
                ContaintementBreachInfoContainer.SetActive(false);
                UpgradeButton.interactable = true;
            }
            else
            {
                UsualInfoContainer.SetActive(false);
                ContaintementBreachInfoContainer.SetActive(true);
                UpgradeButton.interactable = false;
            }
        }
        else
        {
            NoStoringObject.SetActive(true);
            AnomalObjectUI.FirstUI.Hide();
            UsualInfoContainer.SetActive(false);
            ContaintementBreachInfoContainer.SetActive(false);
            UpgradeButton.interactable = true;
        }
    }

    public void SetObjectStorage(ObjectStorage objectStorage)
    {
        this.objectStorage = objectStorage;
        Show();
        RefreshData();
        //transform.SetParent(parentCanvas, false);
    }

    public void ScientistsControl()
    {
        PlayerController.MainController.ShowPeopleControl("Scientists", SessionData.Data.ResourceStorage.FreeScientists, objectStorage.Scientists, 3, ScientistsConfirm);
    }

    public void ScientistsConfirm(List<Human> ScientistsList)
    {
        List<int> oldList = objectStorage.Scientists;
        objectStorage.Scientists = new List<int>();
        foreach (var item in ScientistsList)
        {
            if (item.Activity != Human.ActivityType.Working)
            {
                item.Hire(objectStorage);
            }
            objectStorage.Scientists.Add(item.ID);
            oldList.Remove(item.ID);
        }
        foreach (var item in oldList)
        {
            SessionData.Data.ResourceStorage.People[item].Fire();
        }
        ScientistsCount.text = ScientistsList.Count.ToString();
    }

    public void GuardControl()
    {
        PlayerController.MainController.ShowPeopleControl("Guard", SessionData.Data.ResourceStorage.FreeOperatives, objectStorage.Guard, 5, GuardConfirm);
    }

    public void GuardConfirm(List<Human> GuardList)
    {
        List<int> oldList = objectStorage.Guard;
        objectStorage.Guard = new List<int>();
        foreach (var item in GuardList)
        {
            if (item.Activity != Human.ActivityType.Working)
            {
                item.Hire(objectStorage);
            }
            objectStorage.Guard.Add(item.ID);
            oldList.Remove(item.ID);
        }
        foreach (var item in oldList)
        {
            SessionData.Data.ResourceStorage.People[item].Fire();
        }
        GuardCount.text = objectStorage.HiredGuard.ToString();
        objectStorage.RefreshWillProtection();
        Factors[6].Progress = objectStorage.Protection.WillFactor / ImpactFactors.MaxValue;
    }

    public void LiquidatorControl()
    {
        PlayerController.MainController.ShowPeopleControl("Liquidators", SessionData.Data.ResourceStorage.FreeOperatives, objectStorage.Liquidators, 10, LiquidatorConfirm);
    }

    public void LiquidatorConfirm(List<Human> LiquidatorsList)
    {
        List<int> oldList = objectStorage.Liquidators;
        objectStorage.Liquidators = new List<int>();
        foreach (var item in LiquidatorsList)
        {
            if (item.Activity != Human.ActivityType.Working)
            {
                item.Hire(objectStorage);
            }
            objectStorage.Liquidators.Add(item.ID);
            oldList.Remove(item.ID);
        }
        foreach (var item in oldList)
        {
            SessionData.Data.ResourceStorage.People[item].Fire();
        }
        LiquidatorsCount.text = LiquidatorsList.Count.ToString();
    }

    public void Upgrade()
    {
        PlayerController.MainController.ShowUpgrade(objectStorage);
    }
}
