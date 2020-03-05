using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BULLET_TYPE
{
    NORMAL,
    SPLIT,
    GRENADE
}

public class cBulletManager : MonoBehaviour
{
    public GameObject[] bullets;
    private int bulletAmount; // 발사할 숫자
    private float bulletTerm; // 발사체 발사 간격
    Vector3 tempDir;

    private void Start()
    {
        bullets = new GameObject[this.gameObject.transform.childCount];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = this.gameObject.transform.GetChild(i).gameObject;
        }
    }

    //public void SetBullet(int pBulletAmount, float pBulletTerm, Vector3 pOriginPos, bool pIsGravity, Vector3 pTarget)
    //{
    //    StartCoroutine(SetBulletCor(pBulletAmount, pBulletAmount, pOriginPos, pIsGravity, pTarget));
    //}

    //IEnumerator SetBulletCor(int pBulletAmount, float pBulletTerm, Vector3 pOriginPos, bool pIsGravity, Vector3 pTarget)
    //{
    //    bulletAmount = pBulletAmount;
    //    bulletTerm = pBulletTerm;

    //    int bulletCount = 0;

    //    while (true)
    //    {
    //        if (bulletCount >= bulletAmount)
    //        {
    //            break;
    //        }
    //        Debug.Log("BulletTerm : " + bulletTerm);
    //        Debug.Log("BulletCount : " + bulletCount);
    //        LaunchBullet(pOriginPos, pIsGravity, pTarget);
    //        yield return new WaitForSeconds(bulletTerm);
    //        bulletCount++;
    //    }
    //}

    public void LaunchBullet(Vector3 pOriginPos, bool pIsGravity, Vector3 pTarget, long pDamage, float pPower, float pGravity = 7.0f)
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i].activeSelf.Equals(false))
            {
                bullets[i].transform.position = pOriginPos;
                bullets[i].SetActive(true);

                if (pIsGravity.Equals(true))
                {
                    tempDir = new Vector3(pTarget.x, pTarget.y + 100, pTarget.z) - pOriginPos;
                }
                else
                {
                    tempDir = new Vector3(pTarget.x, pTarget.y, pTarget.z) - pOriginPos;
                }

                tempDir = tempDir.normalized;

                bullets[i].GetComponent<cBullet>().Init(BULLET_TYPE.NORMAL, pPower, pIsGravity, tempDir, pDamage, pGravity);
                break;
            }
        }
    }
}