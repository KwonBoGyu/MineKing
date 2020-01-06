using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class cTileMng : MonoBehaviour
{
    public struct Index2
    {
        int x;
        int y;
    }

    public struct Tile
    {
        public Index2 location;
        public Image img;
        public int level;
        public float hp;
    }

    public int tileSize;
    public int numX;
    public int numY;
    public List<Tile> l_tiles;

    private Tilemap tileMap;

    public GameObject player;
    public cPlayer scr_player;
    Vector3Int[] cellPos;

    private int isUpBlocked_123;
    private int isGrounded_123;

    private void Start()
    {
        player = GameObject.Find("DungeonNormalScene").
            transform.Find("Canvas_main").Find("Player").gameObject;
        scr_player = player.transform.GetChild(0).GetComponent<cPlayer>();
        tileMap = this.GetComponent<Tilemap>();
    }

    private void FixedUpdate()
    {
        CheckTiles();
    }

    private void CheckTiles()
    {
        tileMap.RefreshAllTiles();

        isUpBlocked_123 = 0;
        isGrounded_123 = 0;

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
            Vector3Int worldToCellPos = tileMap.WorldToCell(cellPos[i]);

            TileBase t_tile = tileMap.GetTile(worldToCellPos);

            if (t_tile != null)
            {
                tileMap.SetTileFlags(worldToCellPos, TileFlags.None);
                tileMap.SetColor(worldToCellPos, Color.red);

                switch(i)
                {
                    //위쪽 충돌
                    case 0:
                        isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.y + scr_player.rt.size.y * 0.5f> tileMap.CellToWorld(worldToCellPos).y)
                        {
                            scr_player.isUpBlocked = true;
                            player.gameObject.transform.position = new Vector3(
                                player.transform.position.x,
                                tileMap.CellToWorld(worldToCellPos).y - scr_player.rt.size.y * 0.5f,                                
                                player.transform.position.z
                                );
                        }
                        break;
                    //오른쪽 위 충돌
                    case 1:
                        isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.x + scr_player.rt.size.x * 0.5f > tileMap.CellToWorld(worldToCellPos).x &&
                           player.transform.position.y + scr_player.rt.size.y * 0.5f > tileMap.CellToWorld(worldToCellPos).y)
                        {
                            float distX = Mathf.Abs((player.transform.position.x + scr_player.rt.size.x * 0.5f) -
                                tileMap.CellToWorld(worldToCellPos).x);
                            float distY = Mathf.Abs((player.transform.position.y + scr_player.rt.size.y * 0.5f) -
                                tileMap.CellToWorld(worldToCellPos).y);

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                scr_player.isUpBlocked = true;
                                player.gameObject.transform.position = new Vector3(
                                    player.transform.position.x,
                                    tileMap.CellToWorld(worldToCellPos).y - scr_player.rt.size.y * 0.5f,
                                    player.transform.position.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                player.gameObject.transform.position = new Vector3(
                                tileMap.CellToWorld(worldToCellPos).x - scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                            }

                            
                        }
                        break;
                    //오른쪽 충돌
                    case 2:
                        //충돌하였다면..
                        if(player.transform.position.x + scr_player.rt.size.x * 0.5f > tileMap.CellToWorld(worldToCellPos).x)
                        {
                            player.gameObject.transform.position = new Vector3(
                                tileMap.CellToWorld(worldToCellPos).x - scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                        }
                        break;
                    //오른쪽 아래 충돌
                    case 3:
                        isGrounded_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.x + scr_player.rt.size.x * 0.5f > tileMap.CellToWorld(worldToCellPos).x &&
                           player.transform.position.y - scr_player.rt.size.y * 0.5f < (tileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            float distX = Mathf.Abs((player.transform.position.x + scr_player.rt.size.x * 0.5f) -
                                tileMap.CellToWorld(worldToCellPos).x);
                            float distY = Mathf.Abs((player.transform.position.y - scr_player.rt.size.y * 0.5f) -
                               (tileMap.CellToWorld(worldToCellPos).y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                scr_player.isGrounded = true;
                                player.gameObject.transform.position = new Vector3(
                                    player.transform.position.x,
                                    (tileMap.CellToWorld(worldToCellPos).y + tileSize) + scr_player.rt.size.y * 0.5f,
                                    player.transform.position.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                player.gameObject.transform.position = new Vector3(
                                  tileMap.CellToWorld(worldToCellPos).x - scr_player.rt.size.x * 0.5f,
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
                        if (player.transform.position.y - scr_player.rt.size.y * 0.5f < (tileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            Debug.Log("GROUNDED");
                            scr_player.isGrounded = true;
                            player.gameObject.transform.position = new Vector3(
                                player.transform.position.x,
                                (tileMap.CellToWorld(worldToCellPos).y + tileSize) + scr_player.rt.size.y * 0.5f,
                                player.transform.position.z
                                );
                        }
                        break;
                    //왼쪽 아래 충돌
                    case 5:
                        isGrounded_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.x - scr_player.rt.size.x * 0.5f < (tileMap.CellToWorld(worldToCellPos).x + tileSize) &&
                           player.transform.position.y - scr_player.rt.size.y * 0.5f < (tileMap.CellToWorld(worldToCellPos).y + tileSize))
                        {
                            float distX = Mathf.Abs((player.transform.position.x - scr_player.rt.size.x * 0.5f) -
                                (tileMap.CellToWorld(worldToCellPos).x + tileSize));
                            float distY = Mathf.Abs((player.transform.position.y - scr_player.rt.size.y * 0.5f) -
                               (tileMap.CellToWorld(worldToCellPos).y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                scr_player.isGrounded = true;
                                player.gameObject.transform.position = new Vector3(
                                    player.transform.position.x,
                                    (tileMap.CellToWorld(worldToCellPos).y + tileSize) + scr_player.rt.size.y * 0.5f,
                                    player.transform.position.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                player.gameObject.transform.position = new Vector3(
                                (tileMap.CellToWorld(worldToCellPos).x + tileSize) + scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                            }
                        }
                        break;
                    //왼쪽 충돌
                    case 6:
                        //충돌하였다면..
                        if (player.transform.position.x - scr_player.rt.size.x * 0.5f < (tileMap.CellToWorld(worldToCellPos).x + tileSize))
                        {
                            player.gameObject.transform.position = new Vector3(
                                (tileMap.CellToWorld(worldToCellPos).x +tileSize) + scr_player.rt.size.x * 0.5f,
                                player.transform.position.y,
                                player.transform.position.z
                                );
                        }
                        break;
                    //왼쪽 위 충돌
                    case 7:
                        isUpBlocked_123 += 1;
                        //충돌하였다면..
                        if (player.transform.position.x - scr_player.rt.size.x * 0.5f < (tileMap.CellToWorld(worldToCellPos).x + tileSize) &&
                           player.transform.position.y + scr_player.rt.size.y * 0.5f > tileMap.CellToWorld(worldToCellPos).y)
                        {
                            float distX = Mathf.Abs((player.transform.position.x - scr_player.rt.size.x * 0.5f) -
                                (tileMap.CellToWorld(worldToCellPos).x + tileSize));
                            float distY = Mathf.Abs((player.transform.position.y + scr_player.rt.size.y * 0.5f) -
                               (tileMap.CellToWorld(worldToCellPos).y));

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                scr_player.isUpBlocked = true;
                                player.gameObject.transform.position = new Vector3(
                                    player.transform.position.x,
                                    tileMap.CellToWorld(worldToCellPos).y - scr_player.rt.size.y * 0.5f,
                                    player.transform.position.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                player.gameObject.transform.position = new Vector3(
                                (tileMap.CellToWorld(worldToCellPos).x + tileSize) + scr_player.rt.size.x * 0.5f,
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

        if (isGrounded_123.Equals(0))
            scr_player.isGrounded = false;
        if (isUpBlocked_123.Equals(0))
            scr_player.isUpBlocked = false;
    }
}
