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
                
        tempTile = dic_canHit[convertedWorldPos];
        Debug.Log("어택");

        //해당 위치에 타일이 있다면
        if (tempTile != null)
        {
            Debug.Log("타일 충돌");
            tileMap_canHit.SetColor(worldToCellPos, Color.red);

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
        pObj.isUpBlocked_123 = 0;
        pObj.isGrounded_123 = 0;

        CheckTiles(tileMap_canHit, pObj);
        CheckTiles(tileMap_cannotHit, pObj);

        if (pObj.isUpBlocked_123.Equals(0))
            pObj.isUpBlocked = false;
        if (pObj.isGrounded_123.Equals(0))
            pObj.isGrounded = false;
    }
    
    private void CheckTiles(Tilemap pTileMap, cCharacter pObj)
    {
        pTileMap.RefreshAllTiles();
        Transform originT = pObj.originObj.transform;
        BoxCollider2D originRt = pObj.rt;

        cellPos = new Vector3Int[]
            {
                new Vector3Int((int)originT.position.x, (int)originT.position.y + 150, 0),
                new Vector3Int((int)originT.position.x + 25, (int)originT.position.y + 150, 0),
                new Vector3Int((int)originT.position.x + 60, (int)originT.position.y, 0),
                new Vector3Int((int)originT.position.x + 25, (int)originT.position.y - 150, 0),
                new Vector3Int((int)originT.position.x, (int)originT.position.y - 150, 0),
                new Vector3Int((int)originT.position.x - 25, (int)originT.position.y - 150, 0),
                new Vector3Int((int)originT.position.x - 60, (int)originT.position.y, 0),
                new Vector3Int((int)originT.position.x - 25, (int)originT.position.y + 150, 0),
            };

        for(int i = 0; i < cellPos.Length; i++)
        {
            Vector3Int worldToCellPos = pTileMap.WorldToCell(cellPos[i]);

            TileBase t_tile = pTileMap.GetTile(worldToCellPos);
            
            if (t_tile != null)
            {
                pTileMap.SetTileFlags(worldToCellPos, TileFlags.None);
                pTileMap.SetColor(worldToCellPos, Color.red);         

                switch(i)
                {
                    //위쪽 충돌
                    case 0:
                        pObj.isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (originT.position.y + originRt.size.y * 0.5f> pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            pObj.isUpBlocked = true;
                            pObj.originObj.transform.position = new Vector3(
                                originT.position.x,
                                pTileMap.CellToWorld(worldToCellPos).y - originRt.size.y * 0.5f,
                                originT.position.z
                                );
                        }
                        break;
                    //오른쪽 위 충돌
                    case 1:
                        pObj.isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (originT.position.x + originRt.size.x * 0.5f > pTileMap.CellToWorld(worldToCellPos).x &&
                           originT.position.y + originRt.size.y * 0.5f > pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            float distX = Mathf.Abs((originT.position.x + originRt.size.x * 0.5f) -
                                pTileMap.CellToWorld(worldToCellPos).x);
                            float distY = Mathf.Abs((originT.position.y + originRt.size.y * 0.5f) -
                                pTileMap.CellToWorld(worldToCellPos).y);

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                pObj.isUpBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originT.position.x,
                                    pTileMap.CellToWorld(worldToCellPos).y - originRt.size.y * 0.5f,
                                    originT.position.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                pObj.originObj.transform.position = new Vector3(
                                pTileMap.CellToWorld(worldToCellPos).x - originRt.size.x * 0.5f,
                                originT.position.y,
                                originT.position.z
                                );
                            }
                        }
                        break;
                    //오른쪽 충돌
                    case 2:
                        //충돌하였다면..
                        if(originT.position.x + originRt.size.x * 0.5f > pTileMap.CellToWorld(worldToCellPos).x)
                        {
                            pObj.originObj.transform.position = new Vector3(
                                pTileMap.CellToWorld(worldToCellPos).x - originRt.size.x * 0.5f,
                                originT.position.y,
                                originT.position.z
                                );
                        }
                        break;
                    //오른쪽 아래 충돌
                    case 3:
                        pObj.isGrounded_123 += 1;
                        //충돌하였다면..
                        if (originT.position.x + originRt.size.x * 0.5f > pTileMap.CellToWorld(worldToCellPos).x &&
                           originT.position.y - originRt.size.y * 0.5f < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            float distX = Mathf.Abs((originT.position.x + originRt.size.x * 0.5f) -
                                pTileMap.CellToWorld(worldToCellPos).x);
                            float distY = Mathf.Abs((originT.position.y - originRt.size.y * 0.5f) -
                               (pTileMap.CellToWorld(worldToCellPos).y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                pObj.isGrounded = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originT.position.x,
                                    (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + originRt.size.y * 0.5f,
                                    originT.position.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                pObj.originObj.transform.position = new Vector3(
                                  pTileMap.CellToWorld(worldToCellPos).x - originRt.size.x * 0.5f,
                                  originT.position.y,
                                  originT.position.z
                                  );
                            }
                        }
                        break;
                    //아래쪽 충돌
                    case 4:
                        pObj.isGrounded_123 += 1;
                        //충돌하였다면..
                        if (originT.position.y - originRt.size.y * 0.5f < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            Debug.Log("GROUNDED");
                            pObj.isGrounded = true;
                            pObj.originObj.transform.position = new Vector3(
                                originT.position.x,
                                (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + originRt.size.y * 0.5f,
                                originT.position.z
                                );
                        }
                        break;
                    //왼쪽 아래 충돌
                    case 5:
                        pObj.isGrounded_123 += 1;
                        //충돌하였다면..
                        if (originT.position.x - originRt.size.x * 0.5f < (pTileMap.CellToWorld(worldToCellPos).x + tileSize) &&
                           originT.position.y - originRt.size.y * 0.5f < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            float distX = Mathf.Abs((originT.position.x - originRt.size.x * 0.5f) -
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize));
                            float distY = Mathf.Abs((originT.position.y - originRt.size.y * 0.5f) -
                               (pTileMap.CellToWorld(worldToCellPos).y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                pObj.isGrounded = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originT.position.x,
                                    (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + originRt.size.y * 0.5f,
                                    originT.position.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                pObj.originObj.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize) + originRt.size.x * 0.5f,
                                originT.position.y,
                                originT.position.z
                                );
                            }
                        }
                        break;
                    //왼쪽 충돌
                    case 6:
                        //충돌하였다면..
                        if (originT.position.x - originRt.size.x * 0.5f < (pTileMap.CellToWorld(worldToCellPos).x + tileSize))
                        {
                            pObj.originObj.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x +tileSize) + originRt.size.x * 0.5f,
                                originT.position.y,
                                originT.position.z
                                );
                        }
                        break;
                    //왼쪽 위 충돌
                    case 7:
                        pObj.isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (originT.position.x - originRt.size.x * 0.5f < (pTileMap.CellToWorld(worldToCellPos).x + tileSize) &&
                           originT.position.y + originRt.size.y * 0.5f > pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            float distX = Mathf.Abs((originT.position.x - originRt.size.x * 0.5f) -
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize));
                            float distY = Mathf.Abs((originT.position.y + originRt.size.y * 0.5f) -
                               (pTileMap.CellToWorld(worldToCellPos).y));

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                pObj.isUpBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originT.position.x,
                                    pTileMap.CellToWorld(worldToCellPos).y - originRt.size.y * 0.5f,
                                    originT.position.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                pObj.originObj.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize) + originRt.size.x * 0.5f,
                                originT.position.y,
                                originT.position.z
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
        }

        canHitTile_100 = tileMap_canHit.GetTile(new Vector3Int(-5, -1, 0));
        canHitTile_60 = tileMap_canHit.GetTile(new Vector3Int(-5, -2, 0));
        canHitTile_30 = tileMap_canHit.GetTile(new Vector3Int(-5, -3, 0));

        Debug.Log(dic_canHit.Count);

        cUtil._tileMng = this;
    }
}
