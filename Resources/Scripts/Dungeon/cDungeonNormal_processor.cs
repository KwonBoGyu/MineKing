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
    private cEnemy_monster _e;
    public GameObject _enemyPool;
    private GameObject curStageMap;
    public GameObject[] _stageMap;

    public Button b_bag;
    public Button[] b_exitBag;
    public cBag _bag;

    
    void Start()
    {
        InitDungeon();        
    }

    public void ChangeStage(int pId, bool pIsIn)
    {
        int sId = (int)cUtil._sm._scene;

        //스테이지
        if (pId.Equals(0))
        {
            //들어가는 문
            if (pIsIn.Equals(true))
            {
                Debug.Log(sId);
                Destroy(curStageMap);
                curStageMap = Instantiate(_stageMap[sId].gameObject);
                cUtil._sm._scene += 2;
            }
            //나가는 문
            else
            {
                Destroy(curStageMap);
                curStageMap = Instantiate(_stageMap[sId - 2].gameObject);
                cUtil._sm._scene -= 2;
            }
        }
        //보스
        else if(pId.Equals(1))
        {
            //보스키 없다면 return
            if(cUtil._user.GetInventory().isBossKeyExist().Equals(false))
            {
                Debug.Log("보스 키가 없습니다.");
                return;
            }

            //들어가는 문
            if (pIsIn.Equals(true))
            {
                Destroy(curStageMap);
                //보스 키 삭제
                cUtil._user.GetInventory().destroyBossKey();
                curStageMap = Instantiate(_stageMap[sId - 1].gameObject);
                cUtil._sm._scene += 1;
            }
            //나가는 문
            else
            {
                Destroy(curStageMap);
                curStageMap = Instantiate(_stageMap[sId - 1].gameObject);
                cUtil._sm._scene -= 1;
            }
        }

        //적 초기화
        _enemyPool = curStageMap.transform.Find("Canvas").transform.GetChild(0).gameObject;
    }

    public void ChangeStage(int pId)
    {
        if(curStageMap != null)
            Destroy(curStageMap.gameObject);
        curStageMap = Instantiate(_stageMap[pId].gameObject);         
        //debug
        if(cUtil._sm != null)
            cUtil._sm._scene = (SCENE)(pId + 2);
        //적 초기화
        _enemyPool = curStageMap.transform.Find("Canvas").transform.GetChild(0).gameObject;
    }

    private void InitDungeon()
    {
        //맵 초기화
        ChangeStage(0);

        //플레이어 초기화
        _p = _player.transform.GetChild(0).GetComponent<cPlayer>();
        _e = _enemyPool.transform.GetChild(1).GetComponent<cMonster_stage1_slime>();
        //Debug.Log(cUtil._user.GetPlayerName());
        //_p.Init(cUtil._user.GetPlayerName(), cUtil._user.GetDamage(), 
        //    cUtil._user.GetMoveSpeed(), cUtil._user.GetHp());
        
        _p.Init("asdf", 
            new cProperty("Damage", 2), 
            250,
            new cProperty("MaxHp", 100),
            new cProperty("CurHp", 100));
        cUtil._player = _p;
        _e.Init("Slime",
            new cProperty("Damage", 1),
            250,
            new cProperty("MaxHp", 10),
            new cProperty("CurHp", 10));
        _player.SetActive(true);

        //ui
        b_bag.onClick.AddListener(() => _bag.OpenBag());
        for (byte i = 0; i < b_exitBag.Length; i++)
            b_exitBag[i].onClick.AddListener(() => ExitBag());
    }   

    private void ExitBag()
    {
        _bag.obj_content.SetActive(false);
    }
}
