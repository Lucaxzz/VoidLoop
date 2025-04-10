using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class bloqueado : MonoBehaviour
{
    public Button botao;                  // O botão que será clicado
    public GameObject imagemParaMostrar; // A imagem que vai aparecer por 1 segundo

    [Header("Áudio")]
    public AudioClip somAoMostrarImagem;

    private AudioSource audioSource;

    void Start()
    {
        // Garante que a imagem começa desativada
        if (imagemParaMostrar != null)
            imagemParaMostrar.SetActive(false);

        // Liga a função ao botão
        if (botao != null)
            botao.onClick.AddListener(() => StartCoroutine(MostrarImagemPorUmSegundo()));

        // Configura o AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    IEnumerator MostrarImagemPorUmSegundo()
    {
        if (imagemParaMostrar != null)
        {
            imagemParaMostrar.SetActive(true);

            if (somAoMostrarImagem != null)
                audioSource.PlayOneShot(somAoMostrarImagem);

            yield return new WaitForSeconds(1f);
            imagemParaMostrar.SetActive(false);
        }
    }
}
