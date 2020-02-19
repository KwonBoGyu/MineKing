using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cUseManager : MonoBehaviour
{
    public GameObject bombPrefab;
    public GameObject torchPrefab;
    
    public void SetBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, cUtil._player.originObj.transform.position,
            Quaternion.identity, this.gameObject.transform);

        if (cUtil._player.GetDirection().x > 0)
        {
            bomb.GetComponent<cItem_Bomb_O>().SetDir(new Vector3(0.5f, 0.5f));
        }
        else
        {
            bomb.GetComponent<cItem_Bomb_O>().SetDir(new Vector3(-0.5f, 0.5f));
        }

        bomb.SetActive(true);
    }

    public void SetTorch()
    {
        GameObject torch = Instantiate(torchPrefab, cUtil._player.originObj.transform.position,
            Quaternion.identity, this.gameObject.transform);
        torch.SetActive(true);
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
