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

    private void Start()
    {
        bullets = new GameObject[this.gameObject.transform.childCount];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = this.gameObject.transform.GetChild(i).gameObject;
        }
    }

    public void SetBullet(int pBulletAmount, float pBulletTerm, Vector3 pOriginPos ,bool pIsGravity, Vector3 pTarget)
    {
        bulletAmount = pBulletAmount;
        bulletTerm = pBulletTerm;

        int bulletCount = 0;
        float timer = bulletTerm;

        while (bulletCount <= bulletAmount)
        {
            if (timer >= bulletTerm)
            {
                LaunchBullet(pOriginPos, pIsGravity, pTarget);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }
    
    public void LaunchBullet(Vector3 pOriginPos, bool pIsGravity, Vector3 pTarget)
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i].activeSelf.Equals(false))
            {
                cBullet script = bullets[i].GetComponent<cBullet>();
                script.transform.position = pOriginPos;
                bullets[i].SetActive(true);
                Vector3 tempDir;
                if (pIsGravity.Equals(true))
                {
                    tempDir = new Vector3(pTarget.x, pTarget.y + 100, pTarget.z) - pOriginPos;
                }
                else
                {
                    tempDir = new Vector3(pTarget.x, pTarget.y, pTarget.z) - pOriginPos;
                }
                script.Init(BULLET_TYPE.NORMAL, 10.0f, pIsGravity, tempDir);
                break;
            }
        }
    }
   
    //IEnumerator Launch(int pBulletAmount, BULLET_TYPE pType, bool pIsGravity, Vector3 pTarget)
    //{
    //    int idx = 0;

    //    // 총알 pool에서 false상태인 총알 오브젝트를 인덱스 오름차순으로 활성화
    //    for (int i = 0; i < bullets.Length; i++)
    //    {
    //        if (bullets[i].activeSelf.Equals(false))
    //        {
    //            idx = i;
    //            break;
    //        }
    //    }
    //    int amount = pBulletAmount + idx; // 반복 횟수를 위한 설정

    //    // 활성화할 총알 개수만큼 반복
    //    while (idx < amount)
    //    {
    //        cBullet script = bullets[idx].GetComponent<cBullet>();
    //        script.transform.position = originMonster.transform.position;
    //        bullets[idx].SetActive(true);
    //        Vector3 tempDir = new Vector3(pTarget.x, pTarget.y + 100, pTarget.z) - originMonster.transform.position;
    //        Debug.Log(tempDir);
    //        script.Init(pType, 10.0f, true, tempDir);

    //        yield return new WaitForSeconds(bulletTerm); // 투사체간 발사 간격만큼 대기
    //        idx++;
    //    }
    //}
}
