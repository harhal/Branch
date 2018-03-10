using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventStack : MonoBehaviour {
    public static EventStack CommonStack;

    public GameObject Container;
    public Text NewCount;

    public GameObject NewIndicator;
    public RectTransform StackObject;
    public List<LogItem> Log;
    public LogItem itemPrototype;

    public void Refresh()
    {
        int newCount = 0;
        foreach (LogItem log in Log)
        {
            if (log.isNew) newCount++;

        }
        NewCount.text = newCount.ToString();
        if (newCount > 0)
        {
            NewIndicator.SetActive(true);
            StackObject.anchoredPosition = new Vector2(0, -50);
        }
        else
        {
            NewIndicator.SetActive(false);
            StackObject.anchoredPosition = new Vector2();
        }
    }

    public void AddItem(Report report)
    {
        AddItem(report.Photo, delegate { ReportUI.CommonReport.SetReport(report); });
    }

    public void AddItem(Sprite sprite, UnityEngine.Events.UnityAction action)
    {
        LogItem newItem = GameObject.Instantiate(itemPrototype);
        newItem.Set(sprite, action);
        newItem.transform.SetParent(Container.transform);
        Log.Add(newItem);
        Refresh();
    }

	// Use this for initialization
	void Awake () {
        CommonStack = this;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
