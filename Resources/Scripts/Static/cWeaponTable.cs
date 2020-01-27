using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class cWeaponTable
{
    //무기 정보 가져오기
    public static cAxe GetAxeInfo(short pLevel)
    {
        cAxe tAxe = new cAxe();

        //레벨
        tAxe.level = pLevel;

        //데미지 = 레벨 * 10
        for (byte i = 0; i < 10; i++)
            tAxe.damage.AddValue(0, pLevel);

        //체력 = 레벨 * 5
        for (byte i = 0; i < 5; i++)
            tAxe.hp.AddValue(0, pLevel);

        //가치 = 레벨 * 100
        for (byte i = 0; i < 100; i++)
            tAxe.value.AddValue(0, pLevel);

        tAxe.attackSpeed = 1.0f;
        tAxe.AxeImgNum = 0;

        return tAxe;
    }

    public static short GetMaxUpgradeLevelByMaxGold(short pCurLevel, cGold pMaxGold)
    {
        short tUpgradeNum = 0;

        bool isDone = true;

        while(true)
        {
            for(byte i = 0; i < 100; i++)
            {
                isDone = pMaxGold.RemoveValue(0, pCurLevel);
                if (isDone.Equals(false))
                    break;
            }

            if (isDone.Equals(false))
                break;

            tUpgradeNum += 1;
        }

        return tUpgradeNum;
    }

    public static cGold GetUpgradeGoldByLevel(short pCurLevel, short pMaxLevel)
    {
        cGold tGold = new cGold();

        for(short i = (short)(pCurLevel + 1); i < pMaxLevel; i++)
        {
            tGold.AddValue(GetAxeInfo(i).value);
        }

        return tGold;
    }
}
