using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public enum TILEDIRECTION
{
    NONE,
    UP_PLUS,
    UPRIGHT,
    RIGHT_PLUS,
    RIGHTDOWN,
    DOWN_PLUS,
    DOWNLEFT,
    LEFT_PLUS,
    LEFTUP,
    UP,
    RIGHT,
    DOWN,
    LEFT,
    HORIZONTAL,
    VERTICAL,
    SOLO,
    BLOCKED
}

public class cTileMng : MonoBehaviour
{
    public struct Tile
    {
        public Vector3Int location { get; set; }
        public TileBase tileBase;
        public int level { get; set; }
        public float hp { get; set; }
        public TILEDIRECTION dir { get; set; }

        public Tile(Vector3Int pLocation, TileBase pTileBase, int pLevel, float pHp, TILEDIRECTION pDir)
        {
            location = pLocation;
            tileBase = pTileBase;
            level = pLevel;
            hp = pHp;
            dir = pDir;
        }
    }

    public int tileSize;
    public int numX;
    public int numY;
    public Dictionary<Vector3, Tile> dic_canHit;
    private Tilemap tileMap_cannotHit;
    private Tilemap tileMap_canHit;
    Vector3Int[] cellPos;

    Tile? tempTile;
    private TileBase[] canHitTile_100;
    private TileBase[] canHitTile_60;
    private TileBase[] canHitTile_30;


    private void Start()
    {
        tileMap_canHit = this.transform.GetChild(0).GetComponent<Tilemap>();
        tileMap_cannotHit = this.transform.GetChild(1).GetComponent<Tilemap>();
        tempTile = null;

        SetEntireTiles();
    }

    public bool CheckAttackedTile(Vector3 pWorldPos, float pDamage)
    {
        bool isChecked = false;
        Vector3Int worldToCellPos = tileMap_canHit.WorldToCell(pWorldPos);
        Vector3 convertedWorldPos = tileMap_canHit.CellToWorld(worldToCellPos);
        tempTile = dic_canHit[convertedWorldPos];

        //해당 위치에 타일이 있다면
        if (tempTile != null)
        {
            UpdateAttackedTile(convertedWorldPos);
            isChecked = true;
        }
        return isChecked;
    }

    public void CheckCanGroundTile(cCharacter pObj)
    {
        pObj.notUpBlocked = true;
        pObj.notGrounded = true;
        pObj.notRightBlocked = true;
        pObj.notLeftBlocked = true;
        
        CheckTiles(tileMap_canHit, pObj);
        CheckTiles(tileMap_cannotHit, pObj);
        
        if (pObj.notUpBlocked)
            pObj.isUpBlocked = false;
        if (pObj.notGrounded)
            pObj.isGrounded = false;
        if (pObj.notLeftBlocked)
            pObj.isLeftBlocked = false;
        if (pObj.notRightBlocked)
            pObj.isRightBlocked = false;
    }
    
    private void CheckTiles(Tilemap pTileMap, cCharacter pObj)
    {
        Vector3 originTPos = pObj.originObj.transform.position;
        float originRtXLenHalf = pObj.rt.size.x * 0.5f;
        float originRtYLenHalf = pObj.rt.size.y * 0.5f;

        cellPos = new Vector3Int[]
            {
                new Vector3Int((int)originTPos.x, (int)originTPos.y + 150, 0),
                new Vector3Int((int)originTPos.x + 24, (int)originTPos.y + 150, 0),
                new Vector3Int((int)originTPos.x + 60, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x + 24, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 24, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 60, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x - 24, (int)originTPos.y + 150, 0),
            };

        for (int i = 0; i < cellPos.Length; i++)
        {
            Vector3Int worldToCellPos = pTileMap.WorldToCell(cellPos[i]);
            TileBase t_tile = pTileMap.GetTile(worldToCellPos);

            if (t_tile != null)
            {
                switch (i)
                {
                    //위쪽 충돌
                    case 0:
                        //충돌하였다면..
                        if (originTPos.y + originRtYLenHalf> pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            pObj.notUpBlocked = false;
                            pObj.isUpBlocked = true;
                            pObj.originObj.transform.position = new Vector3(
                                originTPos.x,
                                pTileMap.CellToWorld(worldToCellPos).y - originRtYLenHalf,
                                originTPos.z
                                );
                        }
                        break;
                    //오른쪽 위 충돌
                    case 1:
                        //충돌하였다면..
                        if (originTPos.x + originRtXLenHalf > pTileMap.CellToWorld(worldToCellPos).x - 1 &&
                           originTPos.y + originRtYLenHalf > pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            float distX = Mathf.Abs((originTPos.x + originRtXLenHalf) -
                                pTileMap.CellToWorld(worldToCellPos).x);
                            float distY = Mathf.Abs((originTPos.y + originRtYLenHalf) -
                                pTileMap.CellToWorld(worldToCellPos).y);

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                pObj.notUpBlocked = false;
                                pObj.isUpBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originTPos.x,
                                    pTileMap.CellToWorld(worldToCellPos).y - originRtYLenHalf,
                                    originTPos.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                pObj.notRightBlocked = false;
                                pObj.isRightBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                pTileMap.CellToWorld(worldToCellPos).x - originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                            }
                        }
                        break;
                    //오른쪽 충돌
                    case 2:                        
                        //충돌하였다면..
                        if (originTPos.x + originRtXLenHalf > pTileMap.CellToWorld(worldToCellPos).x-1)
                        {
                            pObj.notRightBlocked = false;
                            pObj.isRightBlocked = true;
                            pObj.originObj.transform.position = new Vector3(
                                pTileMap.CellToWorld(worldToCellPos).x - originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                        }
                        break;
                    //오른쪽 아래 충돌
                    case 3:
                        //충돌하였다면..
                        if (originTPos.x + originRtXLenHalf > pTileMap.CellToWorld(worldToCellPos).x-1 &&
                           originTPos.y - originRtYLenHalf < (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + 1)
                        {
                            float distX = Mathf.Abs((originTPos.x + originRtXLenHalf) -
                                pTileMap.CellToWorld(worldToCellPos).x);
                            float distY = Mathf.Abs((originTPos.y - originRtYLenHalf) -
                               (pTileMap.CellToWorld(worldToCellPos).y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                pObj.notGrounded = false;
                                pObj.isGrounded = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originTPos.x,
                                    (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + originRtYLenHalf,
                                    originTPos.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                pObj.notRightBlocked = false;
                                pObj.isRightBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                  pTileMap.CellToWorld(worldToCellPos).x - originRtXLenHalf,
                                  originTPos.y,
                                  originTPos.z
                                  );
                            }
                        }
                        else if (originTPos.y - originRtYLenHalf < (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + 1)
                        {
                            pObj.notGrounded = false;
                        }
                        break;
                    //아래쪽 충돌
                    case 4:
                        //충돌하였다면..
                        if (originTPos.y - originRtYLenHalf < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            pObj.notGrounded = false;
                            pObj.isGrounded = true;
                            pObj.originObj.transform.position = new Vector3(
                                originTPos.x,
                                (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + originRtYLenHalf,
                                originTPos.z
                                );
                        }
                        else if (originTPos.y - originRtYLenHalf < (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + 1)
                        {
                            pObj.notGrounded = false;
                        }

                        break;
                    //왼쪽 아래 충돌
                    case 5:
                        //충돌하였다면..
                        if (originTPos.x - originRtXLenHalf < (pTileMap.CellToWorld(worldToCellPos).x + tileSize) - 1 &&
                           originTPos.y - originRtYLenHalf < (pTileMap.CellToWorld(worldToCellPos).y + tileSize) - 1)
                        {
                            float distX = Mathf.Abs((originTPos.x - originRtXLenHalf) -
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize));
                            float distY = Mathf.Abs((originTPos.y - originRtYLenHalf) -
                               (pTileMap.CellToWorld(worldToCellPos).y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                pObj.notGrounded = false;
                                pObj.isGrounded = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originTPos.x,
                                    (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + originRtYLenHalf,
                                    originTPos.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                pObj.notLeftBlocked = false;
                                pObj.isLeftBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize) + originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                            }
                        }
                        else if (originTPos.y - originRtYLenHalf < (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + 1)
                        {
                            pObj.notGrounded = false;
                        }
                        if (originTPos.x - originRtXLenHalf < (pTileMap.CellToWorld(worldToCellPos).x + tileSize) + 1)
                        {
                            pObj.notLeftBlocked = false;
                        }
                        break;
                    //왼쪽 충돌
                    case 6:
                        //충돌하였다면..
                        if (originTPos.x - originRtXLenHalf < (pTileMap.CellToWorld(worldToCellPos).x + tileSize)+1)
                        {
                            pObj.notLeftBlocked = false;
                            pObj.isLeftBlocked = true;
                            pObj.originObj.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x +tileSize) + originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                        }
                        if (originTPos.x - originRtXLenHalf < (pTileMap.CellToWorld(worldToCellPos).x + tileSize) - 1)
                        {
                            pObj.notLeftBlocked = false;
                        }
                        break;
                    //왼쪽 위 충돌
                    case 7:
                        //충돌하였다면..
                        if (originTPos.x - originRtXLenHalf < (pTileMap.CellToWorld(worldToCellPos).x + tileSize)+1 &&
                           originTPos.y + originRtYLenHalf > pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            float distX = Mathf.Abs((originTPos.x - originRtXLenHalf) -
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize));
                            float distY = Mathf.Abs((originTPos.y + originRtYLenHalf) -
                               (pTileMap.CellToWorld(worldToCellPos).y));

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                pObj.notUpBlocked = false;
                                pObj.isUpBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originTPos.x,
                                    pTileMap.CellToWorld(worldToCellPos).y - originRtYLenHalf,
                                    originTPos.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                pObj.notLeftBlocked = false;
                                pObj.isLeftBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize) + originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                            }
                        }
                        break;
                }
                originTPos = pObj.originObj.transform.position;
            }
        }
        //========for문 종료========//
    }

    private void SetEntireTiles()
    {
        dic_canHit = new Dictionary<Vector3, Tile>();

        foreach (Vector3Int pos in tileMap_canHit.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            if (!tileMap_canHit.HasTile(localPlace))
                continue;
            if (localPlace.x < 0)
                continue;

            Tile tile = new Tile(localPlace, tileMap_canHit.GetTile(localPlace), 1, 10, (TILEDIRECTION)0);

            dic_canHit.Add(tileMap_canHit.CellToWorld(localPlace), tile);
        }


        Tile tempTileToUse;
        foreach (Vector3Int pos in tileMap_canHit.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 worldPos = tileMap_canHit.CellToWorld(localPlace);

            if (!tileMap_canHit.HasTile(localPlace))
                continue;
            if (localPlace.x < 0)
                continue;

            if (dic_canHit.ContainsKey(worldPos).Equals(true))
            {
                tempTileToUse = dic_canHit[worldPos];
                tempTileToUse.dir = GetTileDirection(worldPos);
                dic_canHit[worldPos] = tempTileToUse;
            }
        }

        foreach (Vector3Int pos in tileMap_canHit.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 worldPos = tileMap_canHit.CellToWorld(localPlace);

            if (!tileMap_canHit.HasTile(localPlace))
                continue;
            if (localPlace.x < 0)
                continue;
        }

        //18, -1, 0
        canHitTile_100 = new TileBase[15];
        canHitTile_60 = new TileBase[15];
        canHitTile_30 = new TileBase[15];
        for (int i = 0; i < 15; i++)
        {
            canHitTile_100[i] = tileMap_canHit.GetTile(new Vector3Int(-18 + i, -1, 0));
            canHitTile_60[i] = tileMap_canHit.GetTile(new Vector3Int(-18 + i, -2, 0));
            canHitTile_30[i] = tileMap_canHit.GetTile(new Vector3Int(-18 + i, -3, 0));
        }
        

        cUtil._tileMng = this;
    }

    private void UpdateAttackedTile(Vector3 pCurPos)
    {
        Vector3Int worldToCellPos = tileMap_canHit.WorldToCell(pCurPos);
        Vector3 cellToWorldPos = tileMap_canHit.CellToWorld(worldToCellPos);
        Tile tempTileToUse;
        
        tempTileToUse = dic_canHit[cellToWorldPos];
        tempTileToUse.dir = GetTileDirection(cellToWorldPos);
        tempTileToUse.hp -= 1;
        dic_canHit[cellToWorldPos] = tempTileToUse;

        Debug.Log(dic_canHit[cellToWorldPos].dir);
        Debug.Log("ATTACKED" + " : " + dic_canHit[cellToWorldPos].hp);

        UpdateTile(cellToWorldPos);

        if (dic_canHit[cellToWorldPos].hp <= 0)
        {
            tempTileToUse.hp = 0;
            tileMap_canHit.SetTile(worldToCellPos, null);

            Vector3 tempPos = Vector3.zero;

            if (isTileExist(0, cellToWorldPos).Equals(true))
            {
                tempPos = new Vector3(cellToWorldPos.x, cellToWorldPos.y + tileSize, cellToWorldPos.z);
                UpdateTile(tempPos);
            }
            if (isTileExist(1, cellToWorldPos).Equals(true))
            {
                tempPos = new Vector3(cellToWorldPos.x + tileSize, cellToWorldPos.y, cellToWorldPos.z);
                UpdateTile(tempPos);
            }
            if (isTileExist(2, cellToWorldPos).Equals(true))
            {
                tempPos = new Vector3(cellToWorldPos.x, cellToWorldPos.y - tileSize, cellToWorldPos.z);
                UpdateTile(tempPos);
            }
            if (isTileExist(3, cellToWorldPos).Equals(true))
            {
                tempPos = new Vector3(cellToWorldPos.x - tileSize, cellToWorldPos.y, cellToWorldPos.z);
                UpdateTile(tempPos);
            }
        }


    }

    private void UpdateTile(Vector3 pCurPos)
    {
        Vector3Int worldToCellPos = tileMap_canHit.WorldToCell(pCurPos);
        Vector3 cellToWorldPos = tileMap_canHit.CellToWorld(worldToCellPos);
        Tile tempTileToUse;

        tempTileToUse = dic_canHit[cellToWorldPos];
        tempTileToUse.dir = GetTileDirection(cellToWorldPos);

        if (tempTileToUse.hp <= 3)
        {
            tileMap_canHit.SetTile(worldToCellPos, canHitTile_30[(int)tempTileToUse.dir - 1]);
        }
        else if (tempTileToUse.hp <= 6)
        {
            tileMap_canHit.SetTile(worldToCellPos, canHitTile_60[(int)tempTileToUse.dir - 1]);
        }
        else
        {
            tileMap_canHit.SetTile(worldToCellPos, canHitTile_100[(int)tempTileToUse.dir - 1]);
        }
        dic_canHit[cellToWorldPos] = tempTileToUse;
    }

    private TILEDIRECTION GetTileDirection(Vector3 pCurPos)
    {
        TILEDIRECTION r_tileDir = TILEDIRECTION.NONE;

        if(isTileExist(0, pCurPos).Equals(false) &&
            isTileExist(1, pCurPos).Equals(false) &&
            isTileExist(2, pCurPos).Equals(true) &&
            isTileExist(3, pCurPos).Equals(false))        
            r_tileDir = TILEDIRECTION.UP_PLUS;
        else if (isTileExist(0, pCurPos).Equals(false) &&
            isTileExist(1, pCurPos).Equals(false) &&
            isTileExist(2, pCurPos).Equals(true) &&
            isTileExist(3, pCurPos).Equals(true))
            r_tileDir = TILEDIRECTION.UPRIGHT;
        else if (isTileExist(0, pCurPos).Equals(false) &&
            isTileExist(1, pCurPos).Equals(false) &&
            isTileExist(2, pCurPos).Equals(false) &&
            isTileExist(3, pCurPos).Equals(true))
            r_tileDir = TILEDIRECTION.RIGHT_PLUS;
        else if (isTileExist(0, pCurPos).Equals(true) &&
            isTileExist(1, pCurPos).Equals(false) &&
            isTileExist(2, pCurPos).Equals(false) &&
            isTileExist(3, pCurPos).Equals(true))
            r_tileDir = TILEDIRECTION.RIGHTDOWN;
        else if (isTileExist(0, pCurPos).Equals(true) &&
            isTileExist(1, pCurPos).Equals(false) &&
            isTileExist(2, pCurPos).Equals(false) &&
            isTileExist(3, pCurPos).Equals(false))
            r_tileDir = TILEDIRECTION.DOWN_PLUS;
        else if (isTileExist(0, pCurPos).Equals(true) &&
            isTileExist(1, pCurPos).Equals(true) &&
            isTileExist(2, pCurPos).Equals(false) &&
            isTileExist(3, pCurPos).Equals(false))
            r_tileDir = TILEDIRECTION.DOWNLEFT;
        else if (isTileExist(0, pCurPos).Equals(false) &&
            isTileExist(1, pCurPos).Equals(true) &&
            isTileExist(2, pCurPos).Equals(false) &&
            isTileExist(3, pCurPos).Equals(false))
            r_tileDir = TILEDIRECTION.LEFT_PLUS;
        else if (isTileExist(0, pCurPos).Equals(false) &&
            isTileExist(1, pCurPos).Equals(true) &&
            isTileExist(2, pCurPos).Equals(true) &&
            isTileExist(3, pCurPos).Equals(false))
            r_tileDir = TILEDIRECTION.LEFTUP;
        else if (isTileExist(0, pCurPos).Equals(false) &&
            isTileExist(1, pCurPos).Equals(true) &&
            isTileExist(2, pCurPos).Equals(true) &&
            isTileExist(3, pCurPos).Equals(true))
            r_tileDir = TILEDIRECTION.UP;
        else if (isTileExist(0, pCurPos).Equals(true) &&
            isTileExist(1, pCurPos).Equals(false) &&
            isTileExist(2, pCurPos).Equals(true) &&
            isTileExist(3, pCurPos).Equals(true))
            r_tileDir = TILEDIRECTION.RIGHT;
        else if (isTileExist(0, pCurPos).Equals(true) &&
            isTileExist(1, pCurPos).Equals(true) &&
            isTileExist(2, pCurPos).Equals(false) &&
            isTileExist(3, pCurPos).Equals(true))
            r_tileDir = TILEDIRECTION.DOWN;
        else if (isTileExist(0, pCurPos).Equals(true) &&
            isTileExist(1, pCurPos).Equals(true) &&
            isTileExist(2, pCurPos).Equals(true) &&
            isTileExist(3, pCurPos).Equals(false))
            r_tileDir = TILEDIRECTION.LEFT;
        else if (isTileExist(0, pCurPos).Equals(true) &&
            isTileExist(1, pCurPos).Equals(false) &&
            isTileExist(2, pCurPos).Equals(true) &&
            isTileExist(3, pCurPos).Equals(false))
            r_tileDir = TILEDIRECTION.HORIZONTAL;
        else if (isTileExist(0, pCurPos).Equals(false) &&
            isTileExist(1, pCurPos).Equals(true) &&
            isTileExist(2, pCurPos).Equals(false) &&
            isTileExist(3, pCurPos).Equals(true))
            r_tileDir = TILEDIRECTION.VERTICAL;
        else if (isTileExist(0, pCurPos).Equals(false) &&
            isTileExist(1, pCurPos).Equals(false) &&
            isTileExist(2, pCurPos).Equals(false) &&
            isTileExist(3, pCurPos).Equals(false))
            r_tileDir = TILEDIRECTION.SOLO;
        else if (isTileExist(0, pCurPos).Equals(true) &&
            isTileExist(1, pCurPos).Equals(true) &&
            isTileExist(2, pCurPos).Equals(true) &&
            isTileExist(3, pCurPos).Equals(true))
            r_tileDir = TILEDIRECTION.BLOCKED;

        return r_tileDir;
    }

    private bool isTileExist(int pDir, Vector3 pCurPos)
    {
        Vector3 calcPos = Vector3.zero;
        bool t = false;

        switch (pDir)
        {
            //위
            case 0:
                calcPos = new Vector3(pCurPos.x, pCurPos.y + tileSize, pCurPos.z);
                break;
                //오른
            case 1:
                calcPos = new Vector3(pCurPos.x + tileSize, pCurPos.y, pCurPos.z);
                break;
                //아래
            case 2:
                calcPos = new Vector3(pCurPos.x, pCurPos.y - tileSize, pCurPos.z);
                break;
                //왼
            case 3:
                calcPos = new Vector3(pCurPos.x - tileSize, pCurPos.y, pCurPos.z);
                break;
        }

        TileBase[] tempTile = new TileBase[2]
        { tileMap_canHit.GetTile(tileMap_canHit.WorldToCell(calcPos)),
        tileMap_cannotHit.GetTile(tileMap_cannotHit.WorldToCell(calcPos))};

        if (tempTile[0] != null || tempTile[1] != null)
            t = true;

        return t;
    }
}
