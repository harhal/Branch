using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusMinusUI : MonoBehaviour {

    public int Value;
    public int MinValue;
    public int MaxValue;
    public Text ValueOutput;

    public delegate void RefreshValue(int delta);

    public event RefreshValue OnValueChanged;

    // Use this for initialization
    void Start()
    {
        OnValueChanged += RefreshText;
    }

    public void RefreshText(int delta)
    {
        if (ValueOutput != null)
            ValueOutput.text = Value.ToString();
    }

    public void AddValue()
    {
        if (Value < MaxValue)
        {
            Value++;
            OnValueChanged(1);
        }
    }

    public void RemoveValue()
    {
        if (Value > MinValue)
        {
            Value--;
            OnValueChanged(-1);
        }
    }

    // Update is called once per frame
    void Update ()
    {  
    }
}
