using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct HumanGeneratorItem
{
    public Human.SexType Sex;
    public string Value;
}

[System.Serializable]
class HumanGeneratorArray
{
    [SerializeField]
    HumanGeneratorItem[] Items;

    [System.NonSerialized]
    string[] SortedItems;
    [System.NonSerialized]
    RangeInt MaleIndexRange;
    [System.NonSerialized]
    RangeInt FemaleIndexRange;

    public HumanGeneratorArray()
    {
        var testItem = new HumanGeneratorItem();
        testItem.Sex = Human.SexType.Male;
        testItem.Value = "TestString";
        Items = new HumanGeneratorItem[] { testItem };
    }

    internal void InitAfterLoad()
    {
        var list = new List<string>();
        MaleIndexRange = new RangeInt(list.Count, 0);
        foreach (var item in Items)
            if (item.Sex == Human.SexType.Male)
                list.Add(item.Value);
        FemaleIndexRange = new RangeInt(list.Count, 0);
        foreach (var item in Items)
            if (item.Sex == Human.SexType.NoMatter)
                list.Add(item.Value);
            MaleIndexRange.length = list.Count - MaleIndexRange.start;
        foreach (var item in Items)
            if (item.Sex == Human.SexType.Female)
                list.Add(item.Value);
        FemaleIndexRange.length = list.Count - FemaleIndexRange.start;
        SortedItems = list.ToArray();
    }

    public string GetItem(Human.SexType Sex)
    {
        switch (Sex)
        {
            case Human.SexType.Male:
                {
                    return SortedItems[Random.Range(MaleIndexRange.start, MaleIndexRange.end)];
                }
            case Human.SexType.Female:
                {
                    return SortedItems[Random.Range(FemaleIndexRange.start, FemaleIndexRange.end)];
                }
            default:
                {
                    return SortedItems[Random.Range(0, SortedItems.Length)];
                }
        }
    }
}

[System.Serializable]
public class HumanGenerator
{
    [SerializeField]
    HumanGeneratorArray FirstNames;
    [SerializeField]
    HumanGeneratorArray LastNames;
    [SerializeField]
    HumanGeneratorArray Faces;

    public Human[] PreGeneratedMen;
    public Human[] PreGeneratedWomen;

    public HumanGenerator()
    {
        FirstNames = new HumanGeneratorArray();
        LastNames = new HumanGeneratorArray();
        Faces = new HumanGeneratorArray();
        PreGeneratedMen = new Human[] { new Human("Liroy Jenkins", Human.SexType.Male, "Pictures/Faces/Default") };
        PreGeneratedWomen = new Human[] { new Human("Lara Croft", Human.SexType.Female, "Pictures/Faces/Default") };
    }

    public Human GenerateHuman(Human.SexType Sex)
    {
        if (Sex == Human.SexType.NoMatter)
            Sex = Random.Range(0, 2) == 0 ? Human.SexType.Male : Human.SexType.Female;
        return new Human(FirstNames.GetItem(Sex) + ' ' + LastNames.GetItem(Sex), Sex, Faces.GetItem(Sex));
    }

    internal void InitAfterLoad()
    {
        FirstNames.InitAfterLoad();
        LastNames.InitAfterLoad();
        Faces.InitAfterLoad();
    }
}
