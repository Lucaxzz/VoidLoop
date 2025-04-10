using UnityEngine;
using UnityEngine.UI;

public class interaçaoobj : MonoBehaviour
{
    public Image inventoryItemIcon; // Imagem do item no inventário (UI)
    public Sprite itemIcon; // Ícone do item que será mostrado no inventário
    public MissionManager missionManager; // Referência ao MissionManager
    public AudioClip pickupSound; // Som que toca ao coletar
    private AudioSource audioSource; // Fonte de áudio

    private bool isNear = false; // Verifica se o jogador está perto do item

    void Start()
    {
        // Adiciona um componente AudioSource se não tiver
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false; // Garante que o som não toque automaticamente
    }

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }

    void PickupItem()
    {
        // Toca o som de coleta
        if (pickupSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(pickupSound);
        }

        // Ativa o ícone do item no inventário
        if (inventoryItemIcon != null)
        {
            inventoryItemIcon.sprite = itemIcon;
            inventoryItemIcon.gameObject.SetActive(true);
        }

        // Concluir missão associada ao item
        if (missionManager != null)
        {
            missionManager.CompleteMission(gameObject);
        }

        // Esconde o objeto visualmente e desativa o collider
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Collider2D collider = GetComponent<Collider2D>();

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (collider != null)
            collider.enabled = false;

        // Destroi o objeto após o tempo do som
        float delay = pickupSound != null ? pickupSound.length : 0f;
        Destroy(gameObject, delay);
    }
}
