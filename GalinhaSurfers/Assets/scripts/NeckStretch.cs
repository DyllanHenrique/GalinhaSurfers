using UnityEngine;

public class NeckStretch : MonoBehaviour
{
    public Transform neckBase;    // osso do in�cio do pesco�o
    public Transform neckEnd;     // osso da ponta do pesco�o
    public Transform neckTarget;  // alvo do pesco�o
    public float moveSpeed = 5f;  // velocidade do pesco�o

    private Vector3 originalPos;

    void Start()
    {
        // guarda a posi��o original do target (onde o pesco�o "descansa")
        originalPos = neckTarget.position;
    }

    void Update()
    {
        Vector3 targetPos;

        // se o bot�o do mouse esquerdo estiver clicado -> vai at� o mouse
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPos = hit.point; // vai at� onde clicou
            }
            else
            {
                targetPos = originalPos; // se n�o acertar nada, volta
            }
        }
        else
        {
            // sem clique -> volta � posi��o original
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
