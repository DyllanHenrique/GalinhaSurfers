using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class fome : MonoBehaviour
{
    public Material materialRainbow;
    private Material materialOriginal;
    private Material materialOriginalBarra;
    public Renderer bFome;
    public SkinnedMeshRenderer personagemRenderer;
    public AudioSource ComidasSons;
    public RectTransform topoFome;
    private float delayAntesDeTUDO = 8f;
    private bool taLiberado = false;

    [Header("Barra Fome")]
    public RectTransform barraFome;
    public float valorAtualFome;
    public float valorMaxFome;
    public float velFome; // velocidade normal de diminuição da fome

    [Header("Barra Pimenta")]
    private float alturaInicialFome;
    private float valorMaxInicialFome;
    public RectTransform barraPimenta;
    public float duracaoPimenta;
    [Range(0f, 1f)]
    public float tamanhoPimenta; // 25%
    private float valorMaxPimenta;
    private float valorAtualPimenta;
    private bool pimentaAtiva;
    private float valorMaxFomeAntesPimenta;
    private float debitoPimenta = 0f;
    public AudioClip pimentaSom;

    [Header("Cookie")]
    public float duracaoCookie;
    private bool cookieAtivo;
    private float tempoRestanteCookie;
    public AudioClip cookieSom;

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
    public AudioSource MscHighScore;

    public RectTransform meioContorno; // objeto que vai ser escalonado
    public float alturaMaxMeio = 433f;
    public float alturaMinMeio = 80f;
    private float posYOriginalPimenta;
    float valorMaxFomeBase = 925.5f;
    private float yInicialDentroPai;
    private Coroutine cogumeloCoroutine;

    public void Start()
    {
        materialOriginalBarra = bFome.material;
        materialOriginal = personagemRenderer.material;

        // Fome
        alturaInicialFome = barraFome.sizeDelta.y;
        valorMaxInicialFome = alturaInicialFome;
        valorMaxFome = valorMaxInicialFome;
        valorAtualFome = valorMaxFome;

        // Pimenta
        barraPimenta.gameObject.SetActive(false);
        pimentaAtiva = false;
        yInicialDentroPai = barraPimenta.anchoredPosition.y;
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

        if (Input.GetKeyDown(KeyCode.E))
            AtivarPimenta();

        AtualizarFome();
        AtualizarTopoFome();
        AtualizarMeioContorno();
        AtualizarPimenta();
        AtualizarCookie();
        AtualizarEscorpion();
        AtualizarCM();

        if (pimentaAtiva)
            AtualizarPosicaoPimenta();

        if (valorAtualFome <= 0 && !Morreu)
            StartCoroutine(GalinhaMorreu());

    }

    void AtualizarFome()
    {
        valorAtualFome -= velFome * Time.deltaTime;
        valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);

        float alturaMaximaFome = alturaInicialFome * (valorMaxFome / valorMaxInicialFome);
        float porcentagemFome = valorAtualFome / valorMaxFome;

        barraFome.sizeDelta = new Vector2(barraFome.sizeDelta.x, alturaMaximaFome * porcentagemFome);
    }

    void AtualizarPimenta()
    {
        if (!pimentaAtiva) return;

        // 1️⃣ Reduz pimenta
        float velPimenta = valorMaxPimenta / duracaoPimenta;
        valorAtualPimenta -= velPimenta * Time.deltaTime;
        valorAtualPimenta = Mathf.Clamp(valorAtualPimenta, 0, valorMaxPimenta);

        // 2️⃣ Altura proporcional da barra (pivot topo)
        float fatorAltura = valorAtualPimenta / valorMaxPimenta;
        float alturaBase = valorMaxPimenta * (462.3f / 925.5f); // 462.3 = topo original relativo ao canvas
        float alturaAtual = alturaBase * fatorAltura;
        barraPimenta.sizeDelta = new Vector2(barraPimenta.sizeDelta.x, alturaAtual);

        // 3️⃣ Mantém o topo fixo relativo ao Canvas
        float topoDesejadoCanvas = 462.3f; // Y no Canvas que queremos manter fixo
        barraPimenta.anchoredPosition = new Vector2(
            barraPimenta.anchoredPosition.x,
            topoDesejadoCanvas - alturaAtual * (1 - barraPimenta.pivot.y)
        );

        Debug.Log("[AtualizarPimenta] alturaAtual: " + alturaAtual +
                  " | anchoredPosition.y: " + barraPimenta.anchoredPosition.y);

        // 4️⃣ Desativa pimenta quando acabar
        if (valorAtualPimenta <= 0f)
        {
            pimentaAtiva = false;
            barraPimenta.gameObject.SetActive(false);

            valorMaxFome = valorMaxFomeAntesPimenta;
            valorMaxFome += debitoPimenta * (1 / tamanhoPimenta);
            debitoPimenta = 0f;

            scriptPontuacao.pimentaMenosSpeed();
        }
    }

    void AtualizarPosicaoPimenta()
    {
        if (!pimentaAtiva) return;

        // 1️⃣ Altura proporcional à pimenta
        float alturaPimenta = valorMaxPimenta * (462.3f / 925.5f);
        float porcentagemPimenta = valorAtualPimenta / valorMaxPimenta;
        barraPimenta.sizeDelta = new Vector2(barraPimenta.sizeDelta.x, alturaPimenta * porcentagemPimenta);

        // 2️⃣ Topo fixo dentro do pai
        float topoPai = 462.3f; // topo dentro do pai
        barraPimenta.anchoredPosition = new Vector2(
            barraPimenta.anchoredPosition.x,
            topoPai - barraPimenta.sizeDelta.y
        );

        Debug.Log("[AtualizarPosicaoPimenta] anchoredPosition.y: " + barraPimenta.anchoredPosition.y);
    }

    void AtualizarCookie()
    {
        if (!cookieAtivo) return;

        valorAtualFome += 200f * Time.deltaTime;
        tempoRestanteCookie -= Time.deltaTime;

        if (tempoRestanteCookie <= 0f)
        {
            bFome.material = materialOriginalBarra;
            personagemRenderer.material = materialOriginal;
            cookieAtivo = false;
            scriptPontuacao.cookieMenosSpeed();
        }
    }

    void AtualizarEscorpion()
    {
        if (!escorpiaoAtivo) return;

        tempoRestanteEscorpion -= Time.deltaTime;

        if (tempoRestanteEscorpion <= 0f)
        {
            escorpiaoAtivo = false;
            scriptPontuacao.escorpiaoMenosSpeed();
        }
    }

    void AtualizarCM()
    {
        if (!CMAtivo) return;

        valorAtualFome += 150f * Time.deltaTime;
        tempoRestanteCM -= Time.deltaTime;

        if (tempoRestanteCM <= 0f) CMAtivo = false;
    }

    public void AtivarPimenta()
    {
        pimentaAtiva = true;
        ComidasSons.clip = pimentaSom;
        ComidasSons.Play();

        if (!barraPimenta.gameObject.activeSelf)
            barraPimenta.gameObject.SetActive(true);

        if (valorAtualPimenta <= 0f)
        {
            scriptPontuacao.pimentaSpeed();
            valorMaxFomeAntesPimenta = valorMaxFome;
            valorMaxPimenta = valorMaxFomeAntesPimenta * tamanhoPimenta;
            valorAtualPimenta = valorMaxPimenta;
            valorMaxFome -= valorMaxPimenta;
        }

        // Atualiza altura e posição imediatamente
        AtualizarPimenta();
    }

    public void AtivarCookie()
    {
        bFome.material = materialRainbow;
        personagemRenderer.material = materialRainbow;
        ComidasSons.clip = cookieSom;
        ComidasSons.Play();

        if (!cookieAtivo) scriptPontuacao.cookieSpeed();

        cookieAtivo = true;
        tempoRestanteCookie = duracaoCookie;
        AtualizarCookie();
    }

    public void AtivarCogumeloMal()
    {
        CMAtivo = true;
        tempoRestanteCM = duracaoCM;
        valorMaxFome = valorMaxFome / 2;
    }

    public void AtivarEscorpiaoLentidao()
    {
        if (!escorpiaoAtivo) scriptPontuacao.escorpiaoSpeed();
        escorpiaoAtivo = true;
        tempoRestanteEscorpion = duracaoEscorpiao;
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
            float partePimenta = quantidade * tamanhoPimenta;
            float parteFome = quantidade * (1 - tamanhoPimenta);

            valorMaxFome += parteFome;
            valorMaxFome = Mathf.Clamp(valorMaxFome, 1, 925.5f);

            valorMaxPimenta += partePimenta;
            valorMaxPimenta = Mathf.Clamp(valorMaxPimenta, 0, 925.5f);

            valorAtualPimenta += partePimenta;
            valorAtualPimenta = Mathf.Clamp(valorAtualPimenta, 0, valorMaxPimenta);
            valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);

            debitoPimenta += partePimenta;
            AtualizarPosicaoPimenta();
        }
        else
        {
            valorMaxFome += quantidade;
            valorMaxFome = Mathf.Clamp(valorMaxFome, 1, 925.5f);
            valorAtualFome = Mathf.Clamp(valorAtualFome, 0, valorMaxFome);
        }
    }

    public void AtivarCogumeloMaluco()
    {
        if (cogumeloCoroutine != null)
        {
            StopCoroutine(cogumeloCoroutine);
        }

        cogumeloCoroutine = StartCoroutine(EfeitoAlucinogeno(duracaoCoguMaluco));
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

        // Restaura os sprites originais
        foreach (var item in originais)
        {
            if (item.Key != null)
            {
                SpriteRenderer sr = item.Key.GetComponent<SpriteRenderer>();
                if (sr != null) sr.sprite = item.Value;
            }
        }

        cogumeloCoroutine = null;
        Debug.Log("Efeito alucinógeno passou!");
    }

    private IEnumerator GalinhaMorreu()
    {
        int recorde = PlayerPrefs.GetInt("pontuacao", 0);
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
            PlayerPrefs.SetInt("pontuacao", pontosAtuais);
            yield return new WaitForSeconds(3f);
            MscHighScore.Play();
            HighScore.SetActive(true);
        }
    }

    void AtualizarMeioContorno()
    {
        float minFome = 230f;
        float maxFome = 925.5f;
        float novaAltura;

        float valorParaCalculo = Mathf.Clamp(valorMaxFomeBase, minFome, maxFome);

        if (valorParaCalculo <= minFome)
            novaAltura = alturaMinMeio;
        else if (valorParaCalculo >= maxFome)
            novaAltura = alturaMaxMeio;
        else
            novaAltura = alturaMinMeio + (valorParaCalculo - minFome) * (alturaMaxMeio - alturaMinMeio) / (maxFome - minFome);

        Vector2 size = meioContorno.sizeDelta;
        size.y = novaAltura;
        meioContorno.sizeDelta = size;
    }

    void AtualizarTopoFome()
    {
        float topoMinY = -245f;
        float topoMaxY = 446.3f;
        float minFome = 230f;
        float maxFome = 925.5f;
        float ajusteVisual = 14f;

        float valorParaCalculo = Mathf.Clamp(valorMaxFomeBase, minFome, maxFome);
        float posY;

        if (valorParaCalculo <= minFome)
            posY = topoMinY;
        else if (valorParaCalculo >= maxFome)
            posY = topoMaxY;
        else
        {
            posY = topoMinY + (valorParaCalculo - minFome) * (topoMaxY - topoMinY) / (maxFome - minFome);
            posY -= ajusteVisual;
        }

        Vector3 pos = topoFome.localPosition;
        pos.y = posY;
        topoFome.localPosition = pos;
    }
}
