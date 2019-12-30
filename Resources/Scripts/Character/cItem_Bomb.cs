using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Bomb : MonoBehaviour
{
    private float timer;
    private bool isTimerOn;
    private bool isExplodeOn;
    private IEnumerator SetBombCoroutine;
    public void SetTimer(float pTimer) { timer = pTimer; }

    public cItem_Bomb()
    {
        timer = 3.0f;
        isTimerOn = false;
        isExplodeOn = false;
        SetBombCoroutine = SetBomb();
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isTimerOn)
        {
            StopCoroutine(SetBombCoroutine);
        }
        
        if(collision.transform.tag.Equals("Tilemap_rock"))
        {
            StartCoroutine("SetBomb");
            if(isExplodeOn)
            {
                Destroy(collision.gameObject);
                isExplodeOn = false;
                isTimerOn = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator SetBomb()
    {
        float time = 0;
        while (time <= timer)
        {
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
        }
        isTimerOn = true;
        isExplodeOn = true;
    }
}
