using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMonster_stage5_fungus : cEnemy_Flying
{
    Transform _playerT;
    Light monsterLight;

    private void Start()
    {
        Init(cEnemyTable.SetMonsterInfo(11));
        curMoveSpeed = curMoveSpeed * 0.5f;
        maxMoveSpeed = maxMoveSpeed * 0.5f;
        changingGravity = 0;
        defaultGravity = 0;
        flyingRangeY = 0.5f;
        _playerT = cUtil._player.transform;
        monsterLight = GameObject.Find("HeadLight").GetComponent<Light>();
    }

    protected override void Move()
    {
        if (isInNoticeRange)
        {
            dir = (_playerT.position - this.transform.position).normalized;
            this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
        }

        // idle상태 : 사인파 형태로 부유
        else if (!isInNoticeRange)
        {
            if (isRightBlocked)
            {
                isRightBlocked = false;
                dir = Vector3.left;
            }
            else if (isLeftBlocked)
            {
                isLeftBlocked = false;
                dir = Vector3.right;
            }

            floatTime += Time.deltaTime;
            dir = new Vector3(dir.x, Mathf.Sin(floatTime) * flyingRangeY, dir.z);

            if (isUpBlocked)
            {
                dir.y = dir.y * -1;
            }
            else if (GetIsGrounded())
            {
                dir.y = dir.y * -1;
            }

            this.transform.Translate(dir * curMoveSpeed * Time.deltaTime);
        }
        // 오버플로우 방지
        if (floatTime >= float.MaxValue - 100.0f)
        {
            floatTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            isDead = true;
            StartCoroutine(BlindPlayer());
        }
    }

    IEnumerator BlindPlayer()
    {
        float orgLight = monsterLight.intensity;
        monsterLight.intensity = 0;

        yield return new WaitForSeconds(5f);

        monsterLight.intensity = orgLight;
    }
}
