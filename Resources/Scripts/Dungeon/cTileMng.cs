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
        public cProperty level { get; set; }
        public cProperty maxHp { get; set; }
        public cProperty curHp { get; set; }
        public cProperty rocks { get; set; }

        public Tile(Vector3Int pLocation, TileBase pTileBase, cProperty pLevel, cProperty pMaxHp, cProperty pCurHp, cProperty pRocks)
        {
            location = pLocation;
            tileBase = pTileBase;
            level = new cProperty(pLevel);
            maxHp = new cProperty(pMaxHp);
            curHp = new cProperty(pCurHp);
            rocks = new cProperty(pRocks);
        }
    }

    public Color HitColor;

    public int tileSize;
    public int numX;
    public int numY;
    public Dictionary<Vector3, Tile> dic_canHit;
    private Tilemap tileMap_cannotHit;
    private Tilemap tileMap_canHit;
    Vector3Int[] cellPos;

    Tile? tempTile;
    public GameObject effect_destroy;

    public GameObject obj_itemDrop;
    public cSoundMng soundMng;

    private void Start()
    {
        tileMap_canHit = this.transform.GetChild(0).GetComponent<Tilemap>();
        tileMap_cannotHit = this.transform.GetChild(1).GetComponent<Tilemap>();
        tempTile = null;
        obj_itemDrop = GameObject.Find("DungeonNormalScene").transform.Find("Canvas_main").Find("ItemDrop").gameObject;
        soundMng = GameObject.Find("DungeonNormalScene").transform.Find("Cam_main").GetComponent<cSoundMng>();

        SetEntireTiles();
    }

    public bool CheckAttackedTile(Vector3 pWorldPos, long pDamage, out float pCurHpPercent)
    {
        bool isChecked = false;
        Vector3Int worldToCellPos = tileMap_canHit.WorldToCell(pWorldPos);
        Vector3 convertedWorldPos = tileMap_canHit.CellToWorld(worldToCellPos);

        //해당 위치에 타일이 있다면
        isChecked = UpdateAttackedTile(convertedWorldPos, pDamage, out pCurHpPercent);

        return isChecked;
    }

    public void CheckCanGroundTile(cObject pObj)
    {
        pObj.notUpBlocked = true;
        pObj.notGrounded = true;
        pObj.notRightBlocked = true;
        pObj.notLeftBlocked = true;

        CheckTiles(tileMap_canHit, pObj, pObj.rt.size);
        CheckTiles(tileMap_cannotHit, pObj, pObj.rt.size);

        if (pObj.notUpBlocked)
            pObj.isUpBlocked = false;
        if (pObj.notGrounded)
            pObj.SetIsGrounded(false);
        if (pObj.notLeftBlocked)
            pObj.isLeftBlocked = false;
        if (pObj.notRightBlocked)
            pObj.isRightBlocked = false;
    }

    public void CheckCanGroundTile(cObject pObj, out bool isChecked)
    {
        isChecked = false;

        pObj.notUpBlocked = true;
        pObj.notGrounded = true;
        pObj.notRightBlocked = true;
        pObj.notLeftBlocked = true;

        CheckTiles(tileMap_canHit, pObj, pObj.rt.size);
        CheckTiles(tileMap_cannotHit, pObj, pObj.rt.size);

        if (pObj.notUpBlocked)
            pObj.isUpBlocked = false;
        else
            isChecked = true;
        if (pObj.notGrounded)
            pObj.SetIsGrounded(false);
        else
            isChecked = true;
        if (pObj.notLeftBlocked)
            pObj.isLeftBlocked = false;
        else
            isChecked = true;
        if (pObj.notRightBlocked)
            pObj.isRightBlocked = false;
        else
            isChecked = true;
    }

    private void CheckTiles(Tilemap pTileMap, cObject pObj, Vector2 pSize)
    {
        Vector3 originTPos = pObj.originObj.transform.position;
        float originRtXLenHalf = pObj.rt.size.x * 0.5f;
        float originRtYLenHalf = pObj.rt.size.y * 0.5f;

        cellPos = new Vector3Int[]
            {
                new Vector3Int((int)originTPos.x, (int)originTPos.y + (int)(pSize.y * 0.7f), 0),
                new Vector3Int((int)originTPos.x + (int)(pSize.x * 0.4f), (int)originTPos.y + (int)(pSize.y * 0.7f), 0),
                new Vector3Int((int)originTPos.x + (int)(pSize.x * 0.8f), (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x + (int)(pSize.x * 0.4f), (int)originTPos.y - (int)(pSize.y * 0.7f), 0),
                new Vector3Int((int)originTPos.x, (int)originTPos.y - (int)(pSize.y * 0.7f), 0),
                new Vector3Int((int)originTPos.x - (int)(pSize.x * 0.4f), (int)originTPos.y - (int)(pSize.y * 0.7f), 0),
                new Vector3Int((int)originTPos.x - (int)(pSize.x * 0.8f), (int)originTPos.y, 0),
                new Vector3Int((int)originTPos.x - (int)(pSize.x * 0.4f), (int)originTPos.y + (int)(pSize.y * 0.7f), 0),
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
                        if (originTPos.y + originRtYLenHalf > cellToWorldPos.y)
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
                        if (originTPos.x + originRtXLenHalf > cellToWorldPos.x - 1)
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
                        if (originTPos.x + originRtXLenHalf > cellToWorldPos.x - 1 &&
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
                        if (originTPos.x - originRtXLenHalf < (cellToWorldPos.x + tileSize) + 1)
                        {
                            pObj.notLeftBlocked = false;
                            pObj.isLeftBlocked = true;
                            pObj.originObj.transform.position = new Vector3(
                                (cellToWorldPos.x + tileSize) + originRtXLenHalf,
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
                        if (originTPos.x - originRtXLenHalf < (cellToWorldPos.x + tileSize) + 1 &&
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
                new cProperty("Level", 1),
                new cProperty("MaxHp", 10),
                new cProperty("CurHp", 10),
                new cProperty("Rocks", 1));

            dic_canHit.Add(tileMap_canHit.CellToWorld(localPlace), tile);
        }

        cUtil._tileMng = this;
    }

    private bool UpdateAttackedTile(Vector3 pCurPos, long pDamage, out float pCurHpPercent)
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
        tempTileToUse.curHp.value -= pDamage;
        dic_canHit[cellToWorldPos] = tempTileToUse;
        pCurHpPercent = (float)tempTileToUse.curHp.value / (float)dic_canHit[cellToWorldPos].maxHp.value;

        if (tempTileToUse.curHp.value == 0)
        {
            isChecked = true;
            pCurHpPercent = 0;
            tileMap_canHit.SetTile(worldToCellPos, null);
            cUtil._player.RootRocks(tempTileToUse.rocks.value);
            soundMng.playTileEffect();

            //금광 타일
            if (dic_canHit[cellToWorldPos].tileBase.name.Contains("img_tile3"))
            {
                PlayTileEffect(1, new Vector3(cellToWorldPos.x + tileSize / 2,
       cellToWorldPos.y + tileSize / 2, cellToWorldPos.z));

                //아이템 드롭
                byte itemNum = (byte)Random.Range(0, citemTable.GetUseItemTotalNum());

                for (byte k = 0; k < obj_itemDrop.transform.GetChild(itemNum).childCount; k++)
                {
                    if (obj_itemDrop.transform.GetChild(itemNum).GetChild(k).gameObject.activeSelf.Equals(false))
                    {
                        obj_itemDrop.transform.GetChild(itemNum).GetChild(k).transform.position = new Vector3(cellToWorldPos.x + tileSize / 2,
   cellToWorldPos.y + tileSize / 2, cellToWorldPos.z);
                        obj_itemDrop.transform.GetChild(itemNum).GetChild(k).gameObject.SetActive(true);
                        obj_itemDrop.transform.GetChild(itemNum).GetChild(k).GetComponent<cItemDrop>().Init();
                        obj_itemDrop.transform.GetChild(itemNum).GetChild(k).GetComponent<cItemDrop>().itemId = itemNum;
                        break;
                    }
                }

            }
            //노말 타일
            else
            {
                PlayTileEffect(0, new Vector3(cellToWorldPos.x + tileSize / 2,
    cellToWorldPos.y + tileSize / 2, cellToWorldPos.z));
            }
        }
        else if (tempTileToUse.curHp.value <= 0)
        {
            tempTileToUse.curHp.value = 0;
            pCurHpPercent = 0;
            isChecked = true;
            tileMap_canHit.SetTile(worldToCellPos, null);
            cUtil._player.RootRocks(tempTileToUse.rocks.value);
            soundMng.playTileEffect();

            //금광 타일
            if (dic_canHit[cellToWorldPos].tileBase.name.Contains("img_tile3"))
            {
                PlayTileEffect(1, new Vector3(cellToWorldPos.x + tileSize / 2,
       cellToWorldPos.y + tileSize / 2, cellToWorldPos.z));

                //아이템 드롭
                byte itemNum = (byte)Random.Range(0, citemTable.GetUseItemTotalNum());
                
                for (byte k = 0; k < obj_itemDrop.transform.GetChild(itemNum).childCount; k++)
                {
                    if (obj_itemDrop.transform.GetChild(itemNum).GetChild(k).gameObject.activeSelf.Equals(false))
                    {
                        obj_itemDrop.transform.GetChild(itemNum).GetChild(k).transform.position = new Vector3(cellToWorldPos.x + tileSize / 2,
   cellToWorldPos.y + tileSize / 2, cellToWorldPos.z);
                        obj_itemDrop.transform.GetChild(itemNum).GetChild(k).gameObject.SetActive(true);
                        obj_itemDrop.transform.GetChild(itemNum).GetChild(k).GetComponent<cItemDrop>().Init();
                        obj_itemDrop.transform.GetChild(itemNum).GetChild(k).GetComponent<cItemDrop>().itemId = itemNum;
                        break;
                    }
                }

            }                
            //노말 타일
            else
            {
                PlayTileEffect(0, new Vector3(cellToWorldPos.x + tileSize / 2,
    cellToWorldPos.y + tileSize / 2, cellToWorldPos.z));
            }
                
        }
        else
        {
            StartCoroutine(TileReaction(tileMap_canHit, worldToCellPos));
            isChecked = true;
        }

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

    private void PlayTileEffect(byte pChar, Vector3 pPos)
    {
        for (byte i = 0; i < effect_destroy.transform.GetChild(pChar).childCount; i++)
        {
            if (effect_destroy.transform.GetChild(pChar).GetChild(i).GetComponent<ParticleSystem>().isPlaying.Equals(true))
                continue;

            effect_destroy.transform.GetChild(pChar).GetChild(i).position = pPos;
            effect_destroy.transform.GetChild(pChar).GetChild(i).GetComponent<ParticleSystem>().Play();
            break;
        }
    }

    private IEnumerator TileReaction(Tilemap pTilemap, Vector3Int pPos)
    {
        float timer = 0;
        float MaxTime = 1;

        pTilemap.SetTileFlags(pPos, TileFlags.None);
        pTilemap.SetColor(pPos, HitColor);
        Color tC = new Color(1 - HitColor.r, 1 - HitColor.g, 1 - HitColor.b, 1);

        while (true)
        {
            yield return new WaitForFixedUpdate();

            if(timer > MaxTime)
            {
                timer = 0;
                pTilemap.SetColor(pPos, Color.white);
                break;
            }
            pTilemap.SetColor(pPos, new Color(HitColor.r + tC.r * timer, HitColor.g + tC.g * timer, HitColor.b + tC.b * timer, 1));

            timer += Time.deltaTime * 3;
        }
    }
}
