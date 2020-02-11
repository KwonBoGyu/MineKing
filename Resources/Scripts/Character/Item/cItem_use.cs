using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_use : cItem
{
    // 디버그 과정에서 amount 줄이지 않음
    public byte amount;

    #region 생성자
    public cItem_use(string pName, string pDesc, cProperty pPrice, byte pAmount, byte pKind)
        :base(pName, pDesc, pPrice, pKind)
    {
        amount = pAmount;
    }

    public cItem_use(cItem_use pIu)
        : base(pIu._name, pIu.desc, pIu.price, pIu.kind)
    {
        this.amount = pIu.amount;
    }
    #endregion

    public void IncreaseAmount()
    {

    }

    public virtual byte UseItem()
    {
        byte curAmount = 0;

        if(this.amount.Equals(0))
        {
            Debug.Log("갯수가 부족합니다.");
            curAmount = 100;
            return curAmount;
        }
        else
        {
            amount -= 1;
            curAmount = amount;
        }

        return curAmount;
    }
}
