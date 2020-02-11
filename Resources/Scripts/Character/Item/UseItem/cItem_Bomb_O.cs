using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Bomb_O : MonoBehaviour
{
    public GameObject explodeCollider;
    private Vector3 originTPos;
    public cProperty damage;
    private bool isAttatched;
    private Vector3 dir;
    public void SetDir(Vector3 pDir) { dir = pDir; }
    private float speed;
    private float changingGravity;
    private float defaultGravity;
    private float timer;
    private bool isTimeDone;
    private bool isExplodeDone;
    private Vector3Int[] explodeRange;

    private void Start()
    {
        damage = new cProperty("damage", 100);
        timer = 0;
        speed = 500f;
        //dir = Vector3.zero;
        changingGravity = 100f;
        defaultGravity = 300f;
        isAttatched = false;
        isTimeDone = false;
        isExplodeDone = false;
        originTPos = Vector3.zero;
    }

    private void OnEnable()
    {
        this.transform.position = cUtil._player.originObj.transform.position;
        explodeCollider.SetActive(false);
        timer = 0;
        isAttatched = false;
        isTimeDone = false;
    }

    private void SetExplodeRange()
    {
        originTPos = this.gameObject.transform.position;
        // 3 x 3 범위 타일
        explodeRange =
            new Vector3Int[]{
                new Vector3Int((int)originTPos.x, (int)originTPos.y + 150, 0),
                new Vector3Int((int)originTPos.x + 60, (int)originTPos.y + 150, 0),
                new Vector3Int((int)originTPos.x + 60, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x + 60, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 60, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 60, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x - 60, (int)originTPos.y + 150, 0)
            };
    }

    private void FixedUpdate()
    {
        if (!isAttatched)
        {
            this.transform.Translate(dir * speed * Time.deltaTime);
            Debug.Log("dir : " + dir);
            Debug.Log("speed : " + speed);

            // 중력 적용
            this.transform.Translate(Vector3.down * changingGravity * Time.deltaTime);
            if (changingGravity <= defaultGravity)
                changingGravity *= 1.2f;
            else if (changingGravity > defaultGravity)
                changingGravity = defaultGravity;
        }

        if (isTimeDone.Equals(false) && isExplodeDone.Equals(false))
        {
            timer += Time.deltaTime;
        }
        else if(isTimeDone)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Tile_cannotHit"))
        {
            isAttatched = true;
        }
        else if (collision.gameObject.tag.Equals("Tile_canHit"))
        {
            isAttatched = true;
        }
    }
}
