using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cFlag : MonoBehaviour
{
    public Button b_nextGate;
    private Button[] b_flagOn;

    private void Start()
    {
        b_flagOn = new Button[this.transform.childCount];

        for (byte i = 0; i < b_flagOn.Length; i++)
        {
            byte k = i;
            b_flagOn[i] = this.transform.GetChild(i).Find("b_flag").GetComponent<Button>();
            b_flagOn[i].onClick.AddListener(() => FlagOn(k));
        }

        b_nextGate.onClick.AddListener(() => OpenNextGate());
    }

    private void FlagOn(byte pNum)
    {
        if (cUtil._user._playerInfo.flaged[pNum].Equals(true))
            return;

        this.transform.GetChild(pNum).GetComponent<Animator>().SetTrigger("FlagOn");
        cUtil._user._playerInfo.flaged[pNum] = true;

        //스테이지에 따라 flaged 변수 확인하고 ShowNextGate()


    }

    //게이트 열림
    private void ShowNextGate()
    {
        b_nextGate.transform.GetChild(0).gameObject.SetActive(false);
    }

    //다음 스테이지로
    private void OpenNextGate()
    {

    }
}
