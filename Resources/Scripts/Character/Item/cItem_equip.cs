using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cItem_equip : cItem
{
    public cProperty damage;
    public cProperty maxHp;
    public float attackSpeed;
    public float drainValue;

    #region 생성자
    public cItem_equip(string pName, string pDesc, cProperty pPrice, byte pKind,
        cProperty pDamage, cProperty pMaxHp, float pAttackSpeed = 0, float pDrainValue = 0)
        : base(pName, pDesc, pPrice, pKind)
    {
        damage = new cProperty(pDamage);
        maxHp = new cProperty(pMaxHp);
        attackSpeed = pAttackSpeed;
        drainValue = pDrainValue;
    }

    public cItem_equip(cItem_equip pIe)
        : base(pIe._name, pIe.desc, pIe.price, pIe.kind)
    {
        damage = new cProperty(pIe.damage);
        maxHp = new cProperty(pIe.maxHp);
        attackSpeed = pIe.attackSpeed;
        drainValue = pIe.drainValue;
    }
    #endregion
}
