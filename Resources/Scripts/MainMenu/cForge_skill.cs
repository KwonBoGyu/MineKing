using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cForge_skill : MonoBehaviour
{
    public GameObject[] obj_skill;

    void Start()
    {
        UpdateValues();
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
