using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cGate : MonoBehaviour
{
    public bool isStage; //true : 스테이지, false: 보스
    public bool isIn;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => OpenGate(isStage, isIn));

    }

    public void OpenGate(bool pId, bool pIsIn)
    {
        if(pId.Equals(true))        
            cUtil._sm.GoToStageGate(pIsIn);        
        else
        {
            //보스키 여부 조사


            cUtil._sm.GoToBossGate(pIsIn);
        }
    }
}
