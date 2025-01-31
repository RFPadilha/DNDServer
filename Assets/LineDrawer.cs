using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Importar para checar se o mouse está sobre um UI element

public class LineDrawer : MonoBehaviour
{
    public Camera mainCamera; // A câmera principal para capturar cliques
    public GameObject linePrefab; // Prefab com LineRenderer e EdgeCollider2D
    private LineRenderer currentLineRenderer;
    private List<Vector2> points = new List<Vector2>();
    private bool isDrawing = false;
    private ToolManager toolManager;

    private void Start()
    {
        toolManager = FindObjectOfType<ToolManager>();
    }

    void Update()
    {
        // Verifica se o mouse está sobre um UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; // Se o mouse estiver sobre um UI, não desenha
        }

        // Verifica se o usuário está clicando com o botão esquerdo para adicionar pontos
        if (Input.GetMouseButtonDown(0))
        {
            if (toolManager.currentMode == ToolManager.Mode.Drawing)
            {
                if (!isDrawing)
                {
                    // Inicia uma nova linha com o primeiro clique
                    StartNewLine();
                }
                else
                {
                    // Adiciona o ponto final ou outro ponto se a tecla Shift estiver pressionada
                    Vector2 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
                    mousePos = new Vector3(mousePos.x, mousePos.y, 0f);
                    AddPoint(mousePos);
                }
            }
        }

        // Se o usuário está desenhando, o ponto final segue o mouse
        if (isDrawing)
        {
            UpdateEndPoint();
        }

        // Botão direito fecha a geometria (se houver 3 ou mais pontos)
        if (Input.GetMouseButtonDown(1) && points.Count >= 3)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            mousePos = new Vector3(mousePos.x, mousePos.y, 0f);
            AddPoint(mousePos);
            CloseShape();
        }

        // Cancela o desenho atual se "Escape" for pressionado
        if (Input.GetKeyDown(KeyCode.Escape) && isDrawing)
        {
            CancelDrawing();
        }
    }

    // Inicia uma nova linha
    void StartNewLine()
    {
        GameObject line = Instantiate(linePrefab);
        currentLineRenderer = line.GetComponent<LineRenderer>();

        points.Clear();
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
        mousePos = new Vector3(mousePos.x, mousePos.y, 0f);

        points.Add(mousePos);
        points.Add(mousePos); // Inicialmente, o ponto final é o mesmo que o inicial

        currentLineRenderer.positionCount = 2;
        currentLineRenderer.SetPosition(0, points[0]);
        currentLineRenderer.SetPosition(1, points[1]);

        line.AddComponent<EdgeCollider2D>().points = points.ToArray();

        isDrawing = true;
    }

    // Adiciona um ponto à linha ou finaliza o desenho se não estiver segurando Shift
    void AddPoint(Vector2 newPoint)
    {
        points[points.Count - 1] = newPoint; // Atualiza o último ponto com a posição final do clique

        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Se estiver segurando Shift, adiciona um novo ponto
            points.Add(newPoint);
            currentLineRenderer.positionCount++;
        }
        else
        {
            // Finaliza o desenho se não estiver segurando Shift
            isDrawing = false;
        }

        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, newPoint);

        // Atualiza o EdgeCollider2D
        EdgeCollider2D edgeCollider = currentLineRenderer.GetComponent<EdgeCollider2D>();
        edgeCollider.points = points.ToArray();
    }

    // Atualiza o ponto final da linha conforme o mouse se move
    void UpdateEndPoint()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
        mousePos = new Vector3(mousePos.x, mousePos.y, 0f);

        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, mousePos);

        // Atualiza o EdgeCollider2D com o ponto temporário
        EdgeCollider2D edgeCollider = currentLineRenderer.GetComponent<EdgeCollider2D>();
        edgeCollider.points = points.ToArray();
    }

    // Fecha a forma ao conectar o primeiro e o último pontos
    void CloseShape()
    {
        points.Add(points[0]); // Conecta o último ponto ao primeiro
        currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, points[0]);

        // Atualiza o EdgeCollider2D para incluir o ponto final
        EdgeCollider2D edgeCollider = currentLineRenderer.GetComponent<EdgeCollider2D>();

        // Atualiza a lista de pontos no EdgeCollider2D, incluindo o ponto final
        List<Vector2> colliderPoints = new List<Vector2>(points);
        edgeCollider.points = colliderPoints.ToArray();

        isDrawing = false;
    }

    // Cancela o desenho atual sem afetar as linhas já desenhadas
    void CancelDrawing()
    {
        Destroy(currentLineRenderer.gameObject); // Remove a linha atual
        points.Clear(); // Limpa os pontos
        isDrawing = false; // Cancela o estado de desenho
    }
}
