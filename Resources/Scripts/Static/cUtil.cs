using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class cUtil
{
    public static cSceneManager _sm;
    public static cUser _user;
    public static cTileMng _tileMng;
    
    public const float pi = 3.141592f;

    public static void SetInstanceInit(GameObject pObj, string pName = null, GameObject pParent = null, 
        bool pExitButton = false, bool pOkayButton = false)
    {
        if (pParent != null)
            pObj.transform.SetParent(pParent.transform);

        if (pName != null)
            pObj.name = pName;

        pObj.transform.localPosition = new Vector3(0, 0, 0);
        pObj.transform.localScale = new Vector3(1, 1, 1);
    }

    
    
}
