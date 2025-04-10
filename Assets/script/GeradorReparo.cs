using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GeradorReparo : MonoBehaviour
{
    public GameObject[] geradorSlots;
    public Sprite geradorConsertadoSprite;
    public List<Image> inventarioSlots;
    public SpriteRenderer geradorSpriteRenderer;

    [Header("Áudio")]
    public AudioClip somColocarItem;

    [Header("Som por distância")]
    public float distanciaMaximaSom = 10f;

    private bool jogadorPerto = false;
    private bool geradorConsertado = false;
    private Transform jogador;

    void Start()
    {
        foreach (GameObject slot in geradorSlots)
        {
            if (slot != null)
                slot.SetActive(false);
        }

        GameObject objJogador = GameObject.FindGameObjectWithTag("Player");
        if (objJogador != null)
        {
            jogador = objJogador.transform;
        }
    }

    void Update()
    {
        if (jogadorPerto && !geradorConsertado && Input.GetKeyDown(KeyCode.E))
        {
            if (TemTodosOsItensNoInventario())
            {
                AdicionarItensAoGerador();
            }
            else
            {
                Debug.Log("Ainda faltam itens no inventário!");
            }
        }
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

    bool TemTodosOsItensNoInventario()
    {
        int itensColetados = 0;
        foreach (Image slot in inventarioSlots)
        {
            if (slot != null && slot.gameObject.activeSelf && slot.sprite != null)
            {
                itensColetados++;
            }
        }

        Debug.Log("Itens coletados: " + itensColetados + "/" + geradorSlots.Length);
        return itensColetados == geradorSlots.Length;
    }

    void AdicionarItensAoGerador()
    {
        // Toca som 3D com spatialBlend
        if (somColocarItem != null && jogador != null)
        {
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.clip = somColocarItem;
            tempSource.spatialBlend = 1f; // Ativa som 3D
            tempSource.minDistance = 1f;
            tempSource.maxDistance = distanciaMaximaSom;
            tempSource.rolloffMode = AudioRolloffMode.Linear;
            tempSource.Play();

            Destroy(tempSource, somColocarItem.length);
        }

        int index = 0;
        foreach (Image slot in inventarioSlots)
        {
            if (slot != null && slot.gameObject.activeSelf && slot.sprite != null)
            {
                geradorSlots[index].SetActive(true);
                geradorSlots[index].GetComponent<SpriteRenderer>().sprite = slot.sprite;

                slot.sprite = null;
                slot.gameObject.SetActive(false);
                index++;
            }
        }

        if (index >= geradorSlots.Length)
        {
            ConsertarGerador();
        }
    }

    void ConsertarGerador()
    {
        if (geradorSpriteRenderer != null && geradorConsertadoSprite != null)
        {
            geradorSpriteRenderer.sprite = geradorConsertadoSprite;
            geradorConsertado = true;
            Debug.Log("Gerador consertado!");

            AparecerPorta portaScript = FindObjectOfType<AparecerPorta>();
            if (portaScript != null)
            {
                portaScript.AtivarPorta();
            }
            else
            {
                Debug.LogError("Script da porta não encontrado!");
            }
        }
    }
}
