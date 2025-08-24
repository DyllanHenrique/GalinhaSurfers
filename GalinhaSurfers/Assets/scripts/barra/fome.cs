using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fome : MonoBehaviour
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

    void Update()
    {
        valorAtual -= velfome * Time.deltaTime;
        valorAtual = Mathf.Clamp(valorAtual, 0, valorMax);

        float alturaMaximaAtual = alturaInicial * (valorMax / valorMaxInicial);
        float porcentagem = valorAtual / valorMax;
        float novaAltura = alturaMaximaAtual * porcentagem;
        barra.sizeDelta = new Vector2(
            barra.sizeDelta.x,
            novaAltura
        );
    }
}
