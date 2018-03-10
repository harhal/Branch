using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnomalObjectUI : MonoBehaviour {

    public static AnomalObjectUI FirstUI;

    public AnomalObject anomalObject;
    public Text ID;
    public Text Name;
    public Image ObjectImage;
    public Text Progress;
    public Text Description;
    public Button MoveButton;

    public RectTransform rectTransform;

    public Transform parentCanvas;
    public Transform hiddenUI;

    void Awake()
    {
        if (FirstUI == null)
            FirstUI = this;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }
	
	// Update is called once per frame
	void Update () {
        if (anomalObject != null)
        {
            RefreshData();
        }
    }

    public void RefreshData()
    {
        if (anomalObject == null) return;
        if (ID != null)
            ID.text = anomalObject.ID.ToString();
        if (Name != null)
            Name.text = anomalObject.Name;
        if (ObjectImage != null)
            ObjectImage.sprite = anomalObject.Image;
        if (Progress != null)
            Progress.text = (anomalObject.Progress * 100).ToString("0") + '%';
        if (Description != null)
            Description.text = anomalObject.Description;
        if (MoveButton != null)
        {
            MoveButton.onClick.RemoveAllListeners();
            MoveButton.onClick.AddListener(SelectInController);
        }
    }

    public void SelectInController()
    {
        PlayerController.MainController.SetAnomalObjectMode(anomalObject);
    }

    public void SetAnomalObject(AnomalObject anomalObject)
    {
        this.anomalObject = anomalObject;
        RefreshData();
        transform.SetParent(parentCanvas, false);
    }

    public void Hide()
    {
        this.anomalObject = null;
        transform.SetParent(hiddenUI, false);
    }
}
