using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cUseManager : MonoBehaviour
{
    public GameObject[] l_Bomb;
    public GameObject[] l_Torch;

    private void Start()
    {
        l_Bomb = new GameObject[this.transform.GetChild(0).transform.childCount];
        l_Torch = new GameObject[this.transform.GetChild(1).transform.childCount];

        for (int i = 0; i < this.transform.GetChild(0).transform.childCount; i++)
        {
            l_Bomb[i] = this.transform.GetChild(0).transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < this.transform.GetChild(1).transform.childCount; i++)
        {
            l_Torch[i] = this.transform.GetChild(1).transform.GetChild(i).gameObject;
        }
    }

    public void SetBomb()
    {
        for (int i = 0; i < l_Bomb.Length; i++)
        {
            if (l_Bomb[i].activeSelf.Equals(false))
            {
                l_Bomb[i].SetActive(true);
                Vector3 dir = new Vector3(cUtil._player.GetDirection().x, 0.5f, cUtil._player.GetDirection().z);
                if (cUtil._player.GetDirection().x > 0)
                {
                    dir = new Vector3(0.5f, 0.5f, cUtil._player.GetDirection().z);
                }
                else
                {
                    dir = new Vector3(-0.5f, 0.5f, cUtil._player.GetDirection().z);
                }
                l_Bomb[i].GetComponent<cItem_Bomb_O>().SetDir(dir);
                break;
            }
        }
    }

    public void SetTorch()
    {
        for (int i = 0; i < l_Torch.Length; i++)
        {
            if (l_Torch[i].activeSelf.Equals(false))
            {
                l_Torch[i].SetActive(true);
                break;
            }
        }
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetBomb();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetTorch();
        }
    }
}
