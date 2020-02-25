using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cForge_skill : MonoBehaviour
{
    public GameObject[] obj_skill;
    private cInventory inven;
    private cAxe axe;

    void Start()
    {
        axe = cUtil._user._playerInfo.weapon;
        inven = cUtil._user._playerInfo.inventory;
        UpdateValues();
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("대쉬 쿨타임 감소");
            axe.dashCoolTime -= 1.0f;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("시야 증가");
            axe.headlightRange *= 2f;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("차지 강화");
            axe.chargeAttackPointMin -= 0.2f;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("연속공격 강화");
            axe.finalAttackPointMin += 0.5f;
        }
    }

    public void UpdateValues()
    {
        for(byte i = 0; i < obj_skill.Length; i++)
        {
            //레벨
            obj_skill[i].transform.GetChild(2).GetComponent<Text>().text = "Lv. " + cUtil._user._playerInfo.skillLevel[i].ToString();
            //영혼석 수량
            obj_skill[i].transform.GetChild(5).GetComponent<Text>().text = 
                cUtil._user._playerInfo.inventory.GetSoul()[i].GetValueToString() + "/1";
        }
    }
}
