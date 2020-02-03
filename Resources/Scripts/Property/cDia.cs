using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cDia : cProperty
{
    public cDia(string pName, long pValue)
        :base(pName, pValue)
    {
    }

    public cDia(string pName)
: base(pName)
    {
    }

    public cDia()
: base("Dia")
    {

    }
}
