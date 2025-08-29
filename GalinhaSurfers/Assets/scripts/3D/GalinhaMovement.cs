using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalinhaMovement : MonoBehaviour
{
    private float[] lanes = { -2f, 0f, 2f };
    private int currentLane = 1;
    public float speed = 1f;
    public float durationJump;
    private Animator animator;
    private bool isJumping = false;
    public tresDoisUm tresDoisUm;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Walk");
    }

    private void Update()
    {
        if (tresDoisUm.TresDoisUmGO == true)
            return;

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
    private float? speedPending = null;
    public void VelGalin()
    {
        if (!isJumping)
        {
            animator.speed = speed;
            durationJump = 0.75f / speed;
        }
        else
        {
            speedPending = speed;
        }
    }

    private IEnumerator JumpToLane(int newLane)
    {
        isJumping = true;
        animator.Play("Jump");
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(lanes[newLane], startPos.y, startPos.z); 
        float elapsed = 0f;
        while (elapsed < durationJump)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / durationJump;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        transform.position = endPos;
        currentLane = newLane;
        animator.Play("Walk");
        isJumping = false;
        if (speedPending.HasValue)
        {
            speed = speedPending.Value;
            animator.speed = speed;
            durationJump = 0.75f / speed;
            speedPending = null;
        }
    }
}
