using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_SandBag : cItem_use
{
    #region 생성자
    public cItem_SandBag(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
        :base(pName, pPrice, pAmount, pKind, pKindNum)
    {
    }
    #endregion
    // 모래주머니 설치
    public override void UseItem()
    {
        base.UseItem();

        Transform tempT = cUtil._user.GetPlayer().transform;
        Vector3 pDir = cUtil._user.GetPlayer().GetDirection();

        // 밟고 있는 타일 검사
        RaycastHit2D rayDown = Physics2D.Raycast(new Vector2(tempT.position.x,
            tempT.position.y) + new Vector2(0, -100.0f), Vector2.down, 100.0f);

        // 바라보는 방향이 막혀있는지 검사
        RaycastHit2D[] rayFront = Physics2D.RaycastAll(new Vector2(tempT.position.x,
            tempT.position.y), pDir, 230.0f);

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
            if (rayDown.collider.transform.position.x >= tempT.position.x)
                position = new Vector2(rayDown.collider.transform.position.x, rayDown.collider.transform.position.y) + new Vector2(-360.0f, 180.0f);
            // 밟고 있는 타일의 반 이상을 넘기지 않은 경우
            else
                position = new Vector2(rayDown.collider.transform.position.x, rayDown.collider.transform.position.y) + new Vector2(-180.0f, 180.0f);
        }
        // 오른쪽을 바라보는 경우
        else if (pDir == Vector3.right)
        {
            // 플레이어가 오른쪽을 바라보고 있고, 밟고 있는 타일의 반 이상을 넘어간 경우
            if (rayDown.collider.transform.position.x <= tempT.position.x)
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

        //GameObject rope = Instantiate(sandBagPreFab, position, Quaternion.identity);
    }
}
