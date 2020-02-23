using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cProjectile : cObject
{
    public Vector3 dir; // triangle에 본 방향벡터가 그리는 삼각형 저장
    protected Vector3 triangle; // 빗변, 밑변, 높이 순으로 저장
    protected bool upBlockedContinue;

    protected bool isReflectOn;
    public bool isGravityOn;
    protected float gravityAmount;
    protected float flyingTime;

    protected float defaultPower;
    protected float changingPower;
    protected float posX;
    protected float posY;

    public virtual void Init()
    {
        flyingTime = 0;
        defaultPower = 12f;
        changingPower = defaultPower;
        isReflectOn = true;
        isGravityOn = true;
        gravityAmount = 9.8f;
    }

    public void SetDir(Vector3 pDir)
    {
        dir = pDir.normalized;
        triangle = new Vector3(Mathf.Sqrt(Mathf.Pow(dir.x, 2) + Mathf.Pow(dir.y, 2)), dir.x, dir.y);
    }

    protected virtual void Move()
    {
        // 이동 궤적
        posX = changingPower * (triangle.y / triangle.x); //코사인
        posY = changingPower * (triangle.z / triangle.x); //사인

        if (isGravityOn)
        {   
            flyingTime += Time.deltaTime * 3;
            posY -= gravityAmount * flyingTime;
        }

        this.transform.position = new Vector3(this.transform.position.x + posX,
            this.transform.position.y + posY,
            this.transform.position.z);

        if (upBlockedContinue.Equals(true))
        {
            upBlockedContinue = true;
            this.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y - 10, this.transform.position.z);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Move();

        if(tileMng != null)
        {
            if (isReflectOn)
            {
                SetReflect();
            }
            else
            {
               tileMng.CheckCanGroundTile(this);
            }
        }
    }

    protected void SetReflect()
    {
        bool isChecked = false;

        tileMng.CheckCanGroundTile(this, out isChecked);

        if (isChecked.Equals(true))
        {
            if (isUpBlocked.Equals(true))
            {
                Debug.Log("UP BLOCKED");
                ReducePower();
                ResetMove();
                upBlockedContinue = true;
            }
            if (isRightBlocked.Equals(true))
            {
                Debug.Log("RIGHT BLOCKED");
                SetDir(new Vector3(dir.x * -1, dir.y, dir.z));
                ReducePower(0.5f);
                ResetMove();
                upBlockedContinue = false;
            }
            if (isLeftBlocked.Equals(true))
            {
                Debug.Log("LEFT BLOCKED");
                SetDir(new Vector3(dir.x * -1, dir.y, dir.z));
                ReducePower(0.5f);
                ResetMove();
                upBlockedContinue = false;
            }
            if (GetIsGrounded().Equals(true))
            {
                Debug.Log("DOWN BLOCKED");
                ReducePower(0.8f);
                ResetMove();
                upBlockedContinue = false;
            }
        }
    }

    protected void ReducePower(float pAmount = 0.76f)
    {
        changingPower = changingPower * pAmount;
    }

    // 중력이 적용되어 있을 경우 새로운 포물선을 그리기 위한 함수
    protected void ResetMove()
    {
        flyingTime = 0;
        posX = 0;
        posY = 0;
    }
}
