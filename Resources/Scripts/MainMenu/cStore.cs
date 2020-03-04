using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cStore : cBuilding
{
    //판매버튼
    public Button[] b_sell;
    //시간이동 버튼
    public Button b_time;
    //전체 판매버튼
    public Button b_sellAll;

    //보석 value 오브젝트들
    public GameObject[] obj_jewerly;

    public short curFrameIdx;

    public cMain_processor mp;
    
    private void Start()
    {
        curFrameIdx = -1;
        b_click.onClick.AddListener(() => ActiveFrame());
        b_sellAll.onClick.AddListener(() => SellAllJewerly());

        for (byte i = 0; i < 5; i++)
        {
            int n = i;
            b_sell[i].onClick.AddListener(() => SellJewerly(n));
        }
    }

    private void OnEnable()
    {
        UpdateSellButton();
        UpdateMyValue();
    }

    //★★★★★현재값 조정★★★★★
    private void UpdateCurValue()
    {
        // 30초 주기로 변경
        
    }

    //보석 판매
    private void SellJewerly(int pChar)
    {
        //추가 완료했냐
        long amount = cUtil._user._playerInfo.inventory.GetJewerly()[pChar].value;

        cUtil._user._playerInfo.inventory.GetJewerly()[pChar].value -= amount;
        cUtil._user._playerInfo.inventory.GetMoney().value += cUtil._user._playerInfo.curStorePrice[pChar].value * amount;

        UpdateSellButton();
        UpdateMyValue();
    }

    private void SellAllJewerly()
    {
        for(byte i = 0; i < 5; i++)
        {
            //없으면 continue
            if (cUtil._user._playerInfo.inventory.GetJewerly()[i].value.Equals(0))
                continue;

            //추가 완료했냐
            long amount = cUtil._user._playerInfo.inventory.GetJewerly()[i].value;

            cUtil._user._playerInfo.inventory.GetJewerly()[i].value -= amount;
            cUtil._user._playerInfo.inventory.GetMoney().value += cUtil._user._playerInfo.curStorePrice[i].value * amount;
        }

        UpdateSellButton();
        UpdateMyValue();
    }

    //판매버튼 업데이트
    private void UpdateSellButton()
    {
        bool canSellAllOn = false;

        for(byte i = 0; i < 5; i++)
        {
            //보유량 없으면 하위 이미지 Active
            if (cUtil._user._playerInfo.inventory.GetJewerly()[i].GetValueToString().Equals("0"))
                b_sell[i].transform.GetChild(0).gameObject.SetActive(true);
            //있으면 inActive
            else
            {
                b_sell[i].transform.GetChild(0).gameObject.SetActive(false);
                canSellAllOn = true;
            }
        }

        //전체 판매 버튼
        if (canSellAllOn.Equals(true))
            b_sellAll.transform.GetChild(0).gameObject.SetActive(false);
        else
            b_sellAll.transform.GetChild(0).gameObject.SetActive(true);
    }

    //보석 보유량 업데이트
    private void UpdateMyValue()
    {
        for (byte i = 0; i < 5; i++)
        {
            obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().text = cUtil._user._playerInfo.inventory.GetJewerly()[i].GetValueToString();
        }

        mp.UpdateValues();
        UpdateSellProperty();
    }

    //예상 수익 업데이트
    private void UpdateSellProperty()
    {
        cProperty sellProperty = new cProperty("adsf", 0);
        
        for (byte i = 0; i < 5; i++)
        {
            sellProperty.value = cUtil._user._playerInfo.curStorePrice[i].value *
                cUtil._user._playerInfo.inventory.GetJewerly()[i].value;

            obj_jewerly[i].transform.GetChild(5).GetComponent<Text>().text = sellProperty.GetValueToString();
        }
    }

    //모든 Value 업데이트
    public void UpdateValue()
    {
        cJewerly tempJ = new cJewerly();

        for (byte i = 0; i < 5; i++)
        {
            //현재값
            obj_jewerly[i].transform.GetChild(2).GetComponent<Text>().text = cUtil._user._playerInfo.curStorePrice[i].GetValueToString();

            //변동값
            tempJ.value = cUtil._user._playerInfo.curStorePrice[i].value - cUtil._user._playerInfo.prevStorePrice[i].value;

            //변동값이 플러스일 때
            if (tempJ.value > 0)
            {
                obj_jewerly[i].transform.GetChild(3).GetComponent<Text>().text = "+";
                obj_jewerly[i].transform.GetChild(3).GetComponent<Text>().color = new Color((float)(150.0f / 255.0f), 1, 0);
            }

            //변동값이 마이너스일 때
            else if(tempJ.value < 0)
            {
                obj_jewerly[i].transform.GetChild(3).GetComponent<Text>().text = "";
                obj_jewerly[i].transform.GetChild(3).GetComponent<Text>().color = new Color(1, 0, 0);
            }            
            else
            {
                obj_jewerly[i].transform.GetChild(3).GetComponent<Text>().text = "";
                obj_jewerly[i].transform.GetChild(3).GetComponent<Text>().color = new Color(0.3f, 0.3f, 0.3f);
            }

            obj_jewerly[i].transform.GetChild(3).GetComponent<Text>().text +=
                 tempJ.GetValueToString();

            //보유량
            obj_jewerly[i].transform.GetChild(4).GetComponent<Text>().text =
                cUtil._user._playerInfo.inventory.GetJewerly()[i].GetValueToString();
        }

        UpdateSellProperty();
        UpdateSellButton();
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
