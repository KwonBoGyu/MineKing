using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Torch : cItem_use
{
    public cItem_Torch(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
       : base(pName, pPrice, pAmount, pKind, pKindNum)
    {

    }

    public override void UseItem()
    {
        base.UseItem();

        Transform tempT = cUtil._user.GetPlayer().transform;

        //GameObject torch = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Item_Torch"), tempT.position,
        //    Quaternion.identity, tempT.parent.transform.parent);
    }
}
