using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cFlagObject : MonoBehaviour
{
    public cTileMng tileMng;
    private float temp;

    public void SetColliBox()
    {
        int tileSize = tileMng.tileSize;

        Vector3 firstPos = new Vector3(this.transform.position.x - tileSize,
            this.transform.position.y + tileSize * 2, 0);

        //왼쪽위 -> 오른아래 순
        for(byte i = 0; i < 3; i++)
        {
            for(byte k = 0; k < 3; k++)
            {
                bool tt = tileMng.CheckAttackedTile(new Vector3(firstPos.x + k * tileSize, firstPos.y - i * tileSize, 0),
                     1000000, out temp);

                Debug.Log(tt);
            }
        }
    }
}
