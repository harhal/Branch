using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectStorageUI : MonoBehaviour {

    public ObjectStorage objectStorage;
    public Text Durability;
    //public Text Properties;
    public bool ToRefreshData;

    /*RectTransform rectTransform;
    Vector2 ShowLocation;
    Vector2 HideLocation;*/
    public Transform parentCanvas;
    public Transform hiddenUI;

    AnomalObjectUI StoringObjectUI;

    // Use this for initialization
    void Start ()
    {
        /*rectTransform = GetComponent<RectTransform>();
        HideLocation = rectTransform.anchoredPosition;
        ShowLocation = HideLocation - Vector2.left * 300;*/
        GameObject finded = GameObject.Find("AnomalObjectInfo");
        if (finded != null)
            StoringObjectUI = finded.GetComponent<AnomalObjectUI>();
        Hide();
        /*finded = GameObject.Find("StorageInfo_Durability");
        if (finded != null)
            Durability = finded.GetComponent<Text>();*/
    }
	
	// Update is called once per frame
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
        if (Durability != null)
            Durability.text = objectStorage.GetPurcentDurability().ToString() + '%';
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
