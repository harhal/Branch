using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FieldOperation
{
    #region Variables
    public string Name;

    public float InvestigationTime;
    public float PreOperationTime;
    public float OperationTime;
    public float InvestigationPoints;
    public float InvestigetionBreakPoint;
    public float OperationPoints;
    public bool FieldAgentIsValid = false;
    public int FieldAgent;
    public List<int> Operatives;

    public float MinInvestigationPoints;
    public float GoodInvestigationPoints;

    public float MinOperationPoints;
    public float GoodOperationPoints;

    public StartInvestigationReport StartReport;
    public DecisionReport InvistigationReport;
    public StartOperationReport PreOperationReport;
    public InfoReport BadEndingReport;
    public InfoReport NormalEndingReport;
    public InfoReport GoodEndingReport;

    public float ReputationPenalty;
    
    public bool BonusRewardIsValid;
    public Human BonusReward;
    public bool RewardIsValid = false;
    public int Reward;

    public float AgentDeathChance;

    int stage = 0;
    #endregion

    internal void Initialize()
    {
        if (BonusRewardIsValid)
        {
            BonusReward = SessionData.Data.ResourceStorage.GetNewHuman(BonusReward.Sex, BonusReward.Profession, 0.1f);
        }
    }

    internal void InitAfterLoad()
    {
        if (FieldAgentIsValid)
            SessionData.Data.ResourceStorage.People[FieldAgent].Destination = this;
        foreach (var item in Operatives)
            SessionData.Data.ResourceStorage.People[item].Destination = this;
    }

    public void ChoseAgent()
    {
        PlayerController.MainController.ShowPeopleControl("Agent", SessionData.Data.ResourceStorage.FreeAgents, new List<Human>(), 1, SendAgent);
    }

    public void SendAgent(List<Human> agent)
    {
        if (agent.Count == 1)
        {
            PlayerController.MainController.CloseTopOverlayWindow();
            agent[0].Hire(this);
            this.FieldAgent = agent[0].ID;
            FieldAgentIsValid = true;
            StartReport.isVariable = false;
        }
    }

    public void ChoseOperatives()
    {
        PlayerController.MainController.ShowPeopleControl("Operatives", SessionData.Data.ResourceStorage.FreeOperatives, new List<Human>(), 7, SendOperatives);
    }

    public void SendOperatives(List<Human> operatives)
    {
        if (operatives.Count > 0)
        {
            foreach (var item in operatives)
            {
                item.Hire(this);
                Operatives.Add(item.ID);
            }
            StartOperation();
        }
    }

    public void ChangeProgress(float Change, string Description)
    {
        InvistigationReport.isVariable = false;
        InvistigationReport.Summary = "You choised: \"" + Description + '"';
        InvestigationPoints += Change;
    }

    public void StartOperation()
    {
        PlayerController.MainController.CloseTopOverlayWindow();
        PreOperationReport.isVariable = false;
        if (Operatives.Count > 0)
            PreOperationReport.Summary = "You sent " + Operatives.Count + " operatives to support Object capture";
        else
            PreOperationReport.Summary = "You didn't send operatives to support Object capture";
        Kanban.Board.CaptureOperationStarted("Capture operation in process", PreOperationReport);
        PreOperationTime = 0;
    }

    void TickInvistigation()
    {
        InvestigationTime -= Time.deltaTime;
        if (FieldAgentIsValid)
        {
            if (InvestigationPoints >= InvestigetionBreakPoint && stage == 1)
            {
                stage = 2;
                InvistigationReport.operation = this;
                string BonusHumanName = BonusReward != null ? BonusReward.Name : "None";
            string AgentName = SessionData.Data.ResourceStorage.People[FieldAgent].Name;
                InvistigationReport.Description = string.Format(InvistigationReport.Description, SessionData.Data.PlayerName, BonusHumanName, AgentName);
                Kanban.Board.InvestigationStopped("Investigation stopped", InvistigationReport);
            }
            InvestigationPoints += GameData.Data.LevelsData.GetAgentPointsAtLevel(SessionData.Data.ResourceStorage.People[FieldAgent].Level) * Time.deltaTime;
        }
        if (stage == 0)
        {
            stage = 1;
            StartReport.operation = this;
            string BonusHumanName = BonusReward != null ? BonusReward.Name : "None";
            StartReport.Description = string.Format(StartReport.Description, SessionData.Data.PlayerName, BonusHumanName);
            Kanban.Board.FirstIncedentReport("Anomal activity detected", StartReport);
        }
    }

    void TickPreOperation()
    {
        //StartReport.isVariable = false;
        if (InvistigationReport.isVariable)
        {
            InvistigationReport.Summary = "You didn't help agent";
            InvistigationReport.isVariable = false;
        }
        if (stage == 1 || stage == 2)
        {
            stage = 3;
            if (InvestigationPoints >= MinInvestigationPoints)
            {
                PreOperationReport.operation = this;
                string BonusHumanName = BonusReward != null ? BonusReward.Name : "None";
                string AgentName = SessionData.Data.ResourceStorage.People[FieldAgent].Name;
                PreOperationReport.Description = string.Format(PreOperationReport.Description, SessionData.Data.PlayerName, BonusHumanName, AgentName);
                Kanban.Board.CaptureOperationReadyToStart("Ready to start operation", PreOperationReport);
                if (InvestigationPoints >= GoodInvestigationPoints)
                    OperationPoints = MinOperationPoints;
            }
            else
                PreOperationTime = 0;
        }
        PreOperationTime -= Time.deltaTime;
    }

    void TickOperation()
    {
        if (StartReport.isVariable)
        {
            StartReport.Summary = "You ignored incedent";
            StartReport.isVariable = false;
        }
        //InvistigationReport.isVariable = false;
        //PreOperationReport.isVariable = false;
        if (Operatives.Count > 0)
            foreach (var item in Operatives)
                if (SessionData.Data.ResourceStorage.People[item].Activity != Human.ActivityType.Working) return;
        if (FieldAgentIsValid)
        {
            OperationPoints += GameData.Data.LevelsData.GetAgentPointsAtLevel(SessionData.Data.ResourceStorage.People[FieldAgent].Level) * Time.deltaTime;
        }
        if (Operatives != null)
        {
            foreach (int operative in Operatives)
                OperationPoints += GameData.Data.LevelsData.GetOperativePointsAtLevel(SessionData.Data.ResourceStorage.People[operative].Level) * Time.deltaTime;
        }
        OperationTime -= Time.deltaTime;
        if (OperationTime <= 0)
            OperationReport();
}

    void OperationReport()
    {
        string Necrology = "";
        foreach (var item in Operatives)
        {
            if (AgentDeathChance * GameData.Data.LevelsData.GetOperativeDeathChanceAtLevel(SessionData.Data.ResourceStorage.People[item].Level) > Random.value)
            {
                SessionData.Data.ResourceStorage.People[item].Kill();
                Necrology += SessionData.Data.ResourceStorage.People[item].Name + "died";
            }
            else
                SessionData.Data.ResourceStorage.People[item].Fire();
        }
        PreOperationReport.isVariable = false;
        if (OperationPoints < MinOperationPoints)
        {
            if (FieldAgentIsValid)
            {
                if (AgentDeathChance * GameData.Data.LevelsData.GetAgentDeathChanceAtLevel(SessionData.Data.ResourceStorage.People[FieldAgent].Level) > Random.value)
                {
                    SessionData.Data.ResourceStorage.People[FieldAgent].Kill();
                    Necrology += SessionData.Data.ResourceStorage.People[FieldAgent].Name + "died";
                }
                else
                    SessionData.Data.ResourceStorage.People[FieldAgent].Fire();
            }
            BadEndingReport.operation = this;
            BadEndingReport.isVariable = false;
            BadEndingReport.Summary = Necrology;
            string BonusHumanName = BonusReward != null ? BonusReward.Name : "None";
            BadEndingReport.Description = string.Format(BadEndingReport.Description, SessionData.Data.PlayerName, BonusHumanName);
            Kanban.Board.CaptureOperationIsOver("Mission failed", BadEndingReport);
            SessionData.Data.ResourceStorage.ChangeReputation(-ReputationPenalty);
        }
        else if (OperationPoints < GoodOperationPoints)
        {
            SessionData.Data.ResourceStorage.People[FieldAgent].Fire();
            NormalEndingReport.Summary = Necrology;
            if (RewardIsValid)
            {
                SessionData.Data.Warehouse.AddAnomalObject(Reward);
                NormalEndingReport.Summary += SessionData.Data.Warehouse.AnomalObjects[Reward].Name + " added";
                Kanban.Board.NewAnomalObject("New \"Object\" \"" + SessionData.Data.Warehouse.AnomalObjects[Reward].Name + "\"",
                    SessionData.Data.Warehouse.AnomalObjects[Reward]);
            }
            NormalEndingReport.operation = this;
            NormalEndingReport.isVariable = false;
            string BonusHumanName = BonusReward != null ? BonusReward.Name : "None";
            string AgentName = SessionData.Data.ResourceStorage.People[FieldAgent].Name;
            NormalEndingReport.Description = string.Format(NormalEndingReport.Description, SessionData.Data.PlayerName, BonusHumanName, AgentName);
            Kanban.Board.CaptureOperationIsOver("Mission completed", NormalEndingReport);
        }
        else
        {
            SessionData.Data.ResourceStorage.People[FieldAgent].Fire();
            GoodEndingReport.Summary = Necrology;
            if (RewardIsValid)
            {
                SessionData.Data.Warehouse.AddAnomalObject(Reward);
                GoodEndingReport.Summary += SessionData.Data.Warehouse.AnomalObjects[Reward].Name + " added\n";
                Kanban.Board.NewAnomalObject("New \"Object\" \"" + SessionData.Data.Warehouse.AnomalObjects[Reward].Name + "\"",
                    SessionData.Data.Warehouse.AnomalObjects[Reward]);
            }
            GoodEndingReport.operation = this;
            GoodEndingReport.isVariable = false;
            string BonusHumanName = BonusReward != null ? BonusReward.Name : "None";
            string AgentName = SessionData.Data.ResourceStorage.People[FieldAgent].Name;
            GoodEndingReport.Description = string.Format(GoodEndingReport.Description, SessionData.Data.PlayerName, BonusHumanName, AgentName);
            Kanban.Board.CaptureOperationIsOver("Mission completed", GoodEndingReport);
            if (BonusReward != null)
            {
                BonusReward.Register();
                switch (BonusReward.Profession)
                {
                    case Human.ProfessionType.Agent:
                        {
                            GoodEndingReport.Summary += BonusReward.Name + " joined as new agent\n";
                            break;
                        }
                    case Human.ProfessionType.Operative:
                        {
                            GoodEndingReport.Summary += BonusReward.Name + " joined as new operatine\n";
                            break;
                        }
                    case Human.ProfessionType.Scientist:
                        {
                            GoodEndingReport.Summary += BonusReward.Name + " joined as new sciendist\n";
                            break;
                        }
                    case Human.ProfessionType.D_Personnel:
                        {
                            GoodEndingReport.Summary += BonusReward.Name + " arrested as new D personnel\n";
                            break;
                        }
                }
            }
        }
    }
    
    public void Update () {
        if (InvestigationTime > 0)
        {
            TickInvistigation();
        }
        else if (PreOperationTime > 0)
        {
            TickPreOperation();
        }
        else if (OperationTime > 0)
        {
            TickOperation();
        }
    }
}

[System.Serializable]
public class StartInvestigationReport : Report
{
    public override VariantButton[] GetChoises()
    {
        if (cashedChoises == null)
            cashedChoises = new VariantButton[] { new ChoseAgentButton("Send agent"), new CloseButton("Close") };
        return cashedChoises;
    }
}

[System.Serializable]
public class DecisionReport : Report
{
    [SerializeField]
    OperationActionButton[] DecisionChoises;

    public override VariantButton[] GetChoises()
    {
        if (cashedChoises == null)
        {
            if (DecisionChoises != null)
            {
                cashedChoises = new VariantButton[DecisionChoises.Length];

                for (int i = 0; i < DecisionChoises.Length; i++)
                    cashedChoises[i] = DecisionChoises[i];
            }
            else
                cashedChoises = new VariantButton[0];
        }
        return cashedChoises;
    }
}

[System.Serializable]
public class StartOperationReport : Report
{
    public override VariantButton[] GetChoises()
    {
        if (cashedChoises == null)
            cashedChoises = new VariantButton[] 
            {
                new ChoseOperativesButton("Send operatives"),
                new CustomButton("Start capture operation", delegate() 
                {
                    ReportUI.UI.CurrentReport.operation.StartOperation();
                }),
                new CloseButton("Close")
            };
        return cashedChoises;
    }
}