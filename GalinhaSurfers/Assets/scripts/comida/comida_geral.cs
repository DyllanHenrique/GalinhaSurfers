using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comida_geral : MonoBehaviour
{
    public comidaConfiig config; // referência para o preset
    private int cliquesRestantes;
    public fome Fome;
    void Start()
    {
        cliquesRestantes = config != null ? config.cliquesParaComer : 1;

        if (Fome == null)
        {
            Fome = FindObjectOfType<fome>();
        }
    }

    void OnMouseDown()
    {
        cliquesRestantes--;

        if (cliquesRestantes <= 0)
            Comer();
    }

    void Comer()
    {
        if (Fome != null && config != null)
        {
            Fome.AdicionarFome(config.valorFome);
            Fome.AlterarMaxFome(config.valorMaxFome);
            if (config.temPoder)
            {
                AtivarPoder(config.nomeComida);
            }
        }
        Debug.Log($"COMEU {config?.nomeComida}");
        Destroy(gameObject);
    }
    void AtivarPoder(string nomePoder)
    {
        switch (nomePoder)
        {
            case "PIMENTA":
                Debug.Log("Poder" +nomePoder);
                Fome.AtivarPimenta();
                break;
            case "COOKIE":
                Debug.Log("Poder" + nomePoder);
                Fome.AtivarCookie();
                break;
            case "Abacaxi":
                Debug.Log("Poder" + nomePoder);
               
                break;
            default:
                Debug.Log("Comida sem poder específico");
                break;
        }
    }
}
