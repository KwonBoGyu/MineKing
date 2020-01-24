using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class cProperty
{
    public string name;
    public string GetName() { return name; }
    public short[] value;
    public short[] GetValue() { return value; }

    public cProperty(string pName, short[] pValue)
    {
        name = pName;
        value = new short[27];
        for (byte i = 0; i < value.Length; i++)
            value[i] = pValue[i];
    }
    public cProperty(string pName)
    {
        name = pName;
        value = new short[27];
        for (byte i = 0; i < value.Length; i++)
            value[i] = 0;
    }

    public virtual void SetValue(short[] pValue)
    {
        for (byte i = 0; i < value.Length; i++)
            value[i] = pValue[i];
    }

    public virtual string GetValueToString()
    {
        string s_return = " ";
        char idx = ' ';
        string prev1 = " ";

        for (byte i =(byte)(value.Length - 1); i >= 0; i--)
        {
            if (value[i] != 0)
            {
                //a미만일 때
                if (i.Equals(0))
                {
                    s_return = value[i].ToString();
                    break;
                }

                //a이상일 때
                prev1 = value[i - 1].ToString();
                s_return = value[i].ToString();

                //아스키코드 이용
                idx = System.Convert.ToChar(96 + i);

                if (prev1.Length > 2)
                    s_return += "." + prev1[0] + idx;
                else
                    s_return += idx;

                break;
            }

            //0원일 때
            if (i.Equals(0))
            {
                s_return = value[i].ToString();
                break;
            }
        }

        Debug.Log(s_return);
        return s_return;
    }

    public virtual void AddValue(byte pIdx, short pValue)
    {
        value[pIdx] += pValue;

        for (byte i = pIdx; i < value.Length; i++)
        {
            if (value[i] > 999)
            {
                if (i.Equals(value.Length - 1))
                {
                    Debug.Log("최대 수량입니다 - 골드");
                    break;
                }

                value[i + 1] += (short)(value[i] - 999);
                value[i] -= 1000;
            }
        }
        GetValue();
    }

    public virtual bool RemoveValue(byte pIdx, short pValue)
    {
        bool canMinus = false;

        //Max인덱스부터 ~ pIdx+1 까지 돈이 있는가?
        for (int i = (value.Length - 1); i > pIdx; i--)
        {
            if (value[i] != 0)
                canMinus = true;
        }

        //없다면 pIdx에는 돈이 충분한가?
        if (canMinus.Equals(false) && value[pIdx] < pValue)
        {
            Debug.Log("골드가 부족합니다.");
            return canMinus;
        }

        //돈이 충분하다면..
        //pIdx에서 해결할 수 있다면..
        if (value[pIdx] >= pValue)
        {
            value[pIdx] -= pValue;
        }
        //해결할 수 없다면 상위 인덱스로..
        else
        {
            value[pIdx] -= pValue;

            for (byte i = pIdx; i < value.Length; i++)
            {
                if (value[i] < 0)
                {
                    value[i + 1] -= 1;
                    value[i] += 1000;
                }
            }
        }
        GetValue();
        return canMinus;
    }
}
