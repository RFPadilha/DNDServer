using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Token token; // Reference to the token this player controls
    private Grid grid; // Reference to the grid
    private Vector2 movementInput; // Input direction (from new Input System)
    private Vector2Int currentGridPosition; // The token's current grid position
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        token = GetComponent<Token>();
        grid = FindObjectOfType<Grid>(); // Find the grid in the scene
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Gameplay.Move.performed += OnMove;
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Gameplay.Move.performed -= OnMove;
    }
    private void Start()
    {
        // Assuming token starts in the center of the grid
        currentGridPosition = Vector2Int.RoundToInt(token.transform.position);
        //SnapTokenToGrid();
    }

    // This will be called by the new Input System when movement keys are pressed
    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("OnMove called");
        // Get input from the new Input System
        movementInput = context.ReadValue<Vector2>();
    }
    public void MoveTokenThroughCanvas(int direction)
    {
        switch(direction)
        {
            case 0://up
                MoveToken(new Vector2(0, 1));
                break;
            case 1://down
                MoveToken(new Vector2(0, -1));
                break;
            case 2://right
                MoveToken(new Vector2(1, 0));
                break;
            case 3://left
                MoveToken(new Vector2(-1, 0));
                break;
            default:
                Debug.LogError("Unexpected input on token movement");
                break;
        }
    }
    private void Update()
    {
        if (movementInput != Vector2.zero)
        {
            MoveToken(movementInput);
            movementInput = Vector2.zero; // Reset input after movement
        }
        if(token.newPosition != currentGridPosition)
        {
            currentGridPosition = new Vector2Int((int)token.newPosition.x, (int)token.newPosition.y);
        }
    }

    private void MoveToken(Vector2 direction)
    {
        Debug.Log("Attempted to move token");
        // Update the grid position based on the input direction
        currentGridPosition += Vector2Int.RoundToInt(direction);

        // Clamp the token's position to the grid bounds (assuming grid has size limits)
        currentGridPosition.x = Mathf.Clamp(currentGridPosition.x, 0, grid.width - 1);
        currentGridPosition.y = Mathf.Clamp(currentGridPosition.y, 0, grid.height - 1);

        SnapTokenToGrid();
    }

    private void SnapTokenToGrid()
    {
        // Snap token to the new grid position
        if (grid.tiles.ContainsKey(currentGridPosition))
        {
            token.transform.position = new Vector3(currentGridPosition.x, currentGridPosition.y, token.transform.position.z);
        }
    }
}
