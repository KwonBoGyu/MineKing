using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cGold : cProperty
{
    public cGold(string pName, long pValue)
        : base(pName, pValue)
    {
    }

    public cGold(string pName)
        :base(pName)
    {
    }

    public cGold()
        : base("Gold")
    {

    }
}
