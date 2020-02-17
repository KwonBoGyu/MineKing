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
    public ParticleSystem p_gageCritEffect;
    public Image img_Critical;
    private float minChargePoint;
    private float maxChargePoint;
    private float chargeStartTimer;
    private bool isChargingOn;
    private float reduceFactor;

    private bool criticalOn;
    private float criticalReduceAmount;
    private float criticalMin;

    private Vector2 prevTouchPos;

    private float chargeTimer;

    void Start()
    {
        chargeTimer = 0;
        minChargePoint = 0;
        maxChargePoint = 2.0f;
        img_gageBar.fillAmount = minChargePoint;
        criticalMin = 0.7f;
        img_Critical.fillAmount = 1 - criticalMin;
        criticalReduceAmount = 0.5f;
        reduceFactor = 0.5f;
        img_gageBar.transform.parent.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        //차치중
        if (isChargingOn.Equals(true))
        {
            Attack();

            if(scr_player.GetIsClimbing().Equals(false))
            {
                Debug.Log("CHARGING");

                chargeTimer += Time.deltaTime;

                if (chargeTimer > maxChargePoint)
                    chargeTimer = maxChargePoint;

                img_gageBar.fillAmount = chargeTimer / maxChargePoint;
                t_gagePercent.text = string.Format("{0:F0}%", 100 * img_gageBar.fillAmount);
                Debug.Log(img_gageBar.GetComponent<RectTransform>().rect.width);
                p_gageEffect.transform.position = new Vector3(
                    img_gageBar.transform.position.x + img_gageBar.GetComponent<RectTransform>().rect.width * 0.5f * 0.7f  * img_gageBar.fillAmount,
                    p_gageEffect.transform.position.y, p_gageEffect.transform.position.z);
            }
        }
        //손 뗐을 때
        else
        {
            chargeTimer -= Time.deltaTime * reduceFactor;
            if (chargeTimer < 0)
            {
                chargeTimer = 0;
                return;
            }

            Debug.Log("CHARGE REDUCING");

            img_gageBar.fillAmount = chargeTimer / maxChargePoint;
            t_gagePercent.text = string.Format("{0:F0}%", 100 * img_gageBar.fillAmount);
            p_gageEffect.transform.position = new Vector3(
img_gageBar.transform.position.x + img_gageBar.GetComponent<RectTransform>().rect.width * 0.5f * 0.7f * img_gageBar.fillAmount,
p_gageEffect.transform.position.y, p_gageEffect.transform.position.z);

            if (img_gageBar.fillAmount < 0.03f)
            {
                chargeTimer = 0;
                img_gageBar.fillAmount = 0;
                t_gagePercent.text = string.Format("{0:F0}%", 100 * img_gageBar.fillAmount);
                p_gageEffect.transform.position = new Vector3(
            img_gageBar.transform.position.x, p_gageEffect.transform.position.y, p_gageEffect.transform.position.z);
                img_gageBar.transform.parent.gameObject.SetActive(false);
            }
        }

        if (img_gageBar.fillAmount < criticalMin)
            criticalOn = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isChargingOn = true;
        Attack();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isChargingOn = false;

        if (img_gageBar.fillAmount >= criticalMin)
            criticalOn = true;
    }

    private void Attack()
    {
        if (scr_player.GetStatus().Equals(CHARACTERSTATUS.ATTACK))
        {
            //풀차지 일 때 크리티컬 공격
            if (criticalOn.Equals(true))
            {
                chargeTimer -= maxChargePoint * criticalReduceAmount;
                
                if (chargeTimer < 0)
                    chargeTimer = 0;

                if (scr_player.GetDirection().Equals(Vector3.up))
                    scr_player.ChargeAttack_up();
                else if (scr_player.GetDirection().Equals(Vector3.down) && scr_player.GetIsClimbing().Equals(false))
                    scr_player.ChargeAttack_down();
                else
                    scr_player.ChargeAttack_front();
                p_gageCritEffect.Play();

                criticalOn = false;
            }
        }
        //일반 공격
        else if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        {
            if (scr_player.GetIsGrounded().Equals(false) && scr_player.GetIsClimbing().Equals(false))
                return;

            if (scr_player.GetDirection().Equals(Vector3.up))
                scr_player.Attack_up();
            else if (scr_player.GetDirection().Equals(Vector3.down) && scr_player.GetIsClimbing().Equals(false))
                scr_player.Attack_down();
            else
                scr_player.Attack_front();

            if (scr_player.GetIsClimbing().Equals(true))
                isChargingOn = false;
            if (scr_player.GetIsGrounded().Equals(true) && scr_player.GetIsClimbing().Equals(false))
            {
                isChargingOn = true;
                img_gageBar.transform.parent.gameObject.SetActive(true);
            }
        }

    }
}
