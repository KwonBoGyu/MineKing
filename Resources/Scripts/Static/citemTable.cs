using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class citemTable
{
    public static void GetItemInfo(out cItem pItem, int pItemNum)
    {
        switch (pItemNum)
        {            
            // 임시 소비
            case 0:
                pItem = new cItem_Bomb("HP 포션", 100, 1, pItemNum, 0);
                return;
            case 1:
                pItem = new cItem_Rope("횃불", 100, 1, pItemNum, 0);
                return;
            case 2:
                pItem = new cItem_SandBag("다이너마이트", 100, 1, pItemNum, 0);
                return;

            // 임시
            case 100:
                pItem = new cItem_etc("임시 재료", 1000,1, 2, 0);
                return;
            // 보스키
            case 101:
                pItem = new cItem_etc("BossKey", 0, 1, 2, 0);
                return;
            // 보스 드랍 강화 아이템
            case 102:
                pItem = new cItem_etc("UpgradeItem", 0, 1, 2, 0);
                return;
        }

        pItem = null;
    } 
}
