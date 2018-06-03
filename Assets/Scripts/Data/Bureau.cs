using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bureau: ISerializationCallbackReceiver {
    public List<FieldOperation> CurrentOperations;
    [SerializeField]
    int[] operationStack;
    [SerializeField]
    float TimeToNextOperation;

    [System.NonSerialized]
    public Stack<int> OperationStack;
    [System.NonSerialized]
    public Dictionary<int, Report> Archive;

    public Bureau()
    {
        CurrentOperations = new List<FieldOperation>();
        OperationStack = new Stack<int>();
    }

    public FieldOperation AddNextFieldOperation()
    {
        if (OperationStack == null) return null;
        if (OperationStack.Count <= 0) return null;
        FieldOperation result = GameData.Data.FieldOperations[OperationStack.Pop()];
        result.Initialize();
        CurrentOperations.Add(result);
        return result;
    }

    public void GenerateOperationStack()
    {
        OperationStack = new Stack<int>();
        var buf = new List<int>();
        for (int i = 0; i < GameData.Data.FieldOperations.Length; i++)
            buf.Add(i);
        for (int i = 0; i < GameData.Data.FieldOperations.Length; i++)
        {
            int item = buf[Random.Range(0, buf.Count)];
            OperationStack.Push(item);
            buf.Remove(item);
        }
    }

    public void Update () {
        TimeToNextOperation -= Time.deltaTime;
        if (TimeToNextOperation <= 0)
        {
            TimeToNextOperation = Random.Range(10, 60);
            AddNextFieldOperation();
        }
        foreach (var item in CurrentOperations)
            item.Update();
        CurrentOperations.RemoveAll(delegate(FieldOperation o)
        { return o.OperationTime <= 0; });
    }

    public void OnBeforeSerialize()
    {
        operationStack = new int[OperationStack.Count];
        while (OperationStack.Count > 0)
            operationStack[OperationStack.Count - 1] = OperationStack.Pop();
    }

    public void OnAfterDeserialize()
    {
        OperationStack = new Stack<int>();
        for (int i = 0; i < operationStack.Length; i++)
            OperationStack.Push(operationStack[i]);
    }
}
