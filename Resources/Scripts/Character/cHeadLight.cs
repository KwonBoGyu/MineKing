using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cHeadLight : MonoBehaviour
{
    public Transform HeadLight;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(HeadLight.position.x, HeadLight.position.y, -121);
    }
}
