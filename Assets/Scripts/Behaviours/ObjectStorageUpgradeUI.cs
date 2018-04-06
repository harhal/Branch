using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStorageUpgradeUI : OverlayWindow
{
    public static ObjectStorageUpgradeUI UI;
    public ObjectStorage UpgradedStorage;
    ImpactFactors UpgradedProtection;

    public Text Level;
    public Text UpgradePoints;
    public GameObject UpgradePointsPanel;
    public ProtectionUpgradeUI[] FactorProtection;
    public Button UpgradeButton;

    // Use this for initialization
    void Awake () {
        if (UI == null)
            UI = this;
        Hide();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (UpgradePoints != null)
            UpgradePoints.text = ProtectionUpgradeUI.FreePoints.ToString();
        if (UpgradePointsPanel != null)
            UpgradePointsPanel.SetActive(ProtectionUpgradeUI.FreePoints > 0);
        UpgradeButton.interactable = SessionData.Data.ResourceStorage.Money >= 40;
    }

    public void SetStorage(ObjectStorage storage)
    {
        UpgradedStorage = storage;
        UpgradedProtection = storage.Protection;
        if (Level != null)
            Level.text = storage.Level.ToString();
        if (UpgradePoints != null)
            UpgradePoints.text = storage.UpgradePoints.ToString();
        if (UpgradePointsPanel != null)
            UpgradePointsPanel.SetActive(storage.UpgradePoints > 0);
        ProtectionUpgradeUI.MaxValue = 10;
        ProtectionUpgradeUI.FreePoints = storage.UpgradePoints;
        for (int i = 0; i < ImpactFactors.FieldsCount; i++)
            if (i < FactorProtection.Length ? FactorProtection[i] != null : false)
                FactorProtection[i].SetBaseValue((int)storage.Protection[i], (int)SessionData.Data.Researches.ResearchedTechs[i]);
        UpgradeButton.interactable = SessionData.Data.ResourceStorage.Money >= 40;
    }

    public void Upgrade()
    {
        SessionData.Data.ResourceStorage.SpendMoney(40);
        UpgradedStorage.LevelUp();
        ResetChanges(); 
    }

    public void Accept()
    {
        for (int i = 0; i < ImpactFactors.FieldsCount; i++)
            if (i < FactorProtection.Length ? FactorProtection[i] != null : false)
                UpgradedStorage.Protection[i] = FactorProtection[i].Value;
        UpgradedStorage.UpgradePoints = ProtectionUpgradeUI.FreePoints;
        ResetChanges();
    }

    public void ResetChanges()
    {
        SetStorage(UpgradedStorage);
    }
}
