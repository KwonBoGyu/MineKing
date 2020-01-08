using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cInventory : MonoBehaviour
{
    //장비 아이템
    private List<cItem_equip> l_itemEquip;
    public List<cItem_equip> GetItemEquip() { return l_itemEquip; }
    //소비 아이템
    private List<cItem_use> l_itemUse;
    public List<cItem_use> GetItemUse() { return l_itemUse; }
    //기타 아이템
    private List<cItem_etc> l_itemEtc;
    public List<cItem_etc> GetItemEtc() { return l_itemEtc; }
    // 돈
    private int money = 1000;
    public int GetMoney() { return money; }
    public void UseMoney(int pMoney)
    {
        if (pMoney > money)
            Debug.Log("Not enough money.");
        else
            money -= pMoney;
    }
    public void EarnMoney(int pMoney)
    {
        money += pMoney;
    }

    //보스 키 존재 여부
    public bool isBossKeyExist()
    {
        for(int i = 0; i < GetItemEtc().Count; i++)        
            if (GetItemEtc()[i]._name.Equals("BossKey"))
                return true;        

        return false;
    }

    public void destroyBossKey()
    {
        int idx = 0;
        while(idx < GetItemEtc().Count)
        {
            if (GetItemEtc()[idx]._name.Equals("BossKey"))
            {
                Debug.Log("BossKey used");
                GetItemEtc().RemoveAt(idx);
            }
            else
            {
                ++idx;
            }
        }
    }

    public void Init()
    {
        //아이템 순서 고정
        l_itemEquip = new List<cItem_equip>();
        l_itemUse = new List<cItem_use>();
        for (int i = 0; i < 6; i++)
        {
            cItem pUse;
            citemTable.GetItemInfo(out pUse, i+1);
            l_itemUse.Add((cItem_use)pUse);
        }
        l_itemEtc = new List<cItem_etc>();
    }
}
