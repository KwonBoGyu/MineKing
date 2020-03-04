using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cSkinScene : MonoBehaviour
{
    public GameObject player;
    public GameObject playerParent;
    private cPlayer scr_player;

    //교체시 초기화 해줄것들
    public cTileMng chg_tileMng;
    public GameObject chg_obj_coolTime;
    public Text chg_t_coolTime;
    public Image chg_img_curHp;
    public Text chg_t_hp;
    public cSoundMng chg_sm;
    public GameObject chg_indicator;
    public cFloatingText chg_floatingText;
    public cUseManager chg_useMng;
    public cJoystick chg_joystick;
    //교체시 넣어줘야 할것들
    public cDungeonNormal_processor chg_dp;
    public cSkinCamObj chg_skinCamObj;
    public cBtn_attack chg_attack;
    public cBtn_jump chg_jump;
    public cBtn_dash chg_dash;
    public cHeadLight chg_light;

    public Button b_exit;

    public Button[] b_changeTheme;

    public GameObject obj_weapons;
    public GameObject obj_clothes;

    void Start()
    {
        b_exit.onClick.AddListener(() => GoToMainScene());

        b_changeTheme[0].onClick.AddListener(() => OnClickChangeButton(false));
        b_changeTheme[1].onClick.AddListener(() => OnClickChangeButton(true));
        OnClickChangeButton(true);

        for(byte i = 0; i < cUtil._user._playerInfo.myWeaponsId.Length; i++)
        {
            byte k = i;
            obj_weapons.transform.GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(() => ChangeItem(true, k));
            obj_weapons.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Image>().sprite = cUtil._user.WeaponImages[i];
        }

        Debug.Log("CHECKED");

        for (byte i = 0; i < cUtil._user._playerInfo.myClothesId.Length; i++)
        {
            byte k = i;
            obj_clothes.transform.GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(() => ChangeItem(false, k));
            obj_clothes.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Image>().sprite = cUtil._user.ClothImages[i];
        }

        ChangeItem(false, cUtil._user._playerInfo.curClothId);
        UpdateValues();
    }

    private void ChangeItem(bool pIsWeapon, byte pChar)
    {
        if(pIsWeapon.Equals(true))
        {
            cUtil._user._playerInfo.curWeaponId = pChar;
            scr_player.img_weapon.sprite = cUtil._user.WeaponEquipImages[cUtil._user._playerInfo.curWeaponId];
        }
        else
        {
            cUtil._user._playerInfo.curClothId = pChar;

            Destroy(player);
            player = Instantiate(Resources.Load<GameObject>("Prefabs/Skin_cloth/Player_skin_" + cUtil._user._playerInfo.curClothId.ToString()));
            player.transform.SetParent(playerParent.transform);
            player.transform.localScale = new Vector3(1, 1, 1);
            player.transform.localPosition = new Vector3(337, 67, 1);
            scr_player = player.transform.GetChild(0).GetComponent<cPlayer>();
            Debug.Log("CHECKED");
            chg_tileMng.Init();
            scr_player.tileMng = chg_tileMng;
            scr_player.obj_coolTime = chg_obj_coolTime;
            scr_player.t_coolTime = chg_t_coolTime;
            scr_player.img_curHp = chg_img_curHp;
            scr_player.t_hp = chg_t_hp;
            scr_player.sm = chg_sm;
            scr_player.img_weapon.sprite = cUtil._user.WeaponEquipImages[cUtil._user._playerInfo.curWeaponId];
            scr_player.indicator = chg_indicator;
            scr_player.ft = chg_floatingText;
            scr_player.useMng = chg_useMng;
            scr_player.joystick = chg_joystick;
            scr_player.dp = chg_dp;
            chg_joystick._player = player.transform;
            chg_joystick.Init();
            chg_dp._player = player;
            chg_skinCamObj.obj_follow = player;
            chg_attack.scr_player = scr_player;
            chg_attack.Init();
            chg_jump.scr_player = scr_player;            
            chg_dash.scr_player = scr_player;
            chg_light.HeadLight = scr_player.lightPos;

            chg_dp.Init();

            
        }


    }

    private void UpdateValues()
    {
        for(byte i = 0; i < cUtil._user._playerInfo.myWeaponsId.Length; i++)
        {
            //무기
            if(cUtil._user._playerInfo.myWeaponsId[i].Equals(false))
            {
                obj_weapons.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = false;
                obj_weapons.transform.GetChild(0).GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                obj_weapons.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = true;
                obj_weapons.transform.GetChild(0).GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
        }

        for (byte i = 0; i < cUtil._user._playerInfo.myClothesId.Length; i++)
        {
            //의상
            if (cUtil._user._playerInfo.myClothesId[i].Equals(false))
            {
                obj_clothes.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = false;
                obj_clothes.transform.GetChild(0).GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                obj_clothes.transform.GetChild(0).GetChild(i).GetComponent<Button>().interactable = true;
                obj_clothes.transform.GetChild(0).GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
        }
    }


    private void OnClickChangeButton(bool isCloth)
    {
        if(isCloth.Equals(true))
        {
            obj_clothes.SetActive(true);
            obj_weapons.SetActive(false);
            b_changeTheme[0].interactable = true;
            b_changeTheme[1].interactable = false;
        }
        else
        {
            obj_clothes.SetActive(false);
            obj_weapons.SetActive(true);
            b_changeTheme[0].interactable = false;
            b_changeTheme[1].interactable = true;
        }
    }
    
    private void GoToMainScene()
    {
        cUtil._sm.ChangeScene("Main");
    }
}
