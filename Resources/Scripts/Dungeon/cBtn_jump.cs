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
        if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        {
            scr_player.isJumpStart = true;
            
            // 점프 횟수 증가
            scr_player.jumpCount++;
            if (scr_player.jumpCount > 2)
                return;
            Jump();
        }
    }

    private void Jump()
    {
        if (scr_player.isUpBlocked.Equals(true))
            return;

        scr_player.StartCoroutine("Jump");
    }
}
