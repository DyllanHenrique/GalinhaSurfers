using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cenario : MonoBehaviour
{
    public float cenarioAndando;
    public Pontos pontos;

    void Update()
    {
        andaa();
    }
    void andaa()
    {
        Vector2 deslocar = new Vector2(0, Time.time * cenarioAndando);
        GetComponent<Renderer>().material.mainTextureOffset = deslocar;
    }
    public void AumentarVelocidade()
    {
        //cenarioAndando *= 1.5f; // aumenta 1 na velocidade
        //Debug.Log("Velocidade ao extremoooo: " + cenarioAndando);
    }
}
