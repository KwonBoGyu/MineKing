﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cAxe
{
    public cProperty level;
    public cProperty damage;
    public cProperty hp;
    public cProperty value;
    public float attackSpeed;
    public byte AxeImgNum;

    public cAxe()
    {
        level = new cProperty("Level");
        damage = new cProperty("Damage");
        hp = new cProperty("Hp");
        value = new cProperty("Value");
        attackSpeed = 1.0f;
        AxeImgNum = 0;
    }
    public cAxe(cProperty pLevel, cProperty pDamage, cProperty pHp, cProperty pValue, float pAs, byte pImgNum)
    {
        level = new cProperty(pLevel);
        damage = new cProperty(pDamage);
        hp = new cProperty(pHp);
        value = new cProperty(pValue);
        attackSpeed = pAs;
        AxeImgNum = pImgNum;
    }
    public cAxe(cAxe pAxe)
    {
        level = new cProperty(pAxe.level);
        damage = new cProperty(pAxe.damage);
        hp = new cProperty(pAxe.hp);
        value = new cProperty(pAxe.value);
        attackSpeed = pAxe.attackSpeed;
        AxeImgNum = pAxe.AxeImgNum;
    }
}
