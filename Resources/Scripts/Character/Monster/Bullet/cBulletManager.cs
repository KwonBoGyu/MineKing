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
    public GameObject originMonster;
    public GameObject[] bullets;
    private float bulletTerm; // 발사체 발사 간격

    private void Start()
    {
        originMonster = this.transform.parent.gameObject;
        bullets = new GameObject[this.gameObject.transform.childCount];
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = this.gameObject.transform.GetChild(i).gameObject;
        }
    }

    // targetType : 0 -> 플레이어 방향 발사
    public void SetBullet(int pBulletAmount, BULLET_TYPE pBulletType, bool pIsGravity, Vector3 pTarget, float pBulletTerm = 0)
    {
        bulletTerm = pBulletTerm;
        StartCoroutine(Launch(pBulletAmount, pBulletType, pIsGravity, pTarget));
    }

    IEnumerator Launch(int pBulletAmount, BULLET_TYPE pType, bool pIsGravity, Vector3 pTarget)
    {
        int idx = 0;

        // 총알 pool에서 false상태인 총알 오브젝트를 인덱스 오름차순으로 활성화
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i].activeSelf.Equals(false))
            {
                idx = i;
                break;
            }
        }
        int amount = pBulletAmount + idx; // 반복 횟수를 위한 설정

        // 활성화할 총알 개수만큼 반복
        while (idx < amount)
        {
            cBullet script = bullets[idx].GetComponent<cBullet>();
            bullets[idx].SetActive(true);
            script.SetType(pType); // 투사체 타입 설정
            script.isGravityOn = pIsGravity; // 중력 적용 여부
            Vector3 tempDir = pTarget - originMonster.transform.position;
            tempDir = tempDir.normalized;
            script.dir = tempDir;

            yield return new WaitForSeconds(bulletTerm); // 투사체간 발사 간격만큼 대기
            idx++;
        }
    }
}
