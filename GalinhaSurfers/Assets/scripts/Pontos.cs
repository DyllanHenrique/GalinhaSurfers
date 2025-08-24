using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pontos : MonoBehaviour
{
    public float MetrosPorSegundo = 3f;
    private float distanciaPercorrida;
    public TMP_Text textMetros;

    // Update is called once per frame
    void Update()
    {
        distanciaPercorrida += MetrosPorSegundo * Time.deltaTime;
        textMetros.text = Mathf.FloorToInt(distanciaPercorrida).ToString() + " m";
    }
}
