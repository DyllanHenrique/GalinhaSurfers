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
    private int Aumento = 150;
    public cenario cenario;
    public RotacaoMundo Mundo;
    public GalinhaMovement galinha;
    private float delayAntesDeTUDO = 8f;
    private bool taLiberado = false;
    private bool morrendo = false;
    public float pimentaIncremento;
    public float cookieIncremento;
    public float escorpiaoIncremento;

    //Morte
    public AudioSource AudMorte;
    public GameObject Frangao;
    public GameObject GALIN;
    public ParticleSystem penasPartic;
    public GameObject smokeObject;

    void Start()

    {
        penasPartic.Stop();
        smokeObject.SetActive(false);

        StartCoroutine(Perai());
    }
    private IEnumerator Perai()
    {
        yield return new WaitForSeconds(delayAntesDeTUDO);
        taLiberado = true;
    }
    void Update()
    {
        if (taLiberado)
        {
            if (!galinhaMorta)
            {
                distanciaNum = Mathf.FloorToInt(distanciaPercorrida);
                distanciaPercorrida += MetrosPorSegundo * Time.deltaTime;
                textMetros.text = distanciaNum.ToString() + " m";

                //como esse caba meeente
                if (distanciaPercorrida >= Aumento)
                {
                    cenario.AumentarVelocidade();
                    AumentarVelocidadeMetros();
                    if (distanciaPercorrida <= 600f)
                    {
                        Aumento += 150;
                    }
                    else if (distanciaPercorrida <= 1800f)
                    {
                        Aumento += 400;
                    }
                    else if (distanciaPercorrida <= 3350f)
                    {
                        Aumento += 500;
                    }
                    else
                    {
                        Aumento += 1000;
                    }
                }
            }
            else if(galinhaMorta && !morrendo)
            {
                StartCoroutine(ParandoTudo());
            }
        } 
    }
    public IEnumerator ParandoTudo()
    {
        morrendo = true;

        penasPartic.Play();
        smokeObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GALIN.SetActive(false);
        Frangao.SetActive(true);
        AudMorte.Play();

        Vector3 startRotation = Mundo.rotationSpeed;
        float startMetros = MetrosPorSegundo;
        float startSpeed = galinha.speed;

        comida_geral.morreu = true;
        comida_geral[] comidas = FindObjectsOfType<comida_geral>();
        foreach (var c in comidas)
        {
            c.IniciarDesaceleracao(3f);
        }

        float duracao = 3f; 
        float t = 0f;
        while (t < duracao)
        {
            t += Time.deltaTime;
            float progress = t / duracao; 
            Mundo.rotationSpeed = Vector3.Lerp(startRotation, Vector3.zero, progress);
            MetrosPorSegundo = Mathf.Lerp(startMetros, 0f, progress);
            galinha.speed = Mathf.Lerp(startSpeed, 0f, progress);

            galinha.VelGalin();

            yield return null; 
        }
        Mundo.rotationSpeed = Vector3.zero;
        MetrosPorSegundo = 0f;
        galinha.speed = 0f;
        galinha.VelGalin();
    }
    public void AumentarVelocidadeMetros()
    {
        Mundo.rotationSpeed *= incremento;
        MetrosPorSegundo *= incremento;
        galinha.speed *= 1.1F;
        if (galinha.speed >= 2.5) galinha.speed = 2.5f;
        galinha.VelGalin();
        //Debug.Log("velSCORE" + MetrosPorSegundo);
        //Debug.Log("velGALIN" + galinha.speed);
        //Debug.Log("velMUNDO" + Mundo.rotationSpeed);
    }
    public void pimentaSpeed()
    {
        Mundo.rotationSpeed *= pimentaIncremento;
        MetrosPorSegundo *= pimentaIncremento;
        galinha.speed *= pimentaIncremento;
        if (galinha.speed >= 2.5) galinha.speed = 2.5f;
        galinha.VelGalin();
    }
    public void pimentaMenosSpeed()
    {
        Mundo.rotationSpeed /= pimentaIncremento;
        MetrosPorSegundo /= pimentaIncremento;
        galinha.speed /= pimentaIncremento;
        if (galinha.speed >= 2.5) galinha.speed = 2.5f;
        galinha.VelGalin();
    }
    public void cookieSpeed()
    {
        Mundo.rotationSpeed *= cookieIncremento;
        MetrosPorSegundo *= cookieIncremento;
        galinha.speed *= cookieIncremento;
        if (galinha.speed >= 2.5) galinha.speed = 2.5f;
        galinha.VelGalin();
        Debug.Log("velGALIN" + galinha.speed);
    }
    public void cookieMenosSpeed()
    {
        Mundo.rotationSpeed /= cookieIncremento;
        MetrosPorSegundo /= cookieIncremento;
        galinha.speed /= cookieIncremento;
        if (galinha.speed >= 2.5) galinha.speed = 2.5f;
        galinha.VelGalin();
        Debug.Log("velGALIN" + galinha.speed);
    }
    public void escorpiaoSpeed()
    {
        Mundo.rotationSpeed *= escorpiaoIncremento;
        MetrosPorSegundo *= escorpiaoIncremento;
        galinha.speed *= escorpiaoIncremento;
        if (galinha.speed >= 2.5) galinha.speed = 2.5f;
        galinha.VelGalin();
        Debug.Log("velGALIN" + galinha.speed);
    }
    public void escorpiaoMenosSpeed()
    {
        Mundo.rotationSpeed /= escorpiaoIncremento;
        MetrosPorSegundo /= escorpiaoIncremento;
        galinha.speed /= escorpiaoIncremento;
        if (galinha.speed >= 2.5) galinha.speed = 2.5f;
        galinha.VelGalin();
        Debug.Log("velGALIN" + galinha.speed);
    }
}
