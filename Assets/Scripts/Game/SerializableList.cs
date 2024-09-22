using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableList<TValue> : System.Collections.Generic.List <TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        values.Clear();
        foreach (TValue val in this)
        {
            values.Add(val);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < values.Count; i++)
        {
            this.Add(values[i]);
        }
    }
}