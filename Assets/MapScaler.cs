using UnityEngine;
using TMPro;

public class MapScaler : MonoBehaviour
{
    public GameObject mapObject; // Referência ao objeto do mapa que será escalado
    public TMP_InputField widthInputField; // Campo de input para a largura
    public TMP_InputField heightInputField; // Campo de input para a altura
    private SpriteRenderer mapRenderer;

    private float originalWidth;
    private float originalHeight;

    void Start()
    {
        // Pega o SpriteRenderer do mapa
        mapRenderer = mapObject.GetComponent<SpriteRenderer>();

        // Armazena as dimensões originais do mapa
        originalWidth = mapRenderer.sprite.bounds.size.x;
        originalHeight = mapRenderer.sprite.bounds.size.y;

        // Configura o listener para os campos de input
        widthInputField.onEndEdit.AddListener(OnWidthChanged);
        heightInputField.onEndEdit.AddListener(OnHeightChanged);

        // Ajusta os valores iniciais de largura e altura
        SetInitialMapDimensions();
    }

    // Define as dimensões iniciais do mapa (com base na escala padrão)
    void SetInitialMapDimensions()
    {
        float initialScale = 1f; // Escala inicial de 75%
        widthInputField.text = (originalWidth * initialScale).ToString();
        heightInputField.text = (originalHeight * initialScale).ToString();
    }

    // Ajusta a largura e altura com base no valor inserido no campo de input
    void OnWidthChanged(string value)
    {
        if (float.TryParse(value, out float newWidth) && newWidth > 0)
        {
            float scale = newWidth / originalWidth;
            mapObject.transform.localScale = new Vector3(scale, mapObject.transform.localScale.y, 1);
        }
    }

    void OnHeightChanged(string value)
    {
        if (float.TryParse(value, out float newHeight) && newHeight > 0)
        {
            float scale = newHeight / originalHeight;
            mapObject.transform.localScale = new Vector3(mapObject.transform.localScale.x, scale, 1);
        }
    }
}
