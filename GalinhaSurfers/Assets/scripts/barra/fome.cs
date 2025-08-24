using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fome : MonoBehaviour
{
    [Header("Barra Fome")]
    public RectTransform barraFome;
    public float valorAtualFome;
    public float valorMaxFome;
    public float velFome; 

    [Header("Barra Pimenta")]
    public RectTransform barraPimenta;
    public float valorAtualPimenta;
    public float valorMaxPimenta;
    public float duracaoPimenta;

    private float alturaInicialFome;
    private float alturaInicialPimenta;
    private float valorMaxInicialFome;

    private bool pimentaAtiva;

    void Start()
    {
        // Fome
        alturaInicialFome = barraFome.sizeDelta.y;
        valorMaxInicialFome = alturaInicialFome;
        valorMaxFome = valorMaxInicialFome;
        valorAtualFome = valorMaxFome;

        // Pimenta
        alturaInicialPimenta = barraPimenta.sizeDelta.y;
        valorMaxPimenta = alturaInicialPimenta;
        valorAtualPimenta = valorMaxPimenta;

        pimentaAtiva = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AtivarPimenta();
        }
        //Pimenta
        if (pimentaAtiva)
        {
            float velPimenta = valorMaxPimenta / duracaoPimenta;
            valorAtualPimenta -= velPimenta * Time.deltaTime;
            valorAtualPimenta = Mathf.Clamp(valorAtualPimenta, 0, valorMaxPimenta);

            float porcentagemPimenta = valorAtualPimenta / valorMaxPimenta;

            barraPimenta.sizeDelta = new Vector2(
                barraPimenta.sizeDelta.x,
                alturaInicialPimenta * porcentagemPimenta
            );

            float novoMaxFome = valorMaxInicialFome - valorMaxPimenta;

            if (valorMaxFome != novoMaxFome)
                valorMaxFome = novoMaxFome;

            if (valorAtualFome > valorMaxFome)
                valorAtualFome = valorMaxFome;

            if (valorAtualPimenta <= 0)
            {
                pimentaAtiva = false;
                barraPimenta.gameObject.SetActive(false);
                valorMaxFome = valorMaxInicialFome;
                if (valorAtualFome > valorMaxFome)
                    valorAtualFome = valorMaxFome;
            }
        }

        //Fome
        valorAtualFome -= velFome * Time.deltaTime;
        valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);

        float alturaMaximaFome = alturaInicialFome * (valorMaxFome / valorMaxInicialFome);
        float porcentagemFome = valorAtualFome / valorMaxFome;
        float novaAlturaFome = alturaMaximaFome * porcentagemFome;

        barraFome.sizeDelta = new Vector2(
            barraFome.sizeDelta.x,
            novaAlturaFome
        );
    }

    public void AtivarPimenta()
    {
        pimentaAtiva = true;
        barraPimenta.gameObject.SetActive(true);
        valorAtualPimenta = valorMaxPimenta;
    }
}
