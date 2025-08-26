using UnityEngine;

public class NeckStretch : MonoBehaviour
{
    public Transform neckBase;    // osso do início do pescoço
    public Transform neckEnd;     // osso da ponta do pescoço
    public Transform neckTarget;  // alvo do pescoço
    public float moveSpeed = 5f;  // velocidade do pescoço

    private Vector3 originalPos;

    void Start()
    {
        // guarda a posição original do target (onde o pescoço "descansa")
        originalPos = neckTarget.position;
    }

    void Update()
    {
        Vector3 targetPos;

        // se o botão do mouse esquerdo estiver clicado -> vai até o mouse
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPos = hit.point; // vai até onde clicou
            }
            else
            {
                targetPos = originalPos; // se não acertar nada, volta
            }
        }
        else
        {
            // sem clique -> volta à posição original
            targetPos = originalPos;
        }

        // move o neckTarget de forma suave, MESMA velocidade nos dois casos
        neckTarget.position = Vector3.MoveTowards(
            neckTarget.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }
}
