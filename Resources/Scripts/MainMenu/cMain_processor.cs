using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cMain_processor : MonoBehaviour
{
    //메인 애니메이터
    public Animator animator_main;
    //사운드 매니저
    public cSoundMng _sm;

    //메인 프레임
    public GameObject obj_mainFrame;
    //메인 프레임 닫기 버튼
    public Button[] b_mainFrameExit;

    //각 요소별 제어 스크립트
    public cStore _store;
    public cForge _forge;
    public cDungeon _dungeon;
    public cBag _bag;
    
    //재화
    public Text t_gold;
    public Text t_rock;
    public Text t_dia;

    //현재 창 인덱스
    private int currentWindowNum;

    void Start()
    {
        currentWindowNum = -1;

        cUtil._user._playerInfo.store = _store;

        _store.b_click.onClick.AddListener(() => OnBuilingButtonClicked(0));
        _forge.b_click.onClick.AddListener(() => OnBuilingButtonClicked(1));
        _dungeon.b_click.onClick.AddListener(() => OnBuilingButtonClicked(2));
        _bag.b_click.onClick.AddListener(() => OnBuilingButtonClicked(3));
        for(byte i = 0; i < b_mainFrameExit.Length; i++)
            b_mainFrameExit[i].onClick.AddListener(() => OnMainFrameExit());

        UpdateValues();
    }
        
    private void OnBuilingButtonClicked(int pChar)
    {
        //창이 떠있는데 다른 버튼 클릭하면 리턴
        if (currentWindowNum != -1)
            return;
        //같은 창이라면 exit
        if(currentWindowNum.Equals(pChar))
        {
            OnMainFrameExit();
            return;
        }
        
        obj_mainFrame.SetActive(true);

        switch (pChar)
        {
            //상점
            case 0:
                
                  _sm.playEffect(0);
                  animator_main.SetTrigger("StoreOn");
                _store.UpdateValue();                                
                    break;
            //대장간
            case 1:
                _sm.playEffect(0);
                animator_main.SetTrigger("IronOn");
                break;
            //던전
            case 2:
                _sm.playEffect(0);
                animator_main.SetTrigger("DungeonOn");
                break;
            //가방
            case 3:
                _bag.OpenBag();
                b_mainFrameExit[0].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                b_mainFrameExit[5].gameObject.SetActive(true);
                b_mainFrameExit[6].gameObject.SetActive(true);
                break;
        }

        currentWindowNum = pChar;
    }

    private void OnMainFrameExit()
    {
        switch (currentWindowNum)
        {
            //상점
            case 0:                
                _sm.playEffect(0);
                animator_main.SetTrigger("StoreOff");
                _store.curFrameIdx = -1;
                break;
            //대장간
            case 1:
                _sm.playEffect(0);
                animator_main.SetTrigger("IronOff");
                break;
            //던전
            case 2:
                _sm.playEffect(0);
                animator_main.SetTrigger("DungeonOff");
                break;
            //가방
            case 3:
                _bag.obj_content.SetActive(false);
                b_mainFrameExit[0].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                b_mainFrameExit[5].gameObject.SetActive(false);
                b_mainFrameExit[6].gameObject.SetActive(false);
                break;
        }
        currentWindowNum = -1;
    }
	
    public void UpdateValues()
    {
        t_gold.text = cUtil._user._playerInfo.inventory.GetMoney().GetValueToString();
        t_rock.text = cUtil._user._playerInfo.inventory.GetRock().GetValueToString();
        t_dia.text = cUtil._user._playerInfo.inventory.GetDia().GetValueToString();
    }

}
