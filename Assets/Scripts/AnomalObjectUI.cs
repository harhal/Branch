using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnomalObjectUI : MonoBehaviour {

    public AnomalObject anomalObject;
    public Text ID;
    public Text Name;
    public Image ObjectImage;
    public Text Progress;
    public Text Description;
    public Button MoveButton;
    //public Text Properties;
    public bool ToRefreshData;

    PlayerController controller;

    public RectTransform rectTransform;

    public Transform parentCanvas;
    public Transform hiddenUI;
    /*Vector2 ShowLocation;
    Vector2 HideLocation;*/

    // Use this for initialization
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        /*HideLocation = rectTransform.anchoredPosition;
        ShowLocation = HideLocation + Vector2.left * 300;*/
        GameObject finded = GameObject.Find("PlayerController");
        if (finded != null)
            controller = finded.GetComponent<PlayerController>();
        Hide();
        /*GameObject finded = GameObject.Find("AnomalObject_CodeName");
        if (finded != null)
            ID = finded.GetComponent<Text>();
        finded = GameObject.Find("AnomalObject_TextName");
        if (finded != null)
            Name = finded.GetComponent<Text>();
        finded = GameObject.Find("AnomalObject_Image");
        if (finded != null)
            ObjectImage = finded.GetComponent<Image>();
        finded = GameObject.Find("AnomalObject_Progress");
        if (finded != null)
            Progress = finded.GetComponent<Text>();
        finded = GameObject.Find("AnomalObject_Description");
        if (finded != null)
            Description = finded.GetComponent<Text>();*/
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
        controller.SetAnomalObjectMode(anomalObject);
    }

    public void SetAnomalObject(AnomalObject anomalObject)
    {
        this.anomalObject = anomalObject;
        RefreshData();
        transform.SetParent(parentCanvas, false);
    }

    public void Hide()
    {
        transform.SetParent(hiddenUI, false);
        //rectTransform.anchoredPosition = HideLocation;
    }
}
