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
    public ScrollRect Scroll;

    public Sprite NewAnomalObjectIcon;
    public Sprite AnomalObjectMissedIcon;
    public Sprite AnomalObjectMovedIcon;
    public Sprite ResearchesUpdatedIcon;
    public Sprite ContaintmentBreachIcon;
    public Sprite ContaintmentRestoredIcon;
    public Sprite BuildingProcessStartedIcon;
    public Sprite BuildingProcessIsOverIcon;
    public Sprite ObjectStorageDestroyedIcon;
    public Sprite FirstIncedentReportIcon;
    public Sprite InvestigationStartedIcon;
    public Sprite InvestigationStoppedIcon;
    public Sprite CaptureOperationReadyToStartIcon;
    public Sprite CaptureOperationStartedIcon;
    public Sprite CaptureOperationIsOverIcon;
    public Sprite AgentHiredIcon;
    public Sprite AgentDiedIcon;
    public Sprite ScientistHiredIcon;
    public Sprite ScientistDiedIcon;
    public Sprite OperativeHiredIcon;
    public Sprite OperativeDiedIcon;
    public Sprite DPersonnelHiredIcon;
    public Sprite DPersonnelDiedIcon;

    void CreateNewAnomalObjectIcon              (string Description, object Invoker) { AddItem(NewAnomalObjectIcon, Invoker); TimeController.timeController.SetTimeScale(0); }
    void CreateAnomalObjectMissedIcon            (string Description, object Invoker) { AddItem(AnomalObjectMovedIcon, Invoker);}
    void CreateAnomalObjectMovedIcon            (string Description, object Invoker) { AddItem(AnomalObjectMovedIcon, Invoker);}
    void CreateResearchesUpdatedIcon            (string Description, object Invoker) { AddItem(ResearchesUpdatedIcon, Invoker);}
    void CreateContaintmentBreachIcon           (string Description, object Invoker) { AddItem(ContaintmentBreachIcon, Invoker); TimeController.timeController.SetTimeScale(0); }
    void CreateContaintmentRestoredIcon         (string Description, object Invoker) { AddItem(ContaintmentRestoredIcon, Invoker);}
    void CreateBuildingProcessStartedIcon       (string Description, object Invoker) { AddItem(BuildingProcessStartedIcon, Invoker);}
    void CreateBuildingProcessIsOverIcon        (string Description, object Invoker) { AddItem(BuildingProcessIsOverIcon, Invoker);}
    void CreateObjectStorageDestroyedIcon       (string Description, object Invoker) { AddItem(ObjectStorageDestroyedIcon, Invoker);}
    void CreateFirstIncedentReportIcon          (string Description, object Invoker) { AddItem(FirstIncedentReportIcon, Invoker); TimeController.timeController.SetTimeScale(0); }
    void CreateInvestigationStartedIcon         (string Description, object Invoker) { AddItem(InvestigationStartedIcon, Invoker);}
    void CreateInvestigationStoppedIcon         (string Description, object Invoker) { AddItem(InvestigationStoppedIcon, Invoker); TimeController.timeController.SetTimeScale(0); }
    void CreateCaptureOperationReadyToStartIcon (string Description, object Invoker) { AddItem(CaptureOperationReadyToStartIcon, Invoker); TimeController.timeController.SetTimeScale(0); }
    void CreateCaptureOperationStartedIcon      (string Description, object Invoker) { AddItem(CaptureOperationStartedIcon, Invoker);}
    void CreateCaptureOperationIsOverIcon       (string Description, object Invoker) { AddItem(CaptureOperationIsOverIcon, Invoker);}
    void CreateAgentHiredIcon                   (string Description, object Invoker) { AddItem(AgentHiredIcon, Invoker);}
    void CreateAgentDiedIcon                    (string Description, object Invoker) { AddItem(AgentDiedIcon, Invoker); }
    void CreateOperativeHiredIcon               (string Description, object Invoker) { AddItem(OperativeHiredIcon, Invoker);}
    void CreateOperativeDiedIcon                (string Description, object Invoker) { AddItem(OperativeDiedIcon, Invoker); }
    void CreateScientistHiredIcon               (string Description, object Invoker) { AddItem(ScientistHiredIcon, Invoker);}
    void CreateScientistDiedIcon                (string Description, object Invoker) { AddItem(ScientistDiedIcon, Invoker); }
    void CreateDPersonnelHiredIcon              (string Description, object Invoker) { AddItem(DPersonnelHiredIcon, Invoker);}
    void CreateDPersonnelDiedIcon               (string Description, object Invoker) { AddItem(DPersonnelDiedIcon, Invoker); }

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
            StackObject.offsetMax = new Vector2(0, -50);
        }
        else
        {
            NewIndicator.SetActive(false);
            StackObject.offsetMax = new Vector2();
        }
        Scroll.verticalNormalizedPosition = -0.1f;
    }

    /**public LogItem AddItem(Report report)
    {
        return AddItem(report.Photo, delegate { ReportUI.CommonReport.SetReport(report); });
    }*/

    public LogItem AddItem(Sprite sprite, object Target)
    {
        LogItem newItem = GameObject.Instantiate(itemPrototype);
        newItem.Set(sprite, Target);
        newItem.transform.SetParent(Container.transform);
        Log.Add(newItem);
        Refresh();
        return newItem;
    }

	// Use this for initialization
	void Awake () {
        CommonStack = this;
        Scroll.verticalNormalizedPosition = -0.2f;
    }

    private void Start()
    {
        Kanban.Board.OnNewAnomalObject += CreateNewAnomalObjectIcon;
        Kanban.Board.OnAnomalObjectMissed += CreateAnomalObjectMissedIcon;
        Kanban.Board.OnAnomalObjectMoved += CreateAnomalObjectMovedIcon;
        Kanban.Board.OnResearchesUpdated += CreateResearchesUpdatedIcon;
        Kanban.Board.OnContaintmentBreach += CreateContaintmentBreachIcon;
        Kanban.Board.OnContaintmentRestored += CreateContaintmentRestoredIcon;
        Kanban.Board.OnBuildingProcessStarted += CreateBuildingProcessStartedIcon;
        Kanban.Board.OnBuildingProcessIsOver += CreateBuildingProcessIsOverIcon;
        Kanban.Board.OnObjectStorageDestroyed += CreateObjectStorageDestroyedIcon;
        Kanban.Board.OnFirstIncedentReport += CreateFirstIncedentReportIcon;
        Kanban.Board.OnInvestigationStarted += CreateInvestigationStartedIcon;
        Kanban.Board.OnInvestigationStopped += CreateInvestigationStoppedIcon;
        Kanban.Board.OnCaptureOperationReadyToStart += CreateCaptureOperationReadyToStartIcon;
        Kanban.Board.OnCaptureOperationStarted += CreateCaptureOperationStartedIcon;
        Kanban.Board.OnCaptureOperationIsOver += CreateCaptureOperationIsOverIcon;
        Kanban.Board.OnAgentHired += CreateAgentHiredIcon;
        Kanban.Board.OnAgentDied +=  CreateAgentDiedIcon;
        Kanban.Board.OnOperativeHired += CreateOperativeHiredIcon;
        Kanban.Board.OnOperativeDied +=  CreateOperativeDiedIcon;
        Kanban.Board.OnScientistHired += CreateScientistHiredIcon;
        Kanban.Board.OnScientistDied +=  CreateScientistDiedIcon;
        Kanban.Board.OnDPersonnelHired += CreateDPersonnelHiredIcon;
        Kanban.Board.OnDPersonnelDied +=  CreateDPersonnelDiedIcon;
    }

    // Update is called once per frame
    void Update () {
		
	}
}