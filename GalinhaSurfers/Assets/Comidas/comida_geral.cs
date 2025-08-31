using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class comida_geral : MonoBehaviour
{
    public comidaConfig config;
    public int cliquesRestantes;
    public fome Fome;
    private Rigidbody rb;
    public Pontos ponto;
    private aranha scriptAranha;
    public static bool morreu = false; 
    private Coroutine desacelerando = null;
    private void Start()
    {
        morreu = false;
        cliquesRestantes = config != null ? config.cliquesParaComer : 1;

        if (Fome == null)
        {
            Fome = FindObjectOfType<fome>();
        }
        if(ponto == null)
        {
            ponto = FindObjectOfType<Pontos>();
        }
        rb = gameObject.GetComponent<Rigidbody>();
        if(scriptAranha == null)
            scriptAranha = GetComponent<aranha>();
    }


    private void Update() 
    {
        if (morreu)
            return;
        float velcomidas = -ponto.MetrosPorSegundo;
        if (CompareTag("Frutas"))
        {
            float amplitude = 1.3f;       // altura do pulo
            float frequencia = 2f;      // velocidade da oscilação
            float y = Mathf.Sin(Time.time * frequencia * Mathf.PI * 2f) * amplitude;
           
            rb.velocity = new Vector3(0, y, velcomidas-1);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, velcomidas - 1);
        }
    }
    public void IniciarDesaceleracao(float duracao = 3f)
    {
        if (desacelerando == null)
            desacelerando = StartCoroutine(Desacelerar(duracao));
    }
    private IEnumerator Desacelerar(float duracao)
    {
        Vector3 startVel = rb.velocity;
        float t = 0f;

        while (t < duracao)
        {
            t += Time.deltaTime;
            float progress = t / duracao;
            rb.velocity = Vector3.Lerp(startVel, Vector3.zero, progress);

            yield return null;
        }

        rb.velocity = Vector3.zero;
        desacelerando = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("destruir")) 
        {
            Destroy(gameObject);
        }
    }
    public void ConsumirClique()
    {
        cliquesRestantes--;

        if (cliquesRestantes <= 0)
            Comer();
    }


    void Comer()
    {
        if (Fome != null && config != null)
        {
            if (config.nomeComida == "ARANHA" && scriptAranha != null)
            {
                if (scriptAranha.EstaArmada())
                {
                    Debug.Log("Aranha estava ARMADA! Nenhum valor nutricional aplicado.");
                    Fome.AtivarEscorpiaoLentidao();
                }
                else
                {
                    AplicarValoresNutricionais();
                }
            }
            else
            {
                AplicarValoresNutricionais();
            }
        }

        Debug.Log($"COMEU {config?.nomeComida}");
        Destroy(gameObject);
    }
    void AplicarValoresNutricionais()
    {
        Fome.AdicionarFome(config.valorFome);
        Fome.AlterarMaxFome(config.valorMaxFome);

        if (config.temPoder)
        {
            AtivarPoder(config.nomeComida);
        }
    }
    void AtivarPoder(string nomePoder)
    {
        switch (nomePoder)
        {
            case "PILULA":
                Debug.Log("Poder" + nomePoder);
                string[] poderes = { "PIMENTA", "COOKIE", "COGUMELOMAL", "COGUMELOMALUCO","ESCORPIAO" };
                int index = Random.Range(0, poderes.Length);
                string poderSorteado = poderes[index];
                Debug.Log("P�lula ativou aleatoriamente: " + poderSorteado);
                AtivarPoder(poderSorteado); 
                break;
            case "PIMENTA":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarPimenta();
                break;
            case "COOKIE":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarCookie();
                break;
            case "COGUMELOMAL":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarCogumeloMal();
                break;
            case "COGUMELOMALUCO":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarCogumeloMaluco();
                break;
            case "ESCORPIAO":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarEscorpiaoLentidao();
                break;

            default:
                Debug.Log("Comida sem poder espec�fico");
                break;
        }
    }

}
