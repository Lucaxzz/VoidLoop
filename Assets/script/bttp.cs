using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bttp : MonoBehaviour
{
    [Header("Teleporte")]
    [SerializeField] private Transform destination;
    [SerializeField] private float fadeDuration = 1f;

    private GameObject player;
    private bool canTeleport = false;
    private bool isFading = false;
    private Image fadeImage;

    [Header("Áudio")]
    public AudioClip somDaPorta;
    private AudioSource audioSource;

    private void Start()
    {
        // Cria o canvas de fade
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        // Cria imagem preta
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform, false);
        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);

        RectTransform rect = fadeImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Áudio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            canTeleport = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTeleport = false;
            player = null;
        }
    }

    private void Update()
    {
        if (canTeleport && player != null && Input.GetKeyDown(KeyCode.E) && !isFading)
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    IEnumerator FadeAndTeleport()
    {
        isFading = true;

        // Som da porta
        if (somDaPorta != null)
        {
            audioSource.PlayOneShot(somDaPorta);
        }

        // Fade in
        yield return StartCoroutine(Fade(0f, 1f));

        // Verifica se player e destino existem
        if (player == null)
        {
            Debug.LogError("⚠️ Player está nulo! O jogador não entrou no trigger?");
        }
        if (destination == null)
        {
            Debug.LogError("⚠️ Destination está nulo! Você arrastou o destino no Inspector?");
        }

        if (player != null && destination != null)
        {
            player.transform.position = new Vector3(
                destination.position.x,
                destination.position.y,
                player.transform.position.z
            );
        }

        yield return new WaitForSeconds(0.2f);

        // Fade out
        yield return StartCoroutine(Fade(1f, 0f));

        isFading = false;
    }

    IEnumerator Fade(float from, float to)
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(from, to, time / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, to);
    }
}
