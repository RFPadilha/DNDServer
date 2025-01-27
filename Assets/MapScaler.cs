using UnityEngine;
using UnityEngine.UI;

public class MapScaler : MonoBehaviour
{
    public GameObject mapObject; // Referência ao objeto do mapa que será escalado
    public Slider scaleSlider;   // Slider de UI para ajustar a escala
    private SpriteRenderer mapRenderer;

    void Start()
    {
        // Pega o SpriteRenderer do mapa
        mapRenderer = mapObject.GetComponent<SpriteRenderer>();

        // Configura um listener para o slider, para ajustar a escala dinamicamente
        scaleSlider.onValueChanged.AddListener(AdjustMapScale);

        // Define um valor inicial de escala no slider
        scaleSlider.value = 1; // Valor padrão 1:1

        // Ajusta a escala inicial ao carregar o mapa
        SetInitialMapScale();
    }

    // Método chamado quando o slider é alterado
    public void AdjustMapScale(float newScale)
    {
        // Ajusta a escala do objeto do mapa
        mapObject.transform.localScale = new Vector3(newScale, newScale, 1);
    }

    // Método para ajustar a escala inicial do mapa
    public void SetInitialMapScale()
    {
        // Define uma escala padrão de 75% do tamanho original
        mapObject.transform.localScale = new Vector3(0.75f, 0.75f, 1);
    }
}
