using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Bomb : cItem_use
{
    #region 생성자
    public cItem_Bomb(string pName, string pDesc, cProperty pPrice, byte pAmount, byte pKind)
        : base(pName, pDesc, pPrice, pAmount, pKind)
    {
    }
    #endregion
    // 폭탄 설치
    public override byte UseItem()
    {
        byte curAmount = 0;

        curAmount = base.UseItem();

        if (curAmount.Equals(100))
            return curAmount;

        cUtil._player.useMng.SetBomb();

        return curAmount;
    }
}
