using UnityEngine;

public class NeckTargetFollowMouse : MonoBehaviour
{
    public Transform neckTarget;              // O Target do pescoço
    public Transform headOriginalPosition;    // Posição original da cabeça
    public float moveSpeed = 10f;             // Velocidade do pescoço
    public float returnDelay = 0.5f;          // Delay antes de voltar

    private bool movingToMouse = false;
    private Vector3 targetPosition;

    void Update()
    {
        // Detecta clique do mouse
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;
                movingToMouse = true;
            }
        }

        // Move o NeckTarget até a posição do clique
        if (movingToMouse)
        {
            neckTarget.position = Vector3.MoveTowards(neckTarget.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(neckTarget.position, targetPosition) < 0.1f)
            {
                movingToMouse = false;
                // Depois de um tempo, volta para posição original
                Invoke("ReturnNeck", returnDelay);
            }
        }
    }

    void ReturnNeck()
    {
        neckTarget.position = headOriginalPosition.position;
    }
}
