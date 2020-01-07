using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_use : cItem
{
    // 디버그 과정에서 amount 줄이지 않음
    public int amount;

    #region 생성자
    public cItem_use(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
        :base(pName, pPrice, pKind, pKindNum)
    {
        amount = pAmount;
    }

    public cItem_use(cItem_use pIu)
        : base(pIu._name, pIu.price, pIu.kind, pIu.kindNum)
    {
        this.amount = pIu.amount;
    }
    #endregion

    public void IncreaseAmount()
    {

    }

    public virtual void UseItem()
    {
        if(this.amount.Equals(0))
        {
            Debug.Log("갯수가 부족합니다.");
            return;
        }
        //else
        //{
        //    amount -= 1;
        //}
    }
}
