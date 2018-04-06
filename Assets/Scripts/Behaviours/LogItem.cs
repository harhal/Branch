using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogItem : MonoBehaviour {

    public Image image;
    public Image isNewIndicator;
    public bool isNew;
    public Button ActionButton;
    public string Description;
    public object Target;

    public void Set(Sprite sprite, object Target)
    {
        isNew = true;
        image.sprite = sprite;
        this.Target = Target;
        ActionButton.onClick.AddListener(Open);
        ActionButton.onClick.AddListener(Read);
    }

    public void Open()
    {
        if (Target is Building)
        {
            if (SessionData.Data.BranchBase.BuildingsList.Contains(Target as Building))
            PlayerController.MainController.SetBuildingControlMode(Target as Building);
        }
        if (Target is Vector3)
        {
            PlayerController.MainController.SetDefaultMode();
            CameraControl.MainCamera.Destination = (Vector3)Target;
        }
        if (Target is AnomalObject)
        {
            AnomalObject anomalObject = Target as AnomalObject;
            if (SessionData.Data.Warehouse.AnomalObjects.ContainsKey(anomalObject.ID))
            {
                if (anomalObject.Storage != null)
                    PlayerController.MainController.SetBuildingControlMode(anomalObject.Storage);
                else
                    PlayerController.MainController.SetAnomalObjectMode(anomalObject);
            }
        }
        if (Target is Report)
            PlayerController.MainController.ShowReport(Target as Report);
        if (Target is Human)
            PlayerController.MainController.ShowHuman(Target as Human);
        if (Target is string)
            PlayerController.MainController.ShowMessage(Target as string);
        Read();
    }

    public void Read()
    {
        isNew = false;
        EventStack.CommonStack.Refresh();
        Refresh();
    }

    public void Refresh()
    {
        if (isNew)
            isNewIndicator.color = Color.yellow;
        else
            isNewIndicator.color = Color.white;
        EventStack.CommonStack.Refresh();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
