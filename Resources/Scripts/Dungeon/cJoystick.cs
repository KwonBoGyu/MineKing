﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cJoystick : MonoBehaviour
{
    public Transform _player;
    public Transform joystick;
    public Button b_attack;
    public Button b_item;
    public Button b_jump;
    public Button b_dash;

    private cPlayer scr_player;
    private Vector3 defaultPos;
    private Vector3 joyDir;
    private float rad;
    public bool isDrag;
    private int jumpCount;
    private float keyTime;

    private bool isDragValuable;
    private float padX;
    private float padY;
    private float padAngle;

    private Vector3 dragPos;


    private void Start()
    {
        rad = this.GetComponent<RectTransform>().sizeDelta.y * 0.3f;
        defaultPos = this.transform.position;
        isDrag = false;
        scr_player = _player.transform.GetChild(0).GetComponent<cPlayer>();
        b_jump.onClick.AddListener(() => Jump());
        b_dash.onClick.AddListener(() => Dash());
        b_item.onClick.AddListener(() => Item());
        scr_player.SetDir(Vector2.right);
        scr_player.SetCurMoveSpeed(0);
        jumpCount = 0;
        keyTime = 0;
    }

    private void Update()
    {
#if UNITY_EDITOR

        //점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyTime = 0;
            if (scr_player.GetStatus() != CHARACTERSTATUS.CROUCH ||
                scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
            {
                // 점프 횟수 증가
                jumpCount++;
                Jump();
            }
        }
        //대쉬
        if (Input.GetKeyDown(KeyCode.D))
        {
            Dash();
        }
        //공격
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
            {
                scr_player.Attack_front();                
            }
        }

        ////제트팩 ON
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    keyTime += Time.deltaTime;
        //    if (keyTime > 0.3f)
        //    {
        //        scr_player.SetJetPackOn();
        //        scr_player.StartCoroutine("JetPack");
        //    }
        //}

        ////제트팩 OFF
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    keyTime = 0;
        //    scr_player.SetJetPackOff();
        //}



#endif
        ////////////////////////////////안드로이드///////////////////////////////////
        if (scr_player.isGrounded.Equals(true))
            jumpCount = 0;

        if (scr_player.GetStatus() == CHARACTERSTATUS.ATTACK)
            scr_player.SetCurMoveSpeed(0);
        else
        {
            if (isDrag.Equals(true))
            {
                //위
                if (Mathf.Abs(joyDir.x) < 0.3f && joyDir.y > 0.7f)
                {
                    CalcDir(0);
                }
                //오른
                else if (Mathf.Abs(joyDir.y) < 0.3f && joyDir.x > 0.7f)
                {
                    CalcDir(1);
                }
                //아래
                else if (Mathf.Abs(joyDir.x) < 0.3f && joyDir.y < -0.7f)
                {
                    CalcDir(2);
                }
                //왼
                else if (Mathf.Abs(joyDir.y) < 0.3f && joyDir.x < -0.7f)
                {
                    CalcDir(3);
                }
            }
        }
    }
    
    public void PointerDown(BaseEventData _data)
    {
        PointerEventData d = _data as PointerEventData;
        dragPos = d.position;
        isDrag = true;

        //방향
        joyDir = (dragPos - this.transform.position).normalized;
        //조이스틱 이동
        joystick.position = dragPos;
        //이동 거리
        float dist = (defaultPos - dragPos).magnitude;

        if (dist > rad)
        {
            joystick.position = joyDir * rad + defaultPos;
        }
        if(scr_player.isRightBlocked)
        {
            //오른
            if (Mathf.Abs(joyDir.y) < 0.3f && joyDir.x > 0.7f)
            {
                scr_player.SetIsClimbing(true);
            }
        }
        else if (scr_player.isLeftBlocked)
        {
            //왼
            if (Mathf.Abs(joyDir.y) < 0.3f && joyDir.x < -0.7f)
            {
                scr_player.SetIsClimbing(true);
            }
        }
    }

    public void Drag(BaseEventData _data)
    {
        PointerEventData d = _data as PointerEventData;
        dragPos = d.position;
        isDrag = true;

        //방향
        joyDir = (dragPos - this.transform.position).normalized;         
        //조이스틱 이동
        joystick.position = dragPos;
        //이동 거리
        float dist = (defaultPos - dragPos).magnitude;

        if(dist > rad)
        {
            joystick.position = joyDir * rad + defaultPos;
        }
    }

    public void DragEnd()
    {
        joystick.localPosition = Vector3.zero;
        this.transform.position = defaultPos;
        joyDir = Vector3.zero;
        isDrag = false;

        scr_player.SetCurMoveSpeed(0);
        scr_player.SetDir(scr_player.GetDirection(), CHARDIRECTION.NONE);
    }

    public void PointerUp(BaseEventData _data)
    {
        this.transform.position = defaultPos;
        joystick.localPosition = Vector3.zero;
        joyDir = Vector3.zero;
        isDrag = false;

        scr_player.SetCurMoveSpeed(0);
        scr_player.SetDir(scr_player.GetDirection(), CHARDIRECTION.NONE);
    }

    private void Jump()
    {
        if (scr_player.isUpBlocked.Equals(true))
            return;

        if (scr_player.isGrounded == true || jumpCount < 2)
        {
            scr_player.StartCoroutine("Jump");
            if (jumpCount >= 2)
            {
                jumpCount = 0;
            }
        }
    }

    private void Dash()
    {
        // 쿨다운이 다 찼을때 대쉬 발동
        if (scr_player.GetStatus() == CHARACTERSTATUS.NONE &&
            scr_player.GetDashCoolDown() == scr_player.GetMaxDashCoolDown())
        {
            if (scr_player.isGrounded == true)
            {
                scr_player.StartCoroutine("Dash");
                scr_player.StartCoroutine("DashCoolDown");
            }
        }
    }

    private void Item()
    {
        //scr_player.SetRope();
        //scr_player.SetSandBag();
        //scr_player.SetBomb();
        //scr_player.UseSpeedPotion();
        //scr_player.UseHpPotion();
    }

    private void Attack()
    {
        if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        {
            scr_player.Attack_front();
            scr_player.SetStatus(CHARACTERSTATUS.ATTACK);
        }
    }

    private void CalcDir(int pDir)
    {
        switch(pDir)
        {
            //UP
            case 0:
                scr_player.SetDir(Vector3.up, CHARDIRECTION.UP);

                //예외처리
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
                {
                    scr_player.SetStatus(CHARACTERSTATUS.NONE);

                    //벽에 붙은 상태일때
                    if (scr_player.GetIsClimbing())
                    {
                        scr_player.SetCurMoveSpeed(150);
                    }
                    //벽에 안붙어있을때
                    else
                    {
                        scr_player.SetCurMoveSpeed(0);
                    }
                }
                break;
            //RIGHT
            case 1:
                scr_player.SetDir(Vector3.right, CHARDIRECTION.RIGHT);
                _player.transform.localScale = new Vector3(1, 1, 1);

                //붙은거 떨어지기
                if(scr_player.isLeftBlocked && scr_player.GetIsClimbing())
                {
                    scr_player.SetIsClimbing(false);
                }

                //달리기
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                     scr_player.GetStatus() != CHARACTERSTATUS.DASH)
                {
                    scr_player.SetStatus(CHARACTERSTATUS.NONE);
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());         
                }
                break;
            //DOWN
            case 2:
                scr_player.SetDir(Vector3.down, CHARDIRECTION.DOWN);

                //예외처리
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
                {
                    //벽에 붙은 상태
                    if (scr_player.GetIsClimbing())
                    {
                        scr_player.SetIsClimbing(false);
                    }
                    //벽에 붙은 상태 아닐때
                    else
                    {
                        scr_player.SetCurMoveSpeed(0);
                    }
                }
                break;
            //LEFT
            case 3:
                scr_player.SetDir(Vector3.left, CHARDIRECTION.LEFT);
                _player.transform.localScale = new Vector3(-1, 1, 1);
                if (scr_player.isRightBlocked && scr_player.GetIsClimbing())
                {
                    scr_player.SetIsClimbing(false);
                }

                //왼쪽 이동
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                    scr_player.GetStatus() != CHARACTERSTATUS.DASH)
                {
                    scr_player.SetStatus(CHARACTERSTATUS.NONE);
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
                }
                break;
        }        
    }
}
