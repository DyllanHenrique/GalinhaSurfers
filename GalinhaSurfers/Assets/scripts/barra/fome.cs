using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class fome : MonoBehaviour
{
    private float delayAntesDeTUDO =8f;
    private bool taLiberado = false;
    [Header("Barra Fome")]
    public RectTransform barraFome;
    public float valorAtualFome;
    public float valorMaxFome;
    public float velFome; // velocidade normal de diminui��o da fome [Header("Barra Pimenta")] 
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
    [Header("CogumeloMaluco")]
    public float duracaoCoguMaluco;
    public List<Sprite> todasFrutas;
    [Header("Escorpiao")]
    private bool escorpiaoAtivo;
    public float duracaoEscorpiao;
    private float tempoRestanteEscorpion;
    [Header("Morte")]
    public GameObject mortehud;
    public Pontos scriptPontuacao;
    private bool Morreu;
    public GameObject HighScore;
    public TMP_Text Score;
    public TMP_Text ScoreSombra;
    public SpawnScript spwanScript;
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
        StartCoroutine(Perai());

    }
    private IEnumerator Perai() 
    { 
        yield return new WaitForSeconds(delayAntesDeTUDO); 
        taLiberado = true; 
    }

    void Update()
    {
        if (!taLiberado) return;
        AtualizarFome();
        AtualizarPimenta();
        AtualizarCookie();
        AtualizarEscorpion();
        AtualizarCM();
        if (pimentaAtiva) AtualizarPosicaoPimenta();
        if (valorAtualFome <= 0 && !Morreu)
        {
            StartCoroutine(GalinhaMorreu());
        }

    }
    void AtualizarFome()
    {
        valorAtualFome -= velFome * Time.deltaTime; valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);
        float alturaMaximaFome = alturaInicialFome * (valorMaxFome / valorMaxInicialFome);
        float porcentagemFome = valorAtualFome / valorMaxFome; barraFome.sizeDelta = new Vector2(barraFome.sizeDelta.x, alturaMaximaFome * porcentagemFome);


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
            scriptPontuacao.pimentaMenosSpeed();
        }
    }
    void AtualizarPosicaoPimenta()
    {
        float fomeTotal = valorMaxFome + valorMaxPimenta;

        // normaliza em rela��o a 1000 (se esse for teu limite fixo)
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
            scriptPontuacao.cookieMenosSpeed();
            GameObject[] frutas = GameObject.FindGameObjectsWithTag("Frutas");
            foreach (GameObject fruta in frutas)
            {
                Collider2D col = fruta.GetComponent<Collider2D>();
                if (col != null)
                    col.enabled = true; // reativa o clique
            }
        }
    }
    void AtualizarEscorpion()
    {
        if (!escorpiaoAtivo)
            return;
        tempoRestanteEscorpion -= Time.deltaTime;
        if (tempoRestanteEscorpion <= 0f)
        {
            escorpiaoAtivo = false;
            scriptPontuacao.escorpiaoMenosSpeed();
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
        scriptPontuacao.pimentaSpeed();
        barraPimenta.gameObject.SetActive(true);
        // Salva o maxFome antes da redu��o da pimenta
        valorMaxFomeAntesPimenta = valorMaxFome;
        // Calcula valor da pimenta (25% do maxFome atual antes de reduzir) 
        valorMaxPimenta = valorMaxFomeAntesPimenta * tamanhoPimenta; valorAtualPimenta = valorMaxPimenta;
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
        GameObject[] frutas = GameObject.FindGameObjectsWithTag("Frutas");
        foreach (GameObject fruta in frutas)
        {
            Collider2D col = fruta.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
        cookieAtivo = true;
        scriptPontuacao.cookieSpeed();
        tempoRestanteCookie = duracaoCookie;
    }
    public void AtivarCogumeloMal()
    {
        CMAtivo = true;
        tempoRestanteCM = duracaoCM;
        valorMaxFome = valorMaxFome / 4;
    }
    public void AtivarEscorpiaoLentidao()
    {
        escorpiaoAtivo = true;
        tempoRestanteEscorpion = duracaoEscorpiao;
        scriptPontuacao.escorpiaoSpeed();
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
            // Divide a altera��o entre fome e pimenta
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

            // D�bito da pimenta
            debitoPimenta += partePimenta;

            // Reposiciona a barra da pimenta
            AtualizarPosicaoPimenta();
        }
        else
        {
            // S� fome mesmo
            valorMaxFome += quantidade;
            valorMaxFome = Mathf.Clamp(valorMaxFome, 1, 1000);
            valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);
        }
    }
    public void AtivarCogumeloMaluco()
    {
        StartCoroutine(EfeitoAlucinogeno(duracaoCoguMaluco));
    }
    private IEnumerator EfeitoAlucinogeno(float duracao)
    {

        float tempoPassado = 0f;
        bool mostrarNovo = true;
        Dictionary<GameObject, Sprite> originais = new Dictionary<GameObject, Sprite>();
        while (tempoPassado < duracao)
        {
            GameObject[] frutas = GameObject.FindGameObjectsWithTag("Frutas");
            foreach (GameObject fruta in frutas)
            {
                if (fruta == null) continue;
                SpriteRenderer sr = fruta.GetComponent<SpriteRenderer>();
                if (sr == null || todasFrutas.Count == 0) continue;
                if (!originais.ContainsKey(fruta))
                    originais.Add(fruta, sr.sprite);
                if (mostrarNovo)
                {
                    Sprite novoSprite;
                    do
                    {
                        novoSprite = todasFrutas[Random.Range(0, todasFrutas.Count)];
                    } while (novoSprite == sr.sprite);

                    sr.sprite = novoSprite;
                }
                else
                {
                    sr.sprite = originais[fruta];
                }
            }

            mostrarNovo = !mostrarNovo;
            yield return new WaitForSeconds(1f);
            tempoPassado += 1f;
        }
        foreach (var item in originais)
        {
            if (item.Key != null)
            {
                SpriteRenderer sr = item.Key.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sprite = item.Value;
            }
        }
        Debug.Log("Efeito alucinógeno passou!");
    }
    private IEnumerator GalinhaMorreu()
    {
        int recorde = PlayerPrefs.GetInt("pontuacao",0);
        Debug.Log(recorde);
        Morreu = true;
        scriptPontuacao.galinhaMorta = true;
        spwanScript.morreu = true;
        yield return new WaitForSeconds(3.5f);
        mortehud.SetActive(true);
        Score.text = "Score:" + scriptPontuacao.distanciaNum;
        ScoreSombra.text = Score.text;
        int pontosAtuais = scriptPontuacao.distanciaNum;
        if (pontosAtuais > recorde)
        {
            Debug.Log(pontosAtuais);
            PlayerPrefs.SetInt("pontuacao", pontosAtuais);
            yield return new WaitForSeconds(3f);
            HighScore.SetActive(true);
        }
        //Usado para atualizar o score em breve
    }
}