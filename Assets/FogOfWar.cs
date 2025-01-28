using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    public GameObject fogOfWarPrefab; // Prefab for the fog of war mesh (each player will get one)
    public LayerMask obstacleLayer; // Layer mask for obstacles
    public float viewRadius = 5f; // Radius of vision for each player
    public float viewAngle = 360; // Full circle of vision
    public float raysCount = 100; // Number of rays used to calculate vision

    public List<Transform> players; // List of players
    private Dictionary<Transform, Mesh> playerMeshes = new Dictionary<Transform, Mesh>(); // Store individual meshes

    void Start()
    {
        // Create a fog of war object for each player
        foreach (Transform player in players)
        {
            GameObject fogObject = Instantiate(fogOfWarPrefab);
            fogObject.transform.SetParent(player, false); // Set as a child of the player without changing its local transform
            fogObject.transform.localPosition = Vector3.zero; // Center the fog of war object at the player's local origin
            fogObject.GetComponent<MeshFilter>().mesh = new Mesh();
            playerMeshes[player] = fogObject.GetComponent<MeshFilter>().mesh;
        }
    }

    void LateUpdate()
    {
        foreach (Transform player in players)
        {
            GeneratePlayerFogOfWarMesh(player);
        }
    }

    // Generate the fog of war mesh for each player
    void GeneratePlayerFogOfWarMesh(Transform player)
    {
        List<Vector3> viewPoints = CalculateViewPoints(player.position);
        UpdateFogMesh(playerMeshes[player], viewPoints, player);
    }

    // Calculate the vision points for a player
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
                viewPoints.Add(hit.point);
            }
            else
            {
                viewPoints.Add(origin + dir * viewRadius);
            }
        }

        // Close the circle by adding the first point as the last one
        if (viewPoints.Count > 0)
        {
            viewPoints.Add(viewPoints[0]);
        }

        return viewPoints;
    }

    // Update the fog mesh for an individual player
    void UpdateFogMesh(Mesh fogMesh, List<Vector3> viewPoints, Transform player)
    {
        fogMesh.Clear();
        Vector3[] vertices = new Vector3[viewPoints.Count + 1];
        int[] triangles = new int[(viewPoints.Count - 1) * 3];

        // Center the mesh at the player's local origin
        vertices[0] = Vector3.zero;

        for (int i = 0; i < viewPoints.Count; i++)
        {
            // Convert world-space view points to the local space of the fog object (child of the player)
            Transform fogObject = player; // Get the fog object's transform
            vertices[i + 1] = fogObject.InverseTransformPoint(viewPoints[i]); // Use the fog object's local space

            if (i < viewPoints.Count - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        fogMesh.vertices = vertices;
        fogMesh.triangles = triangles;
        fogMesh.RecalculateNormals();
    }


    // Calculate direction from angle in degrees
    Vector3 DirFromAngle(float angleInDegrees)
    {
        float radians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
    }
}
