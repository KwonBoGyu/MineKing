using System.Collections;
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
    private Vector3 prevTouchPos;
    private float minSwipeDist;
    public float prevSwipeDist;
    public float swipeDist;


    private void Start()
    {
        rad = this.GetComponent<RectTransform>().sizeDelta.y * 0.2f;
        defaultPos = joystick.transform.position;
        isDrag = false;
        scr_player = _player.transform.GetChild(0).GetComponent<cPlayer>();
        b_jump.onClick.AddListener(() => Jump());
        b_dash.onClick.AddListener(() => Dash());
        b_item.onClick.AddListener(() => Item());
        scr_player.SetDir(Vector2.right);
        scr_player.SetCurMoveSpeed(0);
        jumpCount = 0;
        keyTime = 0;
        minSwipeDist = 110;
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

        if (scr_player.GetStatus() == CHARACTERSTATUS.ATTACK)
            scr_player.SetCurMoveSpeed(0);
        else
        {
            if (isDrag.Equals(true))
            {
                if (swipeDist > minSwipeDist)
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

                    this.transform.position = dragPos;
                    prevTouchPos = this.transform.position;
                }
            }
        }
    }

    public void SetSwipeDistAfterAttack()
    {
        swipeDist = prevSwipeDist;
    }

    public void PointerDown(BaseEventData _data)
    {
        PointerEventData d = _data as PointerEventData;
        Vector3 pos = d.position;
        prevTouchPos = d.position;

        this.transform.position = pos;
    }

    public void Drag(BaseEventData _data)
    {
        PointerEventData d = _data as PointerEventData;
        dragPos = d.position;
        isDrag = true;

        //스와이프 거리
        swipeDist = (dragPos - prevTouchPos).magnitude;
        if (swipeDist > minSwipeDist)
            prevSwipeDist = swipeDist;
        //방향
        joyDir = (dragPos - this.transform.position).normalized;        
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

                    //로프가 있을 때
                    if (scr_player.GetIsOnRope().Equals(true))
                    {
                        scr_player.SetIsAttatchedOnRope(true);
                        scr_player.SetCurMoveSpeed(3);
                    }
                    //로프가 없을 때
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
                    //로프가 있을 때
                    if (scr_player.GetIsAttatchedOnRope().Equals(true))
                    {
                        scr_player.SetCurMoveSpeed(3);
                    }
                    //로프가 없을 때
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
