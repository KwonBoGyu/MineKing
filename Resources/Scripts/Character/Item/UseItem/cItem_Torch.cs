using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Torch : cItem_use
{
    public cItem_Torch(string pName, string pDesc, cProperty pPrice, byte pAmount, byte pKind)
        : base(pName, pDesc, pPrice, pAmount, pKind)
    {
    }

    public override byte UseItem()
    {
        byte curAmount = 0;

        curAmount = base.UseItem();

        if (curAmount.Equals(100))
            return curAmount;

        cUtil._player.useMng.SetTorch();

        return curAmount;
    }
}
