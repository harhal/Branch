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
    //public Text Properties;
    public bool ToRefreshData;

    RectTransform rectTransform;
    Vector2 ShowLocation;
    Vector2 HideLocation;

    // Use this for initialization
    void Start ()
    {
        rectTransform = GetComponent<RectTransform>();
        HideLocation = rectTransform.anchoredPosition;
        ShowLocation = HideLocation + Vector2.left * 300;
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

    void Show()
    {
        rectTransform.anchoredPosition = ShowLocation;
    }

    public void Hide()
    {
        rectTransform.anchoredPosition = HideLocation;
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
    }

    public void SetAnomalObject(AnomalObject anomalObject)
    {
        this.anomalObject = anomalObject;
        RefreshData();
        Show();
    }
}
