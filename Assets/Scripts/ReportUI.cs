using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportUI : MonoBehaviour {

    public static ReportUI CommonReport;
    public Image Photo;
    public Text Description;
    public GameObject ButtonContainer;
    List<VariantButtonUI> Choises;
    public VariantButtonUI ButtonUIPrototype;
    Report CurrentReport;

    public Canvas ParentComponent;

    void Refresh()
    {
        if (CurrentReport != null)
        {
            if (Photo != null)
                Photo.sprite = CurrentReport.Photo;
            if (Description != null)
                Description.text = CurrentReport.Description;
        }
    }

    private void Awake()
    {
        if (CommonReport == null)
            CommonReport = this;
        Choises = new List<VariantButtonUI>();
    }

    public void SetReport(Report report)
    {
        CurrentReport = report;
        Refresh();
        //VariantButtonUI Cancel;
        /*if (Choises.Count == 0)
        {
            Cancel = GameObject.Instantiate(ButtonUIPrototype);
            Cancel.transform.SetParent(ButtonContainer.transform, false);
            Choises.Add(Cancel);
        }
        else
            Cancel = Choises[0];
        Cancel.Set(true, "Cancel", Close);*/
        for (int i = 0; i < report.Choises.Length; i++)
        {
            VariantButtonUI NewButton;
            if (Choises.Count <= i)
            {
                NewButton = GameObject.Instantiate(ButtonUIPrototype);
                NewButton.transform.SetParent(ButtonContainer.transform, false);
                Choises.Add(NewButton);
            }
            else
                NewButton = Choises[i];
            NewButton.gameObject.SetActive(true);
            NewButton.Set(report.Choises[i]);
        }
        for (int i = report.Choises.Length; i < Choises.Count; i++)
        {
            Choises[i].gameObject.SetActive(false);
        }
        ParentComponent.enabled = true;
        Time.timeScale = 0;
    }

    public void Close()
    {
        ParentComponent.enabled = false;
        Time.timeScale = 1;
    }

    public void ChoseAgent()
    {
        CurrentReport.operation.GetAgent(ResourceStorage.resourceStorage.Agents.Send());
    }

    public void ChoseOperatives()
    {
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
