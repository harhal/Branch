using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]
public struct HumanResource
{
    [SerializeField]
    Human[] humans;
    [SerializeField]
    int _alive;
    [SerializeField]
    int _free;

    //Dictionary<int, Human> _humans;
    public Dictionary<int, Human> Humans;
    /*{
        get
        {
            if (_humans == null)
            {
                _humans = new Dictionary<int, Human>();
                if (humans != null)
                    foreach (var item in humans)
                        _humans.Add(item.ID, item);
            }
            return _humans;
        }
        /*private set
        {
            _humans = value;
        }
    }
    public int Alive { get { return _alive; } private set { _alive = value; } }
    public int Free { get { return _free; } private set { _free = value; } }

    public Human this[int key]
    {
        get
        {
            return Humans[key];
        }
    }

    internal void PrepareToSave()
    {
        humans = new Human[Humans.Count];
        int i = 0;
        foreach (var item in Humans)
        {
            humans[i] = item.Value;
            i++;
        }
    }

    internal void InitAfterLoad()
    {
        Humans = new Dictionary<int, Human>();
        foreach (var item in humans)
        {
            Human human = item;
            Humans.Add(human.ID, human);
        }
    }

    public void Send(Human human)
    {
        if (Free >= 1 && Humans.ContainsValue(human))
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
            foreach (var item in Humans)
            {
                if (item.Value.isFree)
                {
                    item.Value.isFree = false;
                    return item.Value;
                }
            }
        }
        return null;
    }

    public void Hire(Human human)
    {
        Alive += 1;
        Free += 1;
        Humans.Add(human.ID = SessionData.Data.ResourceStorage.GetNewHumanID(), human);
        if (human != null)
            human.isFree = true;
    }

    public bool Dead(Human human) //Use only for human on mission
    {
        if (Alive >= 1 && Humans.ContainsValue(human))
        {
            Alive -= 1;
            Humans.Remove(human.ID);
            return true;
        }
        else
            return false;
    }

    public void Return(Human human)
    {
        if (Humans.ContainsValue(human))
        {
            Free += 1;
            human.isFree = true;
        }
    }

    public override string ToString()
    {
        return Free.ToString() + " / " + Alive.ToString();
    }
}*/

[System.Serializable]
public class ResourceStorage
{
    public int Money;
    [SerializeField] int LastHumanID = 526;
    [SerializeField] Human[] people;
    [SerializeField] int[] UsedPregeneratedMen;
    [SerializeField] int[] UsedPregeneratedWomen;
    public float Reputation;
    public float MaxReputation = 100;

    public int AgentsCount { get; private set; }
    public int OperativesCount { get; private set; }
    public int ScientistsCount { get; private set; }
    public int D_PersonnelCount { get; private set; }
    public List<Human> FreeAgents { get; private set; }
    public List<Human> FreeScientists { get; private set; }
    public List<Human> FreeOperatives { get; private set; }
    public List<Human> FreeD_Personnel { get; private set; }
    [System.NonSerialized] public Dictionary<int, Human> People;
    [System.NonSerialized] HashSet<int> FreePregeneratedMen;
    [System.NonSerialized] HashSet<int> FreePregeneratedWomen;


    public ResourceStorage()
    {
        People = new Dictionary<int, Human>();
        FreeAgents =      new List<Human>();
        FreeScientists =  new List<Human>();
        FreeOperatives =  new List<Human>();
        FreeD_Personnel = new List<Human>();
    }

    //Money control
    public bool SpendMoney(int Amount)
    {
        if (Amount <= Money)
        {
            Money -= Amount;
            return true;
        }
        return false;
    }

    public void AddMoney(int Amount)
    {
        Money += Amount;
    }
    /////////////////////////////

    //Reputation control
    public void ChangeReputation(float Amount)
    {
        Reputation += Amount;
        if (Reputation > MaxReputation)
            Reputation = MaxReputation;
        if (Reputation <= 0)
        {
            Reputation = 0;
            Kanban.Board.Defeat("You loose");
        }
    }
    /////////////////////////////

    //Humans control
    public int GetNewHumanID()
    {
        return (LastHumanID += (int)Random.Range(1, 10));
    }

    public Human GetNewHuman(Human.SexType Sex, Human.ProfessionType Profession, float SpecialChance)
    {
        if (Sex == Human.SexType.NoMatter)
            Sex = Random.Range(0, 2) == 0 ? Human.SexType.Male : Human.SexType.Female;
        HashSet<int> FreeSpecial;
        if (Sex == Human.SexType.Male)
            FreeSpecial = FreePregeneratedMen;
        else
            FreeSpecial = FreePregeneratedWomen;
        if (Random.value <= SpecialChance && FreeSpecial.Count > 0)
        {
            var Enum = FreeSpecial.GetEnumerator();
            int elemPos = Random.Range(0, FreeSpecial.Count);
            for (int i = 0; i < elemPos; i++)
                Enum.MoveNext();
            FreeSpecial.Add(Enum.Current);
            if (Sex == Human.SexType.Male)
                return GameData.Data.HumanGenerator.PreGeneratedMen[Enum.Current];
            else
                return GameData.Data.HumanGenerator.PreGeneratedWomen[Enum.Current];
        }
        else
            return GameData.Data.HumanGenerator.GenerateHuman(Sex);
    }

    public void Hire(Human newHuman)
    {
        newHuman.ID = GetNewHumanID();
        People.Add(newHuman.ID, newHuman);
        switch (newHuman.Profession)
        {
            case Human.ProfessionType.Agent:
                {
                    FreeAgents.Add(newHuman);
                    AgentsCount++;
                    break;
                }
            case Human.ProfessionType.Operative:
                {
                    FreeOperatives.Add(newHuman);
                    OperativesCount++;
                    break;
                }
            case Human.ProfessionType.Scientist:
                {
                    FreeScientists.Add(newHuman);
                    ScientistsCount++;
                    break;
                }
            case Human.ProfessionType.D_Personnel:
                {
                    FreeD_Personnel.Add(newHuman);
                    D_PersonnelCount++;
                    break;
                }
        }
    }

    public void Send(Human human)
    {
        switch (human.Profession)
        {
            case Human.ProfessionType.Agent:
                {
                    FreeAgents.Remove(human);
                    break;
                }
            case Human.ProfessionType.Operative:
                {
                    FreeOperatives.Remove(human);
                    break;
                }
            case Human.ProfessionType.Scientist:
                {
                    FreeScientists.Remove(human);
                    break;
                }
            case Human.ProfessionType.D_Personnel:
                {
                    FreeD_Personnel.Remove(human);
                    break;
                }
        }
    }

    public void Return(Human human)
    {
        switch (human.Profession)
        {
            case Human.ProfessionType.Agent:
                {
                    FreeAgents.Add(human);
                    break;
                }
            case Human.ProfessionType.Operative:
                {
                    FreeOperatives.Add(human);
                    break;
                }
            case Human.ProfessionType.Scientist:
                {
                    FreeScientists.Add(human);
                    break;
                }
            case Human.ProfessionType.D_Personnel:
                {
                    FreeD_Personnel.Add(human);
                    break;
                }
        }
    }

    public void Kill(Human human)
    {
        switch (human.Profession)
        {
            case Human.ProfessionType.Agent:
                {
                    if (FreeAgents.Contains(human))
                        FreeAgents.Remove(human);
                    AgentsCount--;
                    break;
                }
            case Human.ProfessionType.Operative:
                {
                    if (FreeOperatives.Contains(human))
                        FreeOperatives.Remove(human);
                    OperativesCount--;
                    break;
                }
            case Human.ProfessionType.Scientist:
                {
                    if (FreeScientists.Contains(human))
                        FreeScientists.Remove(human);
                    ScientistsCount--;
                    break;
                }
            case Human.ProfessionType.D_Personnel:
                {
                    if (FreeD_Personnel.Contains(human))
                        FreeD_Personnel.Remove(human);
                    D_PersonnelCount--;
                    break;
                }
        }
        People.Remove(human.ID);
    }

    //test
    /*internal Human GetFreeAgent()
    {
        if (FreeAgents.Count > 0)
        {
            Human agent = FreeAgents[0];
            return agent;
        }
        else
            return null;
    }

    internal Human GetFreeScientist()
    {
        if (FreeScientists.Count > 0)
        {
            Human scientist = FreeScientists[0];
            return scientist;
        }
        else
            return null;
    }

    internal Human GetFreeOperative()
    {
        if (FreeOperatives.Count > 0)
        {
            Human operative = FreeOperatives[0];
            return operative;
        }
        else
            return null;
    }*/
    /////////////////////////////

    internal void PrepareToSave()
    {
        people = new Human[People.Count];
        int i = 0;
        foreach (var item in People)
        {
            people[i] = item.Value;
            i++;
        }
        UsedPregeneratedMen = new int[GameData.Data.HumanGenerator.PreGeneratedMen.Length - FreePregeneratedMen.Count];
        i = 0;
        for (int j = 0; j < GameData.Data.HumanGenerator.PreGeneratedMen.Length; j++)
            if (!FreePregeneratedMen.Contains(i))
            {
                UsedPregeneratedMen[i] = j;
                i++;
            }
        UsedPregeneratedWomen = new int[GameData.Data.HumanGenerator.PreGeneratedWomen.Length - FreePregeneratedWomen.Count];
        i = 0;
        for (int j = 0; j < GameData.Data.HumanGenerator.PreGeneratedWomen.Length; j++)
            if (!FreePregeneratedWomen.Contains(i))
            {
                UsedPregeneratedWomen[i] = j;
                i++;
            }
    }

    internal void InitAfterLoad()
    {
        People = new Dictionary<int, Human>();
        FreeAgents = new List<Human>();
        FreeScientists = new List<Human>();
        FreeOperatives = new List<Human>();
        FreeD_Personnel = new List<Human>();
        foreach (var item in people)
        {
            Human human = item;
            People.Add(human.ID, human);
            switch (human.Profession)
            {
                case Human.ProfessionType.Agent:
                    {
                        AgentsCount++;
                        if (human.Activity == Human.ActivityType.Free)
                            FreeAgents.Add(human);
                        break;
                    }
                case Human.ProfessionType.Operative:
                    {
                        OperativesCount++;
                        if (human.Activity == Human.ActivityType.Free)
                            FreeOperatives.Add(human);
                        break;
                    }
                case Human.ProfessionType.Scientist:
                    {
                        ScientistsCount++;
                        if (human.Activity == Human.ActivityType.Free)
                            FreeScientists.Add(human);
                        break;
                    }
                case Human.ProfessionType.D_Personnel:
                    {
                        D_PersonnelCount++;
                        if (human.Activity == Human.ActivityType.Free)
                            FreeD_Personnel.Add(human);
                        break;
                    }
            }
        }
        FreePregeneratedMen = new HashSet<int>();
        int i = 0;
        for (int j = 0; j < GameData.Data.HumanGenerator.PreGeneratedMen.Length; j++)
            if (UsedPregeneratedMen != null)
                if (i < UsedPregeneratedMen.Length ? UsedPregeneratedMen[i] != j : true)
                    FreePregeneratedMen.Add(j);
                else
                    i++;
            else
                FreePregeneratedMen.Add(j);
        FreePregeneratedWomen = new HashSet<int>();
        i = 0;
        for (int j = 0; j < GameData.Data.HumanGenerator.PreGeneratedWomen.Length; j++)
            if (UsedPregeneratedWomen != null)
                if (i < UsedPregeneratedWomen.Length ? UsedPregeneratedWomen[i] != j : true)
                    FreePregeneratedWomen.Add(j);
                else
                    i++;
            else
                FreePregeneratedWomen.Add(j);
    }
}
