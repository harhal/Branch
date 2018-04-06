using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeopleControlUI : OverlayWindow {

    public static PeopleControlUI UI;

    public Text MainLabel;
    public VerticalLayoutGroup MainHumanContainer;
    public VerticalLayoutGroup HiredHumanContainer;

    public delegate void ConfirmDelegate(List<Human> HiredHumans);
    PeopleControlUI.ConfirmDelegate OnConfirm;

    public HumanUI HumanShortInfoPrefub;

    List<Human> FreeHumanList;
    List<Human> HiredHumanList;
    List<HumanUI> FreeHumanUIList;
    List<HumanUI> HiredHumanUIList;
    int MaxHiredCount;

    public void ShowPeople(string LabelText, List<Human> FreeHumanList, List<Human> HiredHumanList, int MaxHiredCount, PeopleControlUI.ConfirmDelegate OnConfirm)
    {
        MainLabel.text = LabelText;
        this.MaxHiredCount = MaxHiredCount;
        this.OnConfirm = OnConfirm;
        this.FreeHumanList = new List<Human>();
        this.FreeHumanList.AddRange(FreeHumanList.ToArray());
        this.HiredHumanList = new List<Human>();
        this.HiredHumanList.AddRange(HiredHumanList.ToArray());
        Refresh();
    }

    public void ShowPeople(string LabelText, List<Human> FreeHumanList, List<int> HiredHumanList, int MaxHiredCount, PeopleControlUI.ConfirmDelegate OnConfirm)
    {
        MainLabel.text = LabelText;
        this.MaxHiredCount = MaxHiredCount;
        this.OnConfirm = OnConfirm;
        this.FreeHumanList = new List<Human>();
        this.FreeHumanList.AddRange(FreeHumanList.ToArray());
        this.HiredHumanList = new List<Human>();
        foreach (var item in HiredHumanList)
            this.HiredHumanList.Add(SessionData.Data.ResourceStorage.People[item]);
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < FreeHumanList.Count; i++)
        {
            HumanUI NewHumanUI;
            if (i >= FreeHumanUIList.Count)
            {
                NewHumanUI = GameObject.Instantiate(HumanShortInfoPrefub);
                NewHumanUI.transform.SetParent(MainHumanContainer.transform, false);
                FreeHumanUIList.Add(NewHumanUI);
            }
            else
                NewHumanUI = FreeHumanUIList[i];
            NewHumanUI.gameObject.SetActive(true);
            NewHumanUI.SelectHuman(FreeHumanList[i], false);
        }
        for (int i = FreeHumanList.Count; i < FreeHumanUIList.Count; i++)
        {
            FreeHumanUIList[i].Hide();
        }

        for (int i = 0; i < HiredHumanList.Count; i++)
        {
            HumanUI NewHumanUI;
            if (i >= HiredHumanUIList.Count)
            {
                NewHumanUI = GameObject.Instantiate(HumanShortInfoPrefub);
                NewHumanUI.transform.SetParent(HiredHumanContainer.transform, false);
                HiredHumanUIList.Add(NewHumanUI);
            }
            else
                NewHumanUI = HiredHumanUIList[i];
            NewHumanUI.gameObject.SetActive(true);
            NewHumanUI.SelectHuman(HiredHumanList[i], false);
        }
        for (int i = HiredHumanList.Count; i < HiredHumanUIList.Count; i++)
        {
            HiredHumanUIList[i].Hide();
        }
    }

    public void OnConfirmClick()
    {
        if (OnConfirm != null)
        {
            OnConfirm(HiredHumanList);
            OnConfirm = null;
        }
        PlayerController.MainController.CloseTopOverlayWindow();
    }

    public void HumanClicked(HumanUI humanUI)
    {
        if (FreeHumanList.Contains(humanUI.human))
        {
            if (HiredHumanList.Count < MaxHiredCount)
            {
                FreeHumanList.Remove(humanUI.human);
                FreeHumanUIList.Remove(humanUI);
                HiredHumanList.Add(humanUI.human);
                HiredHumanUIList.Add(humanUI);
                humanUI.transform.SetParent(HiredHumanContainer.transform, false);
            }
        }
        else
        {
            FreeHumanList.Add(humanUI.human);
            FreeHumanUIList.Add(humanUI);
            HiredHumanList.Remove(humanUI.human);
            HiredHumanUIList.Remove(humanUI);
            humanUI.transform.SetParent(MainHumanContainer.transform, false);
        }

    }

    void Awake () {
        if (UI == null)
            UI = this;
        Hide();
        FreeHumanUIList = new List<HumanUI>();
        HiredHumanUIList = new List<HumanUI>();
	}
}
