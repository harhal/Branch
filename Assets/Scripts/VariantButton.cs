using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class VariantButton
{
    public enum ButtonType { Close, ChoseAgent, ChoseOperatives, GetReward, SpendResources }

    public ButtonType Type;

    [System.Serializable]
    public struct ButtonConditionParams
    {
        public int Money;
        public int Agents;
        public int Operatives;
        public int D_Personnel;
    }

    [System.Serializable]
    public struct ButtonActionParams
    {
        public enum HumanType { Agent, Operative, Scientist, D_Personnel }
        public int Money;
        public float Reputation;
        public HumanType BonusRewardType;
        public Human BonusReward;
        public AnomalObject Reward;
        public float ProgressPoints;
    }

    public string Text;

    public ButtonConditionParams ActivityCondition;
    
    public ButtonActionParams Action;

    Dictionary<string, string> ParseString(string input)
    {
        var result = new Dictionary<string, string>();
        string[] splitedInput = input.Split('\n');
        foreach (string line in splitedInput)
        {
            string[] splitedLine = line.Split(' ');
            if (splitedLine.Length >= 2)
                result.Add(splitedLine[0], splitedLine[1]);
            else if (splitedLine.Length >= 1)
                result.Add(splitedLine[0], "");
        }
        return result;
    }

    public bool CheckActivity()
    {
        bool result = true;
        if (ResourceStorage.resourceStorage.Money < ActivityCondition.Money) result = false;
        if (ResourceStorage.resourceStorage.Agents.Free < ActivityCondition.Agents) result = false;
        if (ResourceStorage.resourceStorage.Operatives.Free < ActivityCondition.Operatives) result = false;
        if (ResourceStorage.resourceStorage.D_Personnel.Free < ActivityCondition.D_Personnel) result = false;
        return result;
    }

    public Button.ButtonClickedEvent OnClick()
    {
        var result = new Button.ButtonClickedEvent();
        switch (Type)
        {
            case ButtonType.Close:
                {
                    result.AddListener(ReportUI.CommonReport.Close);
                    break;
                }
            case ButtonType.SpendResources:
                {
                    break;
                }
            case ButtonType.GetReward:
                {
                    if (Action.BonusReward != null)
                        switch (Action.BonusRewardType)
                        {
                            case ButtonActionParams.HumanType.Agent:
                                {
                                    ResourceStorage.resourceStorage.Agents.Hire(Action.BonusReward);
                                    break;
                                }
                            case ButtonActionParams.HumanType.Operative:
                                {
                                    ResourceStorage.resourceStorage.Operatives.Hire(Action.BonusReward);
                                    break;
                                }
                            case ButtonActionParams.HumanType.Scientist:
                                {
                                    ResourceStorage.resourceStorage.Scientists.Hire(Action.BonusReward);
                                    break;
                                }
                            case ButtonActionParams.HumanType.D_Personnel:
                                {
                                    ResourceStorage.resourceStorage.D_Personnel.Hire(Action.BonusReward);
                                    break;
                                }
                        }
                    if (Action.Money > 0)
                        ResourceStorage.resourceStorage.AddMoney((uint)Action.Money);
                    if (Action.Reward != null)
                        ResourceStorage.resourceStorage.AddAnomalObject(Action.Reward);
                    result.AddListener(ReportUI.CommonReport.Close);
                    break;
                }
            case ButtonType.ChoseAgent:
                {
                    result.AddListener(ReportUI.CommonReport.ChoseAgent);
                    result.AddListener(ReportUI.CommonReport.Close);
                    break;
                }
            case ButtonType.ChoseOperatives:
                {
                    result.AddListener(ReportUI.CommonReport.ChoseOperatives);
                    result.AddListener(ReportUI.CommonReport.Close);
                    break;
                }
        }
        return result;
    }


}
