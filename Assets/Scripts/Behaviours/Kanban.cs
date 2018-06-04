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

    public void NewAnomalObject(string Description, object Invoker) { OnNewAnomalObject(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 15; }
    public void AnomalObjectMissed(string Description, object Invoker) { OnAnomalObjectMoved(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += -20; }
    public void AnomalObjectMoved(string Description, object Invoker) { OnAnomalObjectMoved(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void ResearchesUpdated(string Description, object Invoker) { OnResearchesUpdated(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 3; }
    public void ContaintmentBreach(string Description, object Invoker) { OnContaintmentBreach(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += -9; }
    public void ContaintmentRestored(string Description, object Invoker) { OnContaintmentRestored(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 6; }
    public void BuildingProcessStarted(string Description, object Invoker) { OnBuildingProcessStarted(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void BuildingProcessIsOver(string Description, object Invoker) { OnBuildingProcessIsOver(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void ObjectStorageDestroyed(string Description, object Invoker) { OnObjectStorageDestroyed(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += -10; }
    public void FirstIncedentReport(string Description, object Invoker) { OnFirstIncedentReport(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void InvestigationStarted(string Description, object Invoker) { OnInvestigationStarted(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void InvestigationStopped(string Description, object Invoker) { OnInvestigationStopped(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void CaptureOperationReadyToStart(string Description, object Invoker) { OnCaptureOperationReadyToStart(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void CaptureOperationStarted(string Description, object Invoker) { OnCaptureOperationStarted(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void CaptureOperationIsOver(string Description, object Invoker) { OnCaptureOperationIsOver(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 0; }
    public void AgentHired(string Description, object Invoker) { OnAgentHired(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 7; }
    public void AgentDied(string Description, object Invoker) { OnAgentDied(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += -7; }
    public void ScientistHired(string Description, object Invoker) { OnScientistHired(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 8; }
    public void ScientistDied(string Description, object Invoker) { OnScientistDied(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += -8; }
    public void OperativeHired(string Description, object Invoker) { OnOperativeHired(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 5; }
    public void OperativeDied(string Description, object Invoker) { OnOperativeDied(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += -5; }
    public void DPersonnelHired(string Description, object Invoker) { OnDPersonnelHired(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += 1; }
    public void DPersonnelDied(string Description, object Invoker) { OnDPersonnelDied(Description, Invoker); SessionData.Data.ResourceStorage.Reputation += -1; }
    public void Defeat(string Description) { OnDefeat(Description); SessionData.Data.ResourceStorage.Reputation += 0; }

    void Awake()
    {
        if (Board == null)
            Board = this;
    }
}
