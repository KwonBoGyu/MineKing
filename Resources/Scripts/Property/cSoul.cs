using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cSoul : cProperty
{
    public cSoul(string pName, long pValue)
        :base(pName, pValue)
    {
    }

    public cSoul(string pName)
: base(pName)
    {
    }

    public cSoul()
      : base("Soul")
    {

    }
}
