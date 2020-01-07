using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct cTileMap
//{
//    public int[] PlayerPos;
//    public int[,] Map;

//    public cTileMap(int[] pPos,int[,] MapInfo)
//    {
//        PlayerPos = pPos;
//        Map = MapInfo;
//    }
//}
public class cMap : MonoBehaviour
{
    public GameObject p;
    public GameObject originObj;

    public int[] Playerpos = { 3, 3 };
    public int[,] Map = { {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                     {-1,0,0,0,0,0,0,0,0,-1},
                                     {-1,0,0,0,0,0,0,0,0,-1},
                                     {-1,0,0,3,0,0,0,0,0,-1},
                                     {-1,1,1,1,1,1,1,1,1,-1},
                                     {-1,1,1,1,1,1,1,1,1,-1},
                                     {-1,1,1,1,1,1,1,1,1,-1},
                                     {-1,1,1,1,1,1,1,1,1,-1},
                                     {-1,1,1,1,1,1,1,1,1,-1},
                                     {-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},};


    public bool isGrounded;
    public bool isLeftBlocked;
    public bool isRightBlocked;
    public bool isUpBlocked;
    public float pheight;
    public float pwidth;
    public float mapHeight;
    //public cMap()
    //{
    //    cTileMap Test = new cTileMap(Playerpos, Map);
    //}
    // Start is called before the first frame update
    void Start()
    {

    }
    public virtual void ManageCollision()
    {
        Vector2 pPos = originObj.transform.position;

        //맵을 타일 하나의 크기인 180으로 가로와 세로를 나누어 int [,] Map의 행렬에 그 칸의 정보가 들어있게 만듬
        //맵 상의 어떤 위치의 x과 y값을 180으로 나눈 값을 X,Y라 하면 그 위치의 칸은 Map[Y,X]이다. 
        //Map[Y,X]에서 Y가 행, X가 열이다
        //주의!)Map[Y,X]에서 Y가 증가하면 실제 좌표값은 감소하고 Y가 감소하면 좌표값은 증가한다
        //플레이어 위치를 TestPlayerPos라 하고, y값에 해당하는 행은 [0]에 x값에 해당하는 열은 [1]에
        Playerpos = new int[] { (int)(mapHeight - pPos.y) / 180, (int)pPos.x / 180 };

        //현재 칸 빈공간
        if (Map[Playerpos[0],Playerpos[1]].Equals(0))
        {
            //아래 막힘&땅 닿는 범위에 있음
            if ((pPos.y <= (mapHeight - Playerpos[0] * 180.0f) - pheight) && (!Map[Playerpos[0] + 1, Playerpos[1]].Equals(0)))
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, mapHeight - Playerpos[0] * 180 + pheight);
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
            //왼쪽 막힘&땅 닿는 범위에 있음
            if ((pPos.x < Playerpos[1] * 180.0f - pwidth) && (!Map[Playerpos[0], Playerpos[1] - 1].Equals(0)))
            {
                originObj.transform.position = new Vector2((Playerpos[1] - 1) * 180 + pwidth, originObj.transform.position.y);
                isLeftBlocked = true;
            }
            else
            {
                isLeftBlocked = false;
            }
            //오른쪽 막힘&땅 닿는 범위에 있음
            if ((pPos.x > Playerpos[1] * 180.0f + pwidth) && (!Map[Playerpos[0], Playerpos[1] + 1].Equals(0)))
            {
                originObj.transform.position = new Vector2((Playerpos[1] - 1) * 180 + pwidth, originObj.transform.position.y);
                isRightBlocked = true;
            }
            else
            {
                isRightBlocked = false;
            }
            //위쪽 막힘&땅 닿는 범위에 있음
            if ((pPos.y > (mapHeight - Playerpos[0] * 180.0f) + pheight) && (!Map[Playerpos[0] - 1, Playerpos[1]].Equals(0)))
            {
                originObj.transform.position = new Vector2(originObj.transform.position.x, mapHeight - Playerpos[0] * 180 + pheight);
                isUpBlocked = true;
            }
            else
            {
                isUpBlocked = false;
            }
        }
        //현재 칸 채워져있음
        else
        {
            if (pPos.x - Playerpos[1] * 180.0f >= 0)
            {

            }
            else
            {

            }
            if (pPos.y - (mapHeight - Playerpos[0] * 180.0f) >= 0)
            {

            }
            else
            {

            }
        }

    }

}
