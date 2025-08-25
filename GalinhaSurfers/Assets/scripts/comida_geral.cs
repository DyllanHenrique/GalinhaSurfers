using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comida_geral : MonoBehaviour
{
    [Header("Configurações da comida")]
    public float valorFome = 0f;  
    public float valorMaxFome = 0f;  
    public int cliquesParaComer = 1; 

    private int cliquesRestantes;
    public fome Fome;

    void Start()
    {
        cliquesRestantes = cliquesParaComer;
    }

    void OnMouseDown()
    {
        cliquesRestantes--;

        if (cliquesRestantes <= 0)
        {
            Comer();
        }
    }

    void Comer()
    {
        if (Fome != null)
        {
            Fome.AdicionarFome(valorFome);
            Fome.AlterarMaxFome(valorMaxFome);
        }
        Destroy(gameObject);
    }
}
