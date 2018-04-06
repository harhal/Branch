using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanUI : UICollabsiblePanel {

    public Image Photo;
    public Text Name;
    public Text Level;
    public Text Biography;
    public Image ProfessionIco;
    public Text ProfessionText;
    public Image ActivityIco;
    public Text ActivityText;
    public ProgressBar Experience;

    public Human human { get; private set; }
    bool InfoMode;

    public void SelectHuman(Human human, bool InfoMode)
    {
        this.human = human;
        this.InfoMode = InfoMode;
        Refresh();
    }

    public void Refresh()
    {
        if (Photo != null)
            Photo.sprite = human.Photo;
        if (Name != null)
            Name.text = human.Name;
        if (Level != null)
            Level.text = human.Level.ToString();
        if (Experience != null)
            Experience.Progress = human.Experience / GameData.Data.LevelsData.GetExperienceToNextLevel(human.Level);
        if (Biography != null)
            Biography.text = human.Biography;
        if (ProfessionIco != null)
            switch (human.Profession)
            {
                case Human.ProfessionType.Agent: { ProfessionIco.sprite = Resources.Load<Sprite>("Pictures/Ico/Agent"); break; }
                case Human.ProfessionType.Operative: { ProfessionIco.sprite = Resources.Load<Sprite>("Pictures/Ico/Operative"); break; }
                case Human.ProfessionType.Scientist: { ProfessionIco.sprite = Resources.Load<Sprite>("Pictures/Ico/Scientist"); break; }
                case Human.ProfessionType.D_Personnel: { ProfessionIco.sprite = Resources.Load<Sprite>("Pictures/Ico/DPersonnel"); break; }
            }
        if (ProfessionText != null)
            switch (human.Profession)
            {
                case Human.ProfessionType.Agent: { ProfessionText.text = "Agent"; break; }
                case Human.ProfessionType.Operative: { ProfessionText.text = "Operative"; break; }
                case Human.ProfessionType.Scientist: { ProfessionText.text = "Scientist"; break; }
                case Human.ProfessionType.D_Personnel: { ProfessionText.text = "D Personnel"; break; }
            }
        if (ActivityIco != null)
            switch (human.Activity)
            {
                case Human.ActivityType.Free: { ActivityIco.color = Color.green; break; }
                case Human.ActivityType.Going: { ActivityIco.color = Color.yellow; break; }
                case Human.ActivityType.Working: { ActivityIco.color = Color.red; break; }
            }
        if (ActivityText != null)
            switch (human.Activity)
            {
                case Human.ActivityType.Free: { ActivityText.text = "Free"; break; }
                case Human.ActivityType.Going: { ActivityText.text = "Going to work"/* + human.GetWorkSpaceName()*/; break; }
                case Human.ActivityType.Working: { ActivityText.text = "Working"/* + human.GetWorkSpaceName()*/; break; }
            }
    }

    public void Click()
    {
        if (InfoMode)
            PeopleUI.UI.ShowHumanInfo(human);
        else
            PeopleControlUI.UI.HumanClicked(this);
    }
}
