using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cFloatingText : MonoBehaviour
{
    private bool isInit;

    public Text[] t_text;

    private float[] timer;
    private bool[] isDone;
    public float moveSpeed;

    public GameObject DmgImg;
    public Sprite[] DmgFont;
    
    public void Init()
    {
        timer = new float[DmgImg.transform.childCount];
        isDone = new bool[DmgImg.transform.childCount];
        moveSpeed = 100;
        isInit = true;
    }
        
    void Update()
    {
        if(isInit.Equals(true) && cUtil._sm._scene != SCENE.SKIN)
        {
            for (byte i = 0; i < DmgImg.transform.childCount; i++)
            {
                if (isDone[i].Equals(false))
                {
                    timer[i] += Time.deltaTime;
                    DmgImg.transform.GetChild(i).Translate(Vector3.up * moveSpeed * Time.deltaTime);
                    for (byte k = 0; k < DmgImg.transform.GetChild(i).childCount; k++)
                    {
                        DmgImg.transform.GetChild(i).GetChild(k).GetComponent<Image>().color
                            = new Color(t_text[i].color.r, t_text[i].color.g, t_text[i].color.b, 1 - timer[i]);
                    }

                    if (timer[i] > 1.0f)
                    {
                        for (byte k = 0; k < DmgImg.transform.GetChild(i).childCount; k++)
                        {
                            DmgImg.transform.GetChild(i).GetChild(k).GetComponent<Image>().color
        = new Color(t_text[i].color.r, t_text[i].color.g, t_text[i].color.b, 1);
                            DmgImg.transform.GetChild(i).GetChild(k).gameObject.SetActive(false);
                        }
                        timer[i] = 0;
                        isDone[i] = true;
                    }
                }
            }
        }
        
    }

    public void DamageTextOn(string pDamageText, Vector3 pPos, bool pIsCrit = false)
    {
        //for (byte i = 0; i < DmgImg.transform.childCount; i++)
        //{
        //    if(isDone[i].Equals(true))
        //    {
        //        t_text[i].text = "-" + pDamageText;
        //        t_text[i].gameObject.transform.position = new Vector3(pPos.x, pPos.y + 30, pPos.z);
        //        isDone[i] = false;
        //        t_text[i].gameObject.SetActive(true);

        //        break;
        //    }
        //}

        for (byte i = 0; i < DmgImg.transform.childCount; i++)
        {
            if (isDone[i].Equals(true))
            {
                for(byte k = 0; k < pDamageText.Length; k++)
                {                    
                    byte temp = (byte)char.GetNumericValue(pDamageText[k]);
                    if(pIsCrit.Equals(false))
                    {
                        DmgImg.transform.GetChild(i).GetChild(k).GetComponent<Image>().sprite =
                            DmgFont[temp];
                        DmgImg.transform.GetChild(i).GetChild(k).localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    }
                    else
                    {
                        DmgImg.transform.GetChild(i).GetChild(k).GetComponent<Image>().sprite =
                            DmgFont[temp + 10];
                        DmgImg.transform.GetChild(i).GetChild(k).localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    }
                    DmgImg.transform.GetChild(i).GetChild(k).GetComponent<Image>().SetNativeSize();
                    DmgImg.transform.GetChild(i).GetChild(k).gameObject.SetActive(true);
                }
                DmgImg.transform.GetChild(i).position = new Vector3(pPos.x, pPos.y + 30, pPos.z);
                isDone[i] = false;
                break;
            }
        }
    }
}
