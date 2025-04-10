using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HoverEfeitoUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imagemAlvo;               // Imagem do botão
    public TMP_Text textoAlvo;             // Texto abaixo da imagem
    public Color corHoverImagem = new Color(0.6f, 0.6f, 0.6f, 1f); // Cor mais escura
    public Color corHoverTexto = Color.red;                       // Texto vermelho

    private Color corOriginalImagem;
    private Color corOriginalTexto;

    void Start()
    {
        // Salva as cores originais para restaurar depois
        if (imagemAlvo != null)
            corOriginalImagem = imagemAlvo.color;

        if (textoAlvo != null)
            corOriginalTexto = textoAlvo.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (imagemAlvo != null)
            imagemAlvo.color = corHoverImagem;

        if (textoAlvo != null)
            textoAlvo.color = corHoverTexto;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (imagemAlvo != null)
            imagemAlvo.color = corOriginalImagem;

        if (textoAlvo != null)
            textoAlvo.color = corOriginalTexto;
    }
}
