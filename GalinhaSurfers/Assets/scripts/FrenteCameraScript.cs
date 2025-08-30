using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenteCameraScript : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
