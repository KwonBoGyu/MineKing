using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cUser : MonoBehaviour
{
    public cPlayerInfo _playerInfo;
    private string saveString;

    public Sprite[] WeaponImages;
    public Sprite[] WeaponEquipImages;
    public Sprite[] ClothImages;
    
    void Start()
    {
        saveString = "save12";
        LoadUserData();
    }

    public string GetPlayerName() { return _playerInfo.nickName; }
    public cInventory GetInventory() { return _playerInfo.inventory; }

    #region 데이터 저장&불러오기
    public void SaveUserData()
    {
        _playerInfo.SyncData();
        cSaveLoad.SaveData(saveString, _playerInfo);
        Debug.Log("SAVED");
    }

    public void LoadUserData()
    {
        bool _fileExist = true;

        string fileDir = Application.persistentDataPath + "/Saves";
        System.IO.DirectoryInfo _dr = new System.IO.DirectoryInfo(fileDir);

        if (_dr.Exists == false)
            _dr.Create();

        cSaveLoad.LoadData<cPlayerInfo>(saveString, ref _fileExist);

        //생성된 데이터가 없을 때
        if (_fileExist == false)
        {
            //재화 초기화
            cGold money = new cGold();
            cRock rock = new cRock();
            cDia dia = new cDia();
            cJewerly[] jewerly = new cJewerly[5];
            for (byte i = 0; i < jewerly.Length; i++)
                jewerly[i] = new cJewerly();
            cSoul[] soul = new cSoul[4];
            for (byte i = 0; i < soul.Length; i++)
                soul[i] = new cSoul();
            //무기 레벨 초기화
            cProperty axeLevel = new cProperty("AxeLevel", 1);
            cAxe tAxe = new cAxe(cWeaponTable.GetAxeInfo(axeLevel));

            //스킬레벨 초기화
            byte[] skillLevel = new byte[4];
            for (byte i = 0; i < 4; i++)
                skillLevel[i] = 3;

            //점령지 점령 여부 초기화
            bool[] flaged = new bool[15];
            for (byte i = 0; i < flaged.Length; i++)
                flaged[i] = false;

            //보스 점령 여부 초기화
            bool[] bossDone = new bool[5];
            for (byte i = 0; i < bossDone.Length; i++)
                bossDone[i] = false;

            //퀵슬롯 아이템 넘버 초기화
            short[] quickSlotItemNum = new short[4];
            for (byte i = 0; i < 4; i++)
                quickSlotItemNum[i] = -1;

            //보석상점 가격 초기화
            cJewerly[] pPrevStorePrice = new cJewerly[5];
            cJewerly[] pAvStorePrice = new cJewerly[5];
            cJewerly[] pCurStorePrice = new cJewerly[5];
            for (byte i = 0; i < 5; i++)
            {
                pPrevStorePrice[i] = new cJewerly("보석", i * 1000 + 100);
                pAvStorePrice[i] = new cJewerly("보석", i * 1000 + 100);
                pCurStorePrice[i]= new cJewerly("보석", i * 1000 + 100);
            }

            cProperty pClothCoupon = new cProperty("clothCoupon",0);

            bool[] pMyWeaponsId = new bool[15];
            for (byte i = 0; i < pMyWeaponsId.Length; i++)
                pMyWeaponsId[i] = true;
            pMyWeaponsId[0] = true;
            pMyWeaponsId[1] = true;
            pMyWeaponsId[2] = true;

            byte pCurWeaponId = 0;

            bool[] pMyClothesId = new bool[17];
            for (byte i = 0; i < pMyClothesId.Length; i++)
                pMyClothesId[i] = true;
            pMyClothesId[0] = true;
            pMyClothesId[1] = true;
            pMyClothesId[2] = true;

            byte pCurClothId = 0;

            _playerInfo = new cPlayerInfo("이름입니다.", tAxe, skillLevel,flaged, bossDone, quickSlotItemNum, this.GetComponent<cInventory>(), 
                money, rock, dia, jewerly, soul, pPrevStorePrice, pAvStorePrice, pCurStorePrice,
                pClothCoupon, pMyWeaponsId, pCurWeaponId, pMyClothesId, pCurClothId);

            SaveUserData();
            Debug.Log("Initialized Done - CreatedInitData");
        }
        //기존 데이터가 있으면..
        else
        {
            _playerInfo = new cPlayerInfo(cSaveLoad.LoadData<cPlayerInfo>(saveString), this.GetComponent<cInventory>());
            Debug.Log("Initialized Done - Data Exists");
        }        
    }
    #endregion
}
