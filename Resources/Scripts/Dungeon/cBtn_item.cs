using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class cBtn_item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private byte btnIdx;
    
    public GameObject[] obj_buttons;

    private bool isPopUpActive;
    private float popUpStartTimer;

    private IEnumerator cor_buttonDown;

    private void Start()
    {
        UpdateQuickSlot();
        
        cor_buttonDown = ButtonDown();
    }

    public void UpdateQuickSlot()
    {
        for (byte i = 0; i < 4; i++)
        {
            //이미 지정된 단축키 아이템 표시
            if (cUtil._user._playerInfo.quickSlotItemNum[i] != -1)
            {
                //수량 인덱스
                byte itemIdx = 0;
                for (byte k = 0; k < cUtil._user._playerInfo.inventory.GetItemUse().Count; k++)
                {
                    if (cUtil._user._playerInfo.quickSlotItemNum[i].Equals(cUtil._user._playerInfo.inventory.GetItemUse()[k].kind))
                    {
                        itemIdx = k;
                        break;
                    }
                }
                //수량이 없다면..
                if (cUtil._user._playerInfo.inventory.GetItemUse()[itemIdx].amount < 1)
                {
                    obj_buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                    obj_buttons[i].transform.GetChild(1).gameObject.SetActive(false);
                    cUtil._user._playerInfo.quickSlotItemNum[i] = -1;
                }
                //수량이 있다면..
                else
                {
                    obj_buttons[i].transform.GetChild(1).GetComponent<Text>().text =
                        cUtil._user._playerInfo.inventory.GetItemUse()[itemIdx].amount.ToString();
                    obj_buttons[i].transform.GetChild(1).gameObject.SetActive(true);

                    //이미지
                    obj_buttons[i].transform.GetChild(0).GetComponent<Image>().sprite =
                        Resources.LoadAll<Sprite>("Images/Main/img_items")[cUtil._user._playerInfo.quickSlotItemNum[i] + citemTable.GetUseItemTotalNum()];
                    obj_buttons[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                obj_buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                obj_buttons[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPopUpActive = false;
        popUpStartTimer = 0f;

        StartCoroutine(cor_buttonDown);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(cor_buttonDown);

        //퀵슬롯 아이템 사용
        if (isPopUpActive.Equals(true))
        {
            if (btnIdx != 4)
            {
                //퀵슬롯에 지정되어 있다면 실행
                if (cUtil._user._playerInfo.quickSlotItemNum[btnIdx] != -1)
                {
                    byte curAmount = 100;
                    byte itemKind = (byte)cUtil._user._playerInfo.quickSlotItemNum[btnIdx];
                    curAmount = cUtil._player.UseItem(itemKind);

                    //수량 소진시 퀵슬롯 및 인벤토리에서 삭제
                    if (curAmount.Equals(0) || curAmount.Equals(100))
                    {
                        cUtil._user._playerInfo.quickSlotItemNum[btnIdx] = -1;
                        cUtil._user._playerInfo.inventory.RemoveItem(itemKind);
                    }
                }
            }
            
            for (byte i = 1; i < obj_buttons.Length; i++)
                obj_buttons[i].SetActive(false);            
        }
        //메인 아이템 사용
        else
        {
            //퀵슬롯에 지정되어 있다면 실행
            if (cUtil._user._playerInfo.quickSlotItemNum[0] != -1)
                cUtil._player.UseItem((byte)cUtil._user._playerInfo.quickSlotItemNum[0]);            
        }

        UpdateQuickSlot();
    }

    IEnumerator ButtonDown()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (isPopUpActive.Equals(false))
            {
                popUpStartTimer += Time.deltaTime;

                if (popUpStartTimer >= 0.7f)
                {
                    isPopUpActive = true;
                    for (byte i = 1; i < obj_buttons.Length; i++)
                        obj_buttons[i].SetActive(true);
                }
            }
            else
            {
                Vector2 worldPos = Input.mousePosition;
                Ray2D ray = new Ray2D(worldPos, Vector2.left);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit)
                {
                    if (hit.transform.gameObject.tag.Equals("quickSlot_item1"))
                    {
                        btnIdx = 1;
                    }
                    else if (hit.transform.gameObject.tag.Equals("quickSlot_item2"))
                    {
                        btnIdx = 2;
                    }
                    else if (hit.transform.gameObject.tag.Equals("quickSlot_item3"))
                    {
                        btnIdx = 3;
                    }
                    else
                    {
                        btnIdx = 4;                    
                    }
                }
            }
        }
    }
}