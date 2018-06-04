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
    [SerializeField]
    float Stability;

    [System.NonSerialized]
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
    [System.NonSerialized]
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
        for (int i = 0; i < ImpactFactors.FieldsCount; i++)
            SessionData.Data.ResourceStorage.Reputation += Properties[i] * ResearchPoints * 0.1f;
    }

    public bool IsStable()
    {
        return Stability > 0;
    }

    public void RestoreStability()
    {
        Stability = 1;
    }

    public void Update()
    {
        if (Stability > 0)
        {
            if (Storage == null)
                Stability -= (1 / GameData.Data.LevelsData.FreeAnomalObjectBreachTime) * Time.deltaTime;
            else
            {
                float UnprotectedProperties = (float)(Properties - Storage.Protection);
                if (UnprotectedProperties > 0)
                    Stability -= Mathf.Lerp(1 / GameData.Data.LevelsData.FreeAnomalObjectBreachTime,
                                            1 / GameData.Data.LevelsData.OnePointAnomalObjectBreachTime,
                                            (UnprotectedProperties - 1) / (ImpactFactors.FieldsCount * ImpactFactors.MaxValue - 1)) * Time.deltaTime;
                else
                    Stability -= (1 / GameData.Data.LevelsData.ProtectedAnomalObjectBreachTime) * Time.deltaTime;
            }
        }
        if (Stability <= 0 && Storage == null)
        {
            string Action = "missed";
            if (Properties.WillFactor > 3)
                Action = "escaped";
            Kanban.Board.AnomalObjectMissed(string.Format("Object {0} {1} {2} from entrepot", ID.ToString(), Name, Action), null);
            SessionData.Data.Warehouse.MissAnomalObject(ID);
            if (PlayerController.MainController.CurrentMode == PlayerController.ControlMode.AnomalObjectControl &&
                PlayerController.MainController.IsAnomalObjectSelected(this))
                PlayerController.MainController.SetDefaultMode();
        }
    }
}
