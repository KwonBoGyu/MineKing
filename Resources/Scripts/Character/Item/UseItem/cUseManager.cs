using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cUseManager : MonoBehaviour
{
    public GameObject[] l_Bomb;
    public GameObject[] l_Torch;

    private void Start()
    {
        Debug.Log("USE MANAGER INIT");
    }

    public void SetBomb()
    {
        for (int i = 0; i < l_Bomb.Length; i++)
        {
            if (l_Bomb[i].activeSelf.Equals(false))
            {
                l_Bomb[i].SetActive(true);
                cItem_Bomb_O script = l_Bomb[i].GetComponent<cItem_Bomb_O>();
                if (cUtil._player.GetDirection().x > 0)
                {
                    script.isRight = true;
                }
                else
                {
                    script.isRight = false;
                }
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("BONB00");
            SetBomb();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetTorch();
        }
    }
}
