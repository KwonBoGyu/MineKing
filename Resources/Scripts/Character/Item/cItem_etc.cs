using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cItem_etc : cItem
{
    public int amount;

    #region 생성자
    public cItem_etc(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
        : base(pName, pPrice, pKind, pKindNum)
    {
        amount = pAmount;
    }

    public cItem_etc(cItem_etc pIe)
        : base(pIe._name, pIe.price, pIe.kind, pIe.kindNum)
    {
        this.amount = pIe.amount;
    }
    #endregion
}
