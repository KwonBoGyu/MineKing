﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cPlayerInfo
{
    public string nickName;
    public cPlayer player;
    public cAxe weapon;
    public cInventory inventory;
    public cItem_equip[] item_equip;
    public cItem_use[] item_use;
    public cItem_etc[] item_etc;
    public cGold money;
    public cRock rock;
    public cDia dia;
    public cJewerly[] jewerly;
    public cSoul[] soul;
    public byte[] skillLevel; //0 : 대쉬, 1 : 시야증가, 2 : 차지강화, 3 : 점프강화

    #region 생성자
    public cPlayerInfo(string pNickName, cAxe pAxe, byte[] pSkillLevel,cInventory pInventory, 
        cGold pMoney, cRock pRock, cDia pDia, cJewerly[] pJewerly, cSoul[] pSoul,
        cItem_equip[] pItem_equip = null, cItem_use[] pItem_use = null, cItem_etc[] pItem_etc = null)
    {
        weapon = new cAxe(pAxe);

        skillLevel = new byte[4];
        for (byte i = 0; i < skillLevel.Length; i++)
            skillLevel[i] = pSkillLevel[i];

        inventory = pInventory;
        inventory.Init();

        nickName = pNickName;

        inventory.GetMoney().value = pMoney.value;
        inventory.GetRock().value = pRock.value;
        inventory.GetDia().value = pDia.value;

        for (byte i = 0; i < pJewerly.Length; i++)
            inventory.GetJewerly()[i].value = pJewerly[i].value;
        for (byte i = 0; i < pSoul.Length; i++)
            inventory.GetSoul()[i].value = pSoul[i].value;

        if (pItem_equip != null)
        {
            for (byte i = 0; i < pItem_equip.Length; i++)            
                inventory.GetItemEquip().Add(pItem_equip[i]);            
        }

        if(pItem_use != null)
        {
            for (byte i = 0; i < pItem_use.Length; i++)
                inventory.GetItemUse().Add(pItem_use[i]);
        }

        if (pItem_etc != null)
        {
            for (byte i = 0; i < pItem_use.Length; i++)
                inventory.GetItemEtc().Add(pItem_etc[i]);
        }
    }

    public cPlayerInfo(cPlayerInfo pPi, cInventory pInventory)
    {
        weapon = new cAxe(pPi.weapon);
        skillLevel = new byte[4];
        for (byte i = 0; i < skillLevel.Length; i++)
            skillLevel[i] = pPi.skillLevel[i];

        inventory = pInventory;
        inventory.Init();

        this.nickName = pPi.nickName;

        inventory.GetMoney().value = pPi.money.value;
        inventory.GetRock().value = pPi.rock.value;
        inventory.GetDia().value = pPi.dia.value;

        for (byte i = 0; i < pPi.jewerly.Length; i++)
            inventory.GetJewerly()[i].value = pPi.jewerly[i].value;
        for (byte i = 0; i < pPi.soul.Length; i++)
            inventory.GetSoul()[i].value = pPi.soul[i].value;

        if (pPi.item_equip != null)
        {
            for (byte i = 0; i < pPi.item_equip.Length; i++)
                inventory.GetItemEquip().Add(pPi.item_equip[i]);
        }

        if (pPi.item_use != null)
        {
            for (byte i = 0; i < pPi.item_use.Length; i++)
                inventory.GetItemUse().Add(pPi.item_use[i]);
        }

        if (pPi.item_etc != null)
        {
            for (byte i = 0; i < pPi.item_etc.Length; i++)
                inventory.GetItemEtc().Add(pPi.item_etc[i]);
        }
    }
    #endregion

    //데이터 동기화
    public void SyncData()
    {
        money = new cGold();
        rock = new cRock();
        dia = new cDia();
        jewerly = new cJewerly[inventory.GetJewerly().Count];
        for (byte i = 0; i < jewerly.Length; i++)
            jewerly[i] = new cJewerly();
        soul = new cSoul[inventory.GetSoul().Count];
        for (byte i = 0; i < soul.Length; i++)
            soul[i] = new cSoul();

        money.value = inventory.GetMoney().value;
        rock.value = inventory.GetRock().value;
        dia.value = inventory.GetDia().value;
        for (byte i = 0; i < jewerly.Length; i++)
            jewerly[i].value = inventory.GetJewerly()[i].value;
        for (byte i = 0; i < soul.Length; i++)
            soul[i].value = inventory.GetSoul()[i].value;

        item_equip = new cItem_equip[inventory.GetItemEquip().Count];
        for (byte i = 0; i < inventory.GetItemEquip().Count; i++)
        {
            item_equip[i] = new cItem_equip(inventory.GetItemEquip()[i]._name, inventory.GetItemEquip()[i].price, inventory.GetItemEquip()[i].kind, 
                inventory.GetItemEquip()[i].kindNum, inventory.GetItemEquip()[i].damage, inventory.GetItemEquip()[i].maxHp, 
                inventory.GetItemEquip()[i].hitLevel, inventory.GetItemEquip()[i].attackSpeed, inventory.GetItemEquip()[i].drainValue);
        }

        item_use = new cItem_use[inventory.GetItemUse().Count];
        for (byte i = 0; i < inventory.GetItemUse().Count; i++)
        {
            item_use[i] = new cItem_use(inventory.GetItemUse()[i]._name, inventory.GetItemUse()[i].price, inventory.GetItemUse()[i].amount,
                inventory.GetItemUse()[i].kind, inventory.GetItemUse()[i].kindNum);
        }

        item_etc = new cItem_etc[inventory.GetItemEtc().Count];
        for (byte i = 0; i < inventory.GetItemEtc().Count; i++)
        {
            item_etc[i] = new cItem_etc(inventory.GetItemEtc()[i]._name, inventory.GetItemEtc()[i].price, inventory.GetItemEtc()[i].amount,
                inventory.GetItemEtc()[i].kind, inventory.GetItemEtc()[i].kindNum);
        }
    }
}
