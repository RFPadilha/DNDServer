using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public GameObject fogOfWarPlane; // Objeto para a névoa de guerra (um objeto escuro cobrindo o tabuleiro)
    public LayerMask obstacleLayer; // Camada para as linhas/obstáculos desenhadas
    public float viewRadius = 5f; // Raio da visão de cada jogador
    public int viewAngle = 360; // Ângulo de visão (360 para visão completa em todas as direções)
    public int raysCount = 100; // Número de raios emitidos para calcular a área visível

    public List<Transform> players; // Lista dos jogadores no tabuleiro

    private Mesh fogMesh;
    private Vector3[] vertices;
    private int[] triangles;

    void Start()
    {
        fogMesh = new Mesh();
        fogOfWarPlane.GetComponent<MeshFilter>().mesh = fogMesh;
    }

    void LateUpdate()
    {
        GenerateFogOfWarMesh();
    }

    // Gera a malha do campo de visão combinada para todos os jogadores
    void GenerateFogOfWarMesh()
    {
        List<Vector3> combinedViewPoints = new List<Vector3>();

        // Para cada jogador, gera os pontos de visão
        foreach (Transform player in players)
        {
            combinedViewPoints.AddRange(CalculateViewPoints(player.position));
        }

        // Atualiza a malha de névoa de guerra
        UpdateFogMesh(combinedViewPoints);
    }

    // Calcula os pontos de visão para um jogador individual
    List<Vector3> CalculateViewPoints(Vector3 origin)
    {
        List<Vector3> viewPoints = new List<Vector3>();

        float angleStep = viewAngle / raysCount;
        for (int i = 0; i <= raysCount; i++)
        {
            float currentAngle = i * angleStep;
            Vector3 dir = DirFromAngle(currentAngle);
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewRadius, obstacleLayer);

            if (hit.collider != null)
            {
                // Se houver colisão com um obstáculo, o ponto de visão é o ponto de colisão
                viewPoints.Add(hit.point);
            }
            else
            {
                // Caso contrário, o ponto de visão é o extremo do raio
                viewPoints.Add(origin + dir * viewRadius);
            }
        }

        return viewPoints;
    }

    // Atualiza a malha de névoa de guerra baseada nos pontos de visão
    void UpdateFogMesh(List<Vector3> viewPoints)
    {
        fogMesh.Clear();
        vertices = new Vector3[viewPoints.Count + 1]; // Vertices para a malha
        triangles = new int[(viewPoints.Count - 1) * 3]; // Triângulos para a malha

        vertices[0] = Vector3.zero; // Ponto central da malha

        for (int i = 0; i < viewPoints.Count; i++)
        {
            Vector3 localPos = transform.InverseTransformPoint(viewPoints[i]);
            vertices[i + 1] = localPos;
            if (i < viewPoints.Count - 1)
            {
                // Cria triângulos conectando os pontos
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        fogMesh.vertices = vertices;
        fogMesh.triangles = triangles;
        fogMesh.RecalculateNormals();
    }

    // Calcula a direção a partir de um ângulo
    Vector3 DirFromAngle(float angleInDegrees)
    {
        float radians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
    }
}
