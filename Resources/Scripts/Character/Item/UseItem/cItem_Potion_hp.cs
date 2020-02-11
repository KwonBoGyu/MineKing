using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Potion_hp : cItem_use
{
    #region 생성자
    public cItem_Potion_hp(string pName, string pDesc, cProperty pPrice, byte pAmount, byte pKind)
        : base(pName, pDesc, pPrice, pAmount, pKind)
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


    public override byte UseItem()
    {
        byte curAmount = 0;

        curAmount = base.UseItem();

        if (curAmount.Equals(100))
            return curAmount;
        cUtil._player.RestoreHp(new cProperty("HP", (long)(cUtil._player.GetMaxHp().value * 0.25f)));
        return curAmount;
    }
}
