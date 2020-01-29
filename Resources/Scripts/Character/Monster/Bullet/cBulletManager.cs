using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BULLET_TYPE
{
    NORMAL,
    TRIPLE,
    SPLIT,
    GRENADE
}

public class cBulletManager : MonoBehaviour
{
    public GameObject originMonster;
    public GameObject[] bullets;
    private Transform _player;

    private float bulletTerm;

    private void Start()
    {
        originMonster = this.transform.parent.gameObject;
        bullets = new GameObject[this.gameObject.transform.childCount];
        for(int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = this.gameObject.transform.GetChild(i).gameObject;
        }
        _player = cUtil._player.gameObject.transform;
    }
    
    public void SetBullet(int pIdx)
    {
        switch (pIdx)
        {
            // 한발 발사
            case 0:
                bulletTerm = 0;
                StartCoroutine(Launch(1, BULLET_TYPE.NORMAL, false));
                break;
            // 3연발 & 중력 on
            case 1:
                bulletTerm = 0.3f;
                StartCoroutine(Launch(3, BULLET_TYPE.NORMAL, true));
                break;
            // 분열형 & 중력 off
            case 2:
                bulletTerm = 0f;
                StartCoroutine(Launch(1, BULLET_TYPE.SPLIT, false));
                break;
            // 수류탄형 & 중력 on
            case 3:
                bulletTerm = 0.3f;
                StartCoroutine(Launch(5, BULLET_TYPE.GRENADE, true));
                break;
        }
    }

    // 발사
    IEnumerator Launch(int pBulletAmount, BULLET_TYPE pType, bool pIsGravity)
    {
        int idx = 0;

        // 총알 pool에서 false상태인 총알 오브젝트를 검색해 활성화한다
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i].activeSelf.Equals(false))
            {
                idx = i;
                break;
            }
        }
        int amount = pBulletAmount + idx; // 반복 횟수를 위한 설정
        while (idx < amount)
        {
            cBullet script = bullets[idx].GetComponent<cBullet>();
            bullets[idx].SetActive(true);
            script.SetType(pType);
            script.isGravityOn = pIsGravity;

            switch (pType)
            {
                // 일반형
                case BULLET_TYPE.NORMAL:
                    Vector3 tempDir = _player.position - originMonster.transform.position;
                    tempDir = tempDir.normalized;
                    script.dir = tempDir;
                    break;
                // 수류탄형
                case BULLET_TYPE.GRENADE:
                    float x = Random.Range(-0.6f, 0.5f);
                    float y = Random.Range(0.4f, 1.0f);
                    script.dir = new Vector3(x, y, 0);
                    break;
                case BULLET_TYPE.SPLIT:
                    break;
            }

            yield return new WaitForSeconds(bulletTerm);
            idx++;
        }
    }
}
