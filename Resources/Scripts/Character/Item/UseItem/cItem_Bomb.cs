using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Bomb : cItem_use
{
    public float bombTime;

    #region 생성자
    public cItem_Bomb(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
        : base(pName, pPrice, pAmount, pKind, pKindNum)
    {
        bombTime = 3.0f;
    }
    #endregion
    // 폭탄 설치
    public override void UseItem()
    {
        base.UseItem();

        Transform tempT = cUtil._user.GetPlayer().transform;

        GameObject bomb = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Item_Bomb"), tempT.position,
            Quaternion.identity, tempT.parent.transform.parent);
        bomb.GetComponent<cItem_Bomb_O>().SetTimer(bombTime);
    }
}
