using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    private GridManager gridManager;

    void Start()
    {
        // Pega a referência do GridManager
        gridManager = FindObjectOfType<GridManager>();
    }

    // Método para mover o token para uma célula na grid
    public void MoveToPosition(Vector2 worldPosition)
    {
        // Converte a posição mundial para a célula mais próxima
        Vector2 cellPosition = gridManager.GetNearestCellPosition(worldPosition);

        // Move o token para a célula
        transform.position = new Vector3(cellPosition.x, cellPosition.y, 0);
    }
}

