using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    private int row, col; // Grid position

    // Set the grid position for the token
    public void SetGridPosition(int r, int c)
    {
        row = r;
        col = c;
    }

    // Get the token's grid position (row, col)
    public Vector2 GetGridPosition()
    {
        return new Vector2(row, col);
    }
}

