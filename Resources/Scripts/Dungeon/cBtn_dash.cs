using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cBtn_dash : MonoBehaviour, IPointerDownHandler
{
    public cPlayer scr_player;

    public void OnPointerDown(PointerEventData eventData)
    {
        Dash();
    }

    private void Dash()
    {
        // 쿨다운이 다 찼을때 대쉬 발동
        if (scr_player.GetStatus() == CHARACTERSTATUS.NONE &&
            scr_player.GetDashCoolDown() == scr_player.GetMaxDashCoolDown())
        {
            //아래 위 방향이라면 대쉬 안함
            if (scr_player.GetDirection().Equals(Vector3.up) || scr_player.GetDirection().Equals(Vector3.down))
                return;

            if (scr_player.GetIsGrounded().Equals(true))
            {
                scr_player.StartCoroutine("Dash");
                scr_player.StartCoroutine("DashCoolDown");
                scr_player.sm.playEffect(9);
            }
        }
    }
}
