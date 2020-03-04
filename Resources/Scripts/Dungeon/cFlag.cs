using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cFlag : MonoBehaviour
{
    public cGate nextGate;
    public cGate prevGate;
    public cGate bossGate;
    public cCamera cam;
    
    public void Init()
    {                
        cam = GameObject.Find("DungeonNormalScene").transform.Find("Cam_main").GetComponent<cCamera>();

        if(nextGate.gameObject != null)
            cam.trans_nextGate = nextGate.transform;

        //스테이지 점령지를 모두 점령하였다면 게이트 오픈
        if (cUtil._user._playerInfo.flaged[0 + (((int)cUtil._sm._scene - 3))].Equals(true))
        {
            this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("FlagOn");

            nextGate.GetComponent<Animator>().SetTrigger("OpenGate");
            nextGate.GetComponent<Button>().interactable = true;
        }
        else
            nextGate.GetComponent<Button>().interactable = false;
    }

    public void FlagOn(byte pNum)
    {
        if (cUtil._user._playerInfo.flaged[0 + (((int)cUtil._sm._scene - 3))].Equals(true))
            return;

        this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("FlagOn");
        cUtil._user._playerInfo.flaged[0 + (((int)cUtil._sm._scene - 3))] = true;

        //스테이지 점령지를 모두 점령하였다면 게이트 오픈
        if (cUtil._user._playerInfo.flaged[0 + (((int)cUtil._sm._scene - 3))].Equals(true))
        {
            cam.ShowNextGate();
            nextGate.GetComponent<Button>().interactable = true;
        }
    }

    //보스 게이트 클릭
    private void GoToBoss(bool isNextDoor)
    {
        cUtil._sm.GoToBossGate(isNextDoor);
    }
}
