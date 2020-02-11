using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cItem
{
    public string _name;
    public string desc;
    public cProperty price;
    public byte kind;
    
    public cItem(string pName, string pDesc, cProperty pPrice, byte pKind)
    {
        _name = pName;
        desc = pDesc;
        price = new cProperty(pPrice);
        kind = pKind;
    }

}
