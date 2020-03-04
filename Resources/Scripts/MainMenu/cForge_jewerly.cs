using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cForge_jewerly : MonoBehaviour
{
    private float[] jewerlyPercent;
    public Text[] jewerlyPlusPerecent;
    public Text[] t_curJewerly;
    public Text[] t_percent;
    public Text t_myRock;
    public Button b_extract;
    public Button b_percentage;
    public GameObject obj_percentage;
    public Button b_exitPercentage;

    public GameObject obj_extractPopup;
    public GameObject obj_extractJewerlyParent;
    public Button b_exitResult;
    public GameObject img_clickBarrier;

    private float[] percentTable;

    public cMain_processor mp;

    private void Start()
    {
        jewerlyPercent = new float[5];

        CalculatePercent();
        b_extract.onClick.AddListener(() => ExtractRock());
        b_percentage.onClick.AddListener(() => PopUpPercentageFrame());
        b_exitPercentage.onClick.AddListener(() => ExitPercentage());
        b_exitResult.onClick.AddListener(() => ExitResultFrame());

        percentTable = new float[] {0.05f, 0.05f, 0.03f, 0.01f, 0.005f}; // 보석 종류별 광석 1개당 추출 계수
        UpdateValues();
    }

    private void OnEnable()
    {
        UpdateValues();
    }

    //★★★★★확률 계산★★★★★
    private void CalculatePercent()
    {
        jewerlyPercent[4] = 0.05f;
        jewerlyPercent[3] = 0.45f;
        jewerlyPercent[2] = 4.5f;
        jewerlyPercent[1] = 15.0f;
        jewerlyPercent[0] = 80.0f;

        // 보석 종류별
        for (byte i = 0; i < 5; i++)
        {
            //소수점 1자리까지만
            t_percent[i].text = string.Format("{0:F2}", jewerlyPercent[i]) + "%";
        }
    }

    //Value들 업데이트
    public void UpdateValues()
    {
        //광물 보유량
        t_myRock.text = string.Format("광석 보유량 : {0}", cUtil._user._playerInfo.inventory.GetRock().GetValueToString());
        if (cUtil._user._playerInfo.inventory.GetRock().value.Equals(0))
        {
            b_extract.interactable = false;
            b_extract.transform.GetChild(0).GetComponent<Text>().text = "광석 부족";
            b_extract.transform.GetChild(0).GetComponent<Text>().color = Color.red;
        }
        else
        {
            b_extract.interactable = true;
            b_extract.transform.GetChild(0).GetComponent<Text>().text = "추출";
            b_extract.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        }

        //보석 보유량
        for (byte i = 0; i < t_curJewerly.Length; i++)
            t_curJewerly[i].text = cUtil._user._playerInfo.inventory.GetJewerly()[i].GetValueToString();

        mp.UpdateValues();
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

        //광석이 있다면..
        b_extract.interactable = false;
        this.GetComponent<Animator>().SetTrigger("ExtractOn");
        img_clickBarrier.SetActive(true);
    }

    public void PopUpResultFrame()
    {
        //결과값 도출
        float factor = 0;
        long[] addJwerly = new long[5];

        for(long i = 0; i < cUtil._user._playerInfo.inventory.GetRock().value; i++)
        {
            factor = Random.Range(0.0f, 100.0f);

            if(factor <= jewerlyPercent[0])            
                addJwerly[0] += 1;            
            else if(factor > jewerlyPercent[0] && factor <= (jewerlyPercent[0] + jewerlyPercent[1]))            
                addJwerly[1] += 1;
            else if (factor > (jewerlyPercent[0] + jewerlyPercent[1]) && factor <= (jewerlyPercent[0] + jewerlyPercent[1] + jewerlyPercent[2]))
                addJwerly[2] += 1;
            else if (factor > (jewerlyPercent[0] + jewerlyPercent[1] + jewerlyPercent[2]) && 
                factor <= (jewerlyPercent[0] + jewerlyPercent[1] + jewerlyPercent[2] + jewerlyPercent[3]))
                addJwerly[3] += 1;
            else if (factor > (jewerlyPercent[0] + jewerlyPercent[1] + jewerlyPercent[2] + jewerlyPercent[3]) &&
    factor <= (jewerlyPercent[0] + jewerlyPercent[1] + jewerlyPercent[2] + jewerlyPercent[3] + jewerlyPercent[4]))
                addJwerly[4] += 1;
        }

        for(byte i = 0; i < 5; i++)
        {
            if(addJwerly[i] != 0)
            {
                obj_extractJewerlyParent.transform.GetChild(i).gameObject.SetActive(true);
                obj_extractJewerlyParent.transform.GetChild(i).GetChild(0).GetChild(1).GetComponent<Text>().text = addJwerly[i].ToString();
                cUtil._user._playerInfo.inventory.GetJewerly()[i].value += addJwerly[i];
            }
            else
                obj_extractJewerlyParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        cUtil._user._playerInfo.inventory.GetRock().value = 0;

        img_clickBarrier.SetActive(false);
        obj_extractPopup.SetActive(true);
    }

    private void ExitResultFrame()
    {
        obj_extractPopup.SetActive(false);
        b_extract.interactable = true;

        UpdateValues();
    }
    
    //확률 정보 창
    private void PopUpPercentageFrame()
    {
        obj_percentage.SetActive(true);
    }

    private void ExitPercentage()
    {
        obj_percentage.SetActive(false);
    }
}
