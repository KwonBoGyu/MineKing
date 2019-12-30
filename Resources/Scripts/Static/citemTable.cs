using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class citemTable
{
    public static void GetItemInfo(out cItem pItem, int pItemNum)
    {
        switch (pItemNum)
        {
            case 0:
                pItem = new cItem_equip("임시 장비", 1000, 0, 0);
                return;

            case 1:
                pItem = new cItem_use("임시 소비", 1000,1, 1, 0);
                return;

            case 2:
                pItem = new cItem_etc("임시 재료", 1000,1, 2, 0);
                return;
            // 보스키
            case 3:
                pItem = new cItem_etc("BossKey", 0, 1, 2, 0);
                return;
            // 보스 드랍 강화 아이템
            case 4:
                pItem = new cItem_etc("UpgradeItem", 0, 1, 2, 0);
                return;
        }

        pItem = null;
    }
}
