using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cProperty
{
    public string _name { get; set; }
    public long value { get; set; }

    #region 생성자
    public cProperty(string pName, long pValue)
    {
        _name = pName;
        value = pValue;
    }
    public cProperty(string pName)
    {
        _name = pName;
        value = 0;
    }
    public cProperty(cProperty pProperty)
    {
        _name = pProperty._name;
        value = pProperty.value;
    }
    #endregion
    
    //Value String값으로 리턴
    public virtual string GetValueToString()
    {
        string s_return = " ";

        if (value >= 1000000000000000000)        
            s_return = string.Format("{0:F1}f", 1000000000000000000 * 0.000000000000000001);        
        else if (value >= 1000000000000000)        
            s_return = string.Format("{0:F1}e", 1000000000000000 * 0.000000000000001);        
        else if (value >= 1000000000000)        
            s_return = string.Format("{0:F1}d", 1000000000000 * 0.000000000001);        
        else if (value >= 1000000000)        
            s_return = string.Format("{0:F1}c", 1000000000 * 0.000000001);        
        else if (value >= 1000000)        
            s_return = string.Format("{0:F1}b", 1000000 * 0.000001);        
        else if (value >= 1000)        
            s_return = string.Format("{0:F1}a", 1000 * 0.001);        
        else
            s_return = value.ToString();

        return s_return;
    }

}
