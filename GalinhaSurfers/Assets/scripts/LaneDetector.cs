using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneDetector : MonoBehaviour
{
    public List<comida_geral> frutasNaLane = new List<comida_geral>();
    public bool isActiveLane;
    void Start()
    {
        // Se tiver um renderer, desativa
        var rend = GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        comida_geral fruta = other.GetComponent<comida_geral>();
        if (fruta != null && !frutasNaLane.Contains(fruta))
        {
            frutasNaLane.Add(fruta);
        }
    }

    void OnTriggerExit(Collider other)
    {
        comida_geral fruta = other.GetComponent<comida_geral>();
        if (fruta != null && frutasNaLane.Contains(fruta))
        {
            frutasNaLane.Remove(fruta);
        }

    }
    void Update()
    {
        frutasNaLane.RemoveAll(c => c == null);
    }
    public comida_geral GetFrutaMaisNaFrente()
    {
        if (frutasNaLane.Count == 0)
            return null;

        comida_geral alvo = frutasNaLane[0];
        foreach (var fruta in frutasNaLane)
        {
            if (fruta.transform.position.z < alvo.transform.position.z)
            {
                alvo = fruta;
            }
        }
        return alvo;
    }

}
