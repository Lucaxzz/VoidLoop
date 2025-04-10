using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AreaToxica : MonoBehaviour
{
    public GameObject canvasEstamina;
    public Slider barraEstamina;
    public float estaminaMaxima = 100f;
    public float consumoPorSegundo = 20f;
    public float regeneracaoPorSegundo = 10f;
    public Transform localSeguro;
    public Image telaPreta;

    [Header("Áudio")]
    public AudioClip somAoEntrarNaArea;
    private AudioSource audioSource;

    private float estaminaAtual;
    private bool dentroDaAreaToxica = false;
    private GameObject jogador;
    private bool sendoTeletransportado = false;

    void Start()
    {
        estaminaAtual = estaminaMaxima;
        barraEstamina.maxValue = estaminaMaxima;
        barraEstamina.value = estaminaAtual;
        canvasEstamina.SetActive(false);
        jogador = GameObject.FindGameObjectWithTag("Player");

        if (telaPreta != null)
        {
            Color cor = telaPreta.color;
            cor.a = 0f;
            telaPreta.color = cor;
        }

        // Prepara o áudio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D
    }

    void Update()
    {
        if (jogador == null || sendoTeletransportado) return;

        if (dentroDaAreaToxica)
        {
            DiminuirEstamina();
        }
        else if (estaminaAtual < estaminaMaxima)
        {
            RegenerarEstamina();
        }

        if (estaminaAtual <= 0 && !sendoTeletransportado)
        {
            StartCoroutine(EfeitoAntesDeTeleportar());
        }

        if (!dentroDaAreaToxica && estaminaAtual >= estaminaMaxima && canvasEstamina.activeSelf)
        {
            canvasEstamina.SetActive(false);
        }
    }

    void DiminuirEstamina()
    {
        estaminaAtual -= consumoPorSegundo * Time.deltaTime;
        barraEstamina.value = estaminaAtual;
    }

    void RegenerarEstamina()
    {
        estaminaAtual += regeneracaoPorSegundo * Time.deltaTime;
        barraEstamina.value = estaminaAtual;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dentroDaAreaToxica = true;
            canvasEstamina.SetActive(true);

            if (somAoEntrarNaArea != null)
            {
                audioSource.PlayOneShot(somAoEntrarNaArea);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dentroDaAreaToxica = false;

            if (estaminaAtual >= estaminaMaxima)
            {
                canvasEstamina.SetActive(false);
            }
        }
    }

    IEnumerator EfeitoAntesDeTeleportar()
    {
        if (jogador == null) yield break;

        sendoTeletransportado = true;

        if (telaPreta != null)
        {
            yield return StartCoroutine(FadeTela(1f, 0.25f));
        }

        yield return StartCoroutine(FadeOutJogador(0.5f));
        TeletransportarJogador();
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeInJogador(0.5f));
        yield return StartCoroutine(FadeTela(0f, 1f));

        sendoTeletransportado = false;
    }

    IEnumerator FadeTela(float alvoAlpha, float duracao)
    {
        float tempo = 0f;
        Color cor = telaPreta.color;
        float alphaInicial = cor.a;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            cor.a = Mathf.Lerp(alphaInicial, alvoAlpha, tempo / duracao);
            telaPreta.color = cor;
            yield return null;
        }
    }

    IEnumerator FadeOutJogador(float duracao)
    {
        if (jogador == null) yield break;

        float tempo = 0f;
        SpriteRenderer sr = jogador.GetComponent<SpriteRenderer>();
        Color corOriginal = sr.color;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            corOriginal.a = Mathf.Lerp(1f, 0f, tempo / duracao);
            sr.color = corOriginal;
            yield return null;
        }
    }

    IEnumerator FadeInJogador(float duracao)
    {
        if (jogador == null) yield break;

        float tempo = 0f;
        SpriteRenderer sr = jogador.GetComponent<SpriteRenderer>();
        Color corOriginal = sr.color;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            corOriginal.a = Mathf.Lerp(0f, 1f, tempo / duracao);
            sr.color = corOriginal;
            yield return null;
        }
    }

    void TeletransportarJogador()
    {
        if (jogador == null || localSeguro == null) return;

        Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        jogador.transform.position = localSeguro.position;
        estaminaAtual = estaminaMaxima;
        barraEstamina.value = estaminaAtual;
        dentroDaAreaToxica = false;
    }

    public void DesativarAreaToxica()
    {
        dentroDaAreaToxica = false;
        canvasEstamina.SetActive(false);
    }
}
