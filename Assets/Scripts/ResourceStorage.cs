using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStorage : MonoBehaviour {

    [System.Serializable]
    public struct HumanResource
    {
        [SerializeField]
        uint _alive;
        public uint Alive { get { return _alive; } private set { _alive = value; } }
        [SerializeField]
        uint _free;
        public uint Free { get { return _free; } private set { _free = value; } }
        public uint Max;

        public bool Send(uint count)
        {
            if (Free >= count)
            {
                Free -= count;
                return true;
            }
            else
                return false;
        }

        public bool Hire(uint count)
        {
            if (Alive + count <= Max)
            {
                Alive += count;
                Free += count;
                return true;
            }
            else
            {
                Free += Max - Alive;
                Alive = Max;
                return false;
            }
        }

        public bool Dead(uint count) //Use only for human on mission
        {
            if (Alive >= count)
            {
                Alive -= count;
                return true;
            }
            else
                return false;
        }

        public void Return(uint count)
        {
            Free += count;
        }

        public override string ToString()
        {
            return Free.ToString() + " / " + Alive.ToString() + " (" + Max.ToString() + ")";
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

    public HumanResource Scientists;
    public HumanResource Operatives;
    public HumanResource D_Personnel;
    private List<ObjectStorage> storages;

    Text MoneyOutput;
    Text ScientistsOutput;
    Text OperativesOutput;
    Text D_PersonnelOutput;

    // Use this for initialization
    void Start () {
        GameObject finded = GameObject.Find("MoneyOutput");
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
    }
	
	// Update is called once per frame
	void Update () {
        if (MoneyOutput != null)
            MoneyOutput.text = Money.ToString();
        if (ScientistsOutput != null)
            ScientistsOutput.text = Scientists.ToString();
        if (OperativesOutput != null)
            OperativesOutput.text = Operatives.ToString();
        if (D_PersonnelOutput != null)
            D_PersonnelOutput.text = D_Personnel.ToString();
    }
}