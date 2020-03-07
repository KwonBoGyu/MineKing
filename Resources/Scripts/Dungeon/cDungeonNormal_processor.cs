using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class cDungeonNormal_processor : MonoBehaviour
{
    public bool isSkin;

    //교체시 초기화 해줄것들
    public cTileMng chg_tileMng;
    public GameObject chg_obj_coolTime;
    public Text chg_t_coolTime;
    public Image chg_img_curHp;
    public Text chg_t_hp;
    public Image chg_img_curDp;
    public Text chg_t_dp;
    public cSoundMng chg_sm;
    public GameObject chg_indicator;
    public cFloatingText chg_floatingText;
    public cUseManager chg_useMng;
    public cJoystick chg_joystick;
    public cBtn_item chg_quickSlot;
    public Transform chg_flagPos;
    public cFlag chg_Flag;
    //교체시 넣어줘야 할것들
    public cDungeonNormal_processor chg_dp;
    public cCamera chg_camera;
    public cBtn_attack chg_attack;
    public cBtn_jump chg_jump;
    public cBtn_dash chg_dash;
    public cHeadLight chg_light;

    public GameObject playerParent;
    public GameObject _player;
    public GameObject _enemy;
    private cPlayer _p;
    private cEnemy_monster[] enemy;
    public GameObject _enemyPool;

    public Button b_bag;
    public Button[] b_exitBag;
    public cBag _bag;

    public GameObject obj_values;

    public Button b_goHome;
    public cSoundMng soundMng;
    public cTileMng tileMng;

    //보스전일 때
    public cBoss_theme1_stage1 boss_slime;
    public cBoss_Theme1_SkeletonMage boss_SkeletonMage;

    //필드 던전
    private void Start()
    {
        if (cUtil._sm._scene != SCENE.SKIN)
        {
            //일반 스테이지
            if ((int)cUtil._sm._scene > 2 && (int)cUtil._sm._scene < 18)
                InitDungeon();
            //보스
            else
                InitDungeon(true);
        }
    }

    //형상변환 ui
    public void Init()
    {
        _p = _player.transform.GetChild(0).GetComponent<cPlayer>();
        InitDungeon_skin();
    }

    public void InitDungeon(bool isBoss = false)
    {
        //일반 던전
        //플레이어 초기화
        if(isBoss.Equals(false))
        {
            byte curWeaponId = cUtil._user._playerInfo.curWeaponId;
            byte curClothId = cUtil._user._playerInfo.curClothId;

            _player = Instantiate(Resources.Load<GameObject>("Prefabs/Skin_cloth/Player_skin_" + curClothId.ToString()));
            _player.transform.SetParent(playerParent.transform);
            _player.transform.localScale = new Vector3(1, 1, 1);
            _player.transform.localPosition = new Vector3(337, 67, 1);
            _p = _player.transform.GetChild(0).GetComponent<cPlayer>();
            _p.img_curHp = chg_img_curHp;
            _p.t_hp = chg_t_hp;
            _p.img_curDp = chg_img_curDp;
            _p.t_dp = chg_t_dp;
            _p.Init(cUtil._user._playerInfo.nickName,
                cUtil._user._playerInfo.weapon.damage,
                250.0f,
                cUtil._user._playerInfo.weapon.hp,
                cUtil._user._playerInfo.weapon.hp);
            cUtil._player = _p;

            chg_tileMng.Init();
            _p.tileMng = chg_tileMng;
            _p.quickSlot = chg_quickSlot;
            chg_quickSlot.Init();
            _p.flagPos = chg_flagPos;
            chg_Flag.Init();
            _p.obj_coolTime = chg_obj_coolTime;
            _p.t_coolTime = chg_t_coolTime;
            _p.sm = chg_sm;
            _p.indicator = chg_indicator;
            _p.ft = chg_floatingText;
            chg_floatingText.Init();
            _p.useMng = chg_useMng;
            _p.joystick = chg_joystick;
            chg_joystick._player = _player.transform;
            chg_joystick.Init();
            _p.dp = chg_dp;
            chg_attack.scr_player = _p;
            chg_attack.Init();
            chg_jump.scr_player = _p;
            chg_dash.scr_player = _p;
            chg_light.HeadLight = _p.lightPos;
            chg_camera._player = _player;

            //ui 초기화
            b_bag.onClick.AddListener(() => OpenBag());
            for (byte i = 0; i < b_exitBag.Length; i++)
                b_exitBag[i].onClick.AddListener(() => ExitBag());

            b_goHome.onClick.AddListener(() => GoHome());
            
            SetEnemySpriteOrder();
        }
        //보스전
        else
        {
            byte curWeaponId = cUtil._user._playerInfo.curWeaponId;
            byte curClothId = cUtil._user._playerInfo.curClothId;

            _player = Instantiate(Resources.Load<GameObject>("Prefabs/Skin_cloth/Player_skin_" + curClothId.ToString()));
            _player.transform.SetParent(playerParent.transform);
            _player.transform.localScale = new Vector3(1, 1, 1);
            _player.transform.localPosition = new Vector3(337, 67, 1);
            _p = _player.transform.GetChild(0).GetComponent<cPlayer>();
            _p.img_curHp = chg_img_curHp;
            _p.t_hp = chg_t_hp;
            _p.img_curDp = chg_img_curDp;
            _p.t_dp = chg_t_dp;
            _p.Init(cUtil._user._playerInfo.nickName,
                cUtil._user._playerInfo.weapon.damage,
                250.0f,
                cUtil._user._playerInfo.weapon.hp,
                cUtil._user._playerInfo.weapon.hp);
            cUtil._player = _p;

            chg_tileMng.Init();
            _p.tileMng = chg_tileMng;
            _p.quickSlot = chg_quickSlot;
            chg_quickSlot.Init();
            _p.obj_coolTime = chg_obj_coolTime;
            _p.t_coolTime = chg_t_coolTime;
            _p.sm = chg_sm;
            _p.ft = chg_floatingText;
            chg_floatingText.Init();
            _p.useMng = chg_useMng;
            _p.joystick = chg_joystick;
            chg_joystick._player = _player.transform;
            chg_joystick.Init();
            _p.dp = chg_dp;
            chg_attack.scr_player = _p;
            chg_attack.Init();
            chg_jump.scr_player = _p;
            chg_dash.scr_player = _p;
            chg_light.HeadLight = _p.lightPos;
            chg_camera._player = _player;
            chg_camera.bossPlayerPosY = 200;

            //씬별로 보스 다르게 초기화해야함
            boss_slime.InitBoss();
            boss_SkeletonMage.InitBoss();
        }

        UpdateValue();


    }

    public void InitDungeon_skin()
    {
        _p.Init(cUtil._user._playerInfo.nickName,
         cUtil._user._playerInfo.weapon.damage,
         250.0f,
         cUtil._user._playerInfo.weapon.hp,
         cUtil._user._playerInfo.weapon.hp);
        cUtil._player = _p;

        //ui 초기화
        b_bag.onClick.AddListener(() => OpenBag());
        for (byte i = 0; i < b_exitBag.Length; i++)
            b_exitBag[i].onClick.AddListener(() => ExitBag());

        b_goHome.onClick.AddListener(() => GoHome());
        UpdateValue();
    }

    private void OpenBag()
    {
        soundMng.PlayBag(true);
        _bag.OpenBag();
    }

    private void ExitBag()
    {
        soundMng.PlayBag(false);
        _bag.obj_content.SetActive(false);
    }

    private void GoHome()
    {
        cUtil._sm.ChangeScene("Main");
    }

    public void UpdateValue()
    {
        obj_values.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
                cUtil._user._playerInfo.inventory.GetMoney().GetValueToString();
        obj_values.transform.GetChild(1).GetChild(0).GetComponent<Text>().text =
        cUtil._user._playerInfo.inventory.GetRock().GetValueToString();
        obj_values.transform.GetChild(2).GetChild(0).GetComponent<Text>().text =
        cUtil._user._playerInfo.inventory.GetDia().GetValueToString();
    }

    private void SetEnemySpriteOrder()
    {
        int enemyNum = _enemyPool.transform.childCount;
        int eachSpriteNum = 10;

        for (int i = 0; i < enemyNum; i++)
        {
            GameObject enemySprite = _enemyPool.transform.GetChild(i).transform.GetChild(0).transform.GetChild(2).gameObject;

            for (int j = 0; j < enemySprite.transform.childCount ; j++)
            {
                Debug.Log(enemySprite.transform.GetChild(j).GetComponent<Anima2D.SpriteMeshInstance>().sortingOrder);
                enemySprite.transform.GetChild(j).GetComponent<Anima2D.SpriteMeshInstance>().sortingOrder += i * eachSpriteNum;
            }
        }
    }
}
