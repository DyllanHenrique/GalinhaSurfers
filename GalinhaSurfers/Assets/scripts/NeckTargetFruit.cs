using UnityEngine;
using System.Collections;

public class NeckTargetFruit : MonoBehaviour
{
    public Transform neckTarget;              // O ponto que o pescoço segue
    public Transform headOriginalPosition;    // A posição original relativa ao corpo
    public Transform galinhaBody;             // O corpo da galinha
    public float moveSpeed = 8f;              // A velocidade de movimento da cabeça

    // Nova variável para a distância que a cabeça irá para a frente
    public float forwardDistance = 2f;

    // Adicione esta linha para guardar o deslocamento inicial
    private Vector3 originalOffset;
    private bool movingForward = false;
    private Vector3 forwardTargetPosition;

    void Start()
    {
        // Calcula o deslocamento inicial do NeckTarget em relação ao corpo da galinha
        // É importante que o neckTarget esteja na posição "de repouso" da cabeça no Start
        originalOffset = neckTarget.position - galinhaBody.position;
    }

    void Update()
    {
        // Se a barra de espaço for pressionada, define o alvo à frente e começa a se mover
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // --- LINHA MODIFICADA NOVAMENTE ---
            // Calcula a posição para frente usando a direção 'forward' do headOriginalPosition
            forwardTargetPosition = headOriginalPosition.position + headOriginalPosition.forward * forwardDistance;

            movingForward = true;
        }

        Vector3 targetPos;

        if (movingForward)
        {
            // Vai em direção ao alvo à frente
            targetPos = forwardTargetPosition;

            // Se chegou perto do alvo à frente, para o movimento
            if (Vector3.Distance(neckTarget.position, forwardTargetPosition) < 0.2f)
            {
                movingForward = false;
            }
        }
        else
        {
            // A cabeça segue o corpo da galinha
            // Mantém a posição relativa ao corpo, garantindo que ela se mova junto
            targetPos = galinhaBody.position + originalOffset;
        }

        // Movimento suave para o alvo
        neckTarget.position = Vector3.MoveTowards(
            neckTarget.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }
}