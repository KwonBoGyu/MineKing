using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cDEBUG : MonoBehaviour
{
    public cPlayer _p;
    public Text[] _t;
    public Text _dir;
    public Text _isGrounded;
    public Text _isLeftBlocked;
    public Text _isRightBlocked;

    void Update()
    {
        if (_p.GetStatus() == CHARACTERSTATUS.NONE)
            _t[0].color = new Color(1, 1, 1, 1);
        else
            _t[0].color = new Color(1, 1, 1, 0.5f);

        if (_p.GetStatus() == CHARACTERSTATUS.CROUCH)
            _t[1].color = new Color(1, 1, 1, 1);
        else
            _t[1].color = new Color(1, 1, 1, 0.5f);

        if (_p.GetStatus() == CHARACTERSTATUS.ATTACK)
            _t[2].color = new Color(1, 1, 1, 1);
        else
            _t[2].color = new Color(1, 1, 1, 0.5f);

        if (_p.GetDirection() == Vector3.left)
            _dir.text = "DIRECTION : LEFT";
        else if (_p.GetDirection() == Vector3.right)
            _dir.text = "DIRECTION : RIGHT";
        else if (_p.GetDirection() == Vector3.up)
            _dir.text = "DIRECTION : UP";
        else if (_p.GetDirection() == Vector3.down)
            _dir.text = "DIRECTION : DOWN";

        if (_p.GetIsGrounded() == true)
            _isGrounded.text = "GROUNDED : TRUE";
        else
            _isGrounded.text = "GROUNDED : FALSE";

        if (_p.isLeftBlocked.Equals(true))
            _isLeftBlocked.text = "ISLEFTBLOCKED : TRUE";
        else
            _isLeftBlocked.text = "ISLEFTBLOCKED : FALSE";

        if (_p.isRightBlocked.Equals(true))
            _isRightBlocked.text = "ISRIGHTBLOCKED : TRUE";
        else
            _isRightBlocked.text = "ISRIGHTBLOCKED : FALSE";
    }
}
