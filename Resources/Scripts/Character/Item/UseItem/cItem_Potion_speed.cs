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
    public cItem_Potion_speed(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
        : base(pName, pPrice, pAmount, pKind, pKindNum)
    {
        speedUpTime = 5.0f;
        speedUpAmount = 1.5f;
    }
    #endregion

    // n초동안 일정 %만큼의 이동속도 증가
    // 임시 : 5초, 50%
    public override void UseItem()
    {
        base.UseItem();

        cUtil._user.GetPlayer().StartSpeedUp(speedUpAmount, speedUpTime);
    }

}
