using UnityEngine;
using UnityEngine.UI;

public class desativarobjeto : MonoBehaviour
{
    public GameObject objeto1;
    public GameObject objeto2;
    public GameObject objeto3;
    public GameObject canvasEstamina;

    [Header("Áudio")]
    public AudioClip somDesativar;
    private AudioSource audioSource;

    private bool jogadorDentro = false;

    void Start()
    {
        // Cria ou usa um AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (jogadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Tecla E pressionada dentro do trigger.");
            DesativarObjetos();

            if (somDesativar != null)
                audioSource.PlayOneShot(somDesativar);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador entrou no trigger.");
            jogadorDentro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jogador saiu do trigger.");
            jogadorDentro = false;
        }
    }

    void DesativarObjetos()
    {
        if (objeto1 != null) objeto1.SetActive(false);
        if (objeto2 != null) objeto2.SetActive(false);
        if (objeto3 != null) objeto3.SetActive(false);
        if (canvasEstamina != null) canvasEstamina.SetActive(false);
    }
}
