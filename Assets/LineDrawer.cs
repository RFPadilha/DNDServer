using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public Camera mainCamera; // A câmera principal para capturar cliques
    public GameObject linePrefab; // Prefab com LineRenderer e EdgeCollider2D
    private LineRenderer currentLineRenderer;
    private List<Vector2> points = new List<Vector2>();
    private bool isDrawing = false;

    void Update()
    {
        // Verifica se o usuário está clicando com o botão esquerdo para adicionar pontos
        if (Input.GetMouseButtonDown(0))
        {
            if (!isDrawing)
            {
                // Inicia uma nova linha com o primeiro clique
                StartNewLine();
            }
            else
            {
                // Adiciona o ponto final ou outro ponto se a tecla Shift estiver pressionada
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                AddPoint(mousePos);
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
            CloseShape();
        }
    }

    // Inicia uma nova linha
    void StartNewLine()
    {
        GameObject line = Instantiate(linePrefab);
        currentLineRenderer = line.GetComponent<LineRenderer>();

        points.Clear();
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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

        // Atualiza o EdgeCollider2D para fechar a forma
        EdgeCollider2D edgeCollider = currentLineRenderer.GetComponent<EdgeCollider2D>();
        edgeCollider.points = points.ToArray();

        isDrawing = false;
    }
}
