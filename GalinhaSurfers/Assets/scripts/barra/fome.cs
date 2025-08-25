using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fome : MonoBehaviour
{
    [Header("Barra Fome")]
    public RectTransform barraFome;
    public float valorAtualFome;
    public float valorMaxFome;
    public float velFome; // velocidade normal de diminui��o da fome

    [Header("Barra Pimenta")]
    public RectTransform barraPimenta;
    public float duracaoPimenta;
    [Range(0f, 1f)]
    public float tamanhoPimenta = 0.25f; // 25%
    private float valorMaxPimenta;
    private float valorAtualPimenta;
    private bool pimentaAtiva;
    private float valorMaxFomeAntesPimenta;

    [Header("Cookie")]
    public float duracaoCookie;
    private bool cookieAtivo;
    private float tempoRestanteCookie;
    private float alturaInicialFome;
    private float valorMaxInicialFome;

    void Start()
    {
        // Fome
        alturaInicialFome = barraFome.sizeDelta.y;
        valorMaxInicialFome = alturaInicialFome;
        valorMaxFome = valorMaxInicialFome;
        valorAtualFome = valorMaxFome;

        // Pimenta
        barraPimenta.gameObject.SetActive(false);
        pimentaAtiva = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) AtivarPimenta();
        if (Input.GetKeyDown(KeyCode.C)) AtivarCookie();

        AtualizarFome();
        AtualizarPimenta();
        AtualizarCookie();

        if (pimentaAtiva) AtualizarPosicaoPimenta();
    }

    void AtualizarFome()
    {
        // Redu��o normal da fome
        valorAtualFome -= velFome * Time.deltaTime;
        valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);

        // Atualiza altura da barra de fome
        float alturaMaximaFome = alturaInicialFome * (valorMaxFome / valorMaxInicialFome);
        float porcentagemFome = valorAtualFome / valorMaxFome;
        barraFome.sizeDelta = new Vector2(barraFome.sizeDelta.x, alturaMaximaFome * porcentagemFome);
    }

    void AtualizarPimenta()
    {
        if (!pimentaAtiva) return;

        // Redu��o da pimenta
        float velPimenta = valorMaxPimenta / duracaoPimenta;
        valorAtualPimenta -= velPimenta * Time.deltaTime;
        valorAtualPimenta = Mathf.Clamp(valorAtualPimenta, 0, valorMaxPimenta);

        // Atualiza altura da barra da pimenta
        float alturaPimenta = alturaInicialFome * (valorMaxPimenta / valorMaxInicialFome);
        float porcentagemPimenta = valorAtualPimenta / valorMaxPimenta;
        barraPimenta.sizeDelta = new Vector2(barraPimenta.sizeDelta.x, alturaPimenta * porcentagemPimenta);

        Debug.Log(
            $"[DEBUG PIMENTA] Max: {valorMaxPimenta}, Atual: {valorAtualPimenta}, " +
            $"AlturaBase: {alturaPimenta}, Porcentagem: {porcentagemPimenta}, " +
            $"SizeDeltaY: {barraPimenta.sizeDelta.y}"
        );

        if (valorAtualPimenta <= 0f)
        {
            pimentaAtiva = false;
            barraPimenta.gameObject.SetActive(false);
            valorMaxFome = valorMaxFomeAntesPimenta; // Reset maxFome ap�s pimenta
        }
    }

    void AtualizarPosicaoPimenta()
    {
        // Agora usa o valorMaxFomeAtualizadoAntesPimenta
        // para recalcular sempre que o maxFome mudar (ex: fruta)
        float posY = (valorMaxFomeAntesPimenta - 500f) * alturaInicialFome / 1000f;
        Vector3 posPimenta = barraPimenta.localPosition;
        posPimenta.y = posY;
        barraPimenta.localPosition = posPimenta;
    }

    void AtualizarCookie()
    {
        if (!cookieAtivo) return;

        valorAtualFome += 150f * Time.deltaTime;

        tempoRestanteCookie -= Time.deltaTime;
        if (tempoRestanteCookie <= 0f)
        {
            cookieAtivo = false;
        }
    }

    public void AtivarPimenta()
    {
        pimentaAtiva = true;
        barraPimenta.gameObject.SetActive(true);

        // Salva o maxFome antes da redu��o da pimenta
        valorMaxFomeAntesPimenta = valorMaxFome;

        // Calcula valor da pimenta (25% do maxFome atual antes de reduzir)
        valorMaxPimenta = valorMaxFomeAntesPimenta * tamanhoPimenta;
        valorAtualPimenta = valorMaxPimenta;

        // Aplica redu��o no maxFome sem afetar o Y da barra da pimenta
        valorMaxFome -= valorMaxPimenta;

        // Define altura inicial da barra proporcional ao valor da pimenta
        float alturaPimenta = alturaInicialFome * (valorMaxPimenta / valorMaxInicialFome);
        barraPimenta.sizeDelta = new Vector2(barraPimenta.sizeDelta.x, alturaPimenta);

        // Calcula posi��o inicial
        AtualizarPosicaoPimenta();
    }

    public void AtivarCookie()
    {
        cookieAtivo = true;
        tempoRestanteCookie = duracaoCookie;
    }

    public void AdicionarFome(float quantidade)
    {
        valorAtualFome += quantidade;
        valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);
    }

    public void AlterarMaxFome(float quantidade)
    {
        if (pimentaAtiva)
        {
            // Se a pimenta est� ativa, ajustamos o valor base (antes da redu��o da pimenta)
            valorMaxFomeAntesPimenta += quantidade;
            valorMaxFomeAntesPimenta = Mathf.Clamp(valorMaxFomeAntesPimenta, 0, 1000);

            // Recalcula o valor vis�vel com a pimenta aplicada
            valorMaxPimenta = valorMaxFomeAntesPimenta * tamanhoPimenta;
            valorMaxFome = valorMaxFomeAntesPimenta - valorMaxPimenta;

            // Garante que a fome atual n�o passe do limite
            if (valorAtualFome > valorMaxFome)
                valorAtualFome = valorMaxFome;

            // Atualiza a posi��o da barra de pimenta (porque o Y depende do valor base)
            AtualizarPosicaoPimenta();
        }
        else
        {
            // Caso n�o esteja com pimenta, funciona normal
            valorMaxFome += quantidade;
            valorMaxFome = Mathf.Clamp(valorMaxFome, 0, 1000);

            if (valorAtualFome > valorMaxFome)
                valorAtualFome = valorMaxFome;
        }
    }
}
