using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Torch_O : MonoBehaviour
{
    private void OnEnable()
    {
        this.transform.position = cUtil._player.transform.position;
    }
}
