using UnityEngine;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento
    public Animator animator;
    public AudioSource passoAudio; // Som de passos

    private Rigidbody2D rb;
    private bool facingRight = true; // Direção inicial do personagem
    private Vector3 originalScale; // Guarda a escala original do personagem

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        if (passoAudio != null)
        {
            passoAudio.loop = true; // Garante que o som esteja em loop
            passoAudio.playOnAwake = false; // Não toca automaticamente
        }
    }

    private void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Animação
        bool taAndando = moveInput != 0;
        animator.SetBool("taCorrendo", taAndando);

        // Movimento
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Som de passos
        if (passoAudio != null)
        {
            if (taAndando && !passoAudio.isPlaying)
                passoAudio.Play();
            else if (!taAndando && passoAudio.isPlaying)
                passoAudio.Stop();
        }

        // Flip do personagem
        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();

        // Garante escala correta
        transform.localScale = new Vector3(originalScale.x * (facingRight ? 1 : -1), originalScale.y, originalScale.z);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
