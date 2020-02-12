using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cDungeon : cBuilding
{
    public Animator animator_main;
    public Button b_normal;

    private void Start()
    {
        b_normal.onClick.AddListener(() => OnClickStage());
    }

    private void OnClickStage()
    {
        animator_main.SetTrigger("DungeonStart");
        cUtil._sm.ChangeScene("Dungeon_normal_1");
    }
}
