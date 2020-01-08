﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class cPlayer : cCharacter
{
    public cWeapon weapon;
    public bool isDash;
    public cInventory inven;
    public bool isJumpAttack;
    public int jumpAttackPoint;
    private bool isJumpAniDone;

    
    private bool isSpeedUp;
    private float speedUpTime;
    private float speedUpAmount;

    

    public override void Init(string pNickName, float pDamage, float pMoveSpeed, float pMaxHp, float pCurHp)
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
        attackBoxPos[0] = new Vector3(18, 225, 0); 
        attackBoxPos[1] = new Vector3(78, 2.08f, 0); 
        attackBoxPos[2] = new Vector3(22, -128, 0);
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

        if (isGrounded.Equals(false) && isJumpAniDone.Equals(false))
        {
            _animator.SetTrigger("jumping");
            isJumpAniDone = true;
        }
        else if(isGrounded.Equals(true))
            isJumpAniDone = false;

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
            isClimbing = false;
        }
        else
        {
            changingGravity = defaultGravity;
        }
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
            attackBox.transform.localPosition = attackBoxPos[1];
        //아래
        else if (pDir == 2)
            attackBox.transform.localPosition = attackBoxPos[2];

        tileMng.CheckAttackedTile(attackBox.transform.position, damage);
    }
    public void InactiveAttackBox()
    {
        attackBox.SetActive(false);
        status = CHARACTERSTATUS.NONE;
        isJumpAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }

    public void UseItem(ITEM rItem)
    {
        Debug.Log(rItem);
        inven.GetItemUse()[(int)rItem].UseItem();
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
