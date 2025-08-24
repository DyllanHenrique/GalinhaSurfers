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

    [Header("Barra Pimenta")]
    public float duracaoCookie;
    private bool cookieAtivo;
    private float tempoRestanteCookie;
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
            AtivarPimenta();
        if (Input.GetKeyDown(KeyCode.C))
            AtivarCookie();

        //Cookie
        if (cookieAtivo)
        {
            velFome = 0;
            valorAtualFome += 100f * Time.deltaTime;
            valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);

            float alturaMaximaF = alturaInicialFome * (valorMaxFome / valorMaxInicialFome);
            float porcentagemF = valorAtualFome / valorMaxFome;
            barraFome.sizeDelta = new Vector2(barraFome.sizeDelta.x, alturaMaximaF * porcentagemF);
            tempoRestanteCookie -= Time.deltaTime;
            if (tempoRestanteCookie <= 0f)
            {
                cookieAtivo = false;
                velFome = 50f;
            }
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
            valorMaxFome = novoMaxFome;

            if (valorAtualFome > valorMaxFome)
                valorAtualFome = valorMaxFome;

            if (valorAtualPimenta <= 0)
            {
                pimentaAtiva = false;
                barraPimenta.gameObject.SetActive(false);
                valorMaxFome = valorMaxInicialFome;
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
    public void AtivarCookie()
    {
        cookieAtivo = true;
        tempoRestanteCookie = duracaoCookie;
    }
}
