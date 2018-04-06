using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnomalObjectUI : UICollabsiblePanel
{

    public static AnomalObjectUI FirstUI;

    //public AnomalObject anomalObject;
    public Text ID;
    public Text Name;
    public Image ObjectImage;
    public ProgressBar Progress;
    public Text Description;

    public AnomalObject AnomalObject;

    [SerializeField]
    Button EnableMoveModeButton;
    [SerializeField]
    Button DisableMoveModeButton;
    //public Button MoveButton;

    //public RectTransform rectTransform;

    /*public Transform parentCanvas;
    public Transform hiddenUI;*/

    void Awake()
    {
        if (FirstUI == null)
            FirstUI = this;
        //rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (AnomalObject != null && this == FirstUI)
        {
            if (Progress != null)
                Progress.Progress = AnomalObject.Progress;
            if (Description != null)
                Description.text = AnomalObject.Description;
        }
    }

    public void RefreshData()
    {
        if (AnomalObject == null) return;
        if (ID != null)
            ID.text = AnomalObject.ID.ToString();
        if (Name != null)
            Name.text = AnomalObject.Name;
        if (ObjectImage != null)
            ObjectImage.sprite = AnomalObject.Image;
        if (Progress != null)
            Progress.Progress = AnomalObject.Progress;
        if (Description != null)
            Description.text = AnomalObject.Description;
        /*if (MoveButton != null)
        {
            MoveButton.onClick.RemoveAllListeners();
            MoveButton.onClick.AddListener(SelectInController);
        }*/
        /*if (!ObjectStorageUI.UI.gameObject.activeInHierarchy)
            if (AnomalObject.storage != null)
                ObjectStorageUI.UI.SetObjectStorage(AnomalObject.storage);
            else
                ObjectStorageUI.UI.Hide();*/
    }

    public void SelectInController()
    {
        if (AnomalObject == null) return;
        if (AnomalObject.Storage == null)
        {
            ObjectStorage storage;
            if (PlayerController.MainController.CurrentMode == PlayerController.ControlMode.BuildingControl &&
                (storage = PlayerController.MainController.ControllingBuilding as ObjectStorage) != null)
            {
                storage.anomalObject = AnomalObject;
                AnomalObject.Storage = storage;
                PlayerController.MainController.SetBuildingControlMode(PlayerController.MainController.ControllingBuilding);
            }
            else
                PlayerController.MainController.SetAnomalObjectMode(AnomalObject);
        }
        else
            PlayerController.MainController.SetBuildingControlMode(AnomalObject.Storage);
    }

    public void SetAnomalObject(AnomalObject anomalObject)
    {
        this.AnomalObject = anomalObject;
        gameObject.SetActive(true);
        if (anomalObject.Storage == null)
            SwitchToMoveMode();
        else
            SwitchToInfoMode();
        RefreshData();
        //transform.SetParent(parentCanvas, false);
    }

    public void SwitchToMoveMode()
    {
        if (EnableMoveModeButton != null)
            EnableMoveModeButton.gameObject.SetActive(false);
        if (DisableMoveModeButton != null)
            DisableMoveModeButton.gameObject.SetActive(AnomalObject != null ? AnomalObject.Storage != null : false);
    }

    public void EnableMoveMod()
    {
        if (AnomalObject != null)
            PlayerController.MainController.SetAnomalObjectMode(AnomalObject);
    }

    public void SwitchToInfoMode()
    {
        if (EnableMoveModeButton != null)
            EnableMoveModeButton.gameObject.SetActive(AnomalObject != null ? AnomalObject.Storage != null : false);
        if (DisableMoveModeButton != null)
            DisableMoveModeButton.gameObject.SetActive(false);
    }

    public void DisableMoveMod()
    {
        if (AnomalObject != null ? AnomalObject.Storage != null : false)
            PlayerController.MainController.SetBuildingControlMode(AnomalObject.Storage);
    }

    /*public void Hide()
    {
        this.AnomalObject = null;
        gameObject.SetActive(false);
        //transform.SetParent(hiddenUI, false);
    }*/
}
