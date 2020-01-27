using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cForge_jewerly : MonoBehaviour
{
    private float[] jewerlyPercent;
    public Text[] t_curJewerly;
    public Text[] t_percent;
    public Text t_myRock;
    public Button b_extract;

    private void Start()
    {
        jewerlyPercent = new float[5];

        UpdateValues();
        CalculatePercent();
        b_extract.onClick.AddListener(() => ExtractRock());        
    }

    //★★★★★확률 계산★★★★★
    private void CalculatePercent()
    {
        for (byte i = 0; i < 5; i++)
        {
            jewerlyPercent[i] = 0.0f;
            //소수점 1자리까지만
            t_percent[i].text = string.Format("{0:F1}", jewerlyPercent[i]) + "%";
        }
    }

    //Value들 업데이트
    public void UpdateValues()
    {
        //광물 보유량
        t_myRock.text = "광물 투입량 : " + cUtil._user._playerInfo.inventory.GetRock().GetValueToString();
        //보석 보유량
        for (byte i = 0; i < t_curJewerly.Length; i++)
            t_curJewerly[i].text = cUtil._user._playerInfo.inventory.GetJewerly()[i].GetValueToString();
    }

    //보석 추출
    private void ExtractRock()
    {
        //광석이 부족하다면 리턴
        if(cUtil._user._playerInfo.inventory.GetRock().isZero().Equals(true))
        {
            Debug.Log("광석이 부족합니다.");
            return;
        }

        UpdateValues();
    }
}
