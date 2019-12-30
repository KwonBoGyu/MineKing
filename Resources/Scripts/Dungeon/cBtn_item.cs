using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum ITEM
{
    BOMB,
    ROPE,
    SANDBAG,
    POTION_SPEED,
    POTION_HP,
    NONE
}

public class cBtn_item : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ITEM[] items; //0.현재, 1.퀵슬롯1 2.퀵슬롯2 3.퀵슬롯3
    private int toChangeBtnIdx;

    public cPlayer scr_player;

    public GameObject btn1;
    public GameObject btn2;
    public GameObject btn3;

    private bool isPopUpActive;
    private float popUpStartTimer;

    private IEnumerator cor_buttonDown;

    private void Start()
    {
        for (int i = 0; i < items.Length; i++)
            items[i] = (ITEM)i;

        cor_buttonDown = ButtonDown();
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

        //아이템 변경
        if (isPopUpActive.Equals(true))
        {
            //아이템 변경
            if (toChangeBtnIdx != -1)
            {
                ITEM t_item = items[0];
                items[0] = items[toChangeBtnIdx];
                items[toChangeBtnIdx] = t_item;
            }

            btn1.SetActive(false);
            btn2.SetActive(false);
            btn3.SetActive(false);
        }
        //아이템 사용
        else
        {
            switch (items[0])
            {
                case ITEM.BOMB:
                    Debug.Log("bomb");
                    scr_player.SetBomb();
                    break;
                case ITEM.ROPE:
                    Debug.Log("rope");
                    scr_player.SetRope();
                    break;
                case ITEM.SANDBAG:
                    Debug.Log("sandbag");
                    scr_player.SetSandBag();
                    break;
                case ITEM.POTION_HP:
                    Debug.Log("pt_hp");
                    scr_player.UseHpPotion();
                    break;
                case ITEM.POTION_SPEED:
                    Debug.Log("pt_sp");
                    scr_player.UseSpeedPotion();
                    break;
            }
        }
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
                    btn1.SetActive(true);
                    btn2.SetActive(true);
                    btn3.SetActive(true);
                }
            }
            else
            {
                Vector2 worldPos = Input.mousePosition;
                Ray2D ray = new Ray2D(worldPos, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit)
                {
                    if (hit.transform.gameObject.tag.Equals("quickSlot_item1"))
                        toChangeBtnIdx = 1;
                    else if (hit.transform.gameObject.tag.Equals("quickSlot_item2"))
                        toChangeBtnIdx = 2;
                    else if (hit.transform.gameObject.tag.Equals("quickSlot_item3"))
                        toChangeBtnIdx = 3;
                    else
                        toChangeBtnIdx = -1;

                    Debug.Log(hit.transform.gameObject.name);
                }
            }
        }
    }
}