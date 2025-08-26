using UnityEngine;

public class NeckTargetFollowMouse : MonoBehaviour
{
    public Transform neckTarget;              // O Target do pesco�o
    public Transform headOriginalPosition;    // Posi��o original da cabe�a
    public float moveSpeed = 10f;             // Velocidade do pesco�o
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

        // Move o NeckTarget at� a posi��o do clique
        if (movingToMouse)
        {
            neckTarget.position = Vector3.MoveTowards(neckTarget.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(neckTarget.position, targetPosition) < 0.1f)
            {
                movingToMouse = false;
                // Depois de um tempo, volta para posi��o original
                Invoke("ReturnNeck", returnDelay);
            }
        }
    }

    void ReturnNeck()
    {
        neckTarget.position = headOriginalPosition.position;
    }
}
