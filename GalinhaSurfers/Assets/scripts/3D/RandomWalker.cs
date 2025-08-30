using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalker : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float turnSpeed = 200f;

    private Vector3 moveDirection;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        PickNewDirection();
        StartCoroutine(ChangeDirectionRoutine());
    }

    private void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }


        CheckScreenBounds();
    }

    private void PickNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(2f, 12f);
            yield return new WaitForSeconds(waitTime);
            PickNewDirection();
        }
    }

    private void CheckScreenBounds()
    {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        float margin = 0.05f;

        if (viewportPos.x < 0f + margin || viewportPos.x > 1f - margin ||
            viewportPos.y < 0f + margin || viewportPos.y > 1f - margin)
        {
            PickNewDirection();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PickNewDirection();
    }
}
