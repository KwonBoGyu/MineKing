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
    private cPlayer _player;

    private float bulletTerm;

    private void Start()
    {
        originMonster = this.transform.parent.gameObject;
        _player = cUtil._player;
    }
    
    public void SetBullet(BULLET_TYPE pType)
    {
        switch (pType)
        {
            // 한발 발사
            case BULLET_TYPE.NORMAL:
                bulletTerm = 0;
                StartCoroutine(Launch(1, pType, false, _player.transform.position));
                break;

            // 3연발 & 중력 on
            case BULLET_TYPE.TRIPLE:
                bulletTerm = 0.3f;
                StartCoroutine(Launch(3, pType, true, _player.transform.position));
                break;
            // 분열형 & 중력 off
            case BULLET_TYPE.SPLIT:
                bulletTerm = 0f;
                StartCoroutine(Launch(1, pType, false, _player.transform.position));
                break;
            // 수류탄형 & 중력 on
            case BULLET_TYPE.GRENADE:
                bulletTerm = 0.3f;
                StartCoroutine(Launch(5, pType, true));
                break;
        }
    }

    // 방향 미지정형 발사
    IEnumerator Launch(int pBulletAmount, BULLET_TYPE pType, bool pIsGravity)
    {
        int i = 0;
        while (i < pBulletAmount)
        {
            cBullet script = bullets[i].GetComponent<cBullet>();
            bullets[i].SetActive(true);
            script.SetType(pType);
            script.isGravityOn = pIsGravity;

            float x = Random.Range(-0.5f, 0.5f);
            float y = Random.Range(0.5f, 1.0f);
            script.dir = new Vector3(x, y, 0);

            yield return new WaitForSeconds(bulletTerm);
            i++;
        }
    }

    // 방향 지정형 발사
    IEnumerator Launch(int pBulletAmount, BULLET_TYPE pType, bool pIsGravity, Vector3 pDir)
    {
        int i = 0;
        while (i < pBulletAmount)
        {
            cBullet script = bullets[i].GetComponent<cBullet>();
            bullets[i].SetActive(true);
            script.SetType(pType);
            script.isGravityOn = pIsGravity;
            
            Vector3 tempDir = cUtil._player.transform.position - originMonster.transform.position;
            script.dir = tempDir;

            yield return new WaitForSeconds(bulletTerm);
            i++;
        }
    }
}
