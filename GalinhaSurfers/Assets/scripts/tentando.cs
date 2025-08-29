using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentando : MonoBehaviour
{
    public Transform neckBone;
    public float moveSpeed = 5f;
    public float forwardDistance = 2f;
    private float frutaOffsetZ = 1.55f;

    private Vector3 originalLocalPos;
    private bool stretch = false;
    private Vector3 forwardTarget;
    private float velocidadeAtual;

    [SerializeField] private LaneDetector[] lanes;
    private LaneDetector laneAtual;
    private comida_geral frutaAlvo;
    private comida_geral frutaAlvoParaDestruir;

    public tresDoisUm tresDoisUm;
    private Animator animator;
    private Coroutine eatingCoroutine;
    private bool voltandoDoEating = false;

    void Awake()
    {
        // encontra todos os LaneDetector na cena
        lanes = FindObjectsOfType<LaneDetector>();
    }
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
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (tresDoisUm.TresDoisUmGO)
            return;
        if (voltandoDoEating)
            return;
        laneAtual = GetLaneMaisProxima();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            frutaAlvo = laneAtual != null ? laneAtual.GetFrutaMaisNaFrente() : null;

    
            if (stretch && frutaAlvoParaDestruir != null && frutaAlvo == frutaAlvoParaDestruir)
                return;

            if (frutaAlvo != null && animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                bool ultimoClique = frutaAlvo.cliquesRestantes == 1;
                float distancia = Vector3.Distance(frutaAlvo.transform.position, neckBone.position);

                if (ultimoClique)
                {
                    if (distancia < 2f)
                    {
        
                        if (eatingCoroutine != null)
                        {
                            StopCoroutine(eatingCoroutine);
                            eatingCoroutine = null;
                            animator.Play("Walk");
                        }
                        eatingCoroutine = StartCoroutine(TocarEating(frutaAlvo));
                        stretch = false;
                        frutaAlvoParaDestruir = null;
                    }
                    else
                    {
            
                        Vector3 targetPos = frutaAlvo.transform.position - new Vector3(0, 0, frutaOffsetZ);
                        Vector3 deslocamento = targetPos - neckBone.parent.TransformPoint(originalLocalPos);
                        if (deslocamento.magnitude > forwardDistance)
                            deslocamento = deslocamento.normalized * forwardDistance;

                        forwardTarget = neckBone.parent.TransformPoint(originalLocalPos) + deslocamento;
                        stretch = true;
                        frutaAlvoParaDestruir = frutaAlvo;
                    }
                }
                else
                {
            
                    frutaAlvo.ConsumirClique();
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
          
                Vector3 deslocamento = neckBone.forward * forwardDistance;
                forwardTarget = neckBone.parent.TransformPoint(originalLocalPos) + deslocamento;
                stretch = true;
                frutaAlvoParaDestruir = null;
            }
        }
    }

    private IEnumerator TocarEating(comida_geral fruta)
    {
        if (animator != null)
            animator.Play("Eating");

        fruta.ConsumirClique();
        yield return new WaitForSeconds(1.4f); // duração da animação

        if (animator != null)
            animator.Play("Walk");

        eatingCoroutine = null;
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

            if (Vector3.Distance(neckBone.position, forwardTarget) < 0.005f)
            {
                if (frutaAlvoParaDestruir != null)
                {
                    frutaAlvoParaDestruir.ConsumirClique();
                    frutaAlvoParaDestruir = null;

                
                    voltandoDoEating = true;
                }

                forwardTarget = neckBone.parent.TransformPoint(originalLocalPos);
                stretch = false;
            }
        }
        else
        {
            Vector3 targetPos = neckBone.parent.TransformPoint(originalLocalPos);
            if (Vector3.Distance(neckBone.position, targetPos) > 0.01f)
            {
                neckBone.position = Vector3.MoveTowards(
                    neckBone.position,
                    targetPos,
                    velocidadeAtual * Time.deltaTime
                );
            }
            else if (voltandoDoEating)
            {
 
                voltandoDoEating = false;
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
