using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cTitleSingleton : MonoBehaviour
{
    public static cTitleSingleton Instance;
    public Button b_save;
    public double gameTime; // 게임 플레이 시간

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

        gameTime = 0;

        cUtil._titleSingleton = this;
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
