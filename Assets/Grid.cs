using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int width, height;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] RectTransform mapRect;

    public LayerMask tileLayer; // Layer for tiles
    private Dictionary<Vector2, GameObject> tiles;

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
                spawnedTile.layer = tileLayer;
                tiles[tilePos] = spawnedTile;
            }
        }
    }

    //// Detect collisions and highlight the tiles being touched by the line
    //public void DetectCollisionsWithTiles(EdgeCollider2D lineEdgeCollider)
    //{
    //    // List to store results of the tiles hit by the line
    //    List<Collider2D> results = new List<Collider2D>();

    //    // Create a contact filter to only check for tiles on the designated layer
    //    ContactFilter2D contactFilter = new ContactFilter2D();
    //    contactFilter.SetLayerMask(tileLayer);
    //    contactFilter.useTriggers = true;

    //    // Overlap the EdgeCollider2D and check for collisions with tile colliders
    //    lineEdgeCollider.OverlapCollider(contactFilter, results);

    //    // Iterate over the results and highlight the tiles
    //    foreach (Collider2D hit in results)
    //    {
    //        Tile tile = hit.GetComponent<Tile>();
    //        if (tile != null)
    //        {
    //            tile.SetHighlight(true); // Call a method to highlight the tile
    //        }
    //    }
    //}

    //// Optionally, you can add a method to clear all highlights after the line moves
    //public void ClearAllHighlights()
    //{
    //    foreach (GameObject tile in tiles.Values)
    //    {
    //        tile.GetComponent<Tile>().SetHighlight(false); // Reset the highlight for all tiles
    //    }
    //}

    //// Function to highlight a list of tiles
    //public void HighlightTiles(List<GameObject> tiles, bool highlight)
    //{
    //    foreach (var tile in tiles)
    //    {
    //        tile.GetComponent<Tile>().SetHighlight(highlight);
    //    }
    //}

    //// Function to mark tiles as unwalkable
    //public void SetTilesUnwalkable(List<GameObject> tiles)
    //{
    //    foreach (var tile in tiles)
    //    {
    //        tile.GetComponent<Tile>().SetUnwalkable(true); // Assuming Tile has an 'unwalkable' state
    //    }
    //}
}
