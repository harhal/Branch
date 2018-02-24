using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntrepotUI : MonoBehaviour {

    [SerializeField]
    GameObject objectList;
    Dictionary<AnomalObject, AnomalObjectUI> anomalObjects;
    [SerializeField]
    AnomalObjectUI uiPrototype;
    [SerializeField]
    Toggle Filter;
    ResourceStorage Entrepoint;
    [SerializeField]
    Vector2 BaseItemLocation;
    [SerializeField]
    Vector2 ItemLocationOffset;
    public bool ToRefreshData;

    RectTransform listTransform;

    public void RefreshData()
    {
        bool filterOn = false;
        if (Filter != null) filterOn = Filter.isOn;
        int offset = 0;
        foreach (AnomalObject anomalObject in Entrepoint.anomalObjects)
        {
            if (anomalObject.storage == null || !filterOn)
            {
                AnomalObjectUI ui;
                if (anomalObjects.TryGetValue(anomalObject, out ui))
                {
                    ui.SetAnomalObject(anomalObject);
                }
                else
                {
                    if (uiPrototype == null) Debug.LogError("UI prototype is not set");
                    ui = GameObject.Instantiate<AnomalObjectUI>(uiPrototype);
                    ui.parentCanvas = objectList.transform;
                    ui.SetAnomalObject(anomalObject);
                    anomalObjects.Add(anomalObject, ui);
                }
                ui.rectTransform.anchoredPosition = BaseItemLocation + ItemLocationOffset * offset;
                offset++;
            }
            else
            {
                AnomalObjectUI ui;
                if (anomalObjects.TryGetValue(anomalObject, out ui))
                {
                    ui.Hide();
                }
            }

        }
        if (listTransform != null)
            listTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (BaseItemLocation + ItemLocationOffset * offset).x);
    }

	// Use this for initialization
	void Start ()
    {
        anomalObjects = new Dictionary<AnomalObject, AnomalObjectUI>();
        if (objectList != null)
            listTransform = objectList.GetComponent<RectTransform>();
        GameObject finded = GameObject.Find("ResourceStorage");
        if (finded != null)
            Entrepoint = finded.GetComponent<ResourceStorage>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ToRefreshData)
        {
            ToRefreshData = false;
            RefreshData();
        }
    }
}
