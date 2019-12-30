using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cUI_inventory : MonoBehaviour
{
    public GameObject content;
    public Button b_weapon;
    public Button b_use;
    public Button b_etc;
    public GameObject background;
    public Button b_exit;

    public Text t_temp;

    private void Start()
    {
        ItemSync();
        this.GetComponent<Button>().onClick.AddListener(() => OpenInventory());
        b_exit.onClick.AddListener(() => ExitInventory());
        b_weapon.onClick.AddListener(() => ChangeContents(0));
        b_use.onClick.AddListener(() => ChangeContents(1));
        b_etc.onClick.AddListener(() => ChangeContents(2));

        ChangeContents(0);
    }

    //인벤토리 소분류 변경
    private void ChangeContents(int pNum)
    {
        for(int i = 0; i < 3; i++)
            background.transform.GetChild(i).gameObject.SetActive(false);
        background.transform.GetChild(pNum).gameObject.SetActive(true);

        ItemSync(pNum);
    }

    private void ItemSelected(int pKind, int pNum)
    {
        switch(pKind)
        {
            //장비
            case 0:
                t_temp.text = "이름 : " + cUtil._user.GetInventory().GetItemEquip()[pNum]._name + "\n" +
                    "종류 : 장비\n갯수 : 1";
                break;
            //소비
            case 1:
                t_temp.text = "이름 : " + cUtil._user.GetInventory().GetItemUse()[pNum]._name + "\n" +
                    "종류 : 소비\n갯수 : " + cUtil._user.GetInventory().GetItemUse()[pNum].amount;
                break;
            //재료
            case 2:
                t_temp.text = "이름 : " + cUtil._user.GetInventory().GetItemEtc()[pNum]._name + "\n" +
                    "종류 : 재료\n갯수 : " + cUtil._user.GetInventory().GetItemEtc()[pNum].amount;
                break;
        }
    }

    //소유 아이템 동기화
    private void ItemSync(int pKind = -1)
    {
        if(pKind == -1)
        {
            List<cItem_equip> l_itemEquip = new List<cItem_equip>(cUtil._user.GetInventory().GetItemEquip());
            for (int i = 0; i < background.transform.GetChild(0).GetChild(0).childCount; i++)
            {
                if(i > (l_itemEquip.Count - 1))
                {
                    //background.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = 
                    background.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    continue;
                }

                //이미지 설정
                //background.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = 
                int t = i;
                background.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                background.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(() => ItemSelected(0, t));
            }

            List<cItem_use> l_itemUse = new List<cItem_use>(cUtil._user.GetInventory().GetItemUse());
            for (int i = 0; i < background.transform.GetChild(1).GetChild(0).childCount; i++)
            {
                if (i > (l_itemUse.Count - 1))
                {
                    //background.transform.GetChild(1).GetChild(i).GetComponent<Image>().sprite = 
                    background.transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
                    background.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    continue;
                }

                //이미지 설정
                //background.transform.GetChild(1).GetChild(i).GetComponent<Image>().sprite = 
                background.transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true);
                background.transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).GetComponent<Text>().text = l_itemUse[i].amount.ToString();
                int t = i;
                background.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                background.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(() => ItemSelected(1, t));
            }

            List<cItem_etc> l_itemEtc = new List<cItem_etc>(cUtil._user.GetInventory().GetItemEtc());
            for (int i = 0; i < background.transform.GetChild(2).GetChild(0).childCount; i++)
            {
                if (i > (l_itemEtc.Count - 1))
                {
                    //background.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = 
                    background.transform.GetChild(2).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
                    background.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    continue;
                }

                //이미지 설정
                //background.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = 
                background.transform.GetChild(2).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true);
                background.transform.GetChild(2).GetChild(0).GetChild(i).GetChild(0).GetComponent<Text>().text = l_itemEtc[i].amount.ToString();
                int t = i;
                background.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                background.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(() => ItemSelected(2, t));
            }
        }
        else if(pKind == 0)
        {
            List<cItem_equip> l_itemEquip = new List<cItem_equip>(cUtil._user.GetInventory().GetItemEquip());
            for (int i = 0; i < background.transform.GetChild(0).GetChild(0).childCount; i++)
            {
                if (i > (l_itemEquip.Count - 1))
                {
                    //background.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = 
                    background.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    continue;
                }

                //이미지 설정
                //background.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite = 
                int t = i;
                background.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                background.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(() => ItemSelected(0, t));
            }
        }
        else if (pKind == 1)
        {
            List<cItem_use> l_itemUse = new List<cItem_use>(cUtil._user.GetInventory().GetItemUse());
            for (int i = 0; i < background.transform.GetChild(1).GetChild(0).childCount; i++)
            {
                if (i > (l_itemUse.Count - 1))
                {
                    //background.transform.GetChild(1).GetChild(i).GetComponent<Image>().sprite = 
                    background.transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
                    background.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    continue;
                }

                //이미지 설정
                //background.transform.GetChild(1).GetChild(i).GetComponent<Image>().sprite = 
                background.transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true);
                background.transform.GetChild(1).GetChild(0).GetChild(i).GetChild(0).GetComponent<Text>().text = l_itemUse[i].amount.ToString();
                int t = i;
                background.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                background.transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(() => ItemSelected(1, t));
            }
        }
        else if (pKind == 2)
        {
            List<cItem_etc> l_itemEtc = new List<cItem_etc>(cUtil._user.GetInventory().GetItemEtc());
            for (int i = 0; i < background.transform.GetChild(2).GetChild(0).childCount; i++)
            {
                if (i > (l_itemEtc.Count - 1))
                {
                    //background.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = 
                    background.transform.GetChild(2).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(false);
                    background.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    continue;
                }

                //이미지 설정
                //background.transform.GetChild(2).GetChild(i).GetComponent<Image>().sprite = 
                background.transform.GetChild(2).GetChild(0).GetChild(i).GetChild(0).gameObject.SetActive(true);
                background.transform.GetChild(2).GetChild(0).GetChild(i).GetChild(0).GetComponent<Text>().text = l_itemEtc[i].amount.ToString();
                int t = i;
                background.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                background.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(() => ItemSelected(2, t));
            }
        }
    }    

    //인벤토리 열기
    private void OpenInventory()
    {
        if (content.active == false)
        {
            content.SetActive(true);
            ChangeContents(0);
        }
    }

    //인벤토리 닫기
    private void ExitInventory()
    {
        if (content.active == true)
        {
            content.SetActive(false);
        }
    }
}
