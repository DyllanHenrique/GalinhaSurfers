using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tentando : MonoBehaviour
{
    public Transform neckBone;           // Osso real do pesco�o
    public float moveSpeed = 5f;         // Velocidade do Lerp
    public float forwardDistance = 2f;   // Dist�ncia de esticada

    private Vector3 originalLocalPos;    // Posi��o inicial local do pesco�o
    private bool stretch = false;
    private Vector3 forwardTarget;

    void Start()
    {
        if (neckBone == null)
        {
            Debug.LogError("Neck bone n�o atribu�do!");
            enabled = false;
            return;
        }

        originalLocalPos = neckBone.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& Mathf.Abs(neckBone.localPosition.y - originalLocalPos.y) < 0.0001f)
        {
            forwardTarget = neckBone.position + neckBone.forward * forwardDistance;
            stretch = true;
        }
    }

    void LateUpdate()
    {
        if (neckBone == null) return;

        Vector3 targetPos;

        if (stretch)
        {
            targetPos = forwardTarget;

            if (Vector3.Distance(neckBone.position, forwardTarget) < 0.05f)
            {
                stretch = false;
            }
        }
        else
        {
            targetPos = neckBone.parent.TransformPoint(originalLocalPos);
        }
        neckBone.position = Vector3.MoveTowards(neckBone.position, targetPos, moveSpeed * Time.deltaTime);
    }
}