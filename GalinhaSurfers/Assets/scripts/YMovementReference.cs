using UnityEngine;

public class YMovementReference : MonoBehaviour
{
    // Objeto que será a referência para o movimento no eixo X
    [Tooltip("Arraste o objeto cuja coordenada X deve ser monitorada.")]
    public Transform xReferenceObject;

    // O valor a ser adicionado à coordenada Y
    [Tooltip("O valor que será adicionado ao Y quando a condição for verdadeira.")]
    public float yIncreaseAmount = 0.2f;

    public float moveSpeed = 5f;

    private float originalY;

    void Start()
    {
        if (xReferenceObject == null)
        {
            xReferenceObject = this.transform;
            Debug.LogWarning("A referência de X não foi definida. Usando o próprio objeto.");
        }

        originalY = transform.position.y;
    }

    void Update()
    {
        float currentX = xReferenceObject.position.x;
        float newTargetY = originalY;

        if (Mathf.Abs(currentX) >= 2.0f)
        {
            newTargetY = originalY;
        }
        else if (Mathf.Abs(currentX) > 1.0f)
        {
            newTargetY = originalY + yIncreaseAmount;
        }
        else
        {
            newTargetY = originalY;
        }

        // Move o objeto suavemente para a nova altura alvo
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(transform.position.x, newTargetY, transform.position.z),
            moveSpeed * Time.deltaTime
        );
    }
}