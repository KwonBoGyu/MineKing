using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Bomb_O : cCharacter
{
    public GameObject explodeCollider;
    private Vector3 originTPos;
    public cProperty damage;
    private bool isAttatched;
    private float timer;
    private bool isTimeDone;
    private bool isExplodeDone;
    private Vector3Int[] explodeRange;
    private float radius;
    public bool isRight;
    public bool isUp;
    private float flyingTime;
    private float defaultPower;
    private float changingPower;
    private float degree;
    private float speedX;
    private float speedY;
    private bool upBlockedContinue;
    const float gravityConst = 9.8f;

    private void Start()
    {
        rt = originObj.GetComponent<BoxCollider2D>();
        damage = new cProperty("damage", 100);
        timer = 0;
        isAttatched = false;
        isTimeDone = false;
        isExplodeDone = false;
        originTPos = Vector3.zero;
        defaultGravity = 400;
        changingGravity = defaultGravity;
        radius = this.gameObject.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        flyingTime = 0;
        defaultPower = 12f;
        changingPower = defaultPower;
        degree = Mathf.PI * 0.25f;
        speedX = 0;
        speedY = 0;
        isUp = true;
    }

    private void OnEnable()
    {
        this.transform.position = cUtil._player.originObj.transform.position;
        explodeCollider.SetActive(false);
        timer = 0;
        changingPower = defaultPower;
        isAttatched = false;
        isTimeDone = false;
        flyingTime = 0;
        speedX = 0;
        speedY = 0;
    }

    private void SetExplodeRange()
    {
        originTPos = this.gameObject.transform.position;
        // 3 x 3 범위 타일
        explodeRange =
            new Vector3Int[]{
                new Vector3Int((int)originTPos.x, (int)originTPos.y + 150, 0),
                new Vector3Int((int)originTPos.x + 60, (int)originTPos.y + 150, 0),
                new Vector3Int((int)originTPos.x + 150, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x + 60, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 60, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 150, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x - 60, (int)originTPos.y + 150, 0)
            };
    }

    private void Move()
    {
        flyingTime += Time.deltaTime * 3;
        speedX = changingPower * Mathf.Cos(degree);
        speedY = changingPower * Mathf.Sin(degree) - (gravityConst * flyingTime);


        if (isRight)
        {
            this.transform.position = new Vector3(this.transform.position.x + speedX,
               this.transform.position.y + speedY,
                   this.transform.position.z);
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x - speedX,
               this.transform.position.y + speedY,
                   this.transform.position.z);
        }

        if (upBlockedContinue.Equals(true))
        {
            upBlockedContinue = true;
            this.transform.position = new Vector3(this.transform.position.x,
    this.transform.position.y - 10,
        this.transform.position.z);
        }
    }

    protected override void FixedUpdate()
    {
        bool isChecked = false;

        if (cUtil._tileMng != null && tileMng == null)
            tileMng = cUtil._tileMng;

        if (tileMng != null)
        {
            isChecked = tileMng.CheckCanGroundTile_bomb(this);
            if (isChecked.Equals(true))
            {
                if (isUpBlocked.Equals(true))
                {
                    ResetDir();
                    upBlockedContinue = true;
                }
                if (isRightBlocked.Equals(true))
                {
                    isRight = false;
                    ResetDir(1);
                    upBlockedContinue = false;
                }
                if (isLeftBlocked.Equals(true))
                {
                    isRight = true;
                    ResetDir(3);
                    upBlockedContinue = false;
                }
                if (GetIsGrounded().Equals(true))
                {
                    ResetDir(2);
                    upBlockedContinue = false;
                }
            }
        }



        if (!isAttatched)
        {
            Move();
        }

        if (isTimeDone.Equals(false) && isExplodeDone.Equals(false))
        {
            timer += Time.deltaTime;
        }
        else if (isTimeDone)
        {
            Debug.Log("Bomb Explode");
            float temp_TileHp = 0;
            explodeCollider.SetActive(true);

            //폭발 범위 지정 및 폭발
            SetExplodeRange();
            for (int i = 0; i < explodeRange.Length; i++)
            {
                cUtil._tileMng.CheckAttackedTile(explodeRange[i], damage, out temp_TileHp);
            }
            isTimeDone = false;
            isExplodeDone = true;
        }
        // 플레이어와 적 피격처리를 위해 active상태가 1프레임 더 지속되어야 하기 때문에 이 루프를 시행한다.
        else if (isExplodeDone)
        {
            isExplodeDone = false;
            this.gameObject.SetActive(false);
        }

        if (timer >= 3.0f)
        {
            isTimeDone = true;
            timer = 0;
        }

        if (changingPower <= 1f && changingPower >= 0)
        {
            isAttatched = true;
        }
    }

    public void ResetDir(byte pDir = 100)
    {
        flyingTime = 0;
        speedX = 0;
        speedY = 0;

        if (pDir.Equals(100))
        {
            changingPower = changingPower * 0.76f;
        }
        else if (pDir.Equals(0))
        {

        }
        else if (pDir.Equals(1))
        {
            changingPower = changingPower * 0.5f;
        }
        else if (pDir.Equals(2))
        {
            changingPower = changingPower * 0.8f;
        }
        else if (pDir.Equals(3))
        {
            changingPower = changingPower * 0.5f;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag.Equals("Tile_cannotHit") || collision.gameObject.tag.Equals("Tile_canHit"))
    //    {
    //        Vector3 colPos = collision.contacts[0].point;
    //        Debug.Log(collision.transform.position);
    //        //float yDiff = this.GetComponent<RectTransform>().rect.yMin - colPos.y;
    //        //float xDiff = this.GetComponent<Rigidbody2D>().position.x - colPos.x;
    //        //Debug.Log("xdiff :" + xDiff);
    //        //Debug.Log("yDiff :" + yDiff);

    //        //위 충돌
    //        //if(collision.)

    //        //float yMin = this.GetComponent<CircleCollider2D>().bounds.min.y;
    //        //float yInterval = Mathf.Abs(colPos.y - yMin);
    //        //float xInterval;
    //        //if (isRight)
    //        //{
    //        //    float xMax = this.GetComponent<CircleCollider2D>().bounds.max.x;
    //        //    xInterval = Mathf.Abs(colPos.x - xMax);
    //        //}
    //        //else
    //        //{
    //        //    float xMin = this.GetComponent<CircleCollider2D>().bounds.min.x;
    //        //    xInterval = Mathf.Abs(colPos.x - xMin);
    //        //}

    //        //if (Mathf.Abs(xDiff) <= Mathf.Abs(yDiff))
    //        //{
    //        //    Debug.Log("상하충돌");
    //        //    // 위충돌
    //        //    if (speedY >= 0)
    //        //    {
    //        //        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - yInterval - Mathf.Abs(speedY), 0);
    //        //    }
    //        //    //아래충돌
    //        //    else
    //        //    {
    //        //        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + yInterval + Mathf.Abs(speedY), 0);
    //        //    }

    //        //    flyingTime = 0;
    //        //    speedX = 0;
    //        //    speedY = 0;
    //        //    changingPower = changingPower * 0.7f;
    //        //}
    //        //else if (yDiff.Equals(0))
    //        //{
    //        //    Debug.Log("좌우충돌1");
    //        //    if (isRight)
    //        //    {
    //        //        isRight = false;
    //        //    }
    //        //    else
    //        //    {
    //        //        isRight = true;
    //        //    }
    //        //    if (isRight.Equals(false))
    //        //    {
    //        //        this.transform.position = new Vector3(this.transform.position.x - xInterval,
    //        //            this.transform.position.y, 0);
    //        //    }
    //        //    else
    //        //    {
    //        //        this.transform.position = new Vector3(this.transform.position.x + xInterval,
    //        //            this.transform.position.y, 0);
    //        //    }
    //        //    flyingTime = 0;
    //        //    speedX = 0;
    //        //    changingPower = changingPower * 0.7f;

    //        //}
    //        //else if (Mathf.Abs(xDiff) <= Mathf.Abs(yDiff))
    //        //{
    //        //    if (isRight)
    //        //    {
    //        //        this.transform.position = new Vector3(this.transform.position.x - xInterval,
    //        //            this.transform.position.y, 0);
    //        //    }
    //        //    else
    //        //    {
    //        //        this.transform.position = new Vector3(this.transform.position.x + xInterval,
    //        //            this.transform.position.y, 0);
    //        //    }
    //        //    flyingTime = 0;
    //        //    speedX = 0;
    //        //    changingPower = changingPower * 0.7f;
    //        //}
    //    }

    //    //if (collision.gameObject.tag.Equals("Tile_canHit"))
    //    //{
    //    //    Vector3 colPos = collision.contacts[0].point;
    //    //    float yDiff = this.GetComponent<Rigidbody2D>().position.y - colPos.y;
    //    //    float xDiff = this.GetComponent<Rigidbody2D>().position.x - colPos.x;
    //    //    Debug.Log("xdiff :" + xDiff);
    //    //    Debug.Log("yDiff :" + yDiff);

    //    //    float yMin = this.GetComponent<CircleCollider2D>().bounds.min.y;
    //    //    float yInterval = Mathf.Abs(colPos.y - yMin);
    //    //    float xInterval;
    //    //    if (isRight)
    //    //    {
    //    //        float xMax = this.GetComponent<CircleCollider2D>().bounds.max.x;
    //    //        xInterval = Mathf.Abs(colPos.x - xMax);
    //    //    }
    //    //    else
    //    //    {
    //    //        float xMin = this.GetComponent<CircleCollider2D>().bounds.min.x;
    //    //        xInterval = Mathf.Abs(colPos.x - xMin);
    //    //    }

    //    //    if (Mathf.Abs(xDiff) <= Mathf.Abs(yDiff))
    //    //    {
    //    //        Debug.Log("상하충돌");
    //    //        // 위충돌
    //    //        if (speedY >= 0)
    //    //        {
    //    //            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - yInterval - Mathf.Abs(speedY), 0);
    //    //        }
    //    //        //아래충돌
    //    //        else
    //    //        {
    //    //            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + yInterval + Mathf.Abs(speedY), 0);
    //    //        }

    //    //        flyingTime = 0;
    //    //        speedX = 0;
    //    //        speedY = 0;
    //    //        changingPower = changingPower * 0.7f;
    //    //    }
    //    //    else if (yDiff.Equals(0))
    //    //    {
    //    //        Debug.Log("좌우충돌1");
    //    //        if (isRight)
    //    //        {
    //    //            isRight = false;
    //    //        }
    //    //        else
    //    //        {
    //    //            isRight = true;
    //    //        }
    //    //        if (isRight.Equals(false))
    //    //        {
    //    //            this.transform.position = new Vector3(this.transform.position.x - xInterval,
    //    //                this.transform.position.y, 0);
    //    //        }
    //    //        else
    //    //        {
    //    //            this.transform.position = new Vector3(this.transform.position.x + xInterval,
    //    //                this.transform.position.y, 0);
    //    //        }
    //    //        flyingTime = 0;
    //    //        speedX = 0;
    //    //        changingPower = changingPower * 0.7f;

    //    //    }
    //    //    else if (Mathf.Abs(xDiff) <= Mathf.Abs(yDiff))
    //    //    {
    //    //        if (isRight)
    //    //        {
    //    //            this.transform.position = new Vector3(this.transform.position.x - xInterval,
    //    //                this.transform.position.y, 0);
    //    //        }
    //    //        else
    //    //        {
    //    //            this.transform.position = new Vector3(this.transform.position.x + xInterval,
    //    //                this.transform.position.y, 0);
    //    //        }
    //    //        flyingTime = 0;
    //    //        speedX = 0;
    //    //        changingPower = changingPower * 0.7f;
    //    //    }
    //    //}
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag.Equals("Tile_cannotHit") && isCollide.Equals(false))
    //    {
    //        isCollide = true;
    //        Vector3 colPos = collision.contacts[0].point;
    //        float yDiff = this.GetComponent<Rigidbody2D>().position.y - colPos.y;
    //        float xDiff = this.GetComponent<Rigidbody2D>().position.x - colPos.x;
    //        Debug.Log("xdiff :" + xDiff);
    //        Debug.Log("yDiff :" + yDiff);

    //        float yMin = this.GetComponent<CircleCollider2D>().bounds.min.y;
    //        float yInterval = Mathf.Abs(colPos.y - yMin);
    //        float xInterval;
    //        if (isRight)
    //        {
    //            float xMax = this.GetComponent<CircleCollider2D>().bounds.max.x;
    //            xInterval = Mathf.Abs(colPos.x - xMax);
    //        }
    //        else
    //        {
    //            float xMin = this.GetComponent<CircleCollider2D>().bounds.min.x;
    //            xInterval = Mathf.Abs(colPos.x - xMin);
    //        }

    //        if (Mathf.Abs(xDiff) <= Mathf.Abs(yDiff))
    //        {
    //            Debug.Log("상하충돌");
    //            // 위충돌
    //            if (speedY >= 0)
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - yInterval - Mathf.Abs(speedY), 0);
    //            }
    //            //아래충돌
    //            else
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + yInterval + Mathf.Abs(speedY), 0);
    //            }

    //            flyingTime = 0;
    //            speedX = 0;
    //            speedY = 0;
    //            changingPower = changingPower * 0.7f;
    //        }
    //        else if (yDiff.Equals(0))
    //        {
    //            Debug.Log("좌우충돌1");
    //            if (isRight)
    //            {
    //                isRight = false;
    //            }
    //            else
    //            {
    //                isRight = true;
    //            }
    //            if (isRight.Equals(false))
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x - xInterval,
    //                    this.transform.position.y, 0);
    //            }
    //            else
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x + xInterval,
    //                    this.transform.position.y, 0);
    //            }
    //            flyingTime = 0;
    //            speedX = 0;
    //            changingPower = changingPower * 0.7f;

    //        }
    //        else if(Mathf.Abs(xDiff) <= Mathf.Abs(yDiff))
    //        {
    //            if (isRight)
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x - xInterval,
    //                    this.transform.position.y, 0);
    //            }
    //            else
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x + xInterval,
    //                    this.transform.position.y, 0);
    //            }
    //            flyingTime = 0;
    //            speedX = 0;
    //            changingPower = changingPower * 0.7f;
    //        }
    //        isCollide = false;
    //    }

    //    if (collision.gameObject.tag.Equals("Tile_canHit") && isCollide.Equals(false))
    //    {
    //        isCollide = true;
    //        Vector3 colPos = collision.contacts[0].point;
    //        float yDiff = this.GetComponent<Rigidbody2D>().position.y - colPos.y;
    //        float xDiff = this.GetComponent<Rigidbody2D>().position.x - colPos.x;
    //        Debug.Log("xdiff :" + xDiff);
    //        Debug.Log("yDiff :" + yDiff);

    //        float yMin = this.GetComponent<CircleCollider2D>().bounds.min.y;
    //        float yInterval = Mathf.Abs(colPos.y - yMin);
    //        float xInterval;
    //        if (isRight)
    //        {
    //            float xMax = this.GetComponent<CircleCollider2D>().bounds.max.x;
    //            xInterval = Mathf.Abs(colPos.x - xMax);
    //        }
    //        else
    //        {
    //            float xMin = this.GetComponent<CircleCollider2D>().bounds.min.x;
    //            xInterval = Mathf.Abs(colPos.x - xMin);
    //        }

    //        if (Mathf.Abs(xDiff) <= Mathf.Abs(yDiff))
    //        {
    //            Debug.Log("상하충돌");
    //            // 위충돌
    //            if (speedY >= 0)
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - yInterval - Mathf.Abs(speedY), 0);
    //            }
    //            //아래충돌
    //            else
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + yInterval + Mathf.Abs(speedY), 0);
    //            }

    //            flyingTime = 0;
    //            speedX = 0;
    //            speedY = 0;
    //            changingPower = changingPower * 0.7f;
    //        }
    //        else if(yDiff.Equals(0))
    //        {
    //            Debug.Log("좌우충돌1");
    //            if (isRight)
    //            {
    //                isRight = false;
    //            }
    //            else
    //            {
    //                isRight = true;
    //            }
    //            if (isRight.Equals(false))
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x - xInterval,
    //                    this.transform.position.y, 0);
    //            }
    //            else
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x + xInterval,
    //                    this.transform.position.y, 0);
    //            }
    //            flyingTime = 0;
    //            speedX = 0;
    //            changingPower = changingPower * 0.7f;

    //        }     
    //        else if (Mathf.Abs(xDiff) <= Mathf.Abs(yDiff))
    //        {
    //            if (isRight)
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x - xInterval,
    //                    this.transform.position.y, 0);
    //            }
    //            else
    //            {
    //                this.transform.position = new Vector3(this.transform.position.x + xInterval,
    //                    this.transform.position.y, 0);
    //            }
    //            flyingTime = 0;
    //            speedX = 0;
    //            changingPower = changingPower * 0.7f;
    //        }
    //    }
    //    isCollide = false;
    //}
}
