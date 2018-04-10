using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntrepotUI : UICollabsiblePanel {

    public static EntrepotUI UI;

    [SerializeField]
    GameObject objectList;
    Dictionary<AnomalObject, AnomalObjectUI> anomalObjects;
    [SerializeField]
    AnomalObjectUI uiPrototype;
    [SerializeField]
    Toggle Filter;
    public bool ToRefreshData;

    //public Warehouse Warehouse;

    public void RefreshData()
    {
        bool filterOn = false;
        if (Filter != null) filterOn = Filter.isOn;
        int offset = 0;
        foreach (var anomalObject in SessionData.Data.Warehouse.AnomalObjects)
        {
            if (anomalObject.Value.Storage == null || !filterOn)
            {
                AnomalObjectUI ui;
                if (anomalObjects.TryGetValue(anomalObject.Value, out ui))
                {
                    ui.SetAnomalObject(anomalObject.Value);
                }
                else
                {
                    if (uiPrototype == null) Debug.LogError("UI prototype is not set");
                    ui = GameObject.Instantiate<AnomalObjectUI>(uiPrototype);
                    //ui.parentCanvas = objectList.transform;
                    ui.transform.SetParent(objectList.transform);
                    ui.SetAnomalObject(anomalObject.Value);
                    anomalObjects.Add(anomalObject.Value, ui);
                }
                //ui.rectTransform.anchoredPosition = BaseItemLocation + ItemLocationOffset * offset;
                offset++;
            }
            else
            {
                AnomalObjectUI ui;
                if (anomalObjects.TryGetValue(anomalObject.Value, out ui))
                {
                    ui.Hide();
                }
            }

        }
        //if (listTransform != null)
            //listTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (BaseItemLocation + ItemLocationOffset * offset).x);
    }

    void DataChanged(string Description, object Object)
    {
        RefreshData();
    }

    private void OnEnable()
    {
        if (SessionData.Data != null)
            RefreshData();
    }

    void Awake ()
    {
        anomalObjects = new Dictionary<AnomalObject, AnomalObjectUI>();
        if (UI == null)
            UI = this;
    }

    void Start()
    {
        Kanban.Board.OnNewAnomalObject += DataChanged;
        Kanban.Board.OnAnomalObjectMoved += DataChanged;
        Kanban.Board.OnContaintmentBreach += DataChanged;
        Kanban.Board.OnContaintmentRestored += DataChanged;
    }
}
