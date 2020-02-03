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

        //가치 = 레벨 * 100
        tAxe.value.value = 100 * pLevel.value;

        tAxe.attackSpeed = 1.0f;
        tAxe.AxeImgNum = 0;

        return tAxe;
    }
}
