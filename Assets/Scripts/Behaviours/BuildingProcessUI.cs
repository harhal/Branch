using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingProcessUI : UICollabsiblePanel {

    public Text ActionDiscription;
    public ProgressBar Progress;

    BuildingInProcess process;

    public static BuildingProcessUI UI;

    private void Awake()
    {
        if (UI == null)
            UI = this;
    }

    void Refresh()
    {
        if (process != null)
        {
            if (ActionDiscription != null)
                ActionDiscription.text = process.Description;
            if (Progress != null)
                Progress.Progress = process.GetProgress();
        }
    }
    
	void Start () {
        Hide();
    }
	
    public void SetProcess(BuildingInProcess process)
    {
        this.process = process;
        Show();
        Refresh();
    }

    void Update()
    {
        if (process != null)
        {
            if (Progress != null)
                Progress.Progress = process.GetProgress();
        }
    }
}
