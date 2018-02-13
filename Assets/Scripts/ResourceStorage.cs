using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanResource
{
    public uint Alive { get; private set; }
    public uint Free { get; private set; }
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
}

public class ResourceStorage : MonoBehaviour {

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


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
