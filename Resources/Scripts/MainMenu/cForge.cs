using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cForge : cBuilding
{
    public Button[] b_Frame;
    public GameObject[] obj_Frame;
    private short curFrameIdx;

    //LocalPos
    private float originX;
    private float movedX;

    private void Start()
    {
        originX = -478.1f;
        movedX = -523.5f;
        curFrameIdx = -1;

        for (byte i = 0; i < b_Frame.Length; i++)
        {
            byte k = i;
            b_Frame[i].onClick.AddListener(() => ButtonClick(k));
        }

        b_Frame[0].onClick.Invoke();
    }
    private void ButtonClick(byte pNum)
    {
        if (curFrameIdx.Equals(pNum))
        {
            Debug.Log("같은 프레임입니다.");
            return;
        }

        curFrameIdx = pNum;
        for (short i = 0; i < b_Frame.Length; i++)
        {
            if(i.Equals(curFrameIdx))
            {
                b_Frame[i].transform.parent.localPosition = new Vector3(movedX,
                  b_Frame[i].transform.parent.localPosition.y,
                   b_Frame[i].transform.parent.localPosition.z);

                obj_Frame[i].SetActive(true);
                b_Frame[i].transform.parent.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                continue;
            }

            b_Frame[i].transform.parent.localPosition = new Vector3(originX,
                b_Frame[i].transform.parent.localPosition.y,
                b_Frame[i].transform.parent.localPosition.z);

            obj_Frame[i].SetActive(false);
            b_Frame[i].transform.parent.GetComponent<SpriteRenderer>().color = new Color(116.0f / 255.0f, 116.0f / 255.0f, 116.0f / 255.0f);
        }
    }
}
