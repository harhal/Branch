using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Report
{
    [SerializeField]
    string photo;
    public string Description;
    public string Summary;
    public bool isVariable = true;

    [System.NonSerialized]
    public FieldOperation operation;
    [System.NonSerialized]
    Sprite _photo;
    public Sprite Photo
    {
        get
        {
            if (_photo == null)
                _photo = Resources.Load<Sprite>(photo);
            return _photo;
        }
    }
    [System.NonSerialized]
    protected VariantButton[] cashedChoises;

    public virtual VariantButton[] GetChoises()
    {
        if (cashedChoises == null)
            cashedChoises = new VariantButton[0];
        return cashedChoises;
    }
}

[System.Serializable]
public class InfoReport : Report
{
    public override VariantButton[] GetChoises()
    {
        if (cashedChoises == null)
            cashedChoises = new VariantButton[] { new CloseButton("Close") };
        return cashedChoises;
    }
}