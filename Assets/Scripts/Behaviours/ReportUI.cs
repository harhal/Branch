using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportUI : OverlayWindow
{

    public static ReportUI UI;
    public Image Photo;
    public Text Description;
    public GameObject ButtonContainer;
    public GameObject SummaryContainer;
    public Text Summary;
    List<VariantButtonUI> Choises;
    public VariantButtonUI ButtonUIPrototype;
    public Report CurrentReport { get; private set; }

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
        if (UI == null)
            UI = this;
        Choises = new List<VariantButtonUI>();
        print("ReportUI");
        Hide();
    }

    public void SetReport(Report report)
    {
        CurrentReport = report;
        Refresh();
        if (report.isVariable)
        {
            ButtonContainer.SetActive(true);
            SummaryContainer.SetActive(false);
            var choises = report.GetChoises();
            for (int i = 0; i < choises.Length; i++)
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
                NewButton.Set(choises[i]);
            }
            for (int i = choises.Length; i < Choises.Count; i++)
            {
                Choises[i].gameObject.SetActive(false);
            }
        }
        else
        {
            Summary.text = report.Summary;
            ButtonContainer.SetActive(false);
            SummaryContainer.SetActive(true);
        }
    }
}
