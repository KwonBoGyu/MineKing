using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_use : cItem
{
    // 디버그 과정에서 amount 줄이지 않음
    public int amount;
    public float speedUpTime;
    public float speedUpAmount;
    public float bombTime;

    public float GetSpeedUpTime() { return speedUpTime; }
    public float GetSpeedUpAmount() { return speedUpAmount; }

    #region 생성자
    public cItem_use(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
        :base(pName, pPrice, pKind, pKindNum)
    {
        amount = pAmount;
        speedUpTime = 10.0f;
        speedUpAmount = 1.5f;
        bombTime = 3.0f;
    }

    public cItem_use(cItem_use pIu)
        : base(pIu._name, pIu.price, pIu.kind, pIu.kindNum)
    {
        this.amount = pIu.amount;
        speedUpTime = 10.0f;
        speedUpAmount = 1.5f;
        bombTime = 3.0f;
    }
    #endregion

    // 로프 설치
    public void SetRope(GameObject pPlayer)
    {      
        GameObject ropePrefab = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Item_Rope"));
        ropePrefab.transform.SetParent(GameObject.Find("Canvas").transform);

        BoxCollider2D coli = ropePrefab.transform.GetComponent<BoxCollider2D>();
        
        bool isInRange = false;
        float distance = 0;

        // 플레이어 위 4칸 범위에 벽이 있는지 검사
        RaycastHit2D[] hit = Physics2D.RaycastAll(new Vector2(pPlayer.transform.position.x, pPlayer.transform.position.y),
            Vector2.up, 720.0f);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider.tag.Equals("Tilemap_rock"))
            {
                isInRange = true;
                distance = hit[i].distance;
                break;
            }
        }

        // 플레이어 위 4칸 범위에 벽이 있을 경우
        if (isInRange)
        {
            GameObject Rope = Instantiate(ropePrefab, new Vector2(pPlayer.transform.position.x,
                pPlayer.transform.position.y + 720.0f - (coli.bounds.size.y / 2)),
                Quaternion.identity, pPlayer.transform.parent.transform.parent);

            // 로프 크기와 생성 위치 재조정
            Rope.transform.localScale = new Vector2(Rope.transform.localScale.x, Rope.transform.localScale.y * (distance / 720.0f));
            Rope.transform.position = new Vector2(pPlayer.transform.position.x,
                pPlayer.transform.position.y + distance - (coli.bounds.size.y / 2));
        }
        // 플레이어 위가 뚫려있을 경우
        else
        {
            GameObject Rope = Instantiate(ropePrefab, new Vector2(pPlayer.transform.position.x,
                pPlayer.transform.position.y + 720.0f - (coli.bounds.size.y / 2)),
                Quaternion.identity, pPlayer.transform.parent.transform.parent);
        }
    }

    // 모래주머니 설치
    public void SetSandBag(GameObject pPlayer, Vector3 pDir)
    {
        // 밟고 있는 타일 검사
        RaycastHit2D rayDown = Physics2D.Raycast(new Vector2(pPlayer.transform.position.x,
            pPlayer.transform.position.y) + new Vector2(0, -100.0f) , Vector2.down, 100.0f);

        // 바라보는 방향이 막혀있는지 검사
        RaycastHit2D[] rayFront = Physics2D.RaycastAll(new Vector2(pPlayer.transform.position.x,
            pPlayer.transform.position.y), pDir, 230.0f);

        bool isFrontBlocked = false;
        for (int i = 0; i < rayFront.Length; i++)
        {
            if (rayFront[i].transform.tag.Equals("Tilemap_rock"))
                isFrontBlocked = true;
        }
        // 막혔으면 실행 중지
        if (isFrontBlocked)
            return;

        GameObject sandBagPreFab = GameObject.Find("Item_SandBag");

        // 모래주머니를 생성할 위치 변수
        Vector2 position = Vector2.zero;

        // 왼쪽을 바라보는 경우
        if (pDir == Vector3.left)
        {
            // 플레이어가 왼쪽을 바라보고 있고, 밟고 있는 타일의 반 이상을 넘어간 경우
            if (rayDown.collider.transform.position.x >= pPlayer.transform.position.x)
                position = new Vector2(rayDown.collider.transform.position.x, rayDown.collider.transform.position.y) + new Vector2(-360.0f, 180.0f);
            // 밟고 있는 타일의 반 이상을 넘기지 않은 경우
            else
                position = new Vector2(rayDown.collider.transform.position.x, rayDown.collider.transform.position.y) + new Vector2(-180.0f, 180.0f);
        }
        // 오른쪽을 바라보는 경우
        else if (pDir == Vector3.right)
        {
            // 플레이어가 오른쪽을 바라보고 있고, 밟고 있는 타일의 반 이상을 넘어간 경우
            if (rayDown.collider.transform.position.x <= pPlayer.transform.position.x)
                position = new Vector2(rayDown.collider.transform.position.x, rayDown.collider.transform.position.y) + new Vector2(360.0f, 180.0f);
            // 밟고 있는 타일의 반 이상을 넘기지 않은 경우
            else
                position = new Vector2(rayDown.collider.transform.position.x, rayDown.collider.transform.position.y) + new Vector2(180.0f, 180.0f);
        }
        // 위를 바라보는 경우
        else if (pDir == Vector3.up)
        {
            position = new Vector2(rayDown.collider.transform.position.x, rayDown.collider.transform.position.y) + new Vector2(0, 360.0f);
        }

        GameObject rope = Instantiate(sandBagPreFab, position, Quaternion.identity);
    }
    
    // 폭탄 설치
    public void SetBomb(GameObject pPlayer)
    {
        GameObject bomb = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Item_Bomb"), pPlayer.transform.position, 
            Quaternion.identity, pPlayer.transform.parent.transform.parent);
    }

    // n초동안 일정 %만큼의 이동속도 증가
    // 임시 : 5초, 50%
    public void UseSpeedPotion(ref bool pIsSpeedUp)
    {
        pIsSpeedUp = true;
    }

    // 최대 체력의 일정 %만큼 현재 체력 회복
    // 임시 : 10%
    public float UseHpPotion(float pMaxHp)
    {
        return pMaxHp * 0.1f;
    }
}
