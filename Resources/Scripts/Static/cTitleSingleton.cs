using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cTitleSingleton : MonoBehaviour
{
    public static cTitleSingleton Instance;
    private float storeUpdateTime;
    private float storeUpdateTimer;

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
        Screen.SetResolution(1920, 1080, false);

        //상점 업데이트 시간
        storeUpdateTime = 30;

        //static 클래스 설정
        cUtil._sm = this.transform.Find("SceneManager").GetComponent<cSceneManager>();
        cUtil._user = this.GetComponent<cUser>();


        cUtil._titleSingleton = this;
    }

    private void FixedUpdate()
    {
        //상점 가격표 변경
        storeUpdateTimer += Time.deltaTime;
        if (storeUpdateTimer > storeUpdateTime)
        {
            cUtil._user._playerInfo.UpdateStorePrice();
            storeUpdateTimer = 0;
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            cUtil._user._playerInfo.UpdateStorePrice();
        }
    }
}
