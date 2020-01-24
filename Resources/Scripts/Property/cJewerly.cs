using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cJewerly : cProperty
{
    public cJewerly(string pName, short[] pValue)
        : base(pName, pValue)
    {
    }

    public cJewerly(string pName)
    : base(pName)
    {
    }

    public cJewerly()
    : base("Jewerly")
    {

    }
}
