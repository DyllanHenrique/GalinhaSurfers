using UnityEngine;
using System.Collections;

public class NeckTargetFruit : MonoBehaviour
{
    public Transform neckTarget;              // O ponto que o pesco�o segue
    public Transform headOriginalPosition;    // A posi��o original relativa ao corpo
    public Transform galinhaBody;             // O corpo da galinha
    public float moveSpeed = 8f;              // A velocidade de movimento da cabe�a

    // Nova vari�vel para a dist�ncia que a cabe�a ir� para a frente
    public float forwardDistance = 2f;

    // Adicione esta linha para guardar o deslocamento inicial
    private Vector3 originalOffset;
    private bool movingForward = false;
    private Vector3 forwardTargetPosition;

    void Start()
    {
        // Calcula o deslocamento inicial do NeckTarget em rela��o ao corpo da galinha
        // � importante que o neckTarget esteja na posi��o "de repouso" da cabe�a no Start
        originalOffset = neckTarget.position - galinhaBody.position;
    }

    void Update()
    {
        // Se a barra de espa�o for pressionada, define o alvo � frente e come�a a se mover
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // --- LINHA MODIFICADA NOVAMENTE ---
            // Calcula a posi��o para frente usando a dire��o 'forward' do headOriginalPosition
            forwardTargetPosition = headOriginalPosition.position + headOriginalPosition.forward * forwardDistance;

            movingForward = true;
        }

        Vector3 targetPos;

        if (movingForward)
        {
            // Vai em dire��o ao alvo � frente
            targetPos = forwardTargetPosition;

            // Se chegou perto do alvo � frente, para o movimento
            if (Vector3.Distance(neckTarget.position, forwardTargetPosition) < 0.2f)
            {
                movingForward = false;
            }
        }
        else
        {
            // A cabe�a segue o corpo da galinha
            // Mant�m a posi��o relativa ao corpo, garantindo que ela se mova junto
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