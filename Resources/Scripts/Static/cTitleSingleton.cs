﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cTitleSingleton : MonoBehaviour
{
    public static cTitleSingleton Instance;
    public Button b_save;
<<<<<<< HEAD
=======
    public Button b_Plus;
    public double gameTime; // 게임 플레이 시간
>>>>>>> dc0edf459b7e69ee584f47f79c3e4a77477b9b45

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        //해상도 설정
        Screen.SetResolution(1920, 1080, true);

        //static 클래스 설정
        cUtil._sm = this.transform.Find("SceneManager").GetComponent<cSceneManager>();
        cUtil._user = this.GetComponent<cUser>();
        b_save.onClick.AddListener(() => cUtil._user.SaveUserData());
<<<<<<< HEAD
=======
        b_Plus.onClick.AddListener(() => cUtil._user.GetInventory().GetMoney().AddValue(0, 200));

        gameTime = 0;

        cUtil._titleSingleton = this;
>>>>>>> dc0edf459b7e69ee584f47f79c3e4a77477b9b45
    }

    private void FixedUpdate()
    {
        gameTime += Time.deltaTime;

        // 오버플로우 나기 전에 초기화
        if (gameTime >= double.MaxValue - 60.0)
        {
            gameTime = 0;
        }
    }
}
