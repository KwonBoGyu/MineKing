﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cBtn_attack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public cPlayer scr_player;
    public Image img_gageBar;
    private float minChargePoint;
    private float maxChargePoint;
    private float chargeStartTimer;
    private bool isChargingOn;

    private int comboPoint;
    private float chargeTimer;
    private IEnumerator cor_keepPointerDown;

    void Start()
    {
        comboPoint = 0;
        chargeTimer = 0;
        cor_keepPointerDown = KeepPointerDown();
        minChargePoint = 0.61f;
        maxChargePoint = 0.88f;
        img_gageBar.fillAmount = minChargePoint;
        img_gageBar.transform.parent.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isChargingOn = false;
        chargeTimer = 0;
        chargeStartTimer = 0;

        if(scr_player.isGrounded.Equals(true))
            StartCoroutine(cor_keepPointerDown);                
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //일반 연속 공격
        if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK && chargeTimer < 0.2f)
        {
            //점프 공격
            if(scr_player.isGrounded.Equals(false))
            {
                if (scr_player.jumpAttackPoint > 0)
                    return;

                scr_player.jumpAttackPoint += 1;
                scr_player.isJumpAttack = true;
            }

            scr_player.Attack_front();
            scr_player.SetStatus(CHARACTERSTATUS.ATTACK);

            if(comboPoint < 3)
                comboPoint += 1;
            //else
            //3타 공격
        }

        chargeTimer = 0;
        img_gageBar.fillAmount = minChargePoint;
        img_gageBar.transform.parent.gameObject.SetActive(false);
        StopCoroutine(cor_keepPointerDown);
    }

    IEnumerator KeepPointerDown()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if(isChargingOn.Equals(false))
            {
                Debug.Log("CHARGING START");
                chargeStartTimer += Time.deltaTime;

                if (chargeStartTimer > 0.2f)
                {
                    isChargingOn = true;
                    img_gageBar.transform.parent.gameObject.SetActive(true);                    
                }
            }
            //차지 시작
            else
            {
                chargeTimer += Time.deltaTime;
                Debug.Log("CHARGING");
                //범위 0.2
                img_gageBar.fillAmount = minChargePoint + (maxChargePoint - minChargePoint) * chargeTimer;
            }
        }
    }
}