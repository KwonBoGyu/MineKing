using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cObject : MonoBehaviour
{
    public GameObject originObj;
    public BoxCollider2D rt;
    public cTileMng tileMng;

    public bool notUpBlocked;
    public bool notGrounded;
    public bool notLeftBlocked;
    public bool notRightBlocked;

    public bool isGrounded;
    public virtual bool GetIsGrounded() { return isGrounded; }
    public virtual void SetIsGrounded(bool pGrounded)
    {
        if (isGrounded.Equals(pGrounded))
            return;

        isGrounded = pGrounded;
    }
    public bool isRightBlocked;
    public bool isLeftBlocked;
    public bool isUpBlocked;

    private void Start()
    {
        rt = originObj.GetComponent<BoxCollider2D>();
    }
    protected virtual void FixedUpdate()
    {
        if(cUtil._tileMng != null && tileMng == null)
        {
            tileMng = cUtil._tileMng;
        }
    }
}
