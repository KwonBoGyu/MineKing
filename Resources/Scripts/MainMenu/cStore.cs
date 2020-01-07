using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cStore : cBuilding
{
    public Text t_userMoney;

    public GameObject contents;

    public Button b_buy;
    public Button b_sell;

    private List<GameObject> l_contents; // 상점 내 아이템 탭
    private List<cItem_use> l_itemList; // 판매하는 아이템

    private GameObject selected;
    
    private void Start()
    {
        l_contents = new List<GameObject>();
        l_itemList = new List<cItem_use>();

        // 아이템 테이블과 상점 내 아이템 탭 동기화
        for (int i = 0; i < cUtil._user.GetInventory().GetItemUse().Count; i++)
        {
            l_itemList.Add(cUtil._user.GetInventory().GetItemUse()[i]);
        }
        
        b_buy.onClick.AddListener(() => OnBuyButtonClicked());
        b_sell.onClick.AddListener(() => OnSellButtonClicked());
        
        // 판매 아이템 UI
        for (int i = 0; i < l_itemList.Count; i++)
        {
            GameObject temp = contents.transform.GetChild(i).gameObject;

            temp.transform.GetChild(0).GetComponent<Text>().text =
                l_itemList[i].price.ToString();
            temp.transform.GetChild(4).GetComponent<Text>().text =
                l_itemList[i]._name;
            temp.gameObject.SetActive(true);

            l_contents.Add(temp);
            l_contents[i].GetComponent<Button>().onClick.AddListener(() => OnContentButtonClicked());
        }
    }

    // 팝업창이 켜질때마다 호출
    public void ActiveWindow()
    {
        t_userMoney.text = cUtil._user.GetInventory().GetMoney().ToString();
        for (int i = 0; i < l_contents.Count; i++)
        {
            l_contents[i].transform.GetChild(1).gameObject.GetComponent<Text>().text =
                cUtil._user.GetInventory().GetItemUse()[i].amount.ToString();
        }
    }

    private void OnContentButtonClicked()
    {
        Debug.Log("selected : " + EventSystem.current.currentSelectedGameObject.name);
        selected = EventSystem.current.currentSelectedGameObject;
    }

    private void OnBuyButtonClicked()
    {
        int itemIdx = -1;

        for (int i = 0; i < l_contents.Count; i++)
        {
            if (selected.Equals(l_contents[i]))
            {
                int price = l_itemList[i].price;
                if (price > cUtil._user.GetInventory().GetMoney())
                {
                    Debug.Log("돈 부족");
                    return;
                }
                else
                {
                    itemIdx = i;
                    cUtil._user.GetInventory().GetItemUse()[i].amount += 1;
                    cUtil._user.GetInventory().UseMoney(price);
                    break;
                }
            }
            else
            {
                Debug.Log("아이템 없음");
                continue;
            }
        }

        selected.transform.GetChild(1).GetComponent<Text>().text =
            cUtil._user.GetInventory().GetItemUse()[itemIdx].amount.ToString();

        t_userMoney.text = cUtil._user.GetInventory().GetMoney().ToString();
    }

    private void OnSellButtonClicked()
    {
        int itemIdx = -1;
        
        for (int i = 0; i < l_contents.Count; i++)
        {
            if (selected.name.Equals(l_contents[i].name))
            {
                itemIdx = i;
                int price = l_itemList[i].price;

                if (cUtil._user.GetInventory().GetItemUse()[i].amount > 0)
                {
                    cUtil._user.GetInventory().GetItemUse()[i].amount -= 1;
                    cUtil._user.GetInventory().EarnMoney(price);
                    break;
                }
                else
                {
                    Debug.Log("아이템 부족");
                    continue;
                }
            }
        }
            selected.transform.GetChild(1).GetComponent<Text>().text = 
                cUtil._user.GetInventory().GetItemUse()[itemIdx].amount.ToString();

            t_userMoney.text = cUtil._user.GetInventory().GetMoney().ToString();
    }
}
