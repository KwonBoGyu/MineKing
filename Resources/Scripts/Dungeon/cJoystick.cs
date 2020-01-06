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
    private bool isDrag;
    private int jumpCount;
    private float keyTime;

    private bool isDragValuable;
    private float padX;
    private float padY;
    private float padAngle;


    private void Start()
    {
        rad = this.GetComponent<RectTransform>().sizeDelta.y * 0.2f;
        defaultPos = joystick.transform.position;
        isDrag = false;
        scr_player = _player.transform.GetChild(0).GetComponent<cPlayer>();
        b_attack.onClick.AddListener(() => Attack());
        b_jump.onClick.AddListener(() => Jump());
        //b_dash.onClick.AddListener(() => Dash());
        b_item.onClick.AddListener(() => Item());
        scr_player.SetDir(Vector2.right);
        scr_player.SetCurMoveSpeed(0);
        jumpCount = 0;
        keyTime = 0;
    }

    private void Update()
    {
#if UNITY_EDITOR
        //왼쪽 이동
        //if (Input.GetKey(KeyCode.LeftArrow) &&
        //    scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
        //    scr_player.GetStatus() != CHARACTERSTATUS.DASH)
        //{
        //    scr_player.SetDir(Vector3.left);
        //    _player.transform.localScale = new Vector3(-1, 1, 1);
        //    if (scr_player.GetStatus() == CHARACTERSTATUS.CROUCH)
        //        scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed() / 2);
        //    else
        //    {
        //        scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
        //        scr_player.SetStatus(CHARACTERSTATUS.NONE);
        //    }
        //}
        ////오른쪽 이동
        //else if (Input.GetKey(KeyCode.RightArrow) &&
        //    scr_player.GetStatus() != CHARACTERSTATUS.ATTACK &&
        //    scr_player.GetStatus() != CHARACTERSTATUS.DASH)
        //{
        //    scr_player.SetDir(Vector3.right);
        //    _player.transform.localScale = new Vector3(1, 1, 1);
        //    if (scr_player.GetStatus() == CHARACTERSTATUS.CROUCH)
        //        scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed() / 2);
        //    else
        //    {
        //        scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
        //        scr_player.SetStatus(CHARACTERSTATUS.NONE);
        //    }
        //}
        ////이동 안할 때
        //else


        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        //    {
        //        scr_player.SetStatus(CHARACTERSTATUS.NONE);
        //        scr_player.SetDir(Vector3.up);
        //        if (scr_player.GetIsOnRope() && scr_player.GetDirection() == Vector3.up)
        //        {
        //            scr_player.SetIsAttatchedOnRope(true);
        //        }
        //    }
        //}

        ////윗 방향키
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    //예외처리
        //    if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        //    {
        //        scr_player.SetStatus(CHARACTERSTATUS.NONE);
        //        scr_player.SetDir(Vector3.up);
        //        if (scr_player.GetIsAttatchedOnRope())
        //        {
        //            scr_player.SetCurMoveSpeed(3);
        //        }
        //        else
        //        {
        //            scr_player.LookUp(true);
        //            scr_player.SetCurMoveSpeed(0);
        //        }
        //    }
        //}

        //else if (Input.GetKeyUp(KeyCode.UpArrow))
        //{
        //    scr_player.LookUp(false);

        //    if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        //    {
        //        if (_player.transform.localScale.x == -1)
        //            scr_player.SetDir(Vector3.left);
        //        if (_player.transform.localScale.x == 1)
        //            scr_player.SetDir(Vector3.right);
        //    }
        //}

        ////아래 방향키
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    //예외처리
        //    if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        //    {
        //        scr_player.SetDir(Vector3.down);
        //        if (scr_player.GetIsOnRope())
        //        {
        //            scr_player.SetCurMoveSpeed(3);
        //        }
        //        scr_player.LookDown(true);
        //        scr_player.SetStatus(CHARACTERSTATUS.CROUCH);
        //    }
        //}
        //else if (Input.GetKeyUp(KeyCode.DownArrow))
        //{
        //    scr_player.LookDown(false);

        //    if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        //    {
        //        if (_player.transform.localScale.x == -1)
        //            scr_player.SetDir(Vector3.left);
        //        if (_player.transform.localScale.x == 1)
        //            scr_player.SetDir(Vector3.right);
        //        scr_player.SetStatus(CHARACTERSTATUS.NONE);
        //    }
        //}

        //if (scr_player.GetStatus() == CHARACTERSTATUS.ATTACK)
        //    scr_player.SetCurMoveSpeed(0);

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
        if (Input.GetKeyDown(KeyCode.H))
        {
            scr_player.StartSpeedUp(100.0f, 3.0f);
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

        //공격
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
            {
                scr_player.Attack_front();
                scr_player.SetStatus(CHARACTERSTATUS.ATTACK);
            }
        }

#endif
        ////////////////////////////////안드로이드///////////////////////////////////

        if (scr_player.GetStatus() == CHARACTERSTATUS.ATTACK)
            scr_player.SetCurMoveSpeed(0);

        if (scr_player.isGrounded.Equals(true) &&
            scr_player.GetStatus() != CHARACTERSTATUS.ATTACK)
        {
            if (scr_player.GetCharDir().Equals(CHARDIRECTION.UPRIGHT) ||
                scr_player.GetCharDir().Equals(CHARDIRECTION.RIGHT) ||
                scr_player.GetCharDir().Equals(CHARDIRECTION.UPLEFT) ||
                scr_player.GetCharDir().Equals(CHARDIRECTION.LEFT))
                scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed());
            else if (scr_player.GetCharDir().Equals(CHARDIRECTION.DOWNRIGHT) ||
                scr_player.GetCharDir().Equals(CHARDIRECTION.DOWNLEFT))
            {
                scr_player.LookDown(true);
                scr_player.SetCurMoveSpeed(scr_player.GetMaxMoveSpeed() * 0.5f);
            }
        }
        else if(scr_player.isGrounded.Equals(false))
        {
            scr_player.LookDown(false);
            scr_player.LookUp(false);
        }
    }

    public void PointerDown(BaseEventData _data)
    {
        PointerEventData d = _data as PointerEventData;
        Vector3 pos = d.position;
        isDrag = true;

        //방향
        joyDir = (pos - this.transform.position).normalized;

        //이동 거리
        float dist = Vector3.Distance(pos, this.transform.position);

        if (dist <= 70)
            isDragValuable = false;
        else
            isDragValuable = true;

        if (dist < rad)
            joystick.position = this.transform.position + joyDir * dist;
        else
            joystick.position = this.transform.position + joyDir * rad;

        if (isDragValuable.Equals(true))
            CalcDir(pos);
    }

    public void Drag(BaseEventData _data)
    {
        PointerEventData d = _data as PointerEventData;
        Vector3 pos = d.position;
        isDrag = true;

        //방향
        joyDir = (pos - this.transform.position).normalized;

        //이동 거리
        float dist = Vector3.Distance(pos, this.transform.position);
        if (dist <= 70)
            isDragValuable = false;
        else
            isDragValuable = true;

        if (dist < rad)
            joystick.position = this.transform.position + joyDir * dist;
        else
            joystick.position = this.transform.position + joyDir * rad;

        if (isDragValuable.Equals(true))
            CalcDir(pos);
    }

    public void DragEnd()
    {
        joystick.localPosition = Vector3.zero;
        this.transform.position = defaultPos;
        joyDir = Vector3.zero;
        isDrag = false;

        scr_player.SetCurMoveSpeed(0);
        scr_player.LookUp(false);
        scr_player.LookDown(false);
        scr_player.SetDir(scr_player.GetDirection(), CHARDIRECTION.NONE);
    }

    public void PointerUp(BaseEventData _data)
    {
        this.transform.position = defaultPos;
        joystick.localPosition = Vector3.zero;
        joyDir = Vector3.zero;
        isDrag = false;

        scr_player.SetCurMoveSpeed(0);
        scr_player.LookUp(false);
        scr_player.LookDown(false);
        scr_player.SetDir(scr_player.GetDirection(), CHARDIRECTION.NONE);
    }

    private void Jump()
    {
        if (scr_player.isGrounded == true || jumpCount < 2)
        {
            scr_player.LookDown(false);
            scr_player.LookUp(false) ;

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

    private void CalcDir(Vector3 pos)
    {
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

                    if (scr_player.isGrounded.Equals(true))
                    {
                        scr_player.LookUp(true);
                    }
                }
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
                {
                    scr_player.LookDown(true);
                    scr_player.SetStatus(CHARACTERSTATUS.CROUCH);
                }
            }
        }
        //DOWN
        else if (padAngle < -cUtil.pi * 3 / 8 && padAngle > -cUtil.pi * 5 / 8)
        {
            scr_player.SetDir(Vector3.down, CHARDIRECTION.DOWN);

            scr_player.LookUp(false);
            if (scr_player.isGrounded.Equals(false))
                scr_player.LookDown(false);

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

                    if (scr_player.isGrounded.Equals(true))
                    {
                        scr_player.LookDown(true);
                        scr_player.SetStatus(CHARACTERSTATUS.CROUCH);
                    }
                }
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
                {
                    scr_player.LookDown(true);
                    scr_player.SetStatus(CHARACTERSTATUS.CROUCH);
                }
            }
        }
        else
        {
            scr_player.SetCurMoveSpeed(0);
            scr_player.LookUp(false);
            scr_player.LookDown(false);
        }
    }
}
