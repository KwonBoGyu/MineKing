﻿using System.Collections;
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
    private cGold money;
    public cGold GetMoney() { return money; }
    private cRock rock;
    public cRock GetRock() { return rock; }
    private cDia dia;
    public cDia GetDia() { return dia; }
    private List<cJewerly> jewerly;
    public List<cJewerly> GetJewerly() { return jewerly; }
    private List<cSoul> soul;
    public List<cSoul> GetSoul() { return soul; }

    //아이템 추가
    public void AddItem(byte pKind, byte pAmount = 1)
    {
        bool contains = false;

        //아이템이 존재 할 때
        for(byte i = 0; i < l_itemUse.Count; i++)
        {
            if (l_itemUse[i].kind.Equals(pKind))
            {
                l_itemUse[i].amount += pAmount;
                contains = true;
                break;
            }
        }

        //아이템이 존재하지 않을 때
        if (contains.Equals(false))
        {
            cItem pUse;
            citemTable.GetItemInfo(out pUse, pKind);
            l_itemUse.Add((cItem_use)pUse);
            AddItem(pKind, (byte)(pAmount - 1));
        }
    }
    //아이템 삭제
    public void RemoveItem(byte pKind, byte pAmount = 1)
    {
        //아이템이 존재 할 때
        for (byte i = 0; i < l_itemUse.Count; i++)
        {
            if (l_itemUse[i].kind.Equals(pKind))
            {
                if(l_itemUse[i].amount <= pAmount)                
                    l_itemUse.RemoveAt(i);                
                else
                    l_itemUse[i].amount -= pAmount;

                break;
            }
        }
    }

    //보스 키 존재 여부
    public bool isBossKeyExist()
    {
        for(byte i = 0; i < GetItemEtc().Count; i++)        
            if (GetItemEtc()[i]._name.Equals("BossKey"))
                return true;        

        return false;
    }

    public void destroyBossKey()
    {
        byte idx = 0;
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
        for (byte i = 0; i < 3; i++)
        {
            cItem pUse;
            citemTable.GetItemInfo(out pUse, i);
            l_itemUse.Add((cItem_use)pUse);
        }
        l_itemEtc = new List<cItem_etc>();
        
        money = new cGold();
        rock = new cRock();
        dia = new cDia();
        jewerly = new List<cJewerly>();
        for (byte i = 0; i < 5; i++)
            jewerly.Add(new cJewerly());
        soul = new List<cSoul>();
        for (byte i = 0; i < 4; i++)
            soul.Add(new cSoul());
    }
}
