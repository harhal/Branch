using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kanban : MonoBehaviour
{

    public delegate void Action(string Description);
    public delegate void InvokedAction(string Description, object Invoker);

    public static Kanban Board;

    //AnomalObject
    public event InvokedAction OnNewAnomalObject;
    public event InvokedAction OnAnomalObjectMissed;
    public event InvokedAction OnAnomalObjectMoved;
    public event InvokedAction OnResearchesUpdated;
    public event InvokedAction OnContaintmentBreach;
    public event InvokedAction OnContaintmentRestored;
    //BuildingInProcess
    public event InvokedAction OnBuildingProcessStarted;
    public event InvokedAction OnBuildingProcessIsOver;
    //ObjectStorage
    public event InvokedAction OnObjectStorageDestroyed;
    //FieldOperation
    public event InvokedAction OnFirstIncedentReport;
    public event InvokedAction OnInvestigationStarted;
    public event InvokedAction OnInvestigationStopped;
    public event InvokedAction OnCaptureOperationReadyToStart;
    public event InvokedAction OnCaptureOperationStarted;
    public event InvokedAction OnCaptureOperationIsOver;
    //ResourceStorage
    public event InvokedAction OnAgentHired;
    public event InvokedAction OnAgentDied;
    public event InvokedAction OnScientistHired;
    public event InvokedAction OnScientistDied;
    public event InvokedAction OnOperativeHired;
    public event InvokedAction OnOperativeDied;
    public event InvokedAction OnDPersonnelHired;
    public event InvokedAction OnDPersonnelDied;
    //General
    public event Action OnDefeat;

    public void NewAnomalObject(string Description, object Invoker) { OnNewAnomalObject(Description, Invoker); }
    public void AnomalObjectMissed(string Description, object Invoker) { OnAnomalObjectMoved(Description, Invoker); }
    public void AnomalObjectMoved(string Description, object Invoker) { OnAnomalObjectMoved(Description, Invoker); }
    public void ResearchesUpdated(string Description, object Invoker) { OnResearchesUpdated(Description, Invoker); }
    public void ContaintmentBreach(string Description, object Invoker) { OnContaintmentBreach(Description, Invoker); }
    public void ContaintmentRestored(string Description, object Invoker) { OnContaintmentRestored(Description, Invoker); }
    public void BuildingProcessStarted(string Description, object Invoker) { OnBuildingProcessStarted(Description, Invoker); }
    public void BuildingProcessIsOver(string Description, object Invoker) { OnBuildingProcessIsOver(Description, Invoker); }
    public void ObjectStorageDestroyed(string Description, object Invoker) { OnObjectStorageDestroyed(Description, Invoker); }
    public void FirstIncedentReport(string Description, object Invoker) { OnFirstIncedentReport(Description, Invoker); }
    public void InvestigationStarted(string Description, object Invoker) { OnInvestigationStarted(Description, Invoker); }
    public void InvestigationStopped(string Description, object Invoker) { OnInvestigationStopped(Description, Invoker); }
    public void CaptureOperationReadyToStart(string Description, object Invoker) { OnCaptureOperationReadyToStart(Description, Invoker); }
    public void CaptureOperationStarted(string Description, object Invoker) { OnCaptureOperationStarted(Description, Invoker); }
    public void CaptureOperationIsOver(string Description, object Invoker) { OnCaptureOperationIsOver(Description, Invoker); }
    public void AgentHired(string Description, object Invoker) { OnAgentHired(Description, Invoker); }
    public void AgentDied(string Description, object Invoker) { OnAgentDied(Description, Invoker); }
    public void ScientistHired(string Description, object Invoker) { OnScientistHired(Description, Invoker); }
    public void ScientistDied(string Description, object Invoker) { OnScientistDied(Description, Invoker); }
    public void OperativeHired(string Description, object Invoker) { OnOperativeHired(Description, Invoker); }
    public void OperativeDied(string Description, object Invoker) { OnOperativeDied(Description, Invoker); }
    public void DPersonnelHired(string Description, object Invoker) { OnDPersonnelHired(Description, Invoker); }
    public void DPersonnelDied(string Description, object Invoker) { OnDPersonnelDied(Description, Invoker); }
    public void Defeat(string Description) { OnDefeat(Description); }

    void Awake()
    {
        if (Board == null)
            Board = this;
    }
}
