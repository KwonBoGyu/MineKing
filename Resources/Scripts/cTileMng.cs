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

    public GameObject player;
    public cPlayer scr_player;

    private int isUpBlocked_123;
    private int isGrounded_123;

    Tile? tempTile;
    private TileBase canHitTile_100;
    private TileBase canHitTile_60;
    private TileBase canHitTile_30;


    private void Start()
    {
        player = GameObject.Find("DungeonNormalScene").
            transform.Find("Canvas_main").Find("Player").gameObject;
        scr_player = player.transform.GetChild(0).GetComponent<cPlayer>();
        tileMap_canHit = this.transform.GetChild(0).GetComponent<Tilemap>();
        tileMap_cannotHit = this.transform.GetChild(1).GetComponent<Tilemap>();
        tempTile = null;

        SetEntireTiles();
    }

    private void FixedUpdate()
    {
        isUpBlocked_123 = 0;
        isGrounded_123 = 0;

        CheckTiles(tileMap_canHit);
        CheckTiles(tileMap_cannotHit);

        if (isUpBlocked_123.Equals(0))
            scr_player.isUpBlocked = false;
        if (isGrounded_123.Equals(0))
            scr_player.isGrounded = false;
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
    
    private void CheckTiles(Tilemap pTileMap)
    {
        pTileMap.RefreshAllTiles();

        cellPos = new Vector3Int[]
            {
                new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y + 150, 0),
                new Vector3Int((int)player.transform.position.x + 30, (int)player.transform.position.y + 150, 0),
                new Vector3Int((int)player.transform.position.x + 60, (int)player.transform.position.y, 0),
                new Vector3Int((int)player.transform.position.x + 30, (int)player.transform.position.y - 150, 0),
                new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y - 150, 0),
                new Vector3Int((int)player.transform.position.x - 30, (int)player.transform.position.y - 150, 0),
                new Vector3Int((int)player.transform.position.x - 60, (int)player.transform.position.y, 0),
                new Vector3Int((int)player.transform.position.x - 30, (int)player.transform.position.y + 150, 0),
            };

        for(int i = 0; i < cellPos.Length; i++)
        {
            Vector3Int worldToCellPos = pTileMap.WorldToCell(cellPos[i]);

            TileBase t_tile = pTileMap.GetTile(worldToCellPos);
            
            if (t_tile != null)
            {
                pTileMap.SetTileFlags(worldToCellPos, TileFlags.None);
                //pTileMap.SetColor(worldToCellPos, Color.red);         

                switch(i)
                {
                    //위쪽 충돌
                    case 0:
                        isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.y + scr_player.rt.size.y * 0.5f> pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            scr_player.isUpBlocked = true;
                            player.gameObject.transform.position = new Vector3(
                                player.transform.position.x,
                                pTileMap.CellToWorld(worldToCellPos).y - scr_player.rt.size.y * 0.5f,                                
                                player.transform.position.z
                                );
                        }
                        break;
                    //오른쪽 위 충돌
                    case 1:
                        isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.x + scr_player.rt.size.x * 0.5f > pTileMap.CellToWorld(worldToCellPos).x &&
                           player.transform.position.y + scr_player.rt.size.y * 0.5f > pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            float distX = Mathf.Abs((player.transform.position.x + scr_player.rt.size.x * 0.5f) -
                                pTileMap.CellToWorld(worldToCellPos).x);
                            float distY = Mathf.Abs((player.transform.position.y + scr_player.rt.size.y * 0.5f) -
                                pTileMap.CellToWorld(worldToCellPos).y);

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                scr_player.isUpBlocked = true;
                                player.gameObject.transform.position = new Vector3(
                                    player.transform.position.x,
                                    pTileMap.CellToWorld(worldToCellPos).y - scr_player.rt.size.y * 0.5f,
                                    player.transform.position.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                player.gameObject.transform.position = new Vector3(
                                pTileMap.CellToWorld(worldToCellPos).x - scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                            }
                        }
                        break;
                    //오른쪽 충돌
                    case 2:
                        //충돌하였다면..
                        if(player.transform.position.x + scr_player.rt.size.x * 0.5f > pTileMap.CellToWorld(worldToCellPos).x)
                        {
                            player.gameObject.transform.position = new Vector3(
                                pTileMap.CellToWorld(worldToCellPos).x - scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                        }
                        break;
                    //오른쪽 아래 충돌
                    case 3:
                        isGrounded_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.x + scr_player.rt.size.x * 0.5f > pTileMap.CellToWorld(worldToCellPos).x &&
                           player.transform.position.y - scr_player.rt.size.y * 0.5f < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            float distX = Mathf.Abs((player.transform.position.x + scr_player.rt.size.x * 0.5f) -
                                pTileMap.CellToWorld(worldToCellPos).x);
                            float distY = Mathf.Abs((player.transform.position.y - scr_player.rt.size.y * 0.5f) -
                               (pTileMap.CellToWorld(worldToCellPos).y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                scr_player.isGrounded = true;
                                player.gameObject.transform.position = new Vector3(
                                    player.transform.position.x,
                                    (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + scr_player.rt.size.y * 0.5f,
                                    player.transform.position.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                player.gameObject.transform.position = new Vector3(
                                  pTileMap.CellToWorld(worldToCellPos).x - scr_player.rt.size.x * 0.5f,
                                  player.transform.position.y,
                                  player.transform.position.z
                                  );
                            }
                        }
                        break;
                    //아래쪽 충돌
                    case 4:
                        isGrounded_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.y - scr_player.rt.size.y * 0.5f < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            Debug.Log("GROUNDED");
                            scr_player.isGrounded = true;
                            player.gameObject.transform.position = new Vector3(
                                player.transform.position.x,
                                (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + scr_player.rt.size.y * 0.5f,
                                player.transform.position.z
                                );
                        }
                        break;
                    //왼쪽 아래 충돌
                    case 5:
                        isGrounded_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.x - scr_player.rt.size.x * 0.5f < (pTileMap.CellToWorld(worldToCellPos).x + tileSize) &&
                           player.transform.position.y - scr_player.rt.size.y * 0.5f < (pTileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            float distX = Mathf.Abs((player.transform.position.x - scr_player.rt.size.x * 0.5f) -
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize));
                            float distY = Mathf.Abs((player.transform.position.y - scr_player.rt.size.y * 0.5f) -
                               (pTileMap.CellToWorld(worldToCellPos).y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                scr_player.isGrounded = true;
                                player.gameObject.transform.position = new Vector3(
                                    player.transform.position.x,
                                    (pTileMap.CellToWorld(worldToCellPos).y + tileSize) + scr_player.rt.size.y * 0.5f,
                                    player.transform.position.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                player.gameObject.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize) + scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                            }
                        }
                        break;
                    //왼쪽 충돌
                    case 6:
                        //충돌하였다면..
                        if (player.transform.position.x - scr_player.rt.size.x * 0.5f < (pTileMap.CellToWorld(worldToCellPos).x + tileSize))
                        {
                            player.gameObject.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x +tileSize) + scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                        }
                        break;
                    //왼쪽 위 충돌
                    case 7:
                        isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.x - scr_player.rt.size.x * 0.5f < (pTileMap.CellToWorld(worldToCellPos).x + tileSize) &&
                           player.transform.position.y + scr_player.rt.size.y * 0.5f > pTileMap.CellToWorld(worldToCellPos).y)
                        {
                            float distX = Mathf.Abs((player.transform.position.x - scr_player.rt.size.x * 0.5f) -
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize));
                            float distY = Mathf.Abs((player.transform.position.y + scr_player.rt.size.y * 0.5f) -
                               (pTileMap.CellToWorld(worldToCellPos).y));

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                scr_player.isUpBlocked = true;
                                player.gameObject.transform.position = new Vector3(
                                    player.transform.position.x,
                                    pTileMap.CellToWorld(worldToCellPos).y - scr_player.rt.size.y * 0.5f,
                                    player.transform.position.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                player.gameObject.transform.position = new Vector3(
                                (pTileMap.CellToWorld(worldToCellPos).x + tileSize) + scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                            }
                        }
                        break;
                }
            }
        }
        //========for문 종료========//

        //if (isGrounded_123.Equals(0))
        //    scr_player.isGrounded = false;
        if (isUpBlocked_123.Equals(0))
            scr_player.isUpBlocked = false;
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
        scr_player.tileMng = this;
    }
}
