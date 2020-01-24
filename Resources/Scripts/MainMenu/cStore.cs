using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cStore : cBuilding
{
    public Button[] b_sell;
    public Button b_time;
    public Button b_sellAll;

    private short curFrameIdx;
    
    private void Start()
    {
        curFrameIdx = 0;
        b_click.onClick.AddListener(() => ActiveFrame());
    }

    private void ActiveFrame()
    {
        obj_content.SetActive(true);
    }
}
