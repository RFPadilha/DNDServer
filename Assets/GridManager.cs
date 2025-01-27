using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject mapObject; // Objeto do mapa que ser� escalado com base na grid
    public int numRows = 10; // N�mero de linhas
    public int numCols = 10; // N�mero de colunas
    public Color gridColor = Color.white; // Cor da grade
    private SpriteRenderer mapRenderer;
    private float cellWidth;
    private float cellHeight;

    void Start()
    {
        // Pega o SpriteRenderer do mapa para ajustar o tamanho
        mapRenderer = mapObject.GetComponent<SpriteRenderer>();

        // Chama a fun��o que cria a grade
        CreateGrid();
    }

    void CreateGrid()
    {
        // Calcula o tamanho de cada c�lula baseado no tamanho do mapa e no n�mero de linhas/colunas
        float mapWidth = mapRenderer.bounds.size.x;
        float mapHeight = mapRenderer.bounds.size.y;

        // Calcula o tamanho de cada c�lula
        cellWidth = mapWidth / numCols;
        cellHeight = mapHeight / numRows;

        // Desenha a grade visualmente no Gizmos (ou seja, desenha no editor, mas pode ser adaptado para ser desenhado na UI)
    }

    // M�todo para desenhar a grade no editor para visualiza��o
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

    // M�todo para converter uma posi��o mundial para a c�lula da grade mais pr�xima
    public Vector2 GetNearestCellPosition(Vector2 worldPosition)
    {
        // Calcula a c�lula mais pr�xima
        int column = Mathf.FloorToInt((worldPosition.x - mapRenderer.bounds.min.x) / cellWidth);
        int row = Mathf.FloorToInt((worldPosition.y - mapRenderer.bounds.min.y) / cellHeight);

        // Retorna a posi��o central da c�lula mais pr�xima
        float cellX = mapRenderer.bounds.min.x + (column * cellWidth) + (cellWidth / 2);
        float cellY = mapRenderer.bounds.min.y + (row * cellHeight) + (cellHeight / 2);

        return new Vector2(cellX, cellY);
    }
}
