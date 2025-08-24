using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cenario : MonoBehaviour
{
    public float cenarioAndando;

    // Update is called once per frame
    void Update()
    {
        andaa();
    }

    void andaa()
    {
        Vector2 deslocar = new Vector2 (0, Time.time * cenarioAndando);
        GetComponent<Renderer>().material.mainTextureOffset = deslocar;
    }
}
