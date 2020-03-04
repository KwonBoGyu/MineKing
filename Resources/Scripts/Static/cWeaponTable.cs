using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class cWeaponTable
{
    //무기 정보 가져오기    

    public static cAxe GetAxeInfo(cProperty pLevel)
    {
        cAxe tAxe = new cAxe();

        //레벨
        tAxe.level.value = pLevel.value;

        //데미지 = 레벨 * 10
        tAxe.damage.value = 10 * pLevel.value;

        //체력 = 레벨 * 5
        tAxe.hp.value = 5 * pLevel.value;

        //내구도 = 레벨 * 1
        tAxe.indurance.value = 1 * pLevel.value;

        //가치 = 레벨 * 1000
        tAxe.value.value = 1000 * pLevel.value;

        tAxe.attackSpeed = 1.0f;
        tAxe.AxeImgNum = 0;

        return tAxe;
    }

    public static cProperty GetMaxUpgradeLevelByMaxGold(cProperty pCurLevel, cProperty pMoney)
    {
        cProperty canLevel = new cProperty("adsf", 0);

        long factor = 1;
        long upgradeTotalMoney = 0;

        while(true)
        {
            upgradeTotalMoney += 1000 * (pCurLevel.value + factor);
            if (upgradeTotalMoney > 9000000000000000000)
            {
                canLevel.value += factor;
                break;
            }

            if (pMoney.value < upgradeTotalMoney)
            {
                canLevel.value += (factor - 1);
                break;
            }

            factor++;
        }

        return canLevel;
    }

    public static cProperty GetUpgradeGoldByLevel(long pCurLevel, long pNextLevel)
    {
        cProperty money = new cProperty("adsf", 0);

        for(long i = pCurLevel; i < (pCurLevel + pNextLevel); i++)
        {
            money.value += (i + 1) * 1000; 
        }

        return money;
    }
}
