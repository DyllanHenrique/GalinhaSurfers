using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalinhaMovement : MonoBehaviour
{
    private float[] lanes = { -2f, 0f, 2f };
    private int currentLane = 1;
    public float speed = 1f;
    public float duration;
    private Animator animator;
    private bool isJumping = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Walk");
    }

    private void Update()
    {
        if (!isJumping)
        {
            if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
            {
                StartCoroutine(JumpToLane(currentLane - 1));
            }

            if (Input.GetKeyDown(KeyCode.D) && currentLane < lanes.Length - 1)
            {
                StartCoroutine(JumpToLane(currentLane + 1));
            }
        }
    }
    public void VelGalin()
    {
        animator.speed = speed;
    }

    private IEnumerator JumpToLane(int newLane)
    {
        isJumping = true;
        animator.Play("Jump");
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(lanes[newLane], startPos.y, startPos.z); 
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;
        currentLane = newLane;
        animator.Play("Walk");
        isJumping = false;
    }
}
