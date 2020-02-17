using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Bomb_O : cProjectile
{
    public GameObject explodeCollider;
    private Vector3 originTPos;
    public cProperty damage;
    private bool isAttatched;
    private float timer;
    private bool isTimeDone;
    private bool isExplodeDone;
    private Vector3Int[] explodeRange;
    public GameObject effect;

    private void Start()
    {
        rt = originObj.GetComponent<BoxCollider2D>();
        damage = new cProperty("damage", 100);
        timer = 0;
        isAttatched = false;
        isTimeDone = false;
        isExplodeDone = false;
        originTPos = Vector3.zero;
        flyingTime = 0;
        defaultPower = 12f;
        changingPower = defaultPower;
        dir = new Vector3(0.5f, 0.5f);
        isReflectOn = true;
        isGravityOn = true;
        gravityAmount = 9.8f;
        SetDir(dir);
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isAttatched)
        {
            SetDir(Vector3.zero);
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
            effect.transform.position = this.transform.position;
            for(byte i = 0; i < effect.transform.childCount; i++)
                effect.transform.GetChild(i).GetComponent<ParticleSystem>().Play();
        }
        // 플레이어와 적 피격처리를 위해 active상태가 1프레임 더 지속되어야 하기 때문에 이 루프를 시행한다.
        else if (isExplodeDone)
        {
            isExplodeDone = false;
            Destroy(this.gameObject);
        }

        if (timer >= 3.0f)
        {
            isTimeDone = true;
            timer = 0;
        }
    }
}
