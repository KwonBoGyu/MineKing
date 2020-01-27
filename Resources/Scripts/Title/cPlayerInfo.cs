using System.Collections;
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

    #region 생성자
    public cPlayerInfo(string pNickName, cAxe pAxe, cInventory pInventory, 
        cGold pMoney, cRock pRock, cDia pDia, cJewerly[] pJewerly, cSoul[] pSoul,
        cItem_equip[] pItem_equip = null, cItem_use[] pItem_use = null, cItem_etc[] pItem_etc = null)
    {
        weapon = new cAxe(pAxe);        

        inventory = pInventory;
        inventory.Init();

        nickName = pNickName;

        inventory.GetMoney().SetValue(pMoney.GetValue());
        inventory.GetRock().SetValue(pRock.GetValue());
        inventory.GetDia().SetValue(pDia.GetValue());

        for (byte i = 0; i < pJewerly.Length; i++)        
            inventory.GetJewerly()[i].SetValue(pJewerly[i].GetValue());     
        for (byte i = 0; i < pSoul.Length; i++)
            inventory.GetSoul()[i].SetValue(pSoul[i].GetValue());

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

        inventory = pInventory;
        inventory.Init();

        this.nickName = pPi.nickName;

        inventory.GetMoney().SetValue(pPi.money.GetValue());
        inventory.GetRock().SetValue(pPi.rock.GetValue());
        inventory.GetDia().SetValue(pPi.dia.GetValue());
        for (byte i = 0; i < pPi.jewerly.Length; i++)
            inventory.GetJewerly()[i].SetValue(pPi.jewerly[i].GetValue());
        for (byte i = 0; i < pPi.soul.Length; i++)
            inventory.GetSoul()[i].SetValue(pPi.soul[i].GetValue());

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

        money.SetValue(inventory.GetMoney().GetValue());
        rock.SetValue(inventory.GetRock().GetValue());
        dia.SetValue(inventory.GetDia().GetValue());
        for(byte i = 0; i < jewerly.Length; i++)
            jewerly[i].SetValue(inventory.GetJewerly()[i].GetValue());
        for (byte i = 0; i < soul.Length; i++)
            soul[i].SetValue(inventory.GetSoul()[i].GetValue());

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
