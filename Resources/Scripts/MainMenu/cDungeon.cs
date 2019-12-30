using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cDungeon : cBuilding
{
    public Button b_normal;
    public Button b_boss;

    private void Start()
    {
        b_normal.onClick.AddListener(() => OnClickStage());
        b_boss.onClick.AddListener(() => OnClickBoss());
    }

    private void OnClickStage()
    {
        Debug.Log("To Dungeon_normal");
        cUtil._sm.ChangeScene("Dungeon_normal");
    }

    private void OnClickBoss()
    {
        cUtil._sm.ChangeScene("Dungeon_boss");
    }
}
