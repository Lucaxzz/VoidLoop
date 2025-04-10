using UnityEngine;
using System.Collections;

public class ElevadorBotao : MonoBehaviour
{
    public Transform plataformaElevador;
    public Transform destinoElevador;
    public float velocidade = 2f;
    public KeyCode teclaDeAtivacao = KeyCode.E;

    [Header("Botões Visuais")]
    public GameObject botaoVermelho; // GameObject do botão vermelho
    public GameObject botaoVerde;    // GameObject do botão verde

    private Vector3 posicaoInicial;
    private bool jogadorPerto = false;
    private bool elevadorAtivado = false;
    private bool emMovimento = false;

    void Start()
    {
        if (plataformaElevador != null)
            posicaoInicial = plataformaElevador.position;

        if (botaoVermelho != null)
            botaoVermelho.SetActive(true);

        if (botaoVerde != null)
            botaoVerde.SetActive(false);
    }

    void Update()
    {
        if (jogadorPerto && !elevadorAtivado && Input.GetKeyDown(teclaDeAtivacao))
        {
            elevadorAtivado = true;
            TrocarParaBotaoVerde();
            StartCoroutine(MoverElevador());
        }
    }

    IEnumerator MoverElevador()
    {
        emMovimento = true;

        // Sobe até o destino
        while (Vector3.Distance(plataformaElevador.position, destinoElevador.position) > 0.01f)
        {
            plataformaElevador.position = Vector3.MoveTowards(
                plataformaElevador.position,
                destinoElevador.position,
                velocidade * Time.deltaTime
            );
            yield return null;
        }

        yield return new WaitForSeconds(3f); // Tempo lá em cima

        // Desce até a posição inicial
        while (Vector3.Distance(plataformaElevador.position, posicaoInicial) > 0.01f)
        {
            plataformaElevador.position = Vector3.MoveTowards(
                plataformaElevador.position,
                posicaoInicial,
                velocidade * Time.deltaTime
            );
            yield return null;
        }

        // Espera 0.5s antes de liberar o botão de novo
        yield return new WaitForSeconds(0.5f);
        elevadorAtivado = false;
        TrocarParaBotaoVermelho();

        emMovimento = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
        }
    }

    void TrocarParaBotaoVerde()
    {
        if (botaoVermelho != null) botaoVermelho.SetActive(false);
        if (botaoVerde != null) botaoVerde.SetActive(true);
    }

    void TrocarParaBotaoVermelho()
    {
        if (botaoVerde != null) botaoVerde.SetActive(false);
        if (botaoVermelho != null) botaoVermelho.SetActive(true);
    }
}
