using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class fome : MonoBehaviour
{
    [Header("Barra Fome")]
    public RectTransform barraFome;
    public float valorAtualFome;
    public float valorMaxFome;
    public float velFome; // velocidade normal de diminuição da fome [Header("Barra Pimenta")] 
    private float alturaInicialFome;
    private float valorMaxInicialFome;
    public RectTransform barraPimenta;
    public float duracaoPimenta; [Range(0f, 1f)]
    public float tamanhoPimenta; // 25% 
    private float valorMaxPimenta;
    private float valorAtualPimenta;
    private bool pimentaAtiva;
    private float valorMaxFomeAntesPimenta;
    private float debitoPimenta = 0f;
    [Header("Cookie")]
    public float duracaoCookie;
    private bool cookieAtivo;
    private float tempoRestanteCookie;
    [Header("CogumeloMal")]
    public float duracaoCM;
    private bool CMAtivo;
    private float tempoRestanteCM;
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
        if (Input.GetKeyDown(KeyCode.P))
            AtivarCogumeloMal();

        AtualizarFome();
        AtualizarPimenta();
        AtualizarCookie();
        AtualizarCM();
        if (pimentaAtiva) AtualizarPosicaoPimenta();
    }
    void AtualizarFome()
    {
        // Redução normal da fome
        valorAtualFome -= velFome * Time.deltaTime; valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);
        // Atualiza altura da barra de fome
        float alturaMaximaFome = alturaInicialFome * (valorMaxFome / valorMaxInicialFome);
        float porcentagemFome = valorAtualFome / valorMaxFome; barraFome.sizeDelta = new Vector2(barraFome.sizeDelta.x, alturaMaximaFome * porcentagemFome);
    }
    void AtualizarPimenta()
    {
        if (!pimentaAtiva) return;
        // Redução da pimenta
        float velPimenta = valorMaxPimenta / duracaoPimenta;
        valorAtualPimenta -= velPimenta * Time.deltaTime;
        valorAtualPimenta = Mathf.Clamp(valorAtualPimenta, 0, valorMaxPimenta);
        // Atualiza altura da barra da pimenta 
        float alturaPimenta = alturaInicialFome * (valorMaxPimenta / valorMaxInicialFome);
        float porcentagemPimenta = valorAtualPimenta / valorMaxPimenta;
        barraPimenta.sizeDelta = new Vector2(barraPimenta.sizeDelta.x, alturaPimenta * porcentagemPimenta);
        //Debug.Log($"[PIMENTA] Max: {valorMaxPimenta:F2} | Atual: {valorAtualPimenta:F2}");
        if (valorAtualPimenta <= 0f)
        {
            pimentaAtiva = false;
            barraPimenta.gameObject.SetActive(false);
            valorMaxFome = valorMaxFomeAntesPimenta;
            valorMaxFome += debitoPimenta * (1 / tamanhoPimenta);
            debitoPimenta = 0f;
            if (valorMaxFome >= 1000) valorMaxFome = 1000;
            if (valorMaxFome <= 0) valorMaxFome = 0;
        }
    }
    void AtualizarPosicaoPimenta()
    {
        float fomeTotal = valorMaxFome + valorMaxPimenta;

        // normaliza em relação a 1000 (se esse for teu limite fixo)
        float posY = (fomeTotal - 500f) * alturaInicialFome / 1000f;

        Vector3 posPimenta = barraPimenta.localPosition;
        posPimenta.y = posY;
        if (posPimenta.y >= 500) posPimenta.y = 500;
        if (posPimenta.y <= -499) posPimenta.y = -499;
        barraPimenta.localPosition = posPimenta;
    }
    void AtualizarCookie()
    {
        if (!cookieAtivo)
            return;
        valorAtualFome += 200f * Time.deltaTime;
        tempoRestanteCookie -= Time.deltaTime;
        if (tempoRestanteCookie <= 0f)
        {
            cookieAtivo = false;
        }
    }
    void AtualizarCM()
    {
        if (!CMAtivo)
            return;
       
        valorAtualFome += 150f * Time.deltaTime;
        tempoRestanteCM -= Time.deltaTime;
        if (tempoRestanteCM <= 0f)
        {
            CMAtivo = false;
        }
    }
    public void AtivarPimenta()
    {
        pimentaAtiva = true;
        barraPimenta.gameObject.SetActive(true);
        // Salva o maxFome antes da redução da pimenta
        valorMaxFomeAntesPimenta = valorMaxFome;
        // Calcula valor da pimenta (25% do maxFome atual antes de reduzir) 
        valorMaxPimenta = valorMaxFomeAntesPimenta * tamanhoPimenta; valorAtualPimenta = valorMaxPimenta;
        // Aplica redução no maxFome sem afetar o Y da barra da pimenta
        valorMaxFome -= valorMaxPimenta;
        // Define altura inicial da barra proporcional ao valor da pimenta
        float alturaPimenta = alturaInicialFome * (valorMaxPimenta / valorMaxInicialFome);
        barraPimenta.sizeDelta = new Vector2(barraPimenta.sizeDelta.x, alturaPimenta);
        // Calcula posição inicial 
        AtualizarPosicaoPimenta();
    }
    public void AtivarCookie()
    {
        cookieAtivo = true;
        tempoRestanteCookie = duracaoCookie;
    }
    public void AtivarCogumeloMal()
    {
        CMAtivo = true;
        tempoRestanteCM = duracaoCM;
        valorMaxFome = valorMaxFome / 2;
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
            // Divide a alteração entre fome e pimenta
            float partePimenta = quantidade * tamanhoPimenta;
            float parteFome = quantidade * (1 - tamanhoPimenta);

            // Atualiza limites
            valorMaxFome += parteFome;
            valorMaxFome = Mathf.Clamp(valorMaxFome, 1, 1000);

            valorMaxPimenta += partePimenta;
            valorMaxPimenta = Mathf.Clamp(valorMaxPimenta, 0, 1000);

            // Atualiza valores atuais
            valorAtualPimenta += partePimenta;
            valorAtualPimenta = Mathf.Clamp(valorAtualPimenta, 0, valorMaxPimenta);

            valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);

            // Débito da pimenta
            debitoPimenta += partePimenta;

            // Reposiciona a barra da pimenta
            AtualizarPosicaoPimenta();
        }
        else
        {
            // Só fome mesmo
            valorMaxFome += quantidade;
            valorMaxFome = Mathf.Clamp(valorMaxFome, 1, 1000);
            valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);
        }
    }
}