using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class cPlayer : cCharacter
{
    public SpriteRenderer img_weapon;
    public Transform lightPos;

    public GameObject indicator;
    public Transform flagPos;

    public cWeapon weapon;
    public cFloatingText ft;
    public bool isDash;
    public cInventory inven;
    public cUseManager useMng; // 아이템 사용 관리 클래스
    private bool isJumpAniDone;
    public cBtn_item quickSlot;
    public cJoystick joystick;

    private bool isSpeedUp;
    private float speedUpTime;
    private float speedUpAmount;

    private bool isMoveAttack;
    public bool GetIsMoveAttack() { return isMoveAttack; }

    private byte doubleAttackPercentage;
    private bool isFalling;

    //이펙트
    private bool landEffectPlayed;

    public cDungeonNormal_processor dp;

    public override void Init(string pNickName, cProperty pDamage, float pMoveSpeed, cProperty pMaxHp, cProperty pCurHp)
    {
        base.Init(pNickName, pDamage, pMoveSpeed, pMaxHp, pCurHp);

#if UNITY_EDITOR
            this.gameObject.tag = "Untagged";
        
        _animator = this.GetComponent<Animator>();
        originObj = this.transform.parent.gameObject;
        
        rt = originObj.GetComponent<BoxCollider2D>();

        changingGravity = defaultGravity;
        SetIsGrounded(false);
        isClimbing = false;
        isSpeedUp = false;
        speedUpTime = 0.0f;
        speedUpAmount = 0.0f;
        jumpHeight = 200.0f;
        attackBoxPos[0] = new Vector3(18, 280, -1.1f);
        attackBoxPos[1] = new Vector3(180, 58, -1.1f);
        attackBoxPos[2] = new Vector3(60, -128, -1.1f);
        attackBoxPos[3] = new Vector3(100, 142, -1.1f);
        attackBox.transform.position = attackBoxPos[0];
        weapon.damage = damage;
        status = CHARACTERSTATUS.NONE;
        isMoveAttack = false;
        isFalling = false;
                
        {
            //스킬 레벨
            //대쉬
            byte dashFactor = 50;
            float dashCooltimeFactor = 0.5f;
            dashMoveSpeed = maxMoveSpeed + 300;
            maxDashCoolDown = 0;
            dashCoolDown = maxDashCoolDown;
            //시야
            byte lightFactor = 100;
            GameObject.Find("DungeonNormalScene").transform.Find("HeadLight").GetComponent<cHeadLight>().
            SetLightRange(lightFactor);
            //차지 계수
            //cBtn_attack에서 초기화
            //연속공격
            doubleAttackPercentage = (byte)(0);

            //내구도
            maxDp = new cProperty("MaxDp", 100);
            curDp = new cProperty("MaxDp", 100);

            SetDp();
        }
#endif
#if UNITY_ANDROID
        //if (cUtil._sm._scene != SCENE.SKIN)
        //    this.gameObject.tag = "Untagged";

        //_animator = this.GetComponent<Animator>();
        //originObj = this.transform.parent.gameObject;
        //if (cUtil._user != null)
        //{
        //    inven = cUtil._user.GetInventory();
        //}
        //rt = originObj.GetComponent<BoxCollider2D>();

        //changingGravity = defaultGravity;
        //SetIsGrounded(false);
        //isClimbing = false;
        //isSpeedUp = false;
        //speedUpTime = 0.0f;
        //speedUpAmount = 0.0f;
        //jumpHeight = 200.0f;
        //attackBoxPos[0] = new Vector3(18, 280, -1.1f);
        //attackBoxPos[1] = new Vector3(180, 58, -1.1f);
        //attackBoxPos[2] = new Vector3(60, -128, -1.1f);
        //attackBoxPos[3] = new Vector3(100, 142, -1.1f);
        //attackBox.transform.position = attackBoxPos[0];
        //weapon.damage = damage;
        //status = CHARACTERSTATUS.NONE;
        //isMoveAttack = false;
        //isFalling = false;

        //if (this.tag.Equals("player_skin"))
        //{
        //    //스킬 레벨
        //    //대쉬
        //    byte dashFactor = 50;
        //    float dashCooltimeFactor = 0.5f;
        //    dashMoveSpeed = maxMoveSpeed + 300 + dashFactor * 0;
        //    maxDashCoolDown = 4.0f - dashCooltimeFactor * 0;
        //    dashCoolDown = maxDashCoolDown;
        //    //시야
        //    byte lightFactor = 100;

        //    GameObject.Find("SkinScene").transform.Find("HeadLight").GetComponent<cHeadLight>().
        //    SetLightRange(lightFactor * 0);

        //    //차지 계수
        //    //cBtn_attack에서 초기화
        //    //연속공격
        //    doubleAttackPercentage = (byte)(0 + 10 * 0);
        //}
        //else
        //{
        //    //스킬 레벨
        //    //대쉬
        //    byte dashFactor = 50;
        //    float dashCooltimeFactor = 0.5f;
        //    dashMoveSpeed = maxMoveSpeed + 300 + dashFactor * cUtil._user._playerInfo.skillLevel[0];
        //    maxDashCoolDown = 4.0f - dashCooltimeFactor * cUtil._user._playerInfo.skillLevel[0];
        //    dashCoolDown = maxDashCoolDown;
        //    //시야
        //    byte lightFactor = 100;
        //    GameObject.Find("DungeonNormalScene").transform.Find("HeadLight").GetComponent<cHeadLight>().
        //    SetLightRange(lightFactor * cUtil._user._playerInfo.skillLevel[1]);

        //    //차지 계수
        //    //cBtn_attack에서 초기화
        //    //연속공격
        //    doubleAttackPercentage = (byte)(0 + 10 * cUtil._user._playerInfo.skillLevel[3]);

        //    //내구도
        //    maxDp = new cProperty("MaxDp", cUtil._user._playerInfo.weapon.indurance.value);
        //    curDp = new cProperty("MaxDp", cUtil._user._playerInfo.weapon.indurance.value);

        //    SetDp();
        //}

        //img_weapon.sprite = cUtil._user.WeaponEquipImages[cUtil._user._playerInfo.curWeaponId];
#endif

    }

    public override void SetCurMoveSpeed(float pCurMoveSpeed)
    {
        if (isSpeedUp)
        {
            curMoveSpeed = pCurMoveSpeed * speedUpAmount;
            StartCoroutine("SpeedUpTime");
        }
        else
            curMoveSpeed = pCurMoveSpeed;
    }

    IEnumerator SpeedUpTime()
    {
        float time = 0;
        while (time <= speedUpTime)
        {
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
        }
        isSpeedUp = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        originObj.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
        _animator.SetFloat("MoveSpeed", curMoveSpeed);
        _animator.SetBool("isGrounded", GetIsGrounded());

        //인디케이터 - 임시 주석처리
        //if (this.tag != "player_skin")
        //{
        //    Vector3 flagDir = (flagPos.position - originObj.transform.position).normalized;
        //    indicator.transform.position = new Vector3(originObj.transform.position.x + flagDir.x * 120,
        //        originObj.transform.position.y + flagDir.y * 120, originObj.transform.position.z);
        //    indicator.transform.up = flagDir;
        //}

        //점프 모션
        if (GetIsGrounded().Equals(false) && isJumpAniDone.Equals(false)
            && isClimbing.Equals(false) && status != CHARACTERSTATUS.ATTACK 
            && status != CHARACTERSTATUS.DASH )
        {
            _animator.SetTrigger("jumping");
            if (status.Equals(CHARACTERSTATUS.ATTACK))
                status = CHARACTERSTATUS.NONE;
            isJumpAniDone = true;
            landEffectPlayed = false;
            sm.StopRunningEffect();

        }
        else if (GetIsGrounded().Equals(true))
        {
            if (landEffectPlayed.Equals(false))
            {
                effects[0].Play();
                landEffectPlayed = true;
            }
            isJumpAniDone = false;
        }


        if (dir.Equals(Vector3.up))
            _animator.SetInteger("Direction", 0);
        else if (dir.Equals(Vector3.right))
            _animator.SetInteger("Direction", 1);
        else if (dir.Equals(Vector3.down))
            _animator.SetInteger("Direction", 2);
        else if (dir.Equals(Vector3.left))
            _animator.SetInteger("Direction", 3);

        if (charDir.Equals(CHARDIRECTION.NONE))
            _animator.SetInteger("Direction", 1);

        if (!isClimbing && GetIsGrounded().Equals(false))
        {
            SetGravity();
        }
        else
        {
            changingGravity = defaultGravity;
        }

        if (!isRightBlocked && !isLeftBlocked)
        {
            SetIsClimbing(false);
        }

    }

    public override void SetGravity()
    {
        base.SetGravity();

        if (changingGravity >= 1000f && !isFalling)
        {
            StartCoroutine(SetFallDamage());
        }
    }

    IEnumerator SetFallDamage()
    {
        float timer = 0;
        bool isFallDamageOn = false;
        isFalling = true;

        Debug.Log("Fall Start");
        while (true)
        {
            if (isClimbing)
            {
                Debug.Log("Reset Fall Timer");
                isFallDamageOn = false;
                timer = 0;
            }
            if (isGrounded)
            {
                Debug.Log("isGrounded");
                break;
            }

            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                Debug.Log("fall damage on");
                isFallDamageOn = true;
            }

            yield return new WaitForFixedUpdate();
        }

        if (isFallDamageOn)
        {
            Debug.Log("Reduced Hp for Falling : " + maxHp.value * 0.1f * timer);
            ReduceHp((long)(maxHp.value * 0.1f * timer));
        }

        isFalling = false;
    }

    public void ChargeAttack_front()
    {
        _animator.SetTrigger("ChargeAttack_front");
        status = CHARACTERSTATUS.ATTACK;
    }
    public void ChargeAttack_up()
    {
        _animator.SetTrigger("ChargeAttack_up");
        status = CHARACTERSTATUS.ATTACK;
    }
    public void ChargeAttack_down()
    {
        _animator.SetTrigger("ChargeAttack_down");
        status = CHARACTERSTATUS.ATTACK;
    }

    public void Attack_front(bool pIsDounbleAttack = false)
    {
        if(pIsDounbleAttack.Equals(true))        
            _animator.speed = 3;
        else
            _animator.speed = 1;

        _animator.SetTrigger("AttackFront");
        status = CHARACTERSTATUS.ATTACK;
    }
    public void Attack_up(bool pIsDounbleAttack = false)
    {
        if (pIsDounbleAttack.Equals(true))
            _animator.speed = 3;
        else
            _animator.speed = 1;

        _animator.SetTrigger("AttackUp");
        status = CHARACTERSTATUS.ATTACK;
    }
    public void Attack_down(bool pIsDounbleAttack = false)
    {
        if (pIsDounbleAttack.Equals(true))
            _animator.speed = 3;
        else
            _animator.speed = 1;

        if (isClimbing.Equals(true))
            return;
        _animator.SetTrigger("AttackDown");
        status = CHARACTERSTATUS.ATTACK;
    }
    public void SetMoveOnAttack()
    {

        StartCoroutine(MoveOnAttack());
    }

    IEnumerator MoveOnAttack()
    {
        curMoveSpeed = maxMoveSpeed;
        isMoveAttack = true;
        yield return new WaitForSeconds(0.1f);

        isMoveAttack = false;
        curMoveSpeed = 0f;
    }

    IEnumerator DoubleAttack(byte pDir)
    {
        yield return new WaitForSeconds(0.2f);

        ActiveDoubleAttackBox(pDir);

        yield return new WaitForSeconds(0.1f);

        InactiveDoubleAttackBox();
    }

    public void ActiveDoubleAttackBox(int pDir)
    {
        if (pDir > 2)
        {
            pDir -= 3;
            isCritical = true;
            damage_crit.value = (long)(damage.value * 2);
        }
        else
            isCritical = false;

        //위
        if (pDir == 0)
            doubleAttackBox.transform.localPosition = attackBoxPos[0];
        //양옆
        else if (pDir == 1)
        {
            if (isClimbing.Equals(true))
                doubleAttackBox.transform.localPosition = attackBoxPos[3];
            else
            {
                doubleAttackBox.transform.localPosition = attackBoxPos[1];
                if (joystick.GetStickDir() == JOYSTICKDIR.LEFT)
                {
                    SetMoveOnAttack();
                }
                else if (joystick.GetStickDir() == JOYSTICKDIR.RIGHT)
                {
                    SetMoveOnAttack();
                }
            }
        }
        //아래
        else if (pDir == 2)
            doubleAttackBox.transform.localPosition = attackBoxPos[2];

        doubleAttackBox.SetActive(true);

        float t_tileHp = 0;
        long curDmg = isCritical ? damage_crit.value : damage.value;

        if (tileMng.CheckAttackedTile(doubleAttackBox.transform.position, curDmg, out t_tileHp).Equals(true))
        {
            byte i = 1;

            if (t_tileHp.Equals(0) && this.tag != "player_skin")
                quickSlot.UpdateQuickSlot();
            else if (t_tileHp < 0.3f)
                i = 3;
            else if (t_tileHp < 0.7f)
                i = 2;
            else
                i = 1;

            effects[i].transform.position = doubleAttackBox.transform.position;
            effects[i].transform.localScale = new Vector3(originObj.transform.localScale.x,
                effects[i].transform.localScale.y, effects[i].transform.localScale.z);
            effects[i].Play();

            if (isCritical.Equals(true))
            {
                effects[5].transform.position = doubleAttackBox.transform.position;
                effects[5].Play();

                sm.playAxeEffect(true);
                ft.DamageTextOn(damage_crit.GetValueToString(), doubleAttackBox.transform.position, true);
            }
            else
            {
                effects[4].transform.position = doubleAttackBox.transform.position;
                effects[4].Play();
                sm.playAxeEffect(false);

                ft.DamageTextOn(damage.GetValueToString(), doubleAttackBox.transform.position);
            }
                        
            //내구도 하락
            curDp.value -= 1;
            if (curDp.value < 0)
                curDp.value = 0;
            SetDp();
        }
    }
    public void InactiveDoubleAttackBox()
    {
        doubleAttackBox.SetActive(false);
    }

    public void ActiveAttackBox(int pDir)
    {
        if (pDir > 2)
        {
            pDir -= 3;
            isCritical = true;
            damage_crit.value = (long)(damage.value * 2);
        }
        else
            isCritical = false;

        //위
        if (pDir == 0)
            attackBox.transform.localPosition = attackBoxPos[0];
        //양옆
        else if (pDir == 1)
        {
            if (isClimbing.Equals(true))
                attackBox.transform.localPosition = attackBoxPos[3];
            else
            {
                attackBox.transform.localPosition = attackBoxPos[1];
                if (joystick.GetStickDir() == JOYSTICKDIR.LEFT)
                {
                    SetMoveOnAttack();
                }
                else if (joystick.GetStickDir() == JOYSTICKDIR.RIGHT)
                {
                    SetMoveOnAttack();
                }
            }
        }
        //아래
        else if (pDir == 2)
            attackBox.transform.localPosition = attackBoxPos[2];

        attackBox.SetActive(true);

        //더블어택 검사
        byte doubleAt = (byte)Random.Range(1, 101);
        if (doubleAt < doubleAttackPercentage)
            StartCoroutine(DoubleAttack((byte)pDir));

        float t_tileHp = 0;
        long curDmg = isCritical ? damage_crit.value : damage.value;

        if (tileMng.CheckAttackedTile(attackBox.transform.position, curDmg, out t_tileHp).Equals(true))
        {
            byte i = 1;

            if (t_tileHp.Equals(0) && this.tag != "player_skin")
                quickSlot.UpdateQuickSlot();
            else if (t_tileHp < 0.3f)
                i = 3;
            else if (t_tileHp < 0.7f)
                i = 2;
            else
                i = 1;

            effects[i].transform.position = attackBox.transform.position;
            effects[i].transform.localScale = new Vector3(originObj.transform.localScale.x,
                effects[i].transform.localScale.y, effects[i].transform.localScale.z);
            effects[i].Play();

            if (isCritical.Equals(true))
            {
                effects[5].transform.position = attackBox.transform.position;
                effects[5].Play();
                sm.playAxeEffect(true);
                ft.DamageTextOn(damage_crit.GetValueToString(), attackBox.transform.position, true);
            }
            else
            {
                effects[4].transform.position = attackBox.transform.position;
                effects[4].Play();
                sm.playAxeEffect(false);
                ft.DamageTextOn(damage.GetValueToString(), attackBox.transform.position);
            }

            //내구도 하락
            curDp.value -= 1;
            if (curDp.value < 0)
                curDp.value = 0;
            SetDp();
        }
    }
    public void InactiveAttackBox()
    {
        joystick.SetDirOnAttack();
        attackBox.SetActive(false);
        status = CHARACTERSTATUS.NONE;
    }

    public byte UseItem(byte pItemKind)
    {
        byte curAmount = 100;
        for (byte i = 0; i < inven.GetItemUse().Count; i++)
        {
            if (pItemKind.Equals((byte)inven.GetItemUse()[i].kind))
            {
                curAmount = inven.GetItemUse()[i].UseItem();
                break;
            }
        }
        return curAmount;
    }

    public void StartSpeedUp(float pAmount, float pTime)
    {
        StartCoroutine(SpeedUp(pAmount, pTime));
    }

    IEnumerator SpeedUp(float pAmount, float pTime)
    {
        Debug.Log("스피드업 시작");
        maxMoveSpeed += pAmount;

        yield return new WaitForSeconds(pTime);

        Debug.Log("스피드업 끝");
        maxMoveSpeed -= pAmount;
    }

    public void RootRocks(long pVal)
    {
        if(this.tag != "player_skin")
        {
            inven.GetRock().value += pVal;
            dp.UpdateValue();
        }
    }
}
