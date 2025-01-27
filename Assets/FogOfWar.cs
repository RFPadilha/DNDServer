using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public GameObject fogOfWarPlane; // Objeto para a n�voa de guerra (um objeto escuro cobrindo o tabuleiro)
    public LayerMask obstacleLayer; // Camada para as linhas/obst�culos desenhadas
    public float viewRadius = 5f; // Raio da vis�o de cada jogador
    public int viewAngle = 360; // �ngulo de vis�o (360 para vis�o completa em todas as dire��es)
    public int raysCount = 100; // N�mero de raios emitidos para calcular a �rea vis�vel

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

    // Gera a malha do campo de vis�o combinada para todos os jogadores
    void GenerateFogOfWarMesh()
    {
        List<Vector3> combinedViewPoints = new List<Vector3>();

        // Para cada jogador, gera os pontos de vis�o
        foreach (Transform player in players)
        {
            combinedViewPoints.AddRange(CalculateViewPoints(player.position));
        }

        // Atualiza a malha de n�voa de guerra
        UpdateFogMesh(combinedViewPoints);
    }

    // Calcula os pontos de vis�o para um jogador individual
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
                // Se houver colis�o com um obst�culo, o ponto de vis�o � o ponto de colis�o
                viewPoints.Add(hit.point);
            }
            else
            {
                // Caso contr�rio, o ponto de vis�o � o extremo do raio
                viewPoints.Add(origin + dir * viewRadius);
            }
        }

        return viewPoints;
    }

    // Atualiza a malha de n�voa de guerra baseada nos pontos de vis�o
    void UpdateFogMesh(List<Vector3> viewPoints)
    {
        fogMesh.Clear();
        vertices = new Vector3[viewPoints.Count + 1]; // Vertices para a malha
        triangles = new int[(viewPoints.Count - 1) * 3]; // Tri�ngulos para a malha

        vertices[0] = Vector3.zero; // Ponto central da malha

        for (int i = 0; i < viewPoints.Count; i++)
        {
            Vector3 localPos = transform.InverseTransformPoint(viewPoints[i]);
            vertices[i + 1] = localPos;
            if (i < viewPoints.Count - 1)
            {
                // Cria tri�ngulos conectando os pontos
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        fogMesh.vertices = vertices;
        fogMesh.triangles = triangles;
        fogMesh.RecalculateNormals();
    }

    // Calcula a dire��o a partir de um �ngulo
    Vector3 DirFromAngle(float angleInDegrees)
    {
        float radians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
    }
}
