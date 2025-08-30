using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ScreenCapture.CaptureScreenshot("print.png");
        }
    }
}
