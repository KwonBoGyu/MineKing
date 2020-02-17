using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class cDungeonNormal_processor : MonoBehaviour
{
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

    private void Start()
    {
        if (_enemyPool != null)
            InitDungeon();
    }

    public void InitDungeon()
    {
        //플레이어 초기화
        _p = _player.transform.GetChild(0).GetComponent<cPlayer>();
        //Debug.Log(cUtil._user.GetPlayerName());
        //_p.Init(cUtil._user.GetPlayerName(), cUtil._user.GetDamage(), 
        //    cUtil._user.GetMoveSpeed(), cUtil._user.GetHp());        
        _p.Init("asdf", 
            new cProperty("Damage", 2), 
            250,
            new cProperty("MaxHp", 100),
            new cProperty("CurHp", 100));
        cUtil._player = _p;
        _player.SetActive(true);

        //몬스터 초기화
        //for (byte i = 0; i < _enemyPool.transform.childCount; i++)
        //{
        //    _enemyPool.transform.GetChild(i).GetComponent<cEnemy_monster>().
        //        Init(cEnemyTable.SetMonsterInfo(_enemyPool.transform.GetChild(i).GetComponent<cEnemy_monster>().id));
            
        //}

        //ui 초기화
        b_bag.onClick.AddListener(() => _bag.OpenBag());
        for (byte i = 0; i < b_exitBag.Length; i++)
            b_exitBag[i].onClick.AddListener(() => ExitBag());

        b_goHome.onClick.AddListener(() => GoHome());
        UpdateValue();
    }   

    private void ExitBag()
    {
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
}
