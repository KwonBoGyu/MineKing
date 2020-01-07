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
<<<<<<< HEAD
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
=======
        if (Input.GetKeyDown(KeyCode.H))
        {
            scr_player.StartSpeedUp(100.0f, 3.0f);
>>>>>>> ad1a998ea032c6642e840e5b266f9fff114279e7
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
<<<<<<< HEAD
        switch(pDir)
        {
            //UP
            case 0:
                scr_player.SetDir(Vector3.up, CHARDIRECTION.UP);
=======
        padX = pos.x - this.transform.position.x;
        padY = pos.y - this.transform.position.y;
        padAngle = Mathf.Atan2(padY, padX);

        //RIGHT
        if (padAngle <= cUtil.pi / 8 && padAngle >= -cUtil.pi / 8)
        {
            scr_player.SetDir(Vector3.right, CHARDIRECTION.RIGHT);
            _player.transform.localScale = new Vector3(1, 1, 1);

            scr_player.LookUp(false);
            scr_player.LookDown(false);

            //달리기
            if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                 scr_player.GetStatus() != CHARACTERSTATUS.DASH)
            {
                scr_player.SetStatus(CHARACTERSTATUS.NONE);
                
                if (scr_player.isRightBlocked)
                {
                    scr_player.SetCurMoveSpeed(0);
                }
                else
                {
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
                }
            }
        }
        //UPRIGHT
        else if (padAngle < cUtil.pi * 3 / 8 && padAngle > cUtil.pi / 8)
        {
            scr_player.SetDir(Vector3.right, CHARDIRECTION.UPRIGHT);
            _player.transform.localScale = new Vector3(1, 1, 1);
            scr_player.LookUp(false);
            scr_player.LookDown(false);

            //달리기
            if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                 scr_player.GetStatus() != CHARACTERSTATUS.DASH)
            {
                scr_player.SetStatus(CHARACTERSTATUS.NONE);
                
                if (scr_player.isRightBlocked)
                {
                    scr_player.SetCurMoveSpeed(0);
                }
                else
                {
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
                }
            }
        }
        //UP
        else if (padAngle < cUtil.pi * 5 / 8 && padAngle >= cUtil.pi * 3 / 8)
        {
            scr_player.SetDir(Vector3.up, CHARDIRECTION.UP);
            scr_player.LookDown(false);
>>>>>>> ad1a998ea032c6642e840e5b266f9fff114279e7

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
<<<<<<< HEAD
                break;
            //RIGHT
            case 1:
                scr_player.SetDir(Vector3.right, CHARDIRECTION.RIGHT);
                _player.transform.localScale = new Vector3(1, 1, 1);

                //달리기
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                     scr_player.GetStatus() != CHARACTERSTATUS.DASH)
=======
            }
        }
        //UPLEFT
        else if (padAngle < cUtil.pi * 7 / 8 && padAngle > cUtil.pi * 5 / 8)
        {
            scr_player.SetDir(Vector3.left, CHARDIRECTION.UPLEFT);
            _player.transform.localScale = new Vector3(-1, 1, 1);
            scr_player.LookUp(false);
            scr_player.LookDown(false);

            //달리기
            if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                 scr_player.GetStatus() != CHARACTERSTATUS.DASH)
            {
                scr_player.SetStatus(CHARACTERSTATUS.NONE);
                
                if (scr_player.isLeftBlocked)
                {
                    scr_player.SetCurMoveSpeed(0);
                }
                else
                {
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
                }
            }
        }
        //LEFT
        else if (padAngle >= cUtil.pi * 7 / 8 || padAngle <= -cUtil.pi * 7 / 8)
        {
            scr_player.SetDir(Vector3.left, CHARDIRECTION.LEFT);
            _player.transform.localScale = new Vector3(-1, 1, 1);

            scr_player.LookUp(false);
            scr_player.LookDown(false);

            //왼쪽 이동
            if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                scr_player.GetStatus() != CHARACTERSTATUS.DASH)
            {
                scr_player.SetStatus(CHARACTERSTATUS.NONE);
                
                if (scr_player.isLeftBlocked)
                {
                    scr_player.SetCurMoveSpeed(0);
                }
                else
                {
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
                }
            }
        }
        //DOWNLEFT
        else if (padAngle < -cUtil.pi * 5 / 8 && padAngle > -cUtil.pi * 7 / 8)
        {
            scr_player.SetDir(Vector3.left, CHARDIRECTION.DOWNLEFT);

            _player.transform.localScale = new Vector3(-1, 1, 1);

            scr_player.LookUp(false);
            if(scr_player.isGrounded.Equals(false))
                scr_player.LookDown(false);

            if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                 scr_player.GetStatus() != CHARACTERSTATUS.DASH)
            {
                
                if (scr_player.isLeftBlocked)
                {
                    scr_player.SetCurMoveSpeed(0);
                }
                else
                {
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed() / 2.0f);
                }

                if (scr_player.isGrounded.Equals(true))
>>>>>>> ad1a998ea032c6642e840e5b266f9fff114279e7
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
<<<<<<< HEAD
                break;
            //LEFT
            case 3:
                scr_player.SetDir(Vector3.left, CHARDIRECTION.LEFT);
                _player.transform.localScale = new Vector3(-1, 1, 1);

                //왼쪽 이동
                if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                    scr_player.GetStatus() != CHARACTERSTATUS.DASH)
=======
            }
        }
        //DOWNRIGHT
        else if (padAngle < -cUtil.pi * 1 / 8 && padAngle > -cUtil.pi * 3 / 8)
        {
            scr_player.SetDir(Vector3.right, CHARDIRECTION.DOWNRIGHT);
            _player.transform.localScale = new Vector3(1, 1, 1);

            scr_player.LookUp(false);
            if (scr_player.isGrounded.Equals(false))
                scr_player.LookDown(false);

            //기어가기
            if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
                 scr_player.GetStatus() != CHARACTERSTATUS.DASH && 
                 scr_player.isGrounded.Equals(true))
            {
                
                if (scr_player.isRightBlocked)
                {
                    scr_player.SetCurMoveSpeed(0);
                }
                else
                {
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed() / 2.0f);
                }

                if (scr_player.isGrounded.Equals(true))
>>>>>>> ad1a998ea032c6642e840e5b266f9fff114279e7
                {
                    scr_player.SetStatus(CHARACTERSTATUS.NONE);
                    scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
                }
                break;
        }        
    }
}
