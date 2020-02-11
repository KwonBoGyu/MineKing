using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class cJoystick : MonoBehaviour
{
    public Transform _player;
    public Transform joystick;

    private cPlayer scr_player;
    private Vector3 defaultPos;
    private Vector3 joyDir;
    private float rad;
    public bool isDrag;

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
        scr_player.SetDir(Vector2.right);
        scr_player.SetCurMoveSpeed(0);
    }

    private void Update()
    {
#if UNITY_EDITOR

        //점프
        if (Input.GetKeyDown(KeyCode.Space))
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
                Attack();                
            }
        }

#endif
        ////////////////////////////////안드로이드///////////////////////////////////
        if (scr_player.GetIsGrounded().Equals(true))
        {
            scr_player.SetIsClimbing(false);
        }

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
        //벽에 붙기
        if(scr_player.isRightBlocked && scr_player.GetIsGrounded().Equals(false))
        {
            //오른
            if (Mathf.Abs(joyDir.y) < 0.3f && joyDir.x > 0.7f)
            {
                scr_player.SetIsClimbing(true);
                scr_player.jumpCount = 0;
            }
        }
        if (scr_player.isLeftBlocked && scr_player.GetIsGrounded().Equals(false))
        {
            //왼
            if (Mathf.Abs(joyDir.y) < 0.3f && joyDir.x < -0.7f)
            {
                scr_player.SetIsClimbing(true);
                scr_player.jumpCount = 0;
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
        joyDir = Vector3.zero;
        isDrag = false;

        scr_player.SetCurMoveSpeed(0);
        scr_player.SetDir(scr_player.GetDirection(), CHARDIRECTION.NONE);
        scr_player.sm.StopRunningEffect();
    }

    public void PointerUp(BaseEventData _data)
    {
        //this.transform.position = defaultPos;
        joystick.localPosition = Vector3.zero;
        joyDir = Vector3.zero;
        isDrag = false;

        scr_player.SetCurMoveSpeed(0);
        scr_player.SetDir(scr_player.GetDirection(), CHARDIRECTION.NONE);
        scr_player.sm.StopRunningEffect();
    }

    private void Jump()
    {
        if (scr_player.isUpBlocked.Equals(true))
            return;
        scr_player.StartCoroutine("Jump");
    }

    private void Dash()
    {
        // 쿨다운이 다 찼을때 대쉬 발동
        if (scr_player.GetStatus() == CHARACTERSTATUS.NONE &&
            scr_player.GetDashCoolDown() == scr_player.GetMaxDashCoolDown())
        {
            if (scr_player.GetIsGrounded().Equals(true))
            {
                scr_player.StartCoroutine("Dash");
                scr_player.StartCoroutine("DashCoolDown");
            }
        }
    }
        
    private void Attack()
    {
        if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        {
            if (scr_player.GetIsGrounded().Equals(false) && scr_player.GetIsClimbing().Equals(false))
                return;
            
            if (scr_player.GetDirection().Equals(Vector3.up))
                scr_player.Attack_up();
            else if (scr_player.GetDirection().Equals(Vector3.down) && scr_player.GetIsClimbing().Equals(false))
                scr_player.Attack_down();
            else
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
                        scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed() * 0.5f);
                    }
                    //벽에 안붙어있을때
                    else
                    {
                        scr_player.SetIsClimbing(false);
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
                    scr_player.jumpCount = 0;
                }

                //달리기
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                     scr_player.GetStatus() != CHARACTERSTATUS.DASH && 
                     scr_player.GetIsClimbing().Equals(false))
                {
                    scr_player.SetStatus(CHARACTERSTATUS.NONE);
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
                    if(scr_player.GetIsGrounded().Equals(true))
                        scr_player.sm.playRunningEffect();
                }

                if(scr_player.GetIsClimbing().Equals(true))
                    scr_player.SetCurMoveSpeed(0);

                break;
            //DOWN
            case 2:
                scr_player.SetDir(Vector3.down, CHARDIRECTION.DOWN);

                //예외처리
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
                {
                    scr_player.SetStatus(CHARACTERSTATUS.NONE);

                    //벽에 붙은 상태
                    if (scr_player.GetIsClimbing())
                    {
                        scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed() * 0.5f);
                        if (scr_player.GetIsGrounded().Equals(true))
                            scr_player.SetIsClimbing(false);
                    }
                    //벽에 붙은 상태 아닐때
                    else
                    {
                        scr_player.SetCurMoveSpeed(0);
                        scr_player.SetIsClimbing(false);
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
                    scr_player.jumpCount = 0;
                }

                //왼쪽 이동
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                    scr_player.GetStatus() != CHARACTERSTATUS.DASH &&
                     scr_player.GetIsClimbing().Equals(false))
                {
                    scr_player.SetStatus(CHARACTERSTATUS.NONE);
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
                    if (scr_player.GetIsGrounded().Equals(true))
                        scr_player.sm.playRunningEffect();
                }

                if (scr_player.GetIsClimbing().Equals(true))
                    scr_player.SetCurMoveSpeed(0);
                break;
        }        
    }
}
