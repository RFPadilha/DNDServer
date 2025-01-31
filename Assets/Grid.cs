using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] RectTransform mapRect;

    //public LayerMask tileLayer; // Layer for tiles
    public Dictionary<Vector2, GameObject> tiles;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, GameObject>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Vector2 tilePos = new Vector2(x - mapRect.localScale.x / 2 + .5f, y - mapRect.localScale.y / 2 + .5f);
                Vector2 tilePos = new Vector2(x , y);
                GameObject spawnedTile = Instantiate(tilePrefab, tilePos, Quaternion.identity);
                spawnedTile.name = $"Tile[{x},{y}]";
                spawnedTile.transform.parent = transform;
                //spawnedTile.layer = tileLayer;
                tiles[tilePos] = spawnedTile;
            }
        }
    }
}
