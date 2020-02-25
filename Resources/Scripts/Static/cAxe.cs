using System.Collections;
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

    public float headlightRange;
    public float dashCoolTime;
    public float chargeAttackPointMin;
    public float finalAttackPointMin;

    public cAxe()
    {
        level = new cProperty("Level");
        damage = new cProperty("Damage");
        hp = new cProperty("Hp");
        value = new cProperty("Value");
        attackSpeed = 1.0f;
        AxeImgNum = 0;
        headlightRange = 853f;
        dashCoolTime = 4.0f;
        chargeAttackPointMin = 0.7f;
        finalAttackPointMin = 0.5f;
    }
    public cAxe(cProperty pLevel, cProperty pDamage, cProperty pHp, cProperty pValue, float pAs, byte pImgNum,
        float pHeadlightIntensity, float pDashCollTime, float pchargeAttackPointMin, float pfinalAttackPointMin)
    {
        level = new cProperty(pLevel);
        damage = new cProperty(pDamage);
        hp = new cProperty(pHp);
        value = new cProperty(pValue);
        attackSpeed = pAs;
        AxeImgNum = pImgNum;
        pHeadlightIntensity = headlightRange;
        pDashCollTime = dashCoolTime;
        pchargeAttackPointMin = chargeAttackPointMin;
        pfinalAttackPointMin = finalAttackPointMin;
    }
    public cAxe(cAxe pAxe)
    {
        level = new cProperty(pAxe.level);
        damage = new cProperty(pAxe.damage);
        hp = new cProperty(pAxe.hp);
        value = new cProperty(pAxe.value);
        attackSpeed = pAxe.attackSpeed;
        AxeImgNum = pAxe.AxeImgNum;
        headlightRange = pAxe.headlightRange;
        dashCoolTime = pAxe.dashCoolTime;
        chargeAttackPointMin = pAxe.chargeAttackPointMin;
        finalAttackPointMin = pAxe.finalAttackPointMin;
    }
}
