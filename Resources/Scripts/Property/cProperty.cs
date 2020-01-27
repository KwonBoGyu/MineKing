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
    public bool isMinus;

    #region 생성자
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
    public cProperty(cProperty pProperty)
    {
        name = pProperty.GetName();
        value = new short[27];
        for (byte i = 0; i < value.Length; i++)
            value[i] = pProperty.GetValue()[i];
    }
    #endregion

    //Value 세팅
    public virtual void SetValue(short[] pValue)
    {
        for (byte i = 0; i < value.Length; i++)
            value[i] = pValue[i];
    }

    //Value String값으로 리턴
    public virtual string GetValueToString()
    {
        string s_return = " ";
        char idx = ' ';
        string prev1 = " ";

        //내림차순으로 접근
        for (byte i =(byte)(value.Length - 1); i >= 0; i--)
        {
            if(value[i] != 0)
            {
                //a미만일 때 바로 값 그대로 리턴
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

                //소수점 단위 표시
                {
                    if (prev1.Length > 2)
                        s_return += "." + prev1[0] + idx;
                    else
                        s_return += idx;
                }

                break;
            }

            //0일 때 바로 값 그대로 리턴
            if (i.Equals(0))
            {
                s_return = value[i].ToString();
                break;
            }
        }

        return s_return;
    }

    //Value 더하기
    public virtual void AddValue(cProperty pProperty)
    {
        //파라미터 초기화
        byte pIdx = pProperty.GetMaxIdx();
        short[] pValue = new short[pProperty.GetValue().Length];
        for (byte i = 0; i < pValue.Length; i++)
            pValue[i] = pProperty.GetValue()[i];

        for (byte i = 0; i < pIdx; i++)
        {
            AddValue(i, pValue[i]);
        }
    }
    public virtual void AddValue(short[] pValue)
    {
        for (byte i = 0; i < value.Length; i++)
        {
            AddValue(i, pValue[i]);
        }
    }
    public virtual void AddValue(byte pIdx, short pValue)
    {
        value[pIdx] += pValue;

        for (byte i = pIdx; i < value.Length; i++)
        {
            //현재 인덱스의 value값이 999보다 커진다면
            if (value[i] > 999)
            {
                //최대 골드(999z)일 때 Break
                if (i.Equals(value.Length - 1))
                {
                    Debug.Log("최대 수량입니다 - 골드");
                    break;
                }

                //상위 인덱스 value값 + 1
                //현재 인덱스 value값 - 1000
                value[i + 1] += (short)(value[i] - 999);
                value[i] -= 1000;
            }
            else
                break;
        }
    }

    //Value 빼기
    //pForce는 마이너스 계산까지 하고 싶을 때 true로 쓴다
    public virtual bool RemoveValue(cProperty pProperty, bool pForce = false)
    {        
        //파라미터 초기화
        bool canMinus = false;
        byte pIdx = pProperty.GetMaxIdx();
        short[] pValue = new short[pProperty.GetValue().Length];
        for (byte i = 0; i < pValue.Length; i++)
            pValue[i] = pProperty.GetValue()[i];

        //마이너스 계산
        //큰 값에서 작은 값을 빼고
        //isMinus를 true로 바꾼다.
        if(value[GetMaxIdx()] < pValue[pIdx] && pForce.Equals(true))
        {
            isMinus = true;
            cProperty tp = new cProperty(pProperty);

            tp.RemoveValue(this);
            SetValue(tp.GetValue());

            return false;
        }

        for (short i = pIdx; i >= 0; i--)
        {
            canMinus = RemoveValue(pIdx, pValue[i], pForce);

            if (canMinus.Equals(false) && pForce.Equals(false))
                break;
        }
        return canMinus;
    }
    public virtual bool RemoveValue(short[] pValue)
    {
        //파라미터 초기화
        bool canMinus = true;
        
        for (short i = (short)value.Length; i >= 0; i--)
        {
            canMinus = RemoveValue((byte)i, pValue[i]);

            if (canMinus.Equals(false))
                break;
        }
        return canMinus;
    }
    public virtual bool RemoveValue(byte pIdx, short pValue, bool pForce = false)
    {
        bool canMinus = false;

        //Max인덱스부터 ~ pIdx+1 까지 돈이 있는가?
        for (short i = (short)(value.Length - 1); i > pIdx; i--)
        {
            if (value[i] > 0)
                canMinus = true;
        }

        //없다면 pIdx에는 돈이 충분한가?
        if (canMinus.Equals(false) && value[pIdx] < pValue && pForce.Equals(false))
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

        return canMinus;
    }

    //값이 0이냐
    public bool isZero()
    {
        bool b_return = false;

        if (GetMaxIdx().Equals(0) && value[0].Equals(0))
            b_return = true;

        return b_return;
    }

    //최대 인덱스값 불러오기
    public byte GetMaxIdx()
    {
        byte idx = 0;

        for (short i = (short)(value.Length - 1); i >= 0; i--)
        {
            if (value[i] != 0)
                idx = (byte)i;
        }

        return idx;
    }
}
