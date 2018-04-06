using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class VariantButton
{
    public string Text;

    public VariantButton(string Text)
    {
        this.Text = Text;
    }

    public virtual bool CheckActivity()
    {
        return true;
    }

    public virtual void OnClick()
    {
    }
}

[System.Serializable]
public class CustomButton : VariantButton
{
    public UnityAction onClick;

    public CustomButton(string Text, UnityAction onClick) : base (Text)
    {
        this.onClick = onClick;
    }

    public override void OnClick()
    {
        onClick();
    }
}

[System.Serializable]
public class CloseButton : VariantButton
{
    public CloseButton(string Text) : base(Text)
    {
    }

    public override void OnClick()
    {
        PlayerController.MainController.CloseTopOverlayWindow();

    }
}

[System.Serializable]
public class ChoseAgentButton : VariantButton
{
    public ChoseAgentButton(string Text) : base(Text)
    {
    }

    public override bool CheckActivity()
    {
        return SessionData.Data.ResourceStorage.FreeAgents.Count > 0;
    }

    public override void OnClick()
    {
        ReportUI.UI.CurrentReport.operation.ChoseAgent();
    }
}

[System.Serializable]
public class ChoseOperativesButton : VariantButton
{
    public ChoseOperativesButton(string Text) : base(Text)
    {
    }

    public override bool CheckActivity()
    {
        return SessionData.Data.ResourceStorage.FreeOperatives.Count > 0;
    }

    public override void OnClick()
    {
        ReportUI.UI.CurrentReport.operation.ChoseOperatives();
    }
}

[System.Serializable]
public class OperationActionButton : VariantButton
{
    public int Money;
    public float Reputation;
    public float ProgressPoints;

    public OperationActionButton(string Text, int Money, float Reputation, float ProgressPoints) : base(Text)
    {
        this.Money = Money;
        this.Reputation = Reputation;
        this.ProgressPoints = ProgressPoints;
    }

    public override bool CheckActivity()
    {
        return SessionData.Data.ResourceStorage.Money >= Money;
    }

    public override void OnClick()
    {
        if (Money < 0)
            SessionData.Data.ResourceStorage.SpendMoney((int)-Money);
        else
            SessionData.Data.ResourceStorage.AddMoney((int)Money);
        SessionData.Data.ResourceStorage.ChangeReputation(Reputation);
        ReportUI.UI.CurrentReport.operation.ChangeProgress(ProgressPoints, Text);
        PlayerController.MainController.CloseTopOverlayWindow();
    }
}