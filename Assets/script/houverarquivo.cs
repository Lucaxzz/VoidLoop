using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class hoverarquivo : MonoBehaviour
{
    public Image imagemAlvo;           // A imagem que vai escurecer
    public TMP_Text textoAlvo;         // O texto que vai ficar vermelho
    public Color corHoverImagem = new Color(0.6f, 0.6f, 0.6f, 1f);
    public Color corHoverTexto = Color.red;

    private Color corOriginalImagem;
    private Color corOriginalTexto;

    private void Start()
    {
        if (imagemAlvo != null)
            corOriginalImagem = imagemAlvo.color;

        if (textoAlvo != null)
            corOriginalTexto = textoAlvo.color;
    }

    private void OnMouseEnter()
    {
        if (imagemAlvo != null)
            imagemAlvo.color = corHoverImagem;

        if (textoAlvo != null)
            textoAlvo.color = corHoverTexto;
    }

    private void OnMouseExit()
    {
        if (imagemAlvo != null)
            imagemAlvo.color = corOriginalImagem;

        if (textoAlvo != null)
            textoAlvo.color = corOriginalTexto;
    }
}
