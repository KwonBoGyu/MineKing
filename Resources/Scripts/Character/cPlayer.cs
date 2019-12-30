﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class cPlayer : cCharacter
{
    public Animator _animator;
    public cWeapon weapon;
    public bool isDash;
    public cInventory inven;
    public bool isJumpAttack;
    public int jumpAttackPoint;

    private bool isOnRope;
    private bool isAttatchedOnRope;
    private bool isSpeedUp;
    private float speedUpTime;
    private float speedUpAmount;

    public bool GetIsOnRope() { return isOnRope; }
    public bool GetIsAttatchedOnRope() { return isAttatchedOnRope; }
    public void SetIsAttatchedOnRope(bool pBool)
    {
        isAttatchedOnRope = pBool;
    }

    public override void Init(string pNickName, float pDamage, float pMoveSpeed, float pMaxHp, float pCurHp)
    {
        base.Init(pNickName, pDamage, pMoveSpeed, pMaxHp, pCurHp);

        originObj = this.transform.parent.gameObject;
        if(cUtil._user != null)
            inven = cUtil._user.GetInventory();
        rt = originObj.GetComponent<BoxCollider2D>();
        defaultGravity = 300.0f;
        changingGravity = defaultGravity;
        isGrounded = false;
        isOnRope = false;
        isAttatchedOnRope = false;
        isSpeedUp = false;
        speedUpTime = 0.0f;
        speedUpAmount = 0.0f;
        jumpHeight = 200.0f;
        attackBoxPos[0] = new Vector3(18, 225, 0); 
        attackBoxPos[1] = new Vector3(78, 2.08f, 0); 
        attackBoxPos[2] = new Vector3(22, -128, 0);

        weapon.damage = damage;
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

    
    
    public override void FixedUpdate()
    {
        originObj.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
        _animator.SetFloat("MoveSpeed", curMoveSpeed);

        base.FixedUpdate();
        if (!isAttatchedOnRope)
        {
            if(isJumpAttack.Equals(false))
                SetGravity();

            if (isGrounded.Equals(true))
                jumpAttackPoint = 0;
        }
    }

    public void LookUp(bool pB)
    {
        _animator.SetBool("LookUp", pB);
    }
    public void LookDown(bool pB)
    {
        _animator.SetBool("LookDown", pB);
    }

    public void Attack_front()
    {
        _animator.SetTrigger("AttackFront");
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

        attackBox.SetActive(true);
    }
    public void InactiveAttackBox()
    {
        attackBox.SetActive(false);
        status = CHARACTERSTATUS.NONE;
        isJumpAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Rope"))
        {
            isOnRope = true;
            Debug.Log("isOnRope");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Rope"))
        {
            isOnRope = false;
            isAttatchedOnRope = false;
        }
    }

    public void SetRope()
    {
        inven.GetItemUse()[0].SetRope(this.gameObject);
    }

    public void SetSandBag()
    {
        inven.GetItemUse()[0].SetSandBag(this.gameObject, dir);
    }

    public void SetBomb()
    {
        inven.GetItemUse()[0].SetBomb(this.gameObject);
    }

    public void UseSpeedPotion()
    {
        inven.GetItemUse()[0].UseSpeedPotion(ref isSpeedUp);
        speedUpTime = inven.GetItemUse()[0].GetSpeedUpTime();
        speedUpAmount = inven.GetItemUse()[0].GetSpeedUpAmount();
    }

    public void UseHpPotion()
    {
        IncreaseHP(inven.GetItemUse()[0].UseHpPotion(maxHp)); 
        SetHp();
    }
}