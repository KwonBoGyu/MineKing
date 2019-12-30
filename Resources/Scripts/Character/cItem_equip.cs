using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cItem_equip : cItem
{
    public float damage;
    public float defense;
    public int hitLevel;
    public float attackSpeed;
    public float drainValue;

    #region 생성자
    public cItem_equip(string pName, int pPrice, int pKind, int pKindNum,
        float pDamage = 0, float pDefense = 0, int pHitLevel = 0, float pAttackSpeed = 0, float pDrainValue = 0)
        : base(pName, pPrice, pKind, pKindNum)
    {
        damage = pDamage;
        defense = pDefense;
        hitLevel = pHitLevel;
        attackSpeed = pAttackSpeed;
        drainValue = pDrainValue;
    }

    public cItem_equip(cItem_equip pIe)
        : base(pIe._name, pIe.price, pIe.kind, pIe.kindNum)
    {
        this.damage = pIe.damage;
        this.defense = pIe.defense;
        this.hitLevel = pIe.hitLevel;
        this.attackSpeed = pIe.attackSpeed;
        this.drainValue = pIe.drainValue;
    }
    #endregion
}
