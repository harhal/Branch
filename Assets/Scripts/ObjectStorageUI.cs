using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStorageUI : MonoBehaviour {

    public static ObjectStorageUI UI;

    public ObjectStorage objectStorage;
    public Text Durability;
    //public Text Properties;
    public bool ToRefreshData;
    public PlusMinusUI Scientists;
    ResourceStorage resources;

    /*RectTransform rectTransform;
    Vector2 ShowLocation;
    Vector2 HideLocation;*/
    public Transform parentCanvas;
    public Transform hiddenUI;

    AnomalObjectUI StoringObjectUI;

    public void Awake()
    {
        UI = this;
    }

    // Use this for initialization
    void Start ()
    {
        /*rectTransform = GetComponent<RectTransform>();
        HideLocation = rectTransform.anchoredPosition;
        ShowLocation = HideLocation - Vector2.left * 300;*/
        GameObject finded = GameObject.Find("AnomalObjectInfo");
        if (finded != null)
            StoringObjectUI = finded.GetComponent<AnomalObjectUI>();
        finded = GameObject.Find("ResourceStorage");
        if (finded != null)
        {
            resources = finded.GetComponent<ResourceStorage>();
            if (Scientists != null)
            {
                Scientists.OnValueChanged += delegate (int delta)
                {
                    if (delta >= 0)
                        objectStorage.HireScientist();
                    else
                        objectStorage.FireScientist();
                };
            }
        }
        Hide();
    }
	
	// Update is called once per frame
	void Update () {
        if (ToRefreshData)
        {
            ToRefreshData = false;
            RefreshData();
        }
        if (Scientists != null && resources != null && objectStorage != null)
        {
            Scientists.MaxValue = objectStorage.HiredScientists + resources.Scientists.Free;
            Scientists.RefreshText(0);
        }
    }

    public void RefreshData()
    {
        if (objectStorage == null) return;
        if (Durability != null)
            Durability.text = objectStorage.GetPurcentDurability().ToString() + '%';
        if (Scientists != null)
        {
            if (objectStorage.anomalObject != null)
                Scientists.Value = objectStorage.HiredScientists;
        }
        if (objectStorage.anomalObject != null)
            StoringObjectUI.SetAnomalObject(objectStorage.anomalObject);
        else
            StoringObjectUI.Hide();
    }

    public void SetObjectStorage(ObjectStorage objectStorage)
    {
        this.objectStorage = objectStorage;
        RefreshData();
        transform.SetParent(parentCanvas, false);
    }

    public void Hide()
    {
        transform.SetParent(hiddenUI, false);
    }
}
