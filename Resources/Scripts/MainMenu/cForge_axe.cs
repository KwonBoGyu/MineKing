using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cForge_axe : MonoBehaviour
{
    public Image img_axe;
    public Text t_level;
    public Text[] t_curValue;
    public Text[] t_nextValue;
    public Slider slider_upgrade;
    public Text t_curUpgradeNum;
    public Text t_upgradeGold;
    public Button b_upgrade;
    
    //void Start()
    //{
    //    //레벨
    //    t_level.text = "Lv. " + cUtil._user._playerInfo.weapon.level;
    //    //현재 공격력
    //    t_curValue[0].text = cUtil._user._playerInfo.weapon.damage.GetValueToString();
    //    //현재 Hp
    //    t_curValue[1].text = cUtil._user._playerInfo.weapon.hp.GetValueToString();
    //    //현재 공격속도
    //    t_curValue[2].text = string.Format("{0:F2}", cUtil._user._playerInfo.weapon.attackSpeed);
        
    //    //슬라이더 맥시멈값 계산
    //    cProperty MaxLevel = new cProperty("MaxLevel", 9000000000000000000);
    //    cProperty canLevel = cWeaponTable.GetMaxUpgradeLevelByMaxGold(
    //        cUtil._user._playerInfo.weapon.level,
    //        cUtil._user._playerInfo.inventory.GetMoney());

    //    if (MaxLevel < canLevel)
    //        canLevel = MaxLevel;

    //    slider_upgrade.maxValue = canLevel;
    //    slider_upgrade.onValueChanged.AddListener(SliderChanged);
    //    SliderChanged(slider_upgrade.value);
    //}

    //public void UpdateValues()
    //{
    //    SliderChanged(slider_upgrade.value);
    //}

    //private void SliderChanged(float pVal)
    //{
    //    short val = (short)pVal;

    //    //강화 횟수
    //    t_curUpgradeNum.text = val.ToString();

    //    //강화 골드
    //    t_upgradeGold.text = cWeaponTable.GetUpgradeGoldByLevel(cUtil._user._playerInfo.weapon.level,
    //        val).GetValueToString();

    //    short upLevel = cUtil._user._playerInfo.weapon.level;
    //    upLevel += val;
    //    //강화 공격력
    //    t_nextValue[0].text = cWeaponTable.GetAxeInfo(upLevel).damage.GetValueToString();
    //    //강화 Hp
    //    t_nextValue[1].text = cWeaponTable.GetAxeInfo(upLevel).hp.GetValueToString();
    //    //강화 공격속도
    //    t_nextValue[2].text = string.Format("{0:F2}", cWeaponTable.GetAxeInfo(upLevel).attackSpeed);
    //}


}
