using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comida_geral : MonoBehaviour
{
    public comidaConfig config;
    public int cliquesRestantes;
    public fome Fome;
    private Rigidbody rb;
    public Pontos ponto;
    private aranha scriptAranha;

    private void Start()
    {
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
    private IEnumerator pulandinho()
    {
        //continuar aqui
        yield return null;
    }

    private void Update() 
    { 
        rb.velocity = new Vector3(0, 0, -ponto.MetrosPorSegundo); 
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
