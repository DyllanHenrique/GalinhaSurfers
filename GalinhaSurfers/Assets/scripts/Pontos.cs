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
    public float incremento;
    private int Aumento = 100;
    public cenario cenario;
    public RotacaoMundo Mundo;
    public GalinhaMovement galinha;

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
        Mundo.rotationSpeed *= incremento;
        MetrosPorSegundo *= incremento;
        galinha.speed *= 1.1F;
        if (galinha.speed >= 2.5) galinha.speed = 2.5f;
        galinha.VelGalin();
        Debug.Log("velSCORE" + MetrosPorSegundo);
        Debug.Log("velGALIN" + galinha.speed);
        Debug.Log("velMUNDO" + Mundo.rotationSpeed);
    }
}
