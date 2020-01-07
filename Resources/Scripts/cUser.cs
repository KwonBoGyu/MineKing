using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cUser : MonoBehaviour
{
    private cPlayerInfo _playerInfo;
    
    void Start()
    {
        LoadUserData();
    }

    public string GetPlayerName() { return _playerInfo.nickName; }
<<<<<<< HEAD
=======
    public cPlayer GetPlayer() { return _playerInfo.player; }
    public void SetPlayer(cPlayer pP) { _playerInfo.player = pP; }
>>>>>>> ad1a998ea032c6642e840e5b266f9fff114279e7
    public cInventory GetInventory() { return _playerInfo.inventory; }

    #region 데이터 저장&불러오기
    public void SaveUserData()
    {
        _playerInfo.SyncData();
        cSaveLoad.SaveData("saves", _playerInfo);
    }

    public void LoadUserData()
    {
        bool _fileExist = true;

        string fileDir = Application.persistentDataPath + "/Saves";
        System.IO.DirectoryInfo _dr = new System.IO.DirectoryInfo(fileDir);

        if (_dr.Exists == false)
            _dr.Create();

        cSaveLoad.LoadData<cPlayerInfo>("saves", ref _fileExist);

        //생성된 데이터가 없을 때
        if (_fileExist == false)
        {
            _playerInfo = new cPlayerInfo("닉네임", 5, 1, 1, 1, null, null, null);
            SaveUserData();
            Debug.Log("Initialized Done - CreatedInitData");
        }
        //기존 데이터가 있으면..
        else
        {
            _playerInfo = new cPlayerInfo(cSaveLoad.LoadData<cPlayerInfo>("saves"));
            Debug.Log("Initialized Done - Data Exists");
        }        
    }
    #endregion
}
