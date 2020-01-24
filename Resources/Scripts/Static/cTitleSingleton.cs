using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cTitleSingleton : MonoBehaviour
{
    public static cTitleSingleton Instance;
    public Button b_save;
    public Button b_Plus;

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
        b_Plus.onClick.AddListener(() => cUtil._user.GetInventory().GetMoney().AddValue(0, 200));
    }


}
