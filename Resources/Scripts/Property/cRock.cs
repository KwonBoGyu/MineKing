using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cRock : cProperty
{
    public cRock(string pName, short[] pValue)
        : base(pName, pValue)
    {
    }

    public cRock(string pName)
: base(pName)
    {
    }

    public cRock()
  : base("Rock")
    {

    }
}
