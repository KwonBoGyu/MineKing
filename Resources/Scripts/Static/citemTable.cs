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
                pItem = new cItem_use("임시 소비", 1000, 1, 1, 0);
                return;
            case 1:
                pItem = new cItem_Bomb("폭탄", 100, 1, 1, 0);
                return;
            case 2:
                pItem = new cItem_Rope("로프", 100, 1, 1, 0);
                return;
            case 3:
                pItem = new cItem_SandBag("모래주머니", 100, 1, 1, 0);
                return;
            case 4:
                pItem = new cItem_Potion_speed("스피드포션", 100, 1, 1, 0);
                return;
            case 5:
                pItem = new cItem_Potion_hp("체력포션", 100, 1, 1, 0);
                return;
            case 6:
                pItem = new cItem_Torch("횃불", 100, 1, 1, 0);
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
