using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cBag : cBuilding
{
    public GameObject obj_parentGroup;
    public GameObject obj_itemInfoPopUp;
    public GameObject obj_setQuickSlot;
    private short selectedItemNum;

    public cBtn_item quickSlot;

    public void OpenBag()
    {
        selectedItemNum = -1;

        UpdateBagItemInfo();

        obj_content.SetActive(true);
    }

    public void UpdateBagItemInfo()
    {
        //박스 초기화
        byte useItemNum = (byte)citemTable.GetUseItemTotalNum();
        for (byte i = 0; i < useItemNum; i++)
        {
            if(obj_parentGroup.transform.GetChild(i).gameObject != null)
                obj_parentGroup.transform.GetChild(i).gameObject.SetActive(false);        
        }

        //아이템 정보 불러오기
        byte itemNum = (byte)cUtil._user.GetInventory().GetItemUse().Count;
        for (byte i = 0; i < itemNum; i++)
        {
            //수량
            //수량이 없다면 continue;
            if (cUtil._user.GetInventory().GetItemUse()[i].amount < 1)
            {
                obj_parentGroup.transform.GetChild(i).gameObject.SetActive(false);
                continue;
            }
            obj_parentGroup.transform.GetChild(i).GetChild(1).GetComponent<Text>().text =
                cUtil._user.GetInventory().GetItemUse()[i].amount.ToString();
            //이미지
            obj_parentGroup.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite =
                Resources.LoadAll<Sprite>("Images/Main/img_items")[cUtil._user.GetInventory().GetItemUse()[i].kind];
            obj_parentGroup.transform.GetChild(i).GetChild(0).GetComponent<Image>().SetNativeSize();
            //퀵슬롯 여부
            for (byte m = 0; m < 4; m++)
            {
                if (cUtil._user._playerInfo.quickSlotItemNum[m].Equals(cUtil._user.GetInventory().GetItemUse()[i].kind))
                {
                    obj_parentGroup.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                    obj_parentGroup.transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
                    obj_parentGroup.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = "퀵슬롯 " + (m + 1).ToString();
                    break;
                }
                else
                {
                    obj_parentGroup.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
                    obj_parentGroup.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                }
            }
            //상제정보 클릭 버튼
            byte k = (byte)cUtil._user.GetInventory().GetItemUse()[i].kind;
            obj_parentGroup.transform.GetChild(i).GetChild(4).GetComponent<Button>().onClick.
                AddListener(() => PopUpItemInfo(k));

            obj_parentGroup.transform.GetChild(i).gameObject.SetActive(true);
        }

    }

    public void PopUpItemInfo(byte pNum)
    {
        selectedItemNum = pNum;

        //팝업 종료 버튼
        obj_itemInfoPopUp.transform.GetChild(0).GetComponent<Button>().onClick.
            AddListener(() => ExitPopUp(obj_itemInfoPopUp.transform.GetChild(0).gameObject));

        //이미지
        obj_itemInfoPopUp.transform.GetChild(3).GetComponent<Image>().sprite =
            Resources.LoadAll<Sprite>("Images/Main/img_items")[pNum];
        obj_itemInfoPopUp.transform.GetChild(3).GetComponent<Image>().SetNativeSize();
        //이름
        obj_itemInfoPopUp.transform.GetChild(2).GetComponent<Text>().text =
            cUtil._user.GetInventory().GetItemUse()[pNum]._name;
        //설명

        //퀵슬롯 지정 버튼
        obj_itemInfoPopUp.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => PopUpQuickSlot());

        obj_itemInfoPopUp.SetActive(true);
    }

   public void PopUpQuickSlot()
    {
        //아이템 정보창 삭제
        obj_itemInfoPopUp.SetActive(false);

        //팝업 종료 버튼
        obj_setQuickSlot.transform.GetChild(0).GetComponent<Button>().onClick.
            AddListener(() => ExitPopUp(obj_setQuickSlot.transform.GetChild(0).gameObject));

        for (byte i = 0; i < 4; i++)
        {
            //퀵슬롯 설정
            byte k = i;
            obj_setQuickSlot.transform.GetChild(i + 3).GetChild(2).GetComponent<Button>().onClick.
                AddListener(() => SetQuickSlot(k));

            //이미 지정된 단축키 아이템 표시
            if (cUtil._user._playerInfo.quickSlotItemNum[i] != -1)
            {
                obj_setQuickSlot.transform.GetChild(i + 3).GetChild(1).GetComponent<Image>().sprite =
                    Resources.LoadAll<Sprite>("Images/Main/img_items")[cUtil._user._playerInfo.quickSlotItemNum[i]];
                obj_setQuickSlot.transform.GetChild(i + 3).GetChild(1).gameObject.SetActive(true);
            }
            else
                obj_setQuickSlot.transform.GetChild(i + 3).GetChild(1).gameObject.SetActive(false);
        }
        
        obj_setQuickSlot.SetActive(true);
    }

    public void SetQuickSlot(byte pSlotNum)
    {
        for (byte i = 0; i < 4; i++)
            if (cUtil._user._playerInfo.quickSlotItemNum[i].Equals(selectedItemNum))
                cUtil._user._playerInfo.quickSlotItemNum[i] = -1;

        cUtil._user._playerInfo.quickSlotItemNum[pSlotNum] = selectedItemNum;
        if(quickSlot != null)
            quickSlot.UpdateQuickSlot();

        //퀵슬롯 여부
        for(byte i = 0; i < cUtil._user.GetInventory().GetItemUse().Count; i++)
        {
            for (byte m = 0; m < 4; m++)
            {
                if (cUtil._user._playerInfo.quickSlotItemNum[m].Equals(cUtil._user.GetInventory().GetItemUse()[i].kind))
                {
                    obj_parentGroup.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                    obj_parentGroup.transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
                    obj_parentGroup.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = "퀵슬롯 " + (m + 1).ToString();
                    break;
                }
                else
                {
                    obj_parentGroup.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
                    obj_parentGroup.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                }
            }
        }        

        obj_setQuickSlot.SetActive(false);
    }

    public void ExitPopUp(GameObject pObj)
    {
        selectedItemNum = -1;
        pObj.transform.parent.gameObject.SetActive(false);
    }
}
