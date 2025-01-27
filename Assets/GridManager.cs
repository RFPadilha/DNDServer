using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject mapObject; // Objeto do mapa que será escalado com base na grid
    public int numRows = 10; // Número de linhas
    public int numCols = 10; // Número de colunas
    public Color gridColor = Color.white; // Cor da grade
    private SpriteRenderer mapRenderer;
    private float cellWidth;
    private float cellHeight;

    void Start()
    {
        // Pega o SpriteRenderer do mapa para ajustar o tamanho
        mapRenderer = mapObject.GetComponent<SpriteRenderer>();

        // Chama a função que cria a grade
        CreateGrid();
    }

    void CreateGrid()
    {
        // Calcula o tamanho de cada célula baseado no tamanho do mapa e no número de linhas/colunas
        float mapWidth = mapRenderer.bounds.size.x;
        float mapHeight = mapRenderer.bounds.size.y;

        // Calcula o tamanho de cada célula
        cellWidth = mapWidth / numCols;
        cellHeight = mapHeight / numRows;

        // Desenha a grade visualmente no Gizmos (ou seja, desenha no editor, mas pode ser adaptado para ser desenhado na UI)
    }

    // Método para desenhar a grade no editor para visualização
    void OnDrawGizmos()
    {
        if (mapObject != null)
        {
            Gizmos.color = gridColor;

            // Calcula o tamanho do mapa
            float mapWidth = mapObject.GetComponent<SpriteRenderer>().bounds.size.x;
            float mapHeight = mapObject.GetComponent<SpriteRenderer>().bounds.size.y;

            // Desenha as linhas horizontais
            for (int i = 0; i <= numRows; i++)
            {
                Vector3 startPos = new Vector3(mapObject.transform.position.x - mapWidth / 2,
                                                mapObject.transform.position.y - mapHeight / 2 + i * cellHeight,
                                                0);
                Vector3 endPos = new Vector3(mapObject.transform.position.x + mapWidth / 2,
                                              mapObject.transform.position.y - mapHeight / 2 + i * cellHeight,
                                              0);
                Gizmos.DrawLine(startPos, endPos);
            }

            // Desenha as linhas verticais
            for (int j = 0; j <= numCols; j++)
            {
                Vector3 startPos = new Vector3(mapObject.transform.position.x - mapWidth / 2 + j * cellWidth,
                                                mapObject.transform.position.y - mapHeight / 2,
                                                0);
                Vector3 endPos = new Vector3(mapObject.transform.position.x - mapWidth / 2 + j * cellWidth,
                                              mapObject.transform.position.y + mapHeight / 2,
                                              0);
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }

    // Método para converter uma posição mundial para a célula da grade mais próxima
    public Vector2 GetNearestCellPosition(Vector2 worldPosition)
    {
        // Calcula a célula mais próxima
        int column = Mathf.FloorToInt((worldPosition.x - mapRenderer.bounds.min.x) / cellWidth);
        int row = Mathf.FloorToInt((worldPosition.y - mapRenderer.bounds.min.y) / cellHeight);

        // Retorna a posição central da célula mais próxima
        float cellX = mapRenderer.bounds.min.x + (column * cellWidth) + (cellWidth / 2);
        float cellY = mapRenderer.bounds.min.y + (row * cellHeight) + (cellHeight / 2);

        return new Vector2(cellX, cellY);
    }
}
