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
        public cProperty maxHp { get; set; }
        public cProperty curHp { get; set; }

        public Tile(Vector3Int pLocation, TileBase pTileBase, cProperty pMaxHp, cProperty pCurHp)
        {
            location = pLocation;
            tileBase = pTileBase;
            maxHp = new cProperty(pMaxHp);
            curHp = new cProperty(pCurHp);
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
    public GameObject effect_destroy;
        
    private void Start()
    {
        tileMap_canHit = this.transform.GetChild(0).GetComponent<Tilemap>();
        tileMap_cannotHit = this.transform.GetChild(1).GetComponent<Tilemap>();
        tempTile = null;

        SetEntireTiles();
    }

    public bool CheckAttackedTile(Vector3 pWorldPos, cProperty pDamage, out float pCurHpPercent)
    {
        bool isChecked = false;
        Vector3Int worldToCellPos = tileMap_canHit.WorldToCell(pWorldPos);
        Vector3 convertedWorldPos = tileMap_canHit.CellToWorld(worldToCellPos);

        //해당 위치에 타일이 있다면
        isChecked = UpdateAttackedTile(convertedWorldPos, pDamage, out pCurHpPercent);

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
            pObj.SetIsGrounded(false);        
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
                new Vector3Int((int)originTPos.x + 24, (int)originTPos.y + 155, 0),
                new Vector3Int((int)originTPos.x + 60, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x + 24, (int)originTPos.y - 155, 0),
                new Vector3Int((int)originTPos.x, (int)originTPos.y - 150, 0),
                new Vector3Int((int)originTPos.x - 24, (int)originTPos.y - 155, 0),
                new Vector3Int((int)originTPos.x - 60, (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x - 24, (int)originTPos.y + 155, 0),
            };

        for (short i = 0; i < cellPos.Length; i++)
        {
            Vector3Int worldToCellPos = pTileMap.WorldToCell(cellPos[i]);
            Vector3 cellToWorldPos = pTileMap.CellToWorld(worldToCellPos);
            TileBase t_tile = pTileMap.GetTile(worldToCellPos);
            
            if (t_tile != null)
            {
                switch (i)
                {
                    //위쪽 충돌
                    case 0:
                        //충돌하였다면..
                        if (originTPos.y + originRtYLenHalf> cellToWorldPos.y)
                        {
                            pObj.notUpBlocked = false;
                            pObj.isUpBlocked = true;
                            pObj.originObj.transform.position = new Vector3(
                                originTPos.x,
                                cellToWorldPos.y - originRtYLenHalf,
                                originTPos.z
                                );
                        }
                        break;
                    //오른쪽 위 충돌
                    case 1:
                        //충돌하였다면..
                        if (originTPos.x + originRtXLenHalf > cellToWorldPos.x - 1 &&
                           originTPos.y + originRtYLenHalf > cellToWorldPos.y)
                        {
                            float distX = Mathf.Abs((originTPos.x + originRtXLenHalf) -
                                cellToWorldPos.x);
                            float distY = Mathf.Abs((originTPos.y + originRtYLenHalf) -
                                cellToWorldPos.y);

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                pObj.notUpBlocked = false;
                                pObj.isUpBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originTPos.x,
                                    cellToWorldPos.y - originRtYLenHalf,
                                    originTPos.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                pObj.notRightBlocked = false;
                                pObj.isRightBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                cellToWorldPos.x - originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                            }
                        }
                        break;
                    //오른쪽 충돌
                    case 2:                        
                        //충돌하였다면..
                        if (originTPos.x + originRtXLenHalf > cellToWorldPos.x-1)
                        {
                            pObj.notRightBlocked = false;
                            pObj.isRightBlocked = true;
                            pObj.originObj.transform.position = new Vector3(
                                cellToWorldPos.x - originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                        }

                        Vector3 checkUpRightTile = new Vector3(pObj.originObj.transform.position.x,
                            pObj.originObj.transform.position.y + tileSize,
                            pObj.originObj.transform.position.z);
                        break;
                    //오른쪽 아래 충돌
                    case 3:
                        //충돌하였다면..
                        if (originTPos.x + originRtXLenHalf > cellToWorldPos.x-1 &&
                           originTPos.y - originRtYLenHalf < (cellToWorldPos.y + tileSize) + 1)
                        {
                            float distX = Mathf.Abs((originTPos.x + originRtXLenHalf) -
                                cellToWorldPos.x);
                            float distY = Mathf.Abs((originTPos.y - originRtYLenHalf) -
                               (cellToWorldPos.y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                pObj.notGrounded = false;
                                pObj.SetIsGrounded(true);
                                pObj.originObj.transform.position = new Vector3(
                                    originTPos.x,
                                    (cellToWorldPos.y + tileSize) + originRtYLenHalf,
                                    originTPos.z
                                    );
                            }
                            //세로 면적이 크다면 왼쪽으로
                            else
                            {
                                pObj.notRightBlocked = false;
                                pObj.isRightBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                  cellToWorldPos.x - originRtXLenHalf,
                                  originTPos.y,
                                  originTPos.z
                                  );
                            }
                        }
                        else if (originTPos.y - originRtYLenHalf < (cellToWorldPos.y + tileSize) + 1)
                        {
                            pObj.notGrounded = false;
                        }
                        break;
                    //아래쪽 충돌
                    case 4:
                        //충돌하였다면..
                        if (originTPos.y - originRtYLenHalf < (cellToWorldPos.y + tileSize))
                        {
                            pObj.notGrounded = false;
                            pObj.SetIsGrounded(true);
                            pObj.originObj.transform.position = new Vector3(
                                originTPos.x,
                                (cellToWorldPos.y + tileSize) + originRtYLenHalf,
                                originTPos.z
                                );
                        }
                        else if (originTPos.y - originRtYLenHalf < (cellToWorldPos.y + tileSize) + 1)
                        {
                            pObj.notGrounded = false;
                        }

                        break;
                    //왼쪽 아래 충돌
                    case 5:
                        //충돌하였다면..
                        if (originTPos.x - originRtXLenHalf < (cellToWorldPos.x + tileSize) - 1 &&
                           originTPos.y - originRtYLenHalf < (cellToWorldPos.y + tileSize) - 1)
                        {
                            float distX = Mathf.Abs((originTPos.x - originRtXLenHalf) -
                                (cellToWorldPos.x + tileSize));
                            float distY = Mathf.Abs((originTPos.y - originRtYLenHalf) -
                               (cellToWorldPos.y + tileSize));

                            //가로 면적이 크다면 위로
                            if (distX > distY)
                            {
                                pObj.notGrounded = false;
                                pObj.SetIsGrounded(true);
                                pObj.originObj.transform.position = new Vector3(
                                    originTPos.x,
                                    (cellToWorldPos.y + tileSize) + originRtYLenHalf,
                                    originTPos.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                pObj.notLeftBlocked = false;
                                pObj.isLeftBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                (cellToWorldPos.x + tileSize) + originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                            }
                        }
                        else if (originTPos.y - originRtYLenHalf < (cellToWorldPos.y + tileSize) + 1)
                        {
                            pObj.notGrounded = false;
                        }
                        if (originTPos.x - originRtXLenHalf < (cellToWorldPos.x + tileSize) + 1)
                        {
                            pObj.notLeftBlocked = false;
                        }
                        break;
                    //왼쪽 충돌
                    case 6:
                        //충돌하였다면..
                        if (originTPos.x - originRtXLenHalf < (cellToWorldPos.x + tileSize)+1)
                        {
                            pObj.notLeftBlocked = false;
                            pObj.isLeftBlocked = true;
                            pObj.originObj.transform.position = new Vector3(
                                (cellToWorldPos.x +tileSize) + originRtXLenHalf,
                                originTPos.y,
                                originTPos.z
                                );
                        }
                        if (originTPos.x - originRtXLenHalf < (cellToWorldPos.x + tileSize) - 1)
                        {
                            pObj.notLeftBlocked = false;
                        }
                        break;
                    //왼쪽 위 충돌
                    case 7:
                        //충돌하였다면..
                        if (originTPos.x - originRtXLenHalf < (cellToWorldPos.x + tileSize)+1 &&
                           originTPos.y + originRtYLenHalf > cellToWorldPos.y)
                        {
                            float distX = Mathf.Abs((originTPos.x - originRtXLenHalf) -
                                (cellToWorldPos.x + tileSize));
                            float distY = Mathf.Abs((originTPos.y + originRtYLenHalf) -
                               (cellToWorldPos.y));

                            //가로 면적이 크다면 아래로
                            if (distX > distY)
                            {
                                pObj.notUpBlocked = false;
                                pObj.isUpBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                    originTPos.x,
                                    cellToWorldPos.y - originRtYLenHalf,
                                    originTPos.z
                                    );
                            }
                            //세로 면적이 크다면 오른쪽으로
                            else
                            {
                                pObj.notLeftBlocked = false;
                                pObj.isLeftBlocked = true;
                                pObj.originObj.transform.position = new Vector3(
                                (cellToWorldPos.x + tileSize) + originRtXLenHalf,
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

            Tile tile = new Tile(localPlace, 
                tileMap_canHit.GetTile(localPlace), 
                new cProperty("MaxHp", 10),
                new cProperty("CurHp", 10));

            dic_canHit.Add(tileMap_canHit.CellToWorld(localPlace), tile);
        }
               
        cUtil._tileMng = this;
    }

    private bool UpdateAttackedTile(Vector3 pCurPos, cProperty pDamage, out float pCurHpPercent)
    {
        bool isChecked = false;
        Vector3Int worldToCellPos = tileMap_canHit.WorldToCell(pCurPos);
        Vector3 cellToWorldPos = tileMap_canHit.CellToWorld(worldToCellPos);
        if (!dic_canHit.ContainsKey(cellToWorldPos))
        {
            pCurHpPercent = 0;
            return false;
        }
        if (dic_canHit[cellToWorldPos].curHp.value <= 0)
        {
            pCurHpPercent = 0;
            return false;
        }

        Tile tempTileToUse;
        tempTileToUse = dic_canHit[cellToWorldPos];
        tempTileToUse.curHp.value -= pDamage.value;
        dic_canHit[cellToWorldPos] = tempTileToUse;
        pCurHpPercent = (float)tempTileToUse.curHp.value / (float)dic_canHit[cellToWorldPos].maxHp.value;
        
        if (tempTileToUse.curHp.value == 0)
        {
            isChecked = true;
            pCurHpPercent = 0;
            tileMap_canHit.SetTile(worldToCellPos, null);

            //황금 타일
            if(dic_canHit[cellToWorldPos].tileBase.name.Contains("img_tile3").Equals(true))
            {
                PlayDestroyEffect(cellToWorldPos, 1);
            }
            //일반 타일
            else
                PlayDestroyEffect(cellToWorldPos, 0);

        }
        else if (tempTileToUse.curHp.value <= 0)
        {
            tempTileToUse.curHp.value = 0;
            pCurHpPercent = 0;
            isChecked = true;
            tileMap_canHit.SetTile(worldToCellPos, null);
            //황금 타일
            if (dic_canHit[cellToWorldPos].tileBase.name.Contains("img_tile3").Equals(true))
            {
                PlayDestroyEffect(cellToWorldPos, 1);
            }
            //일반 타일
            else
                PlayDestroyEffect(cellToWorldPos, 0);
        }
        else
            isChecked = true;

        return isChecked;
    }

    public bool isTileExist(int pDir, Vector3 pCurPos)
    {
        Vector3Int calcPos = Vector3Int.zero;
        Vector3Int calcPos2 = Vector3Int.zero;

        bool t = false;
        Vector3Int worldToCellPos = tileMap_canHit.WorldToCell(pCurPos);
        Vector3Int worldToCellPos2 = tileMap_cannotHit.WorldToCell(pCurPos);

        switch (pDir)
        {
            //위
            case 0:
                calcPos = new Vector3Int(worldToCellPos.x, worldToCellPos.y + 1, 0);
                calcPos2 = new Vector3Int(worldToCellPos2.x, worldToCellPos2.y + 1, 0);
                break;
                //오른
            case 1:
                calcPos = new Vector3Int(worldToCellPos.x + 1, worldToCellPos.y, 0);
                calcPos2 = new Vector3Int(worldToCellPos2.x + 1, worldToCellPos2.y, 0);
                break;
                //아래
            case 2:
                calcPos = new Vector3Int(worldToCellPos.x, worldToCellPos.y - 1, 0);
                calcPos2 = new Vector3Int(worldToCellPos2.x, worldToCellPos2.y - 1, 0);
                break;
                //왼
            case 3:
                calcPos = new Vector3Int(worldToCellPos.x - 1, worldToCellPos.y, 0);
                calcPos2 = new Vector3Int(worldToCellPos2.x - 1, worldToCellPos2.y, 0);
                break;
        }

        TileBase t_tile1 = tileMap_canHit.GetTile(calcPos);
        TileBase t_tile2 = tileMap_cannotHit.GetTile(calcPos2);

        Debug.Log(t_tile1 + "    " + t_tile2);

        if (t_tile1 != null)
            t = true;
        if (t_tile2 != null)
            t = true;

        return t;
    }

    private void PlayDestroyEffect(Vector3 pPos, byte pChar)
    {
        switch(pChar)
        {
            //일반 타일
            case 0:
                for (byte i = 0; i < effect_destroy.transform.GetChild(0).childCount; i++)
                {
                    if (effect_destroy.transform.GetChild(0).GetChild(i).GetComponent<ParticleSystem>().isPlaying.Equals(false))
                    {
                        effect_destroy.transform.GetChild(0).GetChild(i).position = new Vector3(pPos.x + tileSize / 2, pPos.y + tileSize / 2, pPos.z);
                        effect_destroy.transform.GetChild(0).GetChild(i).GetComponent<ParticleSystem>().Play();
                        break;
                    }
                }
                break;

            //황금 타일
            case 1:
                for (byte i = 0; i < effect_destroy.transform.GetChild(1).childCount; i++)
                {
                    if (effect_destroy.transform.GetChild(1).GetChild(i).GetComponent<ParticleSystem>().isPlaying.Equals(false))
                    {
                        effect_destroy.transform.GetChild(1).GetChild(i).position = new Vector3(pPos.x + tileSize / 2, pPos.y + tileSize / 2, pPos.z);
                        effect_destroy.transform.GetChild(1).GetChild(i).GetComponent<ParticleSystem>().Play();

                        //랜덤으로 아이템 추가
                        byte randomItem = (byte)Random.Range(0, (int)citemTable.GetUseItemTotalNum());
                        cUtil._user._playerInfo.inventory.AddItem(randomItem);

                        break;
                    }
                }
                break;

        }
    }
}
