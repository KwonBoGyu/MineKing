using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cFlag : MonoBehaviour
{
    public cGate nextGate;
    public cGate prevGate;
    public cGate bossGate;
    private Button[] b_flagOn;
    public cCamera cam;
    
    private void Start()
    {
        b_flagOn = new Button[this.transform.childCount];

        for (byte i = 0; i < b_flagOn.Length; i++)
        {
            byte k = i;
            b_flagOn[i] = this.transform.GetChild(i).Find("b_flag").GetComponent<Button>();
            b_flagOn[i].onClick.AddListener(() => FlagOn(k));
        }
                
        cam = GameObject.Find("DungeonNormalScene").transform.Find("Cam_main").GetComponent<cCamera>();

        if(nextGate.gameObject != null)
            cam.trans_nextGate = nextGate.transform;

        //스테이지 점령지를 모두 점령하였다면 게이트 오픈
        if (cUtil._user._playerInfo.flaged[0 + (((int)cUtil._sm._scene - 2) * 3)].Equals(true) &&
            cUtil._user._playerInfo.flaged[1 + (((int)cUtil._sm._scene - 2) * 3)].Equals(true) &&
            cUtil._user._playerInfo.flaged[2 + (((int)cUtil._sm._scene - 2) * 3)].Equals(true))
        {
            this.transform.GetChild(0).GetComponent<Animator>().SetTrigger("FlagOn");
            this.transform.GetChild(1).GetComponent<Animator>().SetTrigger("FlagOn");
            this.transform.GetChild(2).GetComponent<Animator>().SetTrigger("FlagOn");

            nextGate.GetComponent<Animator>().SetTrigger("OpenGate");
            nextGate.GetComponent<Button>().interactable = true;
        }
        else
            nextGate.GetComponent<Button>().interactable = false;
    }

    private void FlagOn(byte pNum)
    {
        if (cUtil._user._playerInfo.flaged[pNum + (((int)cUtil._sm._scene - 2) * 3)].Equals(true))
            return;

        this.transform.GetChild(pNum).GetComponent<Animator>().SetTrigger("FlagOn");
        cUtil._user._playerInfo.flaged[pNum + (((int)cUtil._sm._scene - 2) * 3)] = true;

        //스테이지 점령지를 모두 점령하였다면 게이트 오픈
        if (cUtil._user._playerInfo.flaged[0 + (((int)cUtil._sm._scene - 2) * 3)].Equals(true) &&
            cUtil._user._playerInfo.flaged[1 + (((int)cUtil._sm._scene - 2) * 3)].Equals(true) &&
            cUtil._user._playerInfo.flaged[2 + (((int)cUtil._sm._scene - 2) * 3)].Equals(true))
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
