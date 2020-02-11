using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class citemTable
{
    public static byte GetUseItemTotalNum() { return 3; }

    public static void GetItemInfo(out cItem pItem, int pItemNum)
    {
        switch (pItemNum)
        {            
            // 소비 (0 ~ 10)
            case 0:
                pItem = new cItem_Potion_hp("HP 포션", "주인공의 체력을 5초동안 25% 회복시킨다.", 
                    new cProperty("Price", 1000), 1, (byte)pItemNum);
                return;
            case 1:
                pItem = new cItem_Torch("횃불", "던전 벽에 고정시켜 주변을 밝힐 수 있는 일반적인 횃불이다.",
                    new cProperty("Price", 1000), 1, (byte)pItemNum);
                return;
            case 2:
                pItem = new cItem_Bomb("다이너마이트", "전방으로 투척하여 사용한다. 폭발반경은 3X3타일이다.",
                    new cProperty("Price", 1000), 1, (byte)pItemNum);
                return;

            // 재료(50 ~ 100)
            case 50:
                pItem = new cItem_etc("수상한 등불", "수상한 동굴을 탐험할 수 있도록 주변의 강한 기운을 밝혀주는 등불이다.",
                    new cProperty("price", 0), 1, (byte)pItemNum);
                return;
        }

        pItem = null;
    } 
}
