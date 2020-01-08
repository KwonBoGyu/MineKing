using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM
{
    BOMB,
    ROPE,
    SANDBAG,
    POTION_SPEED,
    POTION_HP,
    TORCH,
    NONE
}

[System.Serializable]
public class cItem : MonoBehaviour
{
    public string _name;
    public int price;
    public int kind;
    public int kindNum;
    
    public cItem(string pName, int pPrice, int pKind, int pKindNum)
    {
        _name = pName;
        price = pPrice;
        kind = pKind;
        kindNum = pKindNum;
    }

}
