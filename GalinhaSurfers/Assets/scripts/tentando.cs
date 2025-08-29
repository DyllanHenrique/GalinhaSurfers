using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentando : MonoBehaviour
{
    public Transform neckBone;
    public float moveSpeed = 5f;
    public float forwardDistance = 2f;
    public float frutaOffsetZ = 0.2f;

    private Vector3 originalLocalPos;
    private bool stretch = false;
    private Vector3 forwardTarget;
    private float velocidadeAtual;
    public LaneDetector[] lanes;
    private LaneDetector laneAtual;
    private comida_geral frutaAlvo;
    private comida_geral frutaAlvoParaDestruir;

    private Animator animator;
    private Coroutine eatingCoroutine;

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
        laneAtual = GetLaneMaisProxima();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            frutaAlvo = laneAtual != null ? laneAtual.GetFrutaMaisNaFrente() : null;

            if (frutaAlvo != null)
            {
                bool ultimoClique = frutaAlvo.cliquesRestantes == 1;
                float distancia = Vector3.Distance(frutaAlvo.transform.position, neckBone.position);

                // Sempre diminui o clique
          

                // Só estica ou come se estiver em Walk
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    if (distancia < 2f)
                    {
                        // Come Eating normalmente
                        if (eatingCoroutine != null) StopCoroutine(eatingCoroutine);
                        eatingCoroutine = StartCoroutine(TocarEating(frutaAlvo));
                        stretch = false;
                        frutaAlvoParaDestruir = null;
                    }
                    else
                    {
                        // Mesmo que seja o último clique, se estiver em Walk, estica para comer
                        Vector3 targetPos = frutaAlvo.transform.position - new Vector3(0, 0, frutaOffsetZ);
                        Vector3 deslocamento = targetPos - neckBone.parent.TransformPoint(originalLocalPos);
                        if (deslocamento.magnitude > forwardDistance)
                            deslocamento = deslocamento.normalized * forwardDistance;

                        forwardTarget = neckBone.parent.TransformPoint(originalLocalPos) + deslocamento;
                        stretch = true;
                        frutaAlvoParaDestruir = frutaAlvo; // desaparece ao chegar perto
                    }
                }
                //frutaAlvo.ConsumirClique();
                // Se não estiver em Walk, não estica nem come, apenas cliques já decrementados
            }
            else
            {
                // Espaço apertado sem fruta
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    Vector3 deslocamento = neckBone.forward * forwardDistance;
                    forwardTarget = neckBone.parent.TransformPoint(originalLocalPos) + deslocamento;
                    stretch = true;
                    frutaAlvoParaDestruir = null;
                }
            }
        }
    }

    private IEnumerator TocarEating(comida_geral fruta)
    {
        if (animator != null)
            animator.Play("Eating");

        yield return new WaitForSeconds(1.5f); // duração da animação

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
                    frutaAlvoParaDestruir.ConsumirClique(); // só aqui a fruta some
                    frutaAlvoParaDestruir = null;
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
