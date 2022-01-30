using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenShadowEffect : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public float speed;

    private float x;
    private float y;
    private bool right;
    private bool up;
    private bool xGoing;

    private void Update()
    {
        if(xGoing)
            x = Mathf.Clamp(right ? x + (Time.deltaTime * speed) : x - (Time.deltaTime * speed), -1, 1);
        else
            y = Mathf.Clamp(up ? y + (Time.deltaTime * speed) : y - (Time.deltaTime * speed), -1, 1);
        if (x == 1 || x == -1) 
        { 
            right = x == -1;
            xGoing = false;
            x = Mathf.Clamp(right ? x + (Time.deltaTime * speed) : x - (Time.deltaTime * speed), -1, 1);
        }
        if (y == 1 || y == -1) 
        {
            up = y == -1;
            xGoing = true;
            y = Mathf.Clamp(up ? y + (Time.deltaTime * speed) : y - (Time.deltaTime * speed), -1, 1);
        }
        text.fontMaterial.SetFloat("_UnderlayOffsetX", x);
        text.fontMaterial.SetFloat("_UnderlayOffsetY", y);
    }
}
