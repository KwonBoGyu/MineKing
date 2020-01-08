using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;



public class cTileMng : MonoBehaviour
{
    public struct Tile
    {
        public Vector3Int location { get; set; }
        public TileBase tileBase;
        public int level { get; set; }
        public float hp { get; set; }

        public Tile(Vector3Int pLocation, TileBase pTileBase, int pLevel, float pHp)
        {
            location = pLocation;
            tileBase = pTileBase;
            level = pLevel;
            hp = pHp;
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
    private TileBase canHitTile_100;
    private TileBase canHitTile_60;
    private TileBase canHitTile_30;


    private void Start()
    {
        tileMap_canHit = this.transform.GetChild(0).GetComponent<Tilemap>();
        tileMap_cannotHit = this.transform.GetChild(1).GetComponent<Tilemap>();
        tempTile = null;

        SetEntireTiles();
    }

    public void CheckAttackedTile(Vector3 pWorldPos, float pDamage)
    {
        Vector3Int worldToCellPos = tileMap_canHit.WorldToCell(pWorldPos);
        Vector3 convertedWorldPos = tileMap_canHit.CellToWorld(worldToCellPos);
        Debug.Log(pWorldPos);
        Debug.Log(worldToCellPos);
        Debug.Log(convertedWorldPos);
        
        tempTile = dic_canHit[convertedWorldPos];
        Debug.Log("어택");

        //해당 위치에 타일이 있다면
        if (tempTile != null)
        {
            Debug.Log("타일 충돌");

            Tile tempTileToUse = dic_canHit[convertedWorldPos];
            tempTileToUse.hp -= pDamage;
            Debug.Log(tempTileToUse.hp);

            if (tempTileToUse.hp <= 3)
            {
                tileMap_canHit.SetTile(worldToCellPos, canHitTile_30);                
            }
            else if (tempTileToUse.hp <= 6)
            {
                tileMap_canHit.SetTile(worldToCellPos, canHitTile_60);
            }
            else
            {
                tileMap_canHit.SetTile(worldToCellPos, canHitTile_100);
                Debug.Log(canHitTile_100);
            }

            if (tempTileToUse.hp <= 0)
            {
                tempTileToUse.hp = 0;
                tileMap_canHit.SetTile(worldToCellPos, null);
            }

            dic_canHit[convertedWorldPos] = tempTileToUse;
        }
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
        pTileMap.RefreshAllTiles();
        Vector3 originTPos = pObj.originObj.transform.position;
        float originRtXLenHalf = pObj.rt.size.x * 0.5f;
        float originRtYLenHalf = pObj.rt.size.y * 0.5f;

        cellPos = new Vector3Int[]
            {
                new Vector3Int((int)originTPos.x, (int)originTPos.y + 150, 0),
                new Vector3Int((int)originTPos.x + 25, (int)originTPos.y + 150, 0),
                new Vector3Int((int)originTPos.x + 60, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x + 25, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 25, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 60, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x - 25, (int)originTPos.y + 150, 0),
            };

        for(int i = 0; i < cellPos.Length; i++)
        {
            Vector3Int worldToCellPos = pTileMap.WorldToCell(cellPos[i]);

            TileBase t_tile = pTileMap.GetTile(worldToCellPos);
            
            if (t_tile != null)
            {
                //pTileMap.SetTileFlags(worldToCellPos, TileFlags.None);
                //pTileMap.SetColor(worldToCellPos, Color.red);         

                switch(i)
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
                        if (originTPos.x + originRtXLenHalf > pTileMap.CellToWorld(worldToCellPos).x-1 &&
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
                           originTPos.y - originRtYLenHalf < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
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
                        if (originTPos.x - originRtXLenHalf < (pTileMap.CellToWorld(worldToCellPos).x + tileSize)+1 &&
                           originTPos.y - originRtYLenHalf < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
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

            if (!tileMap_canHit.HasTile(localPlace)) continue;

            Tile tile = new Tile();
            tile.location = localPlace;
            tile.tileBase = tileMap_canHit.GetTile(localPlace);
            tile.level = 1;
            tile.hp = 10;

            dic_canHit.Add(tileMap_canHit.CellToWorld(localPlace), tile);
            Debug.Log(tileMap_canHit.CellToWorld(localPlace));
        }

        canHitTile_100 = tileMap_canHit.GetTile(new Vector3Int(-5, -1, 0));
        canHitTile_60 = tileMap_canHit.GetTile(new Vector3Int(-5, -2, 0));
        canHitTile_30 = tileMap_canHit.GetTile(new Vector3Int(-5, -3, 0));


        cUtil._tileMng = this;
    }
}
