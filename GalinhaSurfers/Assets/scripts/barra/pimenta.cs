using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pimenta : MonoBehaviour
{
    public RectTransform barra;
    public float valorAtual;
    public float valorMax;

    private float alturaInicial;
    private float valorMaxInicial;

    public float velfome;
    void Start()
    {
        alturaInicial = barra.sizeDelta.y;
        valorMaxInicial = alturaInicial;
        valorMax = valorMaxInicial;
        valorAtual = valorMax;
    }


}
