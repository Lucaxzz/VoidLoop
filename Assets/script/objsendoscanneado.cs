using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class objsendoscaneado : MonoBehaviour
{
    public GameObject scanPanel;
    public RectTransform scanPanelTransform;
    public TMP_Text scanText;
    public GameObject scanImageObject;
    public UnityEngine.UI.Button closeButton;
    public string objectInfo;
    public float textSpeed = 0.05f;
    public float panelMoveSpeed = 5f;
    public float movementDisableTime = 1f;

    private bool isNear;
    private bool isTyping;
    private bool isPanelActive;
    private bool canCloseFromMovement;

    public MissionManager missionManager;

    private Vector3 panelStartPos;
    private Vector3 panelEndPos;
    private Vector3 panelHidePos;

    [Header("Áudio")]
    public AudioClip somEscaneando; // 🔊 Som ao apertar F para escanear
    private AudioSource audioSource;

    void Start()
    {
        scanPanel.SetActive(false);
        closeButton.onClick.AddListener(CloseScan);
        scanImageObject.SetActive(false);

        panelStartPos = new Vector3(scanPanelTransform.anchoredPosition.x, -Screen.height, 0);
        panelEndPos = new Vector3(scanPanelTransform.anchoredPosition.x, 0, 0);
        panelHidePos = new Vector3(scanPanelTransform.anchoredPosition.x, -Screen.height, 0);
        scanPanelTransform.anchoredPosition = panelStartPos;

        // Configura o áudio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.F) && !isTyping)
        {
            // 🔊 Toca som de escaneamento
            if (somEscaneando != null)
            {
                audioSource.PlayOneShot(somEscaneando);
            }

            StartCoroutine(ShowScanPanel());
            missionManager.CompleteMission(gameObject);
        }

        if (isPanelActive && canCloseFromMovement && (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            StartCoroutine(HideScanPanel());
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

    IEnumerator ShowScanPanel()
    {
        scanPanel.SetActive(true);
        scanImageObject.SetActive(true);
        isPanelActive = true;
        canCloseFromMovement = false;

        yield return StartCoroutine(MovePanel(panelStartPos, panelEndPos, panelMoveSpeed));
        yield return StartCoroutine(TypeText(objectInfo));

        closeButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(movementDisableTime);
        canCloseFromMovement = true;
    }

    IEnumerator HideScanPanel()
    {
        isPanelActive = false;
        yield return StartCoroutine(MovePanel(panelEndPos, panelHidePos, panelMoveSpeed));

        scanPanel.SetActive(false);
        scanImageObject.SetActive(false);
        scanPanelTransform.anchoredPosition = panelStartPos;
    }

    IEnumerator MovePanel(Vector3 start, Vector3 end, float speed)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            scanPanelTransform.anchoredPosition = Vector3.Lerp(start, end, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }
        scanPanelTransform.anchoredPosition = end;
    }

    IEnumerator TypeText(string text)
    {
        scanText.text = "";
        closeButton.gameObject.SetActive(false);
        isTyping = true;

        foreach (char letter in text.ToCharArray())
        {
            scanText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        closeButton.gameObject.SetActive(true);
    }

    void CloseScan()
    {
        StartCoroutine(HideScanPanel());
    }
}
