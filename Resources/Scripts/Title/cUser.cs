using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cUser : MonoBehaviour
{
    public cPlayerInfo _playerInfo;
    private string saveString;
    
    void Start()
    {
        saveString = "save2";
        LoadUserData();
    }

    public string GetPlayerName() { return _playerInfo.nickName; }
    public cPlayer GetPlayer()
    {
        return _playerInfo.player;
    }
    public void SetPlayer(cPlayer pP) { _playerInfo.player = pP; }
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
            cGold money = new cGold();
            cRock rock = new cRock();
            cDia dia = new cDia();
            cJewerly[] jewerly = new cJewerly[5];
            for (byte i = 0; i < jewerly.Length; i++)
                jewerly[i] = new cJewerly();
            cSoul[] soul = new cSoul[4];
            for (byte i = 0; i < soul.Length; i++)
                soul[i] = new cSoul();

            short axeLevel = 1;
            cAxe tAxe = new cAxe(cWeaponTable.GetAxeInfo(axeLevel));

            _playerInfo = new cPlayerInfo("이름입니다.", tAxe, this.GetComponent<cInventory>(), 
                money, rock, dia, jewerly, soul);

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
