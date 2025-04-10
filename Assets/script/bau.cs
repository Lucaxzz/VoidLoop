using UnityEngine;
using System.Collections;

public class bau : MonoBehaviour
{
    public Sprite openChestSprite;
    public GameObject hiddenItem;
    public float jumpHeight = 2f;
    public float jumpDistance = 2f;
    public float jumpDuration = 0.5f;
    public float bounceHeight = 0.5f;
    public float bounceDuration = 0.2f;
    public float stayTime = 120f;
    public float floatSpeed = 1f;
    public float floatAmplitude = 0.2f;

    [Header("Áudio")]
    public AudioClip somDoDrop; // 🔊 Som ao droppar o item
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private bool isOpened = false;
    private Coroutine floatCoroutine;
    private bool isInTrigger = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hiddenItem.SetActive(false); // Esconde o item no começo

        // Configura o áudio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (isInTrigger && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
        }
    }

    void OpenChest()
    {
        isOpened = true;
        spriteRenderer.sprite = openChestSprite;
        hiddenItem.SetActive(true);

        // 🔊 Toca o som do drop
        if (somDoDrop != null)
        {
            audioSource.PlayOneShot(somDoDrop);
        }

        StartCoroutine(MoveItem(hiddenItem.transform));
    }

    IEnumerator MoveItem(Transform item)
    {
        if (item == null) yield break;

        float elapsedTime = 0;
        Vector2 startPos = item.position;
        Vector2 endPos = startPos + new Vector2(jumpDistance, 0);

        while (elapsedTime < jumpDuration)
        {
            if (item == null) yield break;

            float t = elapsedTime / jumpDuration;
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            item.position = Vector2.Lerp(startPos, endPos, t) + new Vector2(0, height);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (item != null)
        {
            item.position = endPos;
        }

        elapsedTime = 0;
        Vector2 bounceStart = endPos;
        Vector2 bouncePeak = bounceStart + new Vector2(0, bounceHeight);

        while (elapsedTime < bounceDuration)
        {
            if (item == null) yield break;

            float t = elapsedTime / bounceDuration;
            float height = Mathf.Sin(t * Mathf.PI) * bounceHeight;
            item.position = Vector2.Lerp(bounceStart, bouncePeak, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (item != null)
        {
            item.position = bounceStart;
            floatCoroutine = StartCoroutine(FloatEffect(item));
        }

        yield return new WaitForSeconds(stayTime);

        if (floatCoroutine != null)
        {
            StopCoroutine(floatCoroutine);
            floatCoroutine = null;
        }

        if (item != null && item.gameObject.activeSelf)
        {
            item.gameObject.SetActive(false);
        }
    }

    IEnumerator FloatEffect(Transform item)
    {
        Vector2 originalPos = item.position;
        float elapsedTime = 0;

        while (item != null && item.gameObject.activeSelf)
        {
            elapsedTime += Time.deltaTime * floatSpeed;
            float newY = originalPos.y + Mathf.Sin(elapsedTime) * floatAmplitude;
            item.position = new Vector2(originalPos.x, newY);
            yield return null;
        }
    }
}
