using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cItem_etc : cItem
{
    public byte amount;

    #region 생성자
    public cItem_etc(string pName, string pDesc, cProperty pPrice, byte pAmount, byte pKind)
        : base(pName, pDesc, pPrice, pKind)
    {
        amount = pAmount;
    }

    public cItem_etc(cItem_etc pIe)
        : base(pIe._name, pIe.desc, pIe.price, pIe.kind)
    {
        amount = pIe.amount;
    }
    #endregion
}
