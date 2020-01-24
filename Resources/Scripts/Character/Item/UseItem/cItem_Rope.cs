using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cItem_Rope : cItem_use
{
    public cItem_Rope(string pName, int pPrice, int pAmount, int pKind, int pKindNum)
        : base(pName, pPrice, pAmount, pKind, pKindNum)
    {

    }

    // 로프 설치
    public override void UseItem()
    {
        base.UseItem();

        //Transform tempT = cUtil._user.GetPlayer().transform;

        //GameObject ropePrefab = Instantiate(Resources.Load<GameObject>(cPath.PrefabPath() + "Item_Rope"));
        //ropePrefab.transform.SetParent(GameObject.Find("Canvas").transform);

        //BoxCollider2D coli = ropePrefab.transform.GetComponent<BoxCollider2D>();

        //bool isInRange = false;
        //float distance = 0;

        //// 플레이어 위 4칸 범위에 벽이 있는지 검사
        //RaycastHit2D[] hit = Physics2D.RaycastAll(new Vector2(tempT.position.x, tempT.position.y),
        //    Vector2.up, 720.0f);

        //for (int i = 0; i < hit.Length; i++)
        //{
        //    if (hit[i].collider.tag.Equals("Tilemap_rock"))
        //    {
        //        isInRange = true;
        //        distance = hit[i].distance;
        //        break;
        //    }
        //}

        //// 플레이어 위 4칸 범위에 벽이 있을 경우
        //if (isInRange)
        //{
        //    GameObject Rope = Instantiate(ropePrefab, new Vector2(tempT.position.x,
        //        tempT.position.y + 720.0f - (coli.bounds.size.y / 2)),
        //        Quaternion.identity, tempT.parent.transform.parent);

        //    // 로프 크기와 생성 위치 재조정
        //    Rope.transform.localScale = new Vector2(Rope.transform.localScale.x, Rope.transform.localScale.y * (distance / 720.0f));
        //    Rope.transform.position = new Vector2(tempT.position.x,
        //        tempT.position.y + distance - (coli.bounds.size.y / 2));
        //}
        //// 플레이어 위가 뚫려있을 경우
        //else
        //{
        //    GameObject Rope = Instantiate(ropePrefab, new Vector2(tempT.position.x,
        //        tempT.position.y + 720.0f - (coli.bounds.size.y / 2)),
        //        Quaternion.identity, tempT.parent.transform.parent);
        //}
    }
}
