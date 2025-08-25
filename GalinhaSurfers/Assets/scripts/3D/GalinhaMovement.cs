using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalinhaMovement : MonoBehaviour
{
    public float velocidade = 5f;
    public Animator animator;

    private void Start()
    {
        animator.SetBool("isWalking", false);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * velocidade * Time.deltaTime);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
