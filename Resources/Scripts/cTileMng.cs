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

    public cPlayer player;
    private Transform playerT;
    Vector3Int[] cellPos;

    private void Start()
    {
        player = GameObject.Find("DungeonNormalScene").
            transform.Find("Canvas_main").Find("Player").GetChild(0).GetComponent<cPlayer>();
        playerT = player.transform;
        tileMap = this.GetComponent<Tilemap>();
    }

    private void FixedUpdate()
    {
        CheckTiles();
    }

    private void CheckTiles()
    {
        tileMap.RefreshAllTiles();

        cellPos = new Vector3Int[]
            {
                new Vector3Int((int)playerT.position.x, (int)playerT.position.y + 60, 0),
                new Vector3Int((int)playerT.position.x + 60, (int)playerT.position.y, 0),
                new Vector3Int((int)playerT.position.x, (int)playerT.position.y - 60, 0),
                new Vector3Int((int)playerT.position.x - 60, (int)playerT.position.y, 0)
            };

        for(int i = 0; i < 4; i++)
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
                        if(player.isUpBlocked.Equals(false))
                        {
                            player.isUpBlocked = true;
                            player.transform.position = new Vector3(
                                playerT.position.x,
                                tileMap.CellToWorld(worldToCellPos).y - (player.rt.size.y * 0.5f),
                                playerT.position.z);
                        }

                        Debug.Log(tileMap.CellToWorld(worldToCellPos));
                        break;
                    //오른쪽 충돌
                    case 1: player.isRightBlocked = true; break;
                    //아래쪽 충돌
                    case 2: player.isGrounded = true; break;
                    //왼쪽 충돌
                    case 3: player.isLeftBlocked = true; break;
                }
            }
            else
            {
                switch (i)
                {
                    //위쪽 충돌
                    case 0: player.isUpBlocked = false; break;
                    //오른쪽 충돌
                    case 1: player.isRightBlocked = false; break;
                    //아래쪽 충돌
                    case 2: player.isGrounded = false; break;
                    //왼쪽 충돌
                    case 3: player.isLeftBlocked = false; break;
                }
            }
        }
    }
}
