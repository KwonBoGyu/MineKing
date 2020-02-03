using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Potion_hp : cItem_use
{
    #region 생성자
    public cItem_Potion_hp(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
        : base(pName, pPrice, pAmount, pKind, pKindNum)
    {
    }
    #endregion


    // 최대 체력의 일정 %만큼 현재 체력 회복
    // 임시 : 10%
    //public override void Use(float pMaxHp)
    //{
    //    cUser.
    //    pMaxHp * 0.1f;
    //}

    
    public override void UseItem()
    {
        base.UseItem();

        cUtil._user.GetPlayer().IncreaseHP(new cProperty("HP", (long)(cUtil._user.GetPlayer().GetMaxHp().value * 0.1f)));
    }
}
