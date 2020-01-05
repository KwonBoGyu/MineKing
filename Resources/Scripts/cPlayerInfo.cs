using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cPlayerInfo
{
    public string nickName;
    public cInventory inventory;
    public cItem_equip[] item_equip;
    public cItem_use[] item_use;
    public cItem_etc[] item_etc;

    #region 생성자
    public cPlayerInfo(string pNickName, float pMoveSpeed, float pAttackSpeed, float pDamage,
        float pHp, cItem_equip[] pItem_equip = null, cItem_use[] pItem_use = null, cItem_etc[] pItem_etc = null)
    {
        inventory = new cInventory();
        inventory.Init();

        nickName = pNickName;

        if(pItem_equip != null)
        {
            for (int i = 0; i < pItem_equip.Length; i++)            
                inventory.GetItemEquip().Add(pItem_equip[i]);            
        }

        if(pItem_use != null)
        {
            for (int i = 0; i < pItem_use.Length; i++)
                inventory.GetItemUse().Add(pItem_use[i]);
        }

        if (pItem_etc != null)
        {
            for (int i = 0; i < pItem_use.Length; i++)
                inventory.GetItemEtc().Add(pItem_etc[i]);
        }
    }

    public cPlayerInfo(cPlayerInfo pPi)
    {
        inventory = new cInventory();
        inventory.Init();

        this.nickName = pPi.nickName;

        if (pPi.item_equip != null)
        {
            for (int i = 0; i < pPi.item_equip.Length; i++)
                inventory.GetItemEquip().Add(pPi.item_equip[i]);            
        }

        if (pPi.item_use != null)
        {
            for (int i = 0; i < pPi.item_use.Length; i++)
                inventory.GetItemUse().Add(pPi.item_use[i]);
        }

        if (pPi.item_etc != null)
        {
            for (int i = 0; i < pPi.item_etc.Length; i++)
                inventory.GetItemEtc().Add(pPi.item_etc[i]);
        }
    }
    #endregion

    //데이터 동기화
    public void SyncData()
    {
        item_equip = new cItem_equip[inventory.GetItemEquip().Count];
        for (int i = 0; i < inventory.GetItemEquip().Count; i++)
        {
            item_equip[i] = new cItem_equip(inventory.GetItemEquip()[i]._name, inventory.GetItemEquip()[i].price, inventory.GetItemEquip()[i].kind, 
                inventory.GetItemEquip()[i].kindNum, inventory.GetItemEquip()[i].damage, inventory.GetItemEquip()[i].defense, 
                inventory.GetItemEquip()[i].hitLevel, inventory.GetItemEquip()[i].attackSpeed, inventory.GetItemEquip()[i].drainValue);
        }

        item_use = new cItem_use[inventory.GetItemUse().Count];
        for (int i = 0; i < inventory.GetItemEquip().Count; i++)
        {
            item_use[i] = new cItem_use(inventory.GetItemUse()[i]._name, inventory.GetItemUse()[i].price, inventory.GetItemUse()[i].amount,
                inventory.GetItemUse()[i].kind, inventory.GetItemUse()[i].kindNum);
        }

        item_etc = new cItem_etc[inventory.GetItemEtc().Count];
        for (int i = 0; i < inventory.GetItemEtc().Count; i++)
        {
            item_etc[i] = new cItem_etc(inventory.GetItemEtc()[i]._name, inventory.GetItemEtc()[i].price, inventory.GetItemEtc()[i].amount,
                inventory.GetItemEtc()[i].kind, inventory.GetItemEtc()[i].kindNum);
        }
    }
}
