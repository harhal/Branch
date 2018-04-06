using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStorageUI : UICollabsiblePanel {

    public static ObjectStorageUI UI;

    public ObjectStorage objectStorage;
    public bool ToRefreshData;
    public Text Level;
    public Text ScientistsCount;
    public Text GuardCount;
    public Button ScientistsButton;
    public Button GuardButton;
    public ProgressBar[] Factors;

    public void Awake()
    {
        UI = this;
    }
    
    void Start ()
    {
        Hide();
    }
	
	void Update () {
        if (ToRefreshData)
        {
            ToRefreshData = false;
            RefreshData();
        }
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
            GuardCount.text = objectStorage.HiredOperatives.ToString();
        }
        if (Factors.Length >= 8)
        {
            Factors[0].Progress = objectStorage.Protection.TermoFactor / ImpactFactors.MaxValue;
            Factors[1].Progress = objectStorage.Protection.RealityFactor / ImpactFactors.MaxValue;
            Factors[2].Progress = objectStorage.Protection.MentalFactor / ImpactFactors.MaxValue;
            Factors[3].Progress = objectStorage.Protection.PhisicalFactor / ImpactFactors.MaxValue;
            Factors[4].Progress = objectStorage.Protection.SpaceFactor / ImpactFactors.MaxValue;
            Factors[5].Progress = objectStorage.Protection.RadiationFactor / ImpactFactors.MaxValue;
            Factors[6].Progress = objectStorage.Protection.WillFactor / ImpactFactors.MaxValue;
            Factors[7].Progress = objectStorage.Protection.BiohazardFactor / ImpactFactors.MaxValue;
        }
        if (!AnomalObjectUI.FirstUI.gameObject.activeInHierarchy)
            if (objectStorage.anomalObject != null)
                AnomalObjectUI.FirstUI.SetAnomalObject(objectStorage.anomalObject);
            else
                AnomalObjectUI.FirstUI.Hide();
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
        PlayerController.MainController.ShowPeopleControl("Scientists", SessionData.Data.ResourceStorage.FreeOperatives, objectStorage.Scientists, 10, ScientistsConfirm);
    }

    public void GuardConfirm(List<Human> GuardList)
    {
        List<int> oldList = objectStorage.Scientists;
        objectStorage.Guard = new List<int>();
        foreach (var item in GuardList)
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
        GuardCount.text = GuardList.Count.ToString();
    }

    public void Upgrade()
    {
        PlayerController.MainController.ShowUpgrade(objectStorage);
    }
}
