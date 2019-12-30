using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cTitle_btn : MonoBehaviour
{
    public Image _img;
    private float alpha;
    private bool isTransparent;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => cUtil._sm.ChangeScene("Main"));
    }

    private void FixedUpdate()
    {
        if(isTransparent == false)
        {
            alpha -= 0.03f;

            _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, alpha);
            if(alpha <= 0.0f)
            {
                alpha = 0;
                isTransparent = true;
            }
        }
        else if(isTransparent == true)
        {
            alpha += 0.03f;

            _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, alpha);
            if (alpha >= 1.0f)
            {
                alpha = 1.0f;
                isTransparent = false;
            }
        }
    }

}
