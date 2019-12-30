using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cGate : MonoBehaviour
{
    public int id; //0 : 스테이지, 1: 보스
    public bool isIn;    
    public cDungeonNormal_processor dp;

    private void Awake()
    {
        dp = GameObject.Find("Canvas_main").transform.GetChild(0).GetComponent<cDungeonNormal_processor>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))        
            dp.ChangeStage(id, isIn);        
    }
}
