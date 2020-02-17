using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cFloatingText : MonoBehaviour
{
    public Text[] t_text;
    private float[] timer;
    private bool[] isDone;
    public float moveSpeed;
    
    void Start()
    {
        timer = new float[t_text.Length];
        isDone = new bool[t_text.Length];
        moveSpeed = 100;
    }
        
    void Update()
    {
        for(byte i = 0; i < t_text.Length; i++)
        {
            if(isDone[i].Equals(false))
            {
                timer[i] += Time.deltaTime;

                t_text[i].transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
                //t_text[i].color = new Color(t_text[i].color.r, t_text[i].color.g, t_text[i].color.b, 1 - timer[i]);

                if(timer[i] > 1.0f)
                {
                    t_text[i].gameObject.SetActive(false);
                    //t_text[i].color = new Color(t_text[i].color.r, t_text[i].color.g, t_text[i].color.b, 1);
                    timer[i] = 0;
                    isDone[i] = true;
                }
            }
        }
    }

    public void DamageTextOn(string pDamageText, Vector3 pPos)
    {
        for (byte i = 0; i < t_text.Length; i++)
        {
            if(isDone[i].Equals(true))
            {
                t_text[i].text = "-" + pDamageText;
                t_text[i].gameObject.transform.position = new Vector3(pPos.x, pPos.y + 30, pPos.z);
                isDone[i] = false;
                t_text[i].gameObject.SetActive(true);

                break;
            }
        }
    }
}
