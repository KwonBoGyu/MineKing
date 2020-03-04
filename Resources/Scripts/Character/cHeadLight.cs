using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cHeadLight : MonoBehaviour
{
    public Transform HeadLight;

    public void SetLightRange(float pValue)
    {
        this.GetComponent<Light>().range = 900 + pValue;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(HeadLight.position.x, HeadLight.position.y, -121);
    }
}
