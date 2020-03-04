using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cSkinCamObj : MonoBehaviour
{
    public GameObject obj_follow;
    public float z;

    void Update()
    {
        this.transform.position = new Vector3(obj_follow.transform.position.x + 150, obj_follow.transform.position.y + 50, this.transform.position.z);
    }
}
