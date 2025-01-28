using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public GameObject mapObject;    // Reference to the map object
    public GameObject tokenPrefab;  // Token prefab to be instantiated on the grid
    private Grid grid;              // Reference to the Unity Grid component
    private Vector3 mapScale;       // Scale of the map object

    void Start()
    {
        // Get the Grid component attached to the same GameObject
        grid = mapObject.GetComponent<Grid>();

        // Ensure the grid component is present
        if (grid == null)
        {
            Debug.LogError("Grid component not found on map object.");
            return;
        }

        // Set grid cell size to (1, 1, 0) for Unity's standard coordinate system
        grid.cellSize = new Vector3(1, 1, 0);

        // Get the current scale of the map object
        mapScale = mapObject.transform.localScale;

        // Update the grid size based on the map scaling
        UpdateGridDimensions();
    }

    // Update grid size based on the map's world size and scaling
    void UpdateGridDimensions()
    {
        // Get the scaled map's width and height
        Vector3 mapWorldSize = new Vector3(
            mapObject.GetComponent<SpriteRenderer>().bounds.size.x * mapScale.x,
            mapObject.GetComponent<SpriteRenderer>().bounds.size.y * mapScale.y,
            0f
        );

        // Calculate number of rows and columns that fit within the map (using 1x1 unit cells)
        int numColumns = Mathf.CeilToInt(mapWorldSize.x);  // Use the map's world width directly
        int numRows = Mathf.CeilToInt(mapWorldSize.y);     // Use the map's world height directly

        // Optionally, you could adjust the grid component if needed, though it's not strictly required
        // For now, we'll just use the numRows and numColumns for spawning tokens or other logic
        Debug.Log("Grid Dimensions: Rows = " + numRows + ", Columns = " + numColumns);

        // Example: Spawn a token at a specific grid location
        SpawnTokenAtGrid(0, 0); // Example of placing a token at (0, 0)
        SpawnTokenAtGrid(1, 1);
    }

    // Spawn a token at a specific grid position (row, col)
    public void SpawnTokenAtGrid(int row, int col)
    {
        // Calculate the world position based on the grid position and the cell size (1x1)
        Vector3 gridWorldPosition = grid.CellToWorld(new Vector3Int(col, row, 0));

        // Instantiate the token at the calculated world position
        GameObject newToken = Instantiate(tokenPrefab, gridWorldPosition, Quaternion.identity);

        // Get the token script and assign grid position
        Token tokenScript = newToken.GetComponent<Token>();
        tokenScript.SetGridPosition(row, col);
    }

    // Update grid size dynamically, e.g., by map scaling
    public void UpdateGridSize(Vector2 newCellSize)
    {
        // Update the grid's cell size
        grid.cellSize = newCellSize;

        // Recalculate grid dimensions after the size change
        UpdateGridDimensions();
    }
}
