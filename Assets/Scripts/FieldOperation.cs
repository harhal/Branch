using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOperation : MonoBehaviour {

    public float InvestigationTime;
    public float OperationTime;
    public float InvestigationPoints;
    public float OperationPoints;
    public Human FieldAgent;
    public Human[] Operatives;

    public float MinInvestigationPoints;
    public float GoodInvestigationPoints;

    public float MinOperationPoints;
    public float GoodOperationPoints;

    public Report StartReport;
    public Report InvistigationReport;
    public Report PreOperationReport;
    public Report BadEndingReport;
    public Report NormalEndingReport;
    public Report GoodEndingReport;

    int stage = 0;

    // Use this for initialization
    void Start () {
		
	}

    private void OnEnable()
    {
        print("First incedent");
    }

    public void GetAgent(Human agent)
    {
        FieldAgent = agent;
    }

    public void WriteReport(Report report)
    {
        report.operation = this;
        EventStack.CommonStack.AddItem(report);
    }

    void TickInvistigation()
    {
        InvestigationTime -= Time.deltaTime;
        if (FieldAgent != null)
            InvestigationPoints += (FieldAgent.Level + 20) * Time.deltaTime;
        if (stage == 0)
        {
            stage++;
            stage++; //To delete after add InvistigationReport
            WriteReport(StartReport);
        }
    }

    void TickOperation()
    {
        if (stage == 2)
        {
            stage++;
            if (InvestigationPoints >= MinInvestigationPoints)
            {
                WriteReport(PreOperationReport);
                if (InvestigationPoints >= GoodInvestigationPoints)
                    OperationPoints = MinOperationPoints;
            }
            else
                OperationTime = 0;
        }
        OperationTime -= Time.deltaTime;
    }

    void OperationReport()
    {
        if (OperationPoints < MinOperationPoints)
        {
            WriteReport(BadEndingReport);
        }
        else if (OperationPoints < GoodOperationPoints)
        {
            WriteReport(NormalEndingReport);
        }
        else
        {
            WriteReport(GoodEndingReport);
        }
        enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (InvestigationTime > 0)
        {
            TickInvistigation();
        }
        else if (OperationTime > 0)
        {
            TickOperation();
        }
        else OperationReport();
    }
}
