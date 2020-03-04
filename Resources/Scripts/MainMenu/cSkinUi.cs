using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cSkinUi : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => GoToSkinScene());
    }

    private void GoToSkinScene()
    {
        cUtil._sm.ChangeScene("Main_skin");
    }
}
