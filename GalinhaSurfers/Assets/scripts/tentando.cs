using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentando : MonoBehaviour
{
    public Transform neckBone;           // Osso real do pescoço
    public float moveSpeed = 5f;         // Velocidade do Lerp
    public float forwardDistance = 2f;   // Distância de esticada

    private Vector3 originalLocalPos;    // Posição inicial local do pescoço
    private bool stretch = false;
    private Vector3 forwardTarget;

    public LaneDetector[] lanes;   // <- arrasta os 3 detectors aqui
    private LaneDetector laneAtual;
    private comida_geral frutaAlvo;
    public float stretchSpeed = 0.3f;
    void Start()
    {
        if (neckBone == null)
        {
            Debug.LogError("Neck bone não atribuído!");
            enabled = false;
            return;
        }

        originalLocalPos = neckBone.localPosition;
    }

    void Update()
    {
        laneAtual = GetLaneMaisProxima();

        if (laneAtual != null)
        {
            frutaAlvo = laneAtual.GetFrutaMaisNaFrente();

            if (frutaAlvo != null && Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(neckBone.localPosition.y - originalLocalPos.y) < 0.0001f)
            {
                if (frutaAlvo.cliquesRestantes == 1)
                {
                    forwardTarget = frutaAlvo.transform.position;
                    stretch = true;
                }
                frutaAlvo.ConsumirClique();
            }
        }
    }

    void LateUpdate()
    {
        if (stretch)
        {
            // interpola suavemente do pescoço para o alvo
            neckBone.position = Vector3.Lerp(neckBone.position, forwardTarget, stretchSpeed);

            if (Vector3.Distance(neckBone.position, forwardTarget) < 0.05f)
                stretch = false;
        }
        else
        {
            neckBone.position = Vector3.MoveTowards(neckBone.position, neckBone.parent.TransformPoint(originalLocalPos), moveSpeed * Time.deltaTime);
        }

    }
    LaneDetector GetLaneMaisProxima()
    {
        LaneDetector maisPerto = null;
        float menorDistancia = Mathf.Infinity;

        foreach (var lane in lanes)
        {
            float dist = Mathf.Abs(transform.position.x - lane.transform.position.x);
            if (dist < menorDistancia)
            {
                menorDistancia = dist;
                maisPerto = lane;
            }
        }

        return maisPerto;
    }
}