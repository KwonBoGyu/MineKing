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

    private float[] percentTable;

    private void Start()
    {
        jewerlyPercent = new float[5];

        UpdateValues();
        CalculatePercent();
        b_extract.onClick.AddListener(() => ExtractRock());
        
        percentTable = new float[] {0.005f, 0.005f, 0.003f, 0.001f, 0.0005f}; // 보석 종류별 광석 1개당 추출 계수
    }

    //★★★★★확률 계산★★★★★
    private void CalculatePercent()
    {
        byte maxIdx = cUtil._user.GetInventory().GetRock().GetMaxIdx();
        short[] rocks = cUtil._user.GetInventory().GetRock().GetValue();
        
        // 보석 종류별
        for (byte i = 0; i < 5; i++)
        {
            float percent = 0;
            // 광석별 순회
            for(byte j = 0; j < maxIdx; j++)
            {
                percent += percentTable[i] * rocks[j];
            }
            jewerlyPercent[i] = percent;
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
        if(cUtil._user._playerInfo.inventory.GetRock().value.Equals(0))
        {
            Debug.Log("광석이 부족합니다.");
            return;
        }

        UpdateValues();
    }
}
