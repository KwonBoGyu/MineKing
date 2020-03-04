using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cPlayerInfo
{
    public string nickName;
    public cAxe weapon;
    public cInventory inventory;
    public bool[] flaged;   //스테이지별 점령지
    public bool[] bossDone;
    public cItem_equip[] item_equip;
    public cItem_use[] item_use;
    public cItem_etc[] item_etc;
    public cGold money;
    public cRock rock;
    public cDia dia;
    public cJewerly[] jewerly;
    public cSoul[] soul;
    public byte[] skillLevel; //0 : 대쉬, 1 : 시야증가, 2 : 차지강화, 3 : 점프강화
    public short[] quickSlotItemNum;
    public cJewerly[] prevStorePrice;
    public cJewerly[] avStorePrice;
    public cJewerly[] curStorePrice;
    public cProperty clothCoupon;
    public bool[] myWeaponsId;
    public byte curWeaponId;
    public bool[] myClothesId;
    public byte curClothId;

    public cStore store;

    #region 생성자
    public cPlayerInfo(string pNickName, cAxe pAxe, byte[] pSkillLevel, bool[] pFlaged, bool[] pBossDone,
        short[] pQuickSlotItemNum,cInventory pInventory, 
        cGold pMoney, cRock pRock, cDia pDia, cJewerly[] pJewerly, cSoul[] pSoul, 
        cJewerly[] pPrevStorePrice, cJewerly[] pAvStorePrice, cJewerly[] pCurStorePrice,
        cProperty pClothCoupon, bool[] pMyWeaponsId, byte pCurWeaponId, bool[] pMyClothesId, byte pCurClothId,
        cItem_equip[] pItem_equip = null, cItem_use[] pItem_use = null, cItem_etc[] pItem_etc = null)
    {
        clothCoupon = new cProperty("clothCoupon", pClothCoupon.value);
        myWeaponsId = new bool[pMyWeaponsId.Length];
        for (byte i = 0; i < pMyWeaponsId.Length; i++)
            myWeaponsId[i] = pMyWeaponsId[i];
        curWeaponId = pCurWeaponId;
        myClothesId = new bool[pMyClothesId.Length];
        for (byte i = 0; i < pMyClothesId.Length; i++)
            myClothesId[i] = pMyClothesId[i];
        curClothId = pCurClothId;


        prevStorePrice = new cJewerly[5];
        avStorePrice = new cJewerly[5];
        curStorePrice = new cJewerly[5];
        for (byte i = 0; i < 5; i++)
        {
            prevStorePrice[i] = new cJewerly(pPrevStorePrice[i]._name, pPrevStorePrice[i].value);
            avStorePrice[i] = new cJewerly(pAvStorePrice[i]._name, pAvStorePrice[i].value);
            curStorePrice[i] = new cJewerly(pCurStorePrice[i]._name, pCurStorePrice[i].value);
        }

        weapon = new cAxe(pAxe);
        skillLevel = new byte[4];
        for (byte i = 0; i < skillLevel.Length; i++)
            skillLevel[i] = pSkillLevel[i];

        flaged = new bool[15];
        for (byte i = 0; i < flaged.Length; i++)
            flaged[i] = pFlaged[i];

        bossDone = new bool[5];
        for (byte i = 0; i < bossDone.Length; i++)
            bossDone[i] = pBossDone[i];

        quickSlotItemNum = new short[4];
        for (byte i = 0; i < quickSlotItemNum.Length; i++)
            quickSlotItemNum[i] = pQuickSlotItemNum[i];

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
        clothCoupon = new cProperty("clothCoupon", pPi.clothCoupon.value);
        myWeaponsId = new bool[pPi.myWeaponsId.Length];
        for (byte i = 0; i < pPi.myWeaponsId.Length; i++)
            myWeaponsId[i] = pPi.myWeaponsId[i];
        curWeaponId = pPi.curWeaponId;
        myClothesId = new bool[pPi.myClothesId.Length];
        for (byte i = 0; i < pPi.myClothesId.Length; i++)
            myClothesId[i] = pPi.myClothesId[i];
        curClothId = pPi.curClothId;

        prevStorePrice = new cJewerly[5];
        avStorePrice = new cJewerly[5];
        curStorePrice = new cJewerly[5];
        for (byte i = 0; i < 5; i++)
        {
            prevStorePrice[i] = new cJewerly(pPi.prevStorePrice[i]._name, pPi.prevStorePrice[i].value);
            avStorePrice[i] = new cJewerly(pPi.avStorePrice[i]._name, pPi.avStorePrice[i].value);
            curStorePrice[i] = new cJewerly(pPi.curStorePrice[i]._name, pPi.curStorePrice[i].value);
        }            

        weapon = new cAxe(pPi.weapon);
        skillLevel = new byte[4];
        for (byte i = 0; i < skillLevel.Length; i++)
            skillLevel[i] = pPi.skillLevel[i];

        flaged = new bool[15];
        for (byte i = 0; i < flaged.Length; i++)
            flaged[i] = pPi.flaged[i];

        bossDone = new bool[5];
        for (byte i = 0; i < bossDone.Length; i++)
            bossDone[i] = pPi.bossDone[i];

        quickSlotItemNum = new short[4];
        for (byte i = 0; i < quickSlotItemNum.Length; i++)
            quickSlotItemNum[i] = pPi.quickSlotItemNum[i];

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
            item_equip[i] = new cItem_equip(inventory.GetItemEquip()[i]);
        }

        item_use = new cItem_use[inventory.GetItemUse().Count];
        for (byte i = 0; i < inventory.GetItemUse().Count; i++)
        {
            item_use[i] = new cItem_use(inventory.GetItemUse()[i]);
        }

        item_etc = new cItem_etc[inventory.GetItemEtc().Count];
        for (byte i = 0; i < inventory.GetItemEtc().Count; i++)
        {
            item_etc[i] = new cItem_etc(inventory.GetItemEtc()[i]);
        }
    }

    public void UpdateStorePrice()
    {
        // 실시간 가격 변동
        // y = ax ^ 4 + b
        // y : 가격, a: 기울기, x: 확률변수, b: 평균값
        float maxPrice;
        float lowPrice;
        float maxX = 0;
        float minX = 0;
        float a = 1;
        float randomNum;
        float totalPrice;


        for(byte i = 0; i < 5; i++)
        {
            lowPrice = avStorePrice[i].value - (avStorePrice[i].value * 0.9f);
            maxPrice = avStorePrice[i].value + (avStorePrice[i].value * 0.9f);

            minX = Mathf.Pow((maxPrice - avStorePrice[i].value) / a, 0.25f) * -1;
            maxX = Mathf.Pow((maxPrice - avStorePrice[i].value) / a, 0.25f);
            randomNum = Random.Range(minX, maxX);

            totalPrice = Mathf.Pow(randomNum, 4) + avStorePrice[i].value;

            cUtil._user._playerInfo.prevStorePrice[i].value = cUtil._user._playerInfo.curStorePrice[i].value;
            cUtil._user._playerInfo.curStorePrice[i].value = (long)totalPrice;
        }

        if(store != null)
        {
            store.UpdateValue();
        }
    }
}
