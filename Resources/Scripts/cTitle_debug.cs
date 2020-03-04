using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cTitle_debug : MonoBehaviour
{
    public InputField if_sceneNum;
    public Button b_enter;

    void Start()
    {
        b_enter.onClick.AddListener(() => EnterScene());
    }

    private void EnterScene()
    {
        int sceneNum = int.Parse(if_sceneNum.text);

        cUtil._sm.ChangeScene(sceneNum);
    }
}
