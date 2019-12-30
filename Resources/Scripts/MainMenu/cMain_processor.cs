using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cMain_processor : MonoBehaviour
{
    public GameObject obj_mainFrame;
    public Button b_mainFrameExit;

    public cStore _store;
    public cForge _forge;
    public cDungeon _dungeon;

    public Button b_temp1;
    public Button b_temp2;
    public Button b_temp3;

    private int currentWindowNum;

    void Start()
    {
        currentWindowNum = -1;

        _store.b_click.onClick.AddListener(() => OnBuilingButtonClicked(0));
        _forge.b_click.onClick.AddListener(() => OnBuilingButtonClicked(1));
        _dungeon.b_click.onClick.AddListener(() => OnBuilingButtonClicked(2));
        b_mainFrameExit.onClick.AddListener(() => OnMainFrameExit());

        //임시 디버깅
        b_temp1.onClick.AddListener(() => AddItem(0));
        b_temp2.onClick.AddListener(() => AddItem(1));
        b_temp3.onClick.AddListener(() => AddItem(2));
    }

    //임시 디버깅용 아이템 추가
    private void AddItem(int pKind)
    {
        switch(pKind)
        {
            case 0:
                cItem pEquip;
                citemTable.GetItemInfo(out pEquip, 0);
                cUtil._user.GetInventory().GetItemEquip().Add((cItem_equip)pEquip);
                break;

            case 1:
                cItem pUse;
                citemTable.GetItemInfo(out pUse, 1);

                bool exist = false;
                for(int i = 0; i < cUtil._user.GetInventory().GetItemUse().Count; i++)
                {
                    if (cUtil._user.GetInventory().GetItemUse().Contains((cItem_use)pUse))
                    {
                        cUtil._user.GetInventory().GetItemUse()[i].amount += 1;
                        exist = true;
                        break;
                    }
                } 

                if(exist == false)
                    cUtil._user.GetInventory().GetItemUse().Add((cItem_use)pUse);
                break;

            case 2:
                cItem pEtc;
                citemTable.GetItemInfo(out pEtc, 2);
                cUtil._user.GetInventory().GetItemEtc().Add((cItem_etc)pEtc);
                break;
        }
    }
    
    private void OnBuilingButtonClicked(int pChar)
    {
        //같은 창이라면 리턴, 창이 떠있는데 다른 버튼 클릭하면 안됨
        if (currentWindowNum == pChar || currentWindowNum != -1)
            return;
        
        obj_mainFrame.SetActive(true);

        switch (pChar)
        {
            //상점
            case 0: _store.obj_content.SetActive(true); break;
            //대장간
            case 1: _forge.obj_content.SetActive(true); break;
            //던전
            case 2: _dungeon.obj_content.SetActive(true); break;
        }

        currentWindowNum = pChar;
    }

    private void OnMainFrameExit()
    {
        _store.obj_content.SetActive(false);
        _forge.obj_content.SetActive(false);
        _dungeon.obj_content.SetActive(false);

        obj_mainFrame.SetActive(false);
        currentWindowNum = -1;
    }
	
}
