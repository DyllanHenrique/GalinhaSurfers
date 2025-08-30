using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tentando : MonoBehaviour
{
    public Transform neckBone;
    public float moveSpeed = 5f;
    public float forwardDistance = 2f;
    public AudioSource somComer;
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

    //ImagensComer
    public GameObject[] chompObjects;
    float displayTime = 0.2f;
    float maxOffsetX = 0.6f;
    float maxOffsetY = 0.3f;

    void Awake()
    {
        // encontra todos os LaneDetector na cena
        lanes = FindObjectsOfType<LaneDetector>();
    }
    void Start()
    {
        if (neckBone == null)
        {
            Debug.LogError("Neck bone n�o atribu�do!");
            enabled = false;
            return;
        }

        originalLocalPos = neckBone.localPosition;
        velocidadeAtual = moveSpeed;
        animator = GetComponent<Animator>();

        //chomp
        foreach (GameObject img in chompObjects)
        {
            if (img != null)
                img.SetActive(false);
        }


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
                    //chomp
                    StartCoroutine(ShowRandomImage());
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
            somComer.pitch = Random.Range(0.5f, 2f);
            somComer.Play();
            StartCoroutine(ShowRandomImage());
            animator.Play("Eating");

        fruta.ConsumirClique();
        yield return new WaitForSeconds(1.4f); // dura��o da anima��o

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
                    somComer.pitch = Random.Range(0.5f, 2f);
                    somComer.Play();
                    StartCoroutine(ShowRandomImage());
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

    //ImagComi
    IEnumerator ShowRandomImage()
    {
        int randomIndex = Random.Range(0, chompObjects.Length);
        GameObject selectedImage = chompObjects[randomIndex];

        Vector3 originalPos = selectedImage.transform.position;
        float randomX = originalPos.x + Random.Range(-maxOffsetX, maxOffsetX);
        float randomY = originalPos.y + Random.Range(-maxOffsetY, maxOffsetY);
        selectedImage.transform.position = new Vector3(randomX, randomY, originalPos.z);

        float randomRotation = Random.Range(-35f, 35f);
        selectedImage.transform.rotation = Quaternion.Euler(0, 0, randomRotation);

        selectedImage.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        selectedImage.SetActive(false);
    }
}
