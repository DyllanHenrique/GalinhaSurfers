using System.Collections;
using System.Collections.Generic;
using Autodesk.Fbx;
using UnityEngine;

public class Pulsamento : MonoBehaviour
{
    public RectTransform imageTrans;
    public float pulsevelo = 2f;
    public float pulseAMETRONTADOR = 0.1f;

    private Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        if (imageTrans == null) imageTrans = GetComponent<RectTransform>();
        originalScale = transform.localScale;
        //Fala ai 
    }

    // Update is called once per frame
    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulsevelo) * pulseAMETRONTADOR;
        imageTrans.localScale = originalScale * scale;
    }
}
