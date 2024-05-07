using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();

        Rect rt = cam.rect;

        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 19.5f);
        float scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1)
        {
            rt.height = scaleHeight;
            rt.y = (1f - scaleHeight) / 2f;
        }    
        else
        {
            rt.width = scaleWidth;
            rt.x = (1f - scaleWidth) / 2f;
        }
        cam.rect = rt;
    }
}
