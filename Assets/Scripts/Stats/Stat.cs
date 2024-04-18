using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // serialize a custom class so they are visible in the inspector
public class Stat
{
    [SerializeField]
    private int baseValue;

    public int GetValue()
    {
        return baseValue;
    }
}
