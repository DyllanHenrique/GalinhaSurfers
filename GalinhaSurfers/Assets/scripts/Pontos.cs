using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pontos : MonoBehaviour
{
    public float MetrosPorSegundo = 3f;
    public float distanciaPercorrida;
    public TMP_Text textMetros;
    public int distanciaNum;
    public bool galinhaMorta;

    private int Aumento = 100;
    public cenario cenario;

    // Update is called once per frame
    void Update()
    {
        if (!galinhaMorta)
        { 
            distanciaNum = Mathf.FloorToInt(distanciaPercorrida);
            distanciaPercorrida += MetrosPorSegundo * Time.deltaTime;
            textMetros.text = "Pontos: " + distanciaNum.ToString() + " m";

            if (distanciaPercorrida >= Aumento)
            {
                cenario.AumentarVelocidade();
                AumentarVelocidadeMetros();
                Aumento += 100;
            }
        }
    }

    public void AumentarVelocidadeMetros()
    {
        MetrosPorSegundo *= 1.5f;
        Debug.Log("Moleculas moleculas MOLECULAS por segundo: " + MetrosPorSegundo);
    }
}
