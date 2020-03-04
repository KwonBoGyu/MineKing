using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cForge_skill : MonoBehaviour
{
    public Sprite[] soulImages;
    public Sprite[] skillImages;

    public GameObject[] obj_skill;
    public GameObject obj_cannotLevelUp;
    public GameObject obj_canLevelUp;
    public GameObject obj_levelUpDone;
    private string[] skillName = new string[4] { "대쉬", "시야 증가", "차지", "연속 공격" };

    private byte curSkillChar;

    public void Init()
    {
        obj_cannotLevelUp.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => ExitCannotLevelUp());
        obj_canLevelUp.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ExitCanLevelUp());
        obj_canLevelUp.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() => UpgradeDone());
        obj_levelUpDone.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => ExitLevelUpDone());

        for(byte i = 0; i < obj_skill.Length; i++)
        {
            byte k = i;
            obj_skill[i].GetComponent<Button>().onClick.AddListener(() => UpgradeSkill(k));
        }

        UpdateValues();

    }
    
    public void UpdateValues()
    {
        for(byte i = 0; i < obj_skill.Length; i++)
        {
            //레벨
            obj_skill[i].transform.GetChild(2).GetComponent<Text>().text = string.Format("Lv. {0}", cUtil._user._playerInfo.skillLevel[i].ToString());
            //영혼석 수량
            obj_skill[i].transform.GetChild(5).GetComponent<Text>().text =
                string.Format("{0}/{1}", cUtil._user._playerInfo.inventory.GetSoul()[i].value, cUtil._user._playerInfo.skillLevel[i] + 1);
        }
    }

    private void UpgradeSkill(byte pChar)
    {
        //영혼석 부족
        if (cUtil._user._playerInfo.inventory.GetSoul()[pChar].value < cUtil._user._playerInfo.skillLevel[pChar] + 1)
        {
            obj_cannotLevelUp.transform.GetChild(1).GetComponent<Image>().sprite = soulImages[pChar];
            obj_cannotLevelUp.transform.GetChild(2).GetComponent<Text>().text = string.Format("{0}/{1}",
                cUtil._user._playerInfo.inventory.GetSoul()[pChar].value, cUtil._user._playerInfo.skillLevel[pChar] + 1);
            obj_cannotLevelUp.SetActive(true);

            return;
        }
        
        curSkillChar = pChar;

        obj_canLevelUp.transform.GetChild(1).GetComponent<Image>().sprite = skillImages[pChar];
        obj_canLevelUp.transform.GetChild(2).GetComponent<Text>().text = skillName[pChar];
        obj_canLevelUp.transform.GetChild(3).GetComponent<Text>().text =
            string.Format("Lv. {0}", cUtil._user._playerInfo.skillLevel[pChar]); 
        obj_canLevelUp.transform.GetChild(4).GetComponent<Text>().text =
            string.Format("Lv. {0}", cUtil._user._playerInfo.skillLevel[pChar] + 1);

        obj_canLevelUp.SetActive(true);
    }

    private void UpgradeDone()
    {
        cUtil._user._playerInfo.inventory.GetSoul()[curSkillChar].value -= cUtil._user._playerInfo.skillLevel[curSkillChar] + 1;
        cUtil._user._playerInfo.skillLevel[curSkillChar] += 1;

        obj_levelUpDone.transform.GetChild(1).GetComponent<Image>().sprite = skillImages[curSkillChar];
        obj_levelUpDone.transform.GetChild(2).GetComponent<Text>().text = skillName[curSkillChar];
        obj_levelUpDone.transform.GetChild(3).GetComponent<Text>().text = string.Format("Lv. {0}", cUtil._user._playerInfo.skillLevel[curSkillChar]);

        ExitCanLevelUp();
        obj_levelUpDone.SetActive(true);
        UpdateValues();
    }

    private void ExitCannotLevelUp()
    {
        obj_cannotLevelUp.SetActive(false);
    }

    private void ExitCanLevelUp()
    {
        obj_canLevelUp.SetActive(false);
    }

    private void ExitLevelUpDone()
    {
        obj_levelUpDone.SetActive(false);
        curSkillChar = 4;
    }
}
