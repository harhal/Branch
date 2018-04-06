using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeopleUI : OverlayWindow {

    public static PeopleUI UI;
    
    public Toggle FreeFilter;
    public Toggle AgentsFilter;
    public Toggle OperativesFilter;
    public Toggle ScientistsFilter;
    public Toggle DPersonnelFilter;
    public VerticalLayoutGroup MainHumanList;
    public HumanUI HumanInfo;

    public HumanUI HumanShortInfoPrefub;
    
    List<HumanUI> HumanUIList;

    public void ShowHumanInfo(Human human)
    {
        HumanInfo.Show();
        HumanInfo.SelectHuman(human, true);
    }

    public void ShowPeople(Human.ProfessionType? ProfessionFilter)
    {
        AgentsFilter.isOn = ProfessionFilter.Value == Human.ProfessionType.Agent;
        OperativesFilter.isOn = ProfessionFilter.Value == Human.ProfessionType.Operative;
        ScientistsFilter.isOn = ProfessionFilter.Value == Human.ProfessionType.Scientist;
        DPersonnelFilter.isOn = ProfessionFilter.Value == Human.ProfessionType.D_Personnel;
        this.FreeFilter.isOn = false;
        Refresh();
        HumanInfo.gameObject.SetActive(false);
    }

    bool IsProfessionChecked(Human.ProfessionType prof)
    {
        switch (prof)
        {
            case Human.ProfessionType.Agent: return AgentsFilter.isOn;
            case Human.ProfessionType.Operative: return OperativesFilter.isOn;
            case Human.ProfessionType.Scientist: return ScientistsFilter.isOn;
            case Human.ProfessionType.D_Personnel: return DPersonnelFilter.isOn;
        }
        return false;
    }

    public void Refresh()
    {
        var human = SessionData.Data.ResourceStorage.People.GetEnumerator();
        for (int i = 0; i < SessionData.Data.ResourceStorage.People.Count; i++)
        {
            human.MoveNext();
            HumanUI NewHumanUI;
            if (i >= HumanUIList.Count)
            {
                NewHumanUI = GameObject.Instantiate(HumanShortInfoPrefub);
                NewHumanUI.transform.SetParent(MainHumanList.transform, false);
                HumanUIList.Add(NewHumanUI);
            }
            else
                NewHumanUI = HumanUIList[i];
            NewHumanUI.SelectHuman(human.Current.Value, true);
            if (IsProfessionChecked(human.Current.Value.Profession) &&
                (FreeFilter.isOn ? human.Current.Value.Activity != Human.ActivityType.Working : true))
                NewHumanUI.Show();
            else
                NewHumanUI.Hide();
        }
        for (int i = SessionData.Data.ResourceStorage.People.Count; i < HumanUIList.Count; i++)
        {
            HumanUIList[i].Hide();
        }
    }
    
    void Awake () {
        if (UI == null)
            UI = this;
        Hide();
        HumanUIList = new List<HumanUI>();
	}
}
