﻿using System.Collections;
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
    public cUseManager useMng; // 아이템 사용 관리 클래스
    private bool isJumpAniDone;
    public cBtn_item quickSlot;    

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
        {
            inven = cUtil._user.GetInventory();
        }
        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 500.0f;
        changingGravity = defaultGravity;
        SetIsGrounded(false);
        isClimbing = false;
        isSpeedUp = false;
        speedUpTime = 0.0f;
        speedUpAmount = 0.0f;
        jumpHeight = 200.0f;
        attackBoxPos[0] = new Vector3(18, 250, -1.1f); 
        attackBoxPos[1] = new Vector3(180, 58, -1.1f); 
        attackBoxPos[2] = new Vector3(60, -128, -1.1f);
        attackBoxPos[3] = new Vector3(100, 142, -1.1f);
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
        _animator.SetBool("isGrounded", GetIsGrounded());
        
        if (GetIsGrounded().Equals(false) && isJumpAniDone.Equals(false) 
            && isClimbing.Equals(false) && status != CHARACTERSTATUS.ATTACK)
        {
            _animator.SetTrigger("jumping");
            if (status.Equals(CHARACTERSTATUS.ATTACK))
                status = CHARACTERSTATUS.NONE;
            isJumpAniDone = true;
            landEffectPlayed = false;
            sm.StopRunningEffect();
        }
        else if(GetIsGrounded().Equals(true))
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

    public void Attack_front()
    {
        _animator.SetTrigger("AttackFront");
        status = CHARACTERSTATUS.ATTACK;
    }
    public void Attack_up()
    {
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
        if (pDir > 2)
        {
            pDir -= 3;
            isCritical = true;
        }
        else
            isCritical = false;

        //위
        if (pDir == 0)
            attackBox.transform.localPosition = attackBoxPos[0];
        //양옆
        else if (pDir == 1)
        {
            if(isClimbing.Equals(true))
                attackBox.transform.localPosition = attackBoxPos[3];
            else
                attackBox.transform.localPosition = attackBoxPos[1];
        }
        //아래
        else if (pDir == 2)
            attackBox.transform.localPosition = attackBoxPos[2];

        attackBox.SetActive(true);

        float t_tileHp = 0;

        if(tileMng.CheckAttackedTile(attackBox.transform.position, damage, out t_tileHp).Equals(true))
        {
            byte i = 1;

            if (t_tileHp.Equals(0))            
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

            if(isCritical.Equals(true))
            {
                effects[5].transform.position = attackBox.transform.position;
                effects[5].Play();
                sm.playAxeEffect(true);
            }
            else
            {
                effects[4].transform.position = attackBox.transform.position;
                effects[4].Play();
                sm.playAxeEffect(false);
            }

                        
            ft.DamageTextOn(damage.GetValueToString(), attackBox.transform.position);

        }
        
    }
    public void InactiveAttackBox()
    {
        attackBox.SetActive(false);
        status = CHARACTERSTATUS.NONE;
        _animator.SetBool("ChargeDone", false);
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
}
