using UnityEngine;
using TMPro;
using System.Collections;

public class nuvem : MonoBehaviour
{
    public GameObject textPanel;
    public TMP_Text textDisplay;
    public string firstMessage;
    public string secondMessage;
    public float textSpeed = 0.05f;
    public float delayBetweenTexts = 1f;
    public float delayBetweenTypingAndNextText = 0.5f;
    public float panelHideDelay = 1f;
    public float fadeDuration = 0.5f;

    private bool hasTriggered = false;
    private GameObject playerObj;
    private Animator playerAnimator;
    private player playerScript;
    private CanvasGroup panelCanvasGroup;

    [Header("Áudio de passos")]
    public AudioSource audioPassos; // 🎵 Arraste aqui o AudioSource do som de passos no Inspetor

    void Start()
    {
        textPanel.SetActive(false);
        panelCanvasGroup = textPanel.GetComponent<CanvasGroup>();

        if (panelCanvasGroup == null)
        {
            panelCanvasGroup = textPanel.AddComponent<CanvasGroup>();
        }

        panelCanvasGroup.alpha = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            playerObj = other.gameObject;
            playerAnimator = playerObj.GetComponent<Animator>();
            playerScript = playerObj.GetComponent<player>();

            if (playerScript != null)
                playerScript.enabled = false;

            if (playerAnimator != null)
                playerAnimator.SetBool("taCorrendo", false);

            // 🔇 Desativa o som de passos
            if (audioPassos != null)
            {
                audioPassos.Stop();
                audioPassos.enabled = false;
            }

            StartCoroutine(ShowTextSequence());
        }
    }

    IEnumerator ShowTextSequence()
    {
        textPanel.SetActive(true);
        yield return StartCoroutine(FadePanel(1));
        yield return StartCoroutine(TypeText(firstMessage));
        yield return new WaitForSeconds(delayBetweenTypingAndNextText);
        yield return StartCoroutine(TypeText(secondMessage));
        yield return new WaitForSeconds(panelHideDelay);
        yield return StartCoroutine(FadePanel(0));
        textPanel.SetActive(false);

        // ✅ Reativa o script e o som de passos
        if (playerScript != null)
            playerScript.enabled = true;

        if (audioPassos != null)
            audioPassos.enabled = true;
    }

    IEnumerator TypeText(string message)
    {
        textDisplay.text = "";
        foreach (char letter in message.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator FadePanel(float targetAlpha)
    {
        float startAlpha = panelCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        panelCanvasGroup.alpha = targetAlpha;
    }
}
