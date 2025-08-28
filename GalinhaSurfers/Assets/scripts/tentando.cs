using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentando : MonoBehaviour
{
    public Transform neckBone;         
    public float moveSpeed = 5f;      
    public float forwardDistance = 2f;  

    private Vector3 originalLocalPos;   
    private bool stretch = false;
    private Vector3 forwardTarget;
    private float velocidadeAtual;
    public LaneDetector[] lanes;
    private LaneDetector laneAtual;
    private comida_geral frutaAlvo;
    private comida_geral frutaAlvoParaDestruir;
    public float frutaOffsetZ = 0.2f;
    void Start()
    {
        if (neckBone == null)
        {
            Debug.LogError("Neck bone não atribuído!");
            enabled = false;
            return;
        }

        originalLocalPos = neckBone.localPosition;
        velocidadeAtual = moveSpeed; 
    }

    void Update()
    {
        laneAtual = GetLaneMaisProxima();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            frutaAlvo = laneAtual != null ? laneAtual.GetFrutaMaisNaFrente() : null;

            if (frutaAlvo != null)
            {

                forwardTarget = frutaAlvo.transform.position - new Vector3(0, 0, frutaOffsetZ);
                stretch = true;

                // marca para destruir depois
                frutaAlvoParaDestruir = frutaAlvo;
            }
            else
            {
     
                forwardTarget = neckBone.position + neckBone.forward * forwardDistance;
                stretch = true;

                frutaAlvoParaDestruir = null; 
            }
        }
    }

    void LateUpdate()
    {
        if (stretch)
        {
         
            neckBone.position = Vector3.MoveTowards(
                neckBone.position,
                forwardTarget,
                velocidadeAtual * Time.deltaTime
            );

            if (Vector3.Distance(neckBone.position, forwardTarget) < 0.05f)
            {
           
                if (frutaAlvoParaDestruir != null)
                {
                    frutaAlvoParaDestruir.ConsumirClique();
                    frutaAlvoParaDestruir = null;
                }

          
                Vector3 posOriginal = neckBone.parent.TransformPoint(originalLocalPos);
                posOriginal.x = neckBone.parent.position.x; 
                forwardTarget = posOriginal;
                stretch = false; 
            }
        }
        else
        {
     
            Vector3 targetPos = neckBone.parent.TransformPoint(originalLocalPos);
            targetPos.x = neckBone.parent.position.x; 

            if (Vector3.Distance(neckBone.position, targetPos) > 0.01f)
            {
                neckBone.position = Vector3.MoveTowards(
                    neckBone.position,
                    targetPos,
                    velocidadeAtual * Time.deltaTime
                );
            }
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