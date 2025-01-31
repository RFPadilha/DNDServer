using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    private int row, col; // Grid position
    private bool isDragging = false;
    private Vector2 originalPosition;

    private Camera mainCamera;
    private Grid grid;
    private ToolManager toolManager;

    private void Start()
    {
        mainCamera = Camera.main;
        grid = FindObjectOfType<Grid>(); // Assuming there's only one grid in the scene
        toolManager = FindObjectOfType<ToolManager>(); // Reference to the mode manager
    }

    private void OnMouseDown()
    {
        // Check if we are in Token mode before allowing dragging
        if (toolManager.currentMode == ToolManager.Mode.Token)
        {
            originalPosition = transform.position;
            isDragging = true;
            Debug.Log("Clicked token");
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging && toolManager.currentMode == ToolManager.Mode.Token)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }

    private void OnMouseUp()
    {
        if (isDragging && toolManager.currentMode == ToolManager.Mode.Token)
        {
            isDragging = false;

            // Snap the token to the nearest grid tile
            Vector2 snappedPosition = SnapToGrid(transform.position);
            transform.position = snappedPosition;
        }
    }

    private Vector2 SnapToGrid(Vector2 worldPos)
    {
        Vector2 nearestTilePos = Vector2.zero;
        float shortestDistance = float.MaxValue;

        foreach (var tilePos in grid.tiles.Keys)
        {
            float distance = Vector2.Distance(worldPos, tilePos);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestTilePos = tilePos;
            }
        }

        return nearestTilePos;
    }
}
