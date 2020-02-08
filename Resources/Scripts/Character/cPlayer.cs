using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class cPlayer : cCharacter
{
    public cWeapon weapon;
    public cFloatingText ft;
    public bool isDash;
    public cInventory inven;
    public bool isJumpAttack;
    public int jumpAttackPoint;
    private bool isJumpAniDone;
        
    private bool isSpeedUp;
    private float speedUpTime;
    private float speedUpAmount;

    //이펙트
    private bool landEffectPlayed;
       
    public override void Init(string pNickName, cProperty pDamage, float pMoveSpeed, cProperty pMaxHp, cProperty pCurHp)
    {
        base.Init(pNickName, pDamage, pMoveSpeed, pMaxHp, pCurHp);

        _animator = this.GetComponent<Animator>();
        originObj = this.transform.parent.gameObject;
        if(cUtil._user != null)
            inven = cUtil._user.GetInventory();
        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 500.0f;
        changingGravity = defaultGravity;
        isGrounded = false;
        isClimbing = false;
        isSpeedUp = false;
        speedUpTime = 0.0f;
        speedUpAmount = 0.0f;
        jumpHeight = 200.0f;
        attackBoxPos[0] = new Vector3(18, 225, -1.1f); 
        attackBoxPos[1] = new Vector3(180, 0, -1.1f); 
        attackBoxPos[2] = new Vector3(22, -128, -1.1f);
        attackBoxPos[3] = new Vector3(180, 133, -1.1f);
        attackBox.transform.position = attackBoxPos[0];
        weapon.damage = damage;
        status = CHARACTERSTATUS.NONE;
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
        _animator.SetBool("isGrounded", isGrounded);

        if (isGrounded.Equals(false) && isJumpAniDone.Equals(false) 
            && isClimbing.Equals(false))
        {
            _animator.SetTrigger("jumping");
            if (status.Equals(CHARACTERSTATUS.ATTACK))
                status = CHARACTERSTATUS.NONE;
            isJumpAniDone = true;
            landEffectPlayed = false;
            sm.StopRunningEffect();
        }
        else if(isGrounded.Equals(true))
        {
            if (landEffectPlayed.Equals(false))
            {
                effects[0].Play();
                landEffectPlayed = true;
            }
            isJumpAniDone = false;
        }

        if(dir.Equals(Vector3.up))
            _animator.SetInteger("Direction", 0);
        else if (dir.Equals(Vector3.right))
            _animator.SetInteger("Direction", 1);
        else if (dir.Equals(Vector3.down))
            _animator.SetInteger("Direction", 2);
        else if (dir.Equals(Vector3.left))
            _animator.SetInteger("Direction", 3);  

        if(charDir.Equals(CHARDIRECTION.NONE))
            _animator.SetInteger("Direction", 1);
        
        if (!isClimbing)
        {
            if(!isJumpAttack)
                SetGravity();
            if (isGrounded)
                jumpAttackPoint = 0;
        }
        else if (!isRightBlocked && !isLeftBlocked)
        {
            SetIsClimbing(false);
        }
        else
        {
            changingGravity = defaultGravity;
        }

        if (isGrounded || GetIsClimbing())
            jumpCount = 0;
    }
       
    public void ChargeStart()
    {
        _animator.SetTrigger("isCharging");
    }
    public void ChargeOn()
    {
        _animator.SetBool("ChargeDone", true);
        status = CHARACTERSTATUS.ATTACK;
    }
    public void ChargeFail()
    {
        _animator.SetTrigger("ChargeFail");
        status = CHARACTERSTATUS.NONE;
    }

    public void Attack_front()
    {
        //if (isClimbing.Equals(true))
        //    return;
        _animator.SetTrigger("AttackFront");
        status = CHARACTERSTATUS.ATTACK;
    }
    public void Attack_up()
    {
        //if (isClimbing.Equals(true))
        //    return;
        _animator.SetTrigger("AttackUp");
        status = CHARACTERSTATUS.ATTACK;
    }
    public void Attack_down()
    {
        if (isClimbing.Equals(true))
            return;
        _animator.SetTrigger("AttackDown");
        status = CHARACTERSTATUS.ATTACK;
    }

    public void ActiveAttackBox(int pDir)
    {
        //위
        if (pDir == 0)
            attackBox.transform.localPosition = attackBoxPos[0];
        //양옆
        else if (pDir == 1)
        {
            if(GetIsClimbing().Equals(true))
            {
                attackBox.transform.localPosition = attackBoxPos[3];
            }
            else
                attackBox.transform.localPosition = attackBoxPos[1];
        }
        //아래
        else if (pDir == 2)
            attackBox.transform.localPosition = attackBoxPos[2];

        float t_tileHp = 0;

        if(tileMng.CheckAttackedTile(attackBox.transform.position, damage, out t_tileHp).Equals(true))
        {
            byte i = 1;

            if (t_tileHp < 0.3f)
                i = 3;
            else if (t_tileHp < 0.7f)
                i = 2;
            else
                i = 1;

            effects[i].transform.position = attackBox.transform.position;
            effects[i].transform.localScale = new Vector3(originObj.transform.localScale.x,
                effects[i].transform.localScale.y, effects[i].transform.localScale.z);
            effects[i].Play();
                        
            ft.DamageTextOn(damage.GetValueToString(), attackBox.transform.position);

            sm.playAxeEffect();
        }
        
    }
    public void InactiveAttackBox()
    {
        attackBox.SetActive(false);
        status = CHARACTERSTATUS.NONE;
        isJumpAttack = false;
        _animator.SetBool("ChargeDone", false);
    }
    
    public void UseItem(ITEM rItem)
    {
        Debug.Log(rItem);
        inven.GetItemUse()[(int)rItem].UseItem();
        inven.GetItemUse()[5].UseItem();
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
}
