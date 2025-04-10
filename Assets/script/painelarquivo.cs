using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class painelarquivo : MonoBehaviour
{
    [Header("Referências do Canvas e Painéis")]
    public GameObject canvasPrincipal;
    public GameObject painelInteracao;
    public GameObject painelSenha;

    [Header("Botões")]
    public Button botaoAbrirPainelSenha;
    public Button botaoConfirmarSenha;

    [Header("Elementos da Senha")]
    public TMP_InputField campoSenha;
    public TMP_Text mensagemErro;

    [Header("Áudios")]
    public AudioClip somAbrirPainelInteracao;
    public AudioClip somAbrirPainelSenha;
    public AudioClip somErroSenha;
    public AudioClip somTecla; // Novo áudio para som das teclas

    private AudioSource audioSource;

    private bool jogadorNoTrigger = false;
    private bool bloqueado = false;

    void Start()
    {
        if (canvasPrincipal != null) canvasPrincipal.SetActive(false);
        if (painelInteracao != null) painelInteracao.SetActive(false);
        if (painelSenha != null) painelSenha.SetActive(false);
        if (mensagemErro != null) mensagemErro.gameObject.SetActive(false);

        if (botaoConfirmarSenha != null)
            botaoConfirmarSenha.interactable = false; // Começa desativado

        if (botaoAbrirPainelSenha != null)
            botaoAbrirPainelSenha.onClick.AddListener(AbrirPainelSenha);

        if (botaoConfirmarSenha != null)
            botaoConfirmarSenha.onClick.AddListener(() => StartCoroutine(TentarSenha()));

        if (campoSenha != null)
        {
            campoSenha.characterValidation = TMP_InputField.CharacterValidation.Integer;
            campoSenha.characterLimit = 5;

            campoSenha.onValueChanged.AddListener(delegate {
                TocarSomTecla();
                VerificarTamanhoSenha();
            });
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (jogadorNoTrigger && Input.GetKeyDown(KeyCode.E) && !bloqueado)
        {
            if (canvasPrincipal != null) canvasPrincipal.SetActive(true);
            if (painelInteracao != null) painelInteracao.SetActive(true);

            if (somAbrirPainelInteracao != null)
                audioSource.PlayOneShot(somAbrirPainelInteracao);

            Debug.Log("Painel de interação ativado!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNoTrigger = true;
            Debug.Log("Entrou no trigger!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNoTrigger = false;
            FecharTodosPaineis();
            Debug.Log("Saiu do trigger!");
        }
    }

    void AbrirPainelSenha()
    {
        if (painelSenha != null) painelSenha.SetActive(true);

        if (somAbrirPainelSenha != null)
            audioSource.PlayOneShot(somAbrirPainelSenha);

        Debug.Log("Painel de senha ativado!");
    }

    IEnumerator TentarSenha()
    {
        if (mensagemErro != null)
            mensagemErro.gameObject.SetActive(true);

        if (somErroSenha != null)
            audioSource.PlayOneShot(somErroSenha);

        Debug.Log("Senha incorreta! Exibindo mensagem de erro por 1 segundo.");

        yield return new WaitForSeconds(1f);

        bloqueado = true;
        FecharTodosPaineis();
        Debug.Log("Painéis bloqueados. Jogador não pode interagir novamente.");
    }

    void FecharTodosPaineis()
    {
        if (canvasPrincipal != null) canvasPrincipal.SetActive(false);
        if (painelInteracao != null) painelInteracao.SetActive(false);
        if (painelSenha != null) painelSenha.SetActive(false);
        if (mensagemErro != null) mensagemErro.gameObject.SetActive(false);
        if (campoSenha != null) campoSenha.text = "";

        if (botaoConfirmarSenha != null)
            botaoConfirmarSenha.interactable = false;

        Debug.Log("Todos os painéis foram fechados.");
    }

    void VerificarTamanhoSenha()
    {
        if (botaoConfirmarSenha != null && campoSenha != null)
        {
            botaoConfirmarSenha.interactable = campoSenha.text.Length == 5;
        }
    }

    void TocarSomTecla()
    {
        if (somTecla != null)
            audioSource.PlayOneShot(somTecla);
    }
}
