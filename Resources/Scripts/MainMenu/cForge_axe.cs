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

    private cProperty upLevel;
    private cProperty upGold;


    public ParticleSystem[] effects;
    public cMain_processor mp;

    void Start()
    {
        b_upgrade.onClick.AddListener(() => UpgradeAxe());
        UpdateValues();
    }

    private void OnEnable()
    {
        UpdateValues();
    }

    private void UpgradeAxe()
    {
        //강화 불가하다면..
        if(slider_upgrade.maxValue.Equals(0) || slider_upgrade.value.Equals(0))        
            return;        

        //돈이 있다면..
        cUtil._user._playerInfo.inventory.GetMoney().value -= upGold.value;
        //레벨
        cUtil._user._playerInfo.weapon.level.value = upLevel.value;
        //현재 공격력
        cUtil._user._playerInfo.weapon.damage.value = cWeaponTable.GetAxeInfo(upLevel).damage.value;
        t_curValue[0].text = cUtil._user._playerInfo.weapon.damage.GetValueToString();
        //현재 Hp
        cUtil._user._playerInfo.weapon.hp.value = cWeaponTable.GetAxeInfo(upLevel).hp.value;
        t_curValue[1].text = cUtil._user._playerInfo.weapon.hp.GetValueToString();
        //현재 공격속도
        cUtil._user._playerInfo.weapon.attackSpeed = cWeaponTable.GetAxeInfo(upLevel).attackSpeed;
        t_curValue[2].text = string.Format("{0:F2}", cUtil._user._playerInfo.weapon.attackSpeed);
        //현재 내구도
        cUtil._user._playerInfo.weapon.indurance.value = cWeaponTable.GetAxeInfo(upLevel).indurance.value;
        t_curValue[3].text = cUtil._user._playerInfo.weapon.indurance.GetValueToString();

        for (byte i = 0; i < effects.Length; i++)
        {
            effects[i].Stop();
            effects[i].Play();
        }

        mp.UpdateValues();
        UpdateValues();
    }

    public void UpdateValues()
    {
        //레벨
        t_level.text = string.Format("Lv. {0}", cUtil._user._playerInfo.weapon.level.GetValueToString());
        //현재 공격력
        t_curValue[0].text = cUtil._user._playerInfo.weapon.damage.GetValueToString();
        //현재 Hp
        t_curValue[1].text = cUtil._user._playerInfo.weapon.hp.GetValueToString();
        //현재 공격속도
        t_curValue[2].text = string.Format("{0:F2}", cUtil._user._playerInfo.weapon.attackSpeed);
        //현재 내구도
        t_curValue[3].text = cUtil._user._playerInfo.weapon.indurance.GetValueToString();

        //슬라이더 맥시멈값 계산
        cProperty MaxLevel = new cProperty("MaxLevel", 9000000000000000000);
        cProperty canLevel = cWeaponTable.GetMaxUpgradeLevelByMaxGold(
            cUtil._user._playerInfo.weapon.level,
            cUtil._user._playerInfo.inventory.GetMoney());
        if (MaxLevel.value < canLevel.value)
            canLevel.value = MaxLevel.value;

        slider_upgrade.maxValue = canLevel.value;
        slider_upgrade.onValueChanged.AddListener(SliderChanged);
        slider_upgrade.value = 0;
        SliderChanged(slider_upgrade.value);
    }

    private void SliderChanged(float pVal)
    {
        upLevel = new cProperty("asdfa", 0);
        upGold = new cProperty("asdf", 0);

        long val = (long)pVal;

        //강화 횟수
        t_curUpgradeNum.text = val.ToString();

        //강화 레벨
        upLevel.value = cUtil._user._playerInfo.weapon.level.value;
        upLevel.value += val;
        //강화 골드
        upGold.value = cWeaponTable.GetUpgradeGoldByLevel(cUtil._user._playerInfo.weapon.level.value, val).value;
        t_upgradeGold.text = upGold.GetValueToString();
        //강화 공격력
        t_nextValue[0].text = cWeaponTable.GetAxeInfo(upLevel).damage.GetValueToString();
        //강화 Hp
        t_nextValue[1].text = cWeaponTable.GetAxeInfo(upLevel).hp.GetValueToString();
        //강화 공격속도
        t_nextValue[2].text = string.Format("{0:F2}", cWeaponTable.GetAxeInfo(upLevel).attackSpeed);
        //강화 내구도
        t_nextValue[3].text = cWeaponTable.GetAxeInfo(upLevel).indurance.GetValueToString();
    }


}
