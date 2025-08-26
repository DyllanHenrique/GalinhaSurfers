using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalinhaMovement : MonoBehaviour
{
    private float[] lanes = { -2f, 0f, 2f };
    private int currentLane = 1;
    public float speed = 10f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentLane > 0)
                currentLane--;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < lanes.Length - 1)
                currentLane++;
        }

        Vector3 targetPosition = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
