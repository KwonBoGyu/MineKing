using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cBtn_attack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public cPlayer scr_player;
    public Image img_gageBar;
    public Text t_gagePercent;
    public ParticleSystem p_gageEffect;
    private float minChargePoint;
    private float maxChargePoint;
    private float chargeStartTimer;
    private bool isChargingOn;

    private Vector2 prevTouchPos;

    private int comboPoint;
    private float chargeTimer;
    private IEnumerator cor_keepPointerDown;

    void Start()
    {
        comboPoint = 0;
        chargeTimer = 0;
        cor_keepPointerDown = KeepPointerDown();
        minChargePoint = 0;
        maxChargePoint = 2.0f;
        img_gageBar.fillAmount = minChargePoint;
        img_gageBar.transform.parent.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerEventData d = eventData as PointerEventData;
        prevTouchPos = d.position;

        StopCoroutine(cor_keepPointerDown);

        isChargingOn = false;
        chargeTimer = 0;
        chargeStartTimer = 0;

        if(scr_player.isGrounded)
            StartCoroutine(cor_keepPointerDown);                
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerEventData d = eventData as PointerEventData;
        float dist = (d.position - prevTouchPos).magnitude;

        StopCoroutine(cor_keepPointerDown);

        //차지공격
        if(img_gageBar.fillAmount > 0.5f && isChargingOn.Equals(true))
        {
            scr_player.ChargeOn();
        }
        else if (img_gageBar.fillAmount < 0.5f && isChargingOn.Equals(true))
        {
            scr_player.ChargeFail();
        }
        //일반 연속 공격
        else if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK && isChargingOn.Equals(false))
        {
            //점프 공격
            //if (scr_player.isGrounded.Equals(false))
            //{
            //    if (scr_player.jumpAttackPoint > 0)
            //        return;

            //    scr_player.jumpAttackPoint += 1;
            //    scr_player.isJumpAttack = true;
            //}

            if (scr_player.GetDirection().Equals(Vector3.up))
                scr_player.Attack_up();
            else if (scr_player.GetDirection().Equals(Vector3.down))
            {
                if (scr_player.GetIsClimbing().Equals(false))
                    scr_player.Attack_down();
                else
                    return;
            }
            else
                scr_player.Attack_front();

            scr_player.SetStatus(CHARACTERSTATUS.ATTACK);

            if (comboPoint < 3)
                comboPoint += 1;
            //else
            //3타 공격
        }

        chargeTimer = 0;
        chargeStartTimer = 0;
        img_gageBar.fillAmount = minChargePoint;
        p_gageEffect.transform.position = new Vector3(
    img_gageBar.transform.position.x, p_gageEffect.transform.position.y, p_gageEffect.transform.position.z);
        img_gageBar.transform.parent.gameObject.SetActive(false);
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

                if (chargeStartTimer > 0.5f)
                {
                    isChargingOn = true;
                    img_gageBar.transform.parent.gameObject.SetActive(true);
                    scr_player.ChargeStart();
                }
            }
            //차지 시작
            else
            {
                chargeTimer += Time.deltaTime;
                Debug.Log("CHARGING");
                //범위 0.2
                img_gageBar.fillAmount = chargeTimer / maxChargePoint;
                t_gagePercent.text = string.Format("{0:F0}%", 100 * img_gageBar.fillAmount);
                p_gageEffect.transform.position = new Vector3(
                    img_gageBar.transform.position.x + img_gageBar.GetComponent<RectTransform>().sizeDelta.x /2 * img_gageBar.fillAmount,
                    p_gageEffect.transform.position.y, p_gageEffect.transform.position.z);
            }
        }
    }
}
