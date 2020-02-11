using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Potion_speed : cItem_use
{
    public float speedUpTime;
    public float speedUpAmount;

    public float GetSpeedUpTime() { return speedUpTime; }
    public float GetSpeedUpAmount() { return speedUpAmount; }
    public void SetSpeedUpTime(float pSpeedUpTime) { this.speedUpTime = pSpeedUpTime; }
    public void SetSpeedUpAmount(float pSpeepUpAmount) { this.speedUpAmount = pSpeepUpAmount; }
    #region 생성자
    public cItem_Potion_speed(string pName, string pDesc, cProperty pPrice, byte pAmount, byte pKind)
        : base(pName, pDesc, pPrice, pAmount, pKind)
    {
        speedUpTime = 5.0f;
        speedUpAmount = 1.5f;
    }
    #endregion

    // n초동안 일정 %만큼의 이동속도 증가
    // 임시 : 5초, 50%
    public override byte UseItem()
    {
        byte curAmount = 0;

        curAmount = base.UseItem();

        if (curAmount.Equals(100))
            return curAmount;

        cUtil._player.StartSpeedUp(speedUpAmount, speedUpTime);

        return curAmount;

    }

}
