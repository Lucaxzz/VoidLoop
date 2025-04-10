using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulo : MonoBehaviour
{
    public float jumpForce = 5f;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private Animator animator;
    public AudioSource audioPulo; // 🎵 Som do pulo

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            Jump();
        }
    }

    private void Jump()
    {
        isJumping = true;
        rb.velocity = Vector2.up * jumpForce;

        // Ativa a animação de pulo
        animator.SetTrigger("taPulando");

        // Toca o som do pulo
        if (audioPulo != null)
        {
            audioPulo.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;

            // Desativa a animação de pulo
            animator.ResetTrigger("taPulando");
        }
    }
}
