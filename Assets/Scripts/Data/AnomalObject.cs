using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResearchRepot
{
    public float ProgressPoints;
    [TextArea]
    public string Report;
}

[System.Serializable]
public class AnomalObject
{
    public int ID;
    public string Name;
    [SerializeField]
    string image;
    public float FullResearchPoints;
    public float CurrentResearchPoints;
    [TextArea]
    public string Description;
    public ResearchRepot[] Reports;
    int currentReport = 0;
    public ImpactFactors Properties;
    [SerializeField]
    bool storageIsValid = false;
    [SerializeField]
    int storage;

    [NonSerialized]
    Sprite _image;
    public Sprite Image
    {
        get
        {
            if (_image == null)
                _image = Resources.Load<Sprite>(image);
            return _image;
        }
    }
    public float Progress { get { return Mathf.Clamp(CurrentResearchPoints / FullResearchPoints, 0, 1); } }
    [NonSerialized]
    ObjectStorage _storage;
    public ObjectStorage Storage
    {
        get
        {
            if (_storage == null && storageIsValid)
                _storage = SessionData.Data.BranchBase.BuildingsDB[storage];
            else if (_storage != null)
                if (!storageIsValid)
                    _storage = null;
                else if (_storage.ID != storage)
                    _storage = SessionData.Data.BranchBase.BuildingsDB[storage];
            return _storage;
        }
        set
        {
            if (value != null)
            {
                storage = value.ID;
                _storage = SessionData.Data.BranchBase.BuildingsDB[value.ID];
                storageIsValid = true;
            }
            else
            {
                storageIsValid = false;
                _storage = null;
            }
        }
    }

    //public delegate void Message();
    //public event Message ResearchIsOver;
    //public event Message ResearchUpdated;

    public AnomalObject(int ID, string Name, string Image, float FullResearchPoints, string Description, ResearchRepot[] Reports, ImpactFactors Properties)
    {
        this.ID = ID;
        this.Name = Name;
        this.image = Image;
        this.FullResearchPoints = FullResearchPoints;
        this.Description = Description;
        this.Reports = Reports;
        this.Properties = Properties;
    }

    public void Research(float ResearchPoints)
    {
        CurrentResearchPoints += ResearchPoints;
        if (currentReport < Reports.Length)
        {
            if (CurrentResearchPoints >= Reports[currentReport].ProgressPoints)
            {
                Description += '\n' + Reports[currentReport].Report;
                Kanban.Board.ResearchesUpdated(Reports[currentReport].Report, this);
                currentReport++;
            }
        }
        SessionData.Data.Researches.Research(Properties * ResearchPoints, this);
    }
}
