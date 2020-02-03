using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public struct StoreJewerlyList
{
    //0 : 평균, 1 : 현재, 2: 변동
    public cJewerly[] jewerlyValue;
}

public class cStore : cBuilding
{
    //판매버튼
    public Button[] b_sell;
    //시간이동 버튼
    public Button b_time;
    //전체 판매버튼
    public Button b_sellAll;

    //보석별 value 리스트
    public StoreJewerlyList[] jewerlyList;
    //보석 value 오브젝트들
    public GameObject[] obj_jewerly;

<<<<<<< HEAD
    public short curFrameIdx;
=======
    private short curFrameIdx;

    private double updateTime; // 상점 가격 변동됐던 시각
>>>>>>> dc0edf459b7e69ee584f47f79c3e4a77477b9b45
    
    private void Start()
    {
        curFrameIdx = -1;
        b_click.onClick.AddListener(() => ActiveFrame());
        
        jewerlyList = new StoreJewerlyList[5];
        for (byte i = 0; i < 5; i++)
        {
            jewerlyList[i].jewerlyValue = new cJewerly[3];
            
            for(byte k = 0; k < 3; k++)
            {
                jewerlyList[i].jewerlyValue[k] = new cJewerly();
            }

            //평균값 초기화
            jewerlyList[i].jewerlyValue[0].value += i * 1000 + 100;
            //현재값 초기화
            jewerlyList[i].jewerlyValue[1].value +=i * 1000 + 100;

            int n = i;
            b_sell[i].onClick.AddListener(() => SellJewerly(n));
        }
        updateTime = 0;
    }

    //★★★★★현재값 조정★★★★★
    private void UpdateCurValue()
    {
        // 10분 주기로 변경
        if (cUtil._titleSingleton.gameTime - updateTime >= 600.0)
        {
            updateTime = cUtil._titleSingleton.gameTime;

            for (byte i = 0; i < jewerlyList.Length; i++)
            {
                cJewerly average = jewerlyList[i].jewerlyValue[0];
                cJewerly now = jewerlyList[i].jewerlyValue[1];
                short[] tempPrev = now.value;
                short[] tempDiff;
                
                // 0:폭락, 1:하락, 2:유지, 3:상승, 4:급상승 결정
                byte IncOrDec = (byte)Random.Range((int)0, (int)4);

                now.value = average.value;
                short changeAmount = now.value[(int)now.GetMaxIdx()];

                switch (IncOrDec)
                {
                    case 0:
                        changeAmount = (short)(changeAmount / 4);
                        now.RemoveValue(now.GetMaxIdx(), changeAmount, false);
                        break;
                    case 1:
                        changeAmount = (short)(changeAmount / 8);
                        now.RemoveValue(now.GetMaxIdx(), changeAmount, false);
                        break;
                    case 2:
                        break;
                    case 3:
                        changeAmount = (short)(changeAmount / 8);
                        now.AddValue(now.GetMaxIdx(), changeAmount);
                        break;
                    case 4:
                        changeAmount = (short)(changeAmount / 4);
                        now.AddValue(now.GetMaxIdx(), changeAmount);
                        break;
                }

                tempDiff = now.value;

                for (int j = 0; j < tempPrev.Length; j++)
                {
                    tempDiff[j] = (short)(tempDiff[j] - tempPrev[j]);
                }

                jewerlyList[i].jewerlyValue[2].value = tempDiff;
            }
        }
    }

    //보석 판매
    private void SellJewerly(int pChar)
    {
        //추가할 골드
        cGold tGold = new cGold();
        //추가 완료했냐
        long amount = cUtil._user._playerInfo.inventory.GetJewerly()[pChar].value;

        cUtil._user._playerInfo.inventory.GetJewerly()[pChar].value -= amount;
        cUtil._user._playerInfo.inventory.GetMoney().value += jewerlyList[pChar].jewerlyValue[1].value * amount;

        UpdateSellButton();
        UpdateMyValue();
    }

    //판매버튼 업데이트
    private void UpdateSellButton()
    {
        for(byte i = 0; i < 5; i++)
        {
            //보유량 없으면 하위 이미지 Active
            if (cUtil._user._playerInfo.inventory.GetJewerly()[i].GetValueToString().Equals("0"))
                b_sell[i].transform.GetChild(0).gameObject.SetActive(true);
            //있으면 inActive
            else
                b_sell[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    //보석 보유량 업데이트
    private void UpdateMyValue()
    {
        for (byte i = 0; i < 5; i++)
        {
            obj_jewerly[i].transform.GetChild(5).GetComponent<Text>().text =
                cUtil._user._playerInfo.inventory.GetJewerly()[i].GetValueToString();
        }
    }

    //모든 Value 업데이트
    private void UpdateValue()
    {
        for (byte i = 0; i < 5; i++)
        {
            //평균값
            obj_jewerly[i].transform.GetChild(2).GetComponent<Text>().text = jewerlyList[i].jewerlyValue[0].GetValueToString();
            //현재값
            obj_jewerly[i].transform.GetChild(3).GetComponent<Text>().text = jewerlyList[i].jewerlyValue[1].GetValueToString();
            //변동값
            jewerlyList[i].jewerlyValue[2].value = jewerlyList[i].jewerlyValue[1].value - jewerlyList[i].jewerlyValue[0].value;
            Debug.Log(jewerlyList[i].jewerlyValue[2].value);
            //변동값이 플러스일 때
            if (jewerlyList[i].jewerlyValue[2].value > 0)
            {
                obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().text = "+";
                obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().color = new Color((float)(150.0f / 255.0f), 1, 0);
            }
            //변동값이 마이너스일 때
            else if(jewerlyList[i].jewerlyValue[2].value < 0)
            {
                obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().text = "";
                obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().color = new Color(1, 0, 0);
            }            
            else
            {
                obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().text = "";
                obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().color = new Color(0.3f, 0.3f, 0.3f);
            }

            obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().text +=
                 jewerlyList[i].jewerlyValue[2].GetValueToString();

            //보유량
            obj_jewerly[i].transform.GetChild(5).GetComponent<Text>().text =
                cUtil._user._playerInfo.inventory.GetJewerly()[i].GetValueToString();
        }
    }

    private void ActiveFrame()
    {
        if (curFrameIdx.Equals(0))
        {
            Debug.Log("같은 프레임입니다.");
            return;
        }

        curFrameIdx = 0;
        obj_content.SetActive(true);

        UpdateValue();
        UpdateSellButton();
    }
}
