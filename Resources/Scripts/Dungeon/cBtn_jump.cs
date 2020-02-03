using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cBtn_jump : MonoBehaviour, IPointerDownHandler
{
    public cPlayer scr_player;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (scr_player.GetStatus() != CHARACTERSTATUS.CROUCH ||
            scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        {
            // 점프 횟수 증가
            scr_player.jumpCount++;
            Jump();
        }
    }

    private void Jump()
    {
        if (scr_player.isUpBlocked.Equals(true))
            return;

        if (scr_player.isGrounded.Equals(true) || scr_player.jumpCount < 2)
        {
            scr_player.StartCoroutine("Jump");
            if (scr_player.jumpCount >= 2)
            {
                scr_player.jumpCount = 0;
            }
        }
    }
}
