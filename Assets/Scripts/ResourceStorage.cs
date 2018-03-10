using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStorage : MonoBehaviour {

    public static ResourceStorage resourceStorage;

    [System.Serializable]
    public struct HumanResource
    {
        List<Human> _list;
        public List<Human> List { get { if (_list == null) _list = new List<Human>(); return _list; } private set { _list = value; }  }
        [SerializeField]
        uint _alive;
        public uint Alive { get { return _alive; } private set { _alive = value; } }
        [SerializeField]
        uint _free;
        public uint Free { get { return _free; } private set { _free = value; } }

        public void Send(Human human)
        {
            if (Free >= 1 && List.Contains(human))
            {
                Free -= 1;
                human.isFree = false;
            }
        }

        public Human Send()
        {
            if (Free >= 1)
            {
                Free -= 1;
                int i = 0;
                while (!List[i].isFree) i++;
                List[i].isFree = false;
                return List[i];
            }
            return null;
        }

        public void Hire(Human human)
        {
            Alive += 1;
            Free += 1;
            List.Add(human);
            human.isFree = true;
        }

        public bool Dead(Human human) //Use only for human on mission
        {
            if (Alive >= 1 && List.Contains(human))
            {
                Alive -= 1;
                List.Remove(human);
                return true;
            }
            else
                return false;
        }

        public void Return(Human human)
        {
            if (List.Contains(human))
            {
                Free += 1;
                human.isFree = true;
            }
        }

        public override string ToString()
        {
            return Free.ToString() + " / " + Alive.ToString();
        }
    }

    [SerializeField]
    private uint money;

    public uint Money { get { return money; } private set { money = value; } }

    public bool SpendMoney(uint Amount)
    {
        if (Amount <= Money)
        {
            Money -= Amount;
            return true;
        }
        return false;
    }

    public void AddMoney(uint Amount)
    {
        Money += Amount;
    }

    public void AddAnomalObject(AnomalObject anomalObject)
    {
        anomalObjects.Add(anomalObject);
        entrepotUI.RefreshData();
    }

    public void CreateAnomalObject(AnomalObject prototype)
    {
        var newObj = GameObject.Instantiate<AnomalObject>(prototype);
        newObj.ID = (int)Random.Range(100, 500);
        AddAnomalObject(newObj);
    }

    public HumanResource Agents;
    public HumanResource Scientists;
    public HumanResource Operatives;
    public HumanResource D_Personnel;
    public List<AnomalObject> anomalObjects;
    public float Reputation;

    public Text MoneyOutput;
    public Text AgentsOutput;
    public Text ScientistsOutput;
    public Text OperativesOutput;
    public Text D_PersonnelOutput;
    public EntrepotUI entrepotUI;

    public void Awake()
    {
        resourceStorage = this;
    }

    // Use this for initialization
    void Start () {
        anomalObjects = new List<AnomalObject>();
        /*GameObject finded = GameObject.Find("MoneyOutput");
        if (finded != null)
            MoneyOutput = finded.GetComponent<Text>();
        finded = GameObject.Find("ScientistsOutput");
        if (finded != null)
            ScientistsOutput = finded.GetComponent<Text>();
        finded = GameObject.Find("OperativesOutput");
        if (finded != null)
            OperativesOutput = finded.GetComponent<Text>();
        finded = GameObject.Find("D_PersonnelOutput");
        if (finded != null)
            D_PersonnelOutput = finded.GetComponent<Text>();
        finded = GameObject.Find("EntrepotUI");
        if (finded != null)
            entrepotUI = finded.GetComponent<EntrepotUI>();*/
    }
	
	// Update is called once per frame
	void Update () {
        if (MoneyOutput != null)
            MoneyOutput.text = Money.ToString();
        if (AgentsOutput != null)
            AgentsOutput.text = Agents.ToString();
        if (ScientistsOutput != null)
            ScientistsOutput.text = Scientists.ToString();
        if (OperativesOutput != null)
            OperativesOutput.text = Operatives.ToString();
        if (D_PersonnelOutput != null)
            D_PersonnelOutput.text = D_Personnel.ToString();
    }
}